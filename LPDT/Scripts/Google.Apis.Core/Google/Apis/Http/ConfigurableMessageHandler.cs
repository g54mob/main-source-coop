using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Logging;
using Google.Apis.Testing;
using Google.Apis.Util;

namespace Google.Apis.Http
{
	public class ConfigurableMessageHandler : DelegatingHandler
	{
		[Flags]
		public enum LogEventType
		{
			None = 0,
			RequestUri = 1,
			RequestHeaders = 2,
			RequestBody = 4,
			ResponseStatus = 8,
			ResponseHeaders = 0x10,
			ResponseBody = 0x20,
			ResponseAbnormal = 0x40
		}

		private const string QuotaProjectHeaderName = "x-goog-user-project";

		private static readonly ILogger Logger = ApplicationContext.Logger.ForType<ConfigurableMessageHandler>();

		[VisibleForTestOnly]
		public const int MaxAllowedNumTries = 20;

		public const string UnsuccessfulResponseHandlerKey = "__UnsuccessfulResponseHandlerKey";

		public const string ExceptionHandlerKey = "__ExceptionHandlerKey";

		public const string ExecuteInterceptorKey = "__ExecuteInterceptorKey";

		public const string ResponseStreamInterceptorProviderKey = "__ResponseStreamInterceptorProviderKey";

		public const string CredentialKey = "__CredentialKey";

		private static readonly string ApiVersion = Utilities.GetLibraryVersion();

		private static readonly string UserAgentSuffix = "google-api-dotnet-client/" + ApiVersion + " (gzip)";

		private readonly object unsuccessfulResponseHandlersLock = new object();

		private readonly object exceptionHandlersLock = new object();

		private readonly object executeInterceptorsLock = new object();

		private readonly IList<IHttpUnsuccessfulResponseHandler> unsuccessfulResponseHandlers = new List<IHttpUnsuccessfulResponseHandler>();

		private readonly IList<IHttpExceptionHandler> exceptionHandlers = new List<IHttpExceptionHandler>();

		private readonly IList<IHttpExecuteInterceptor> executeInterceptors = new List<IHttpExecuteInterceptor>();

		private int _loggingRequestId;

		private ILogger _instanceLogger = Logger;

		private int numTries = 3;

		private int numRedirects = 10;

		[Obsolete("Use AddUnsuccessfulResponseHandler or RemoveUnsuccessfulResponseHandler instead.")]
		public IList<IHttpUnsuccessfulResponseHandler> UnsuccessfulResponseHandlers => unsuccessfulResponseHandlers;

		[Obsolete("Use AddExceptionHandler or RemoveExceptionHandler instead.")]
		public IList<IHttpExceptionHandler> ExceptionHandlers => exceptionHandlers;

		[Obsolete("Use AddExecuteInterceptor or RemoveExecuteInterceptor instead.")]
		public IList<IHttpExecuteInterceptor> ExecuteInterceptors => executeInterceptors;

		internal ILogger InstanceLogger
		{
			get
			{
				return _instanceLogger;
			}
			set
			{
				_instanceLogger = value.ForType<ConfigurableMessageHandler>();
			}
		}

		public int NumTries
		{
			get
			{
				return numTries;
			}
			set
			{
				if (value > 20 || value < 1)
				{
					throw new ArgumentOutOfRangeException("NumTries");
				}
				numTries = value;
			}
		}

		public int NumRedirects
		{
			get
			{
				return numRedirects;
			}
			set
			{
				if (value > 20 || value < 1)
				{
					throw new ArgumentOutOfRangeException("NumRedirects");
				}
				numRedirects = value;
			}
		}

		public bool FollowRedirect { get; set; }

		public bool IsLoggingEnabled { get; set; }

		public LogEventType LogEvents { get; set; }

		public string ApplicationName { get; set; }

		public string GoogleApiClientHeader { get; set; }

		public IHttpExecuteInterceptor Credential { get; set; }

		public void AddUnsuccessfulResponseHandler(IHttpUnsuccessfulResponseHandler handler)
		{
			lock (unsuccessfulResponseHandlersLock)
			{
				unsuccessfulResponseHandlers.Add(handler);
			}
		}

		public void RemoveUnsuccessfulResponseHandler(IHttpUnsuccessfulResponseHandler handler)
		{
			lock (unsuccessfulResponseHandlersLock)
			{
				unsuccessfulResponseHandlers.Remove(handler);
			}
		}

		public void AddExceptionHandler(IHttpExceptionHandler handler)
		{
			lock (exceptionHandlersLock)
			{
				exceptionHandlers.Add(handler);
			}
		}

		public void RemoveExceptionHandler(IHttpExceptionHandler handler)
		{
			lock (exceptionHandlersLock)
			{
				exceptionHandlers.Remove(handler);
			}
		}

		public void AddExecuteInterceptor(IHttpExecuteInterceptor interceptor)
		{
			lock (executeInterceptorsLock)
			{
				executeInterceptors.Add(interceptor);
			}
		}

		public void RemoveExecuteInterceptor(IHttpExecuteInterceptor interceptor)
		{
			lock (executeInterceptorsLock)
			{
				executeInterceptors.Remove(interceptor);
			}
		}

		public ConfigurableMessageHandler(HttpMessageHandler httpMessageHandler)
			: base(httpMessageHandler)
		{
			FollowRedirect = true;
			IsLoggingEnabled = true;
			LogEvents = LogEventType.RequestUri | LogEventType.ResponseStatus | LogEventType.ResponseAbnormal;
		}

		private void LogHeaders(string initialText, HttpHeaders headers1, HttpHeaders headers2)
		{
			IEnumerable<KeyValuePair<string, IEnumerable<string>>> enumerable = headers1;
			IEnumerable<KeyValuePair<string, IEnumerable<string>>> first = enumerable ?? Enumerable.Empty<KeyValuePair<string, IEnumerable<string>>>();
			enumerable = headers2;
			List<KeyValuePair<string, IEnumerable<string>>> list = first.Concat(enumerable ?? Enumerable.Empty<KeyValuePair<string, IEnumerable<string>>>()).ToList();
			object[] array = new object[list.Count * 2];
			StringBuilder stringBuilder = new StringBuilder(list.Count * 32);
			stringBuilder.Append(initialText);
			StringBuilder stringBuilder2 = new StringBuilder();
			for (int i = 0; i < list.Count; i++)
			{
				stringBuilder.Append($"\n  [{{{i * 2}}}] '{{{1 + i * 2}}}'");
				array[i * 2] = list[i].Key;
				stringBuilder2.Clear();
				array[1 + i * 2] = string.Join("; ", list[i].Value);
			}
			InstanceLogger.Debug(stringBuilder.ToString(), array);
		}

		private async Task LogBody(string fmtText, HttpContent content)
		{
			byte[] array = ((content == null) ? new byte[0] : (await content.ReadAsByteArrayAsync().ConfigureAwait(continueOnCapturedContext: false)));
			byte[] array2 = array;
			char[] array3 = new char[array2.Length];
			for (int i = 0; i < array2.Length; i++)
			{
				byte b = array2[i];
				array3[i] = (char)((b >= 32 && b <= 126) ? b : 46);
			}
			InstanceLogger.Debug(fmtText, new string(array3));
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			bool loggable = IsLoggingEnabled && InstanceLogger.IsDebugEnabled;
			string loggingRequestId = "";
			if (loggable)
			{
				loggingRequestId = Interlocked.Increment(ref _loggingRequestId).ToString("X8");
			}
			int triesRemaining = NumTries;
			int redirectRemaining = NumRedirects;
			string value = ((ApplicationName == null) ? "" : (ApplicationName + " ")) + UserAgentSuffix;
			request.Headers.Add("User-Agent", value);
			string googleApiClientHeader = GoogleApiClientHeader;
			if (googleApiClientHeader != null)
			{
				request.Headers.Add("x-goog-api-client", googleApiClientHeader);
			}
			HttpResponseMessage response = null;
			Exception lastException;
			do
			{
				response?.Dispose();
				response = null;
				lastException = null;
				cancellationToken.ThrowIfCancellationRequested();
				List<IHttpExecuteInterceptor> list;
				lock (executeInterceptorsLock)
				{
					list = executeInterceptors.ToList();
				}
				if (request.Properties.TryGetValue("__ExecuteInterceptorKey", out var value2) && value2 is List<IHttpExecuteInterceptor> collection)
				{
					list.AddRange(collection);
				}
				foreach (IHttpExecuteInterceptor item in list)
				{
					await item.InterceptAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				}
				CheckValidAfterInterceptors(request);
				await CredentialInterceptAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				if (loggable)
				{
					if ((LogEvents & LogEventType.RequestUri) != LogEventType.None)
					{
						InstanceLogger.Debug("Request[{0}] (triesRemaining={1}) URI: '{2}'", loggingRequestId, triesRemaining, request.RequestUri);
					}
					if ((LogEvents & LogEventType.RequestHeaders) != LogEventType.None)
					{
						LogHeaders("Request[" + loggingRequestId + "] Headers:", request.Headers, request.Content?.Headers);
					}
					if ((LogEvents & LogEventType.RequestBody) != LogEventType.None)
					{
						await LogBody("Request[" + loggingRequestId + "] Body: '{0}'", request.Content).ConfigureAwait(continueOnCapturedContext: false);
					}
				}
				try
				{
					response = await base.SendAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception ex)
				{
					lastException = ex;
				}
				if (response == null || response.StatusCode >= HttpStatusCode.BadRequest || response.StatusCode < HttpStatusCode.OK)
				{
					triesRemaining--;
				}
				bool flag2;
				if (response == null)
				{
					bool flag = false;
					List<IHttpExceptionHandler> list2;
					lock (exceptionHandlersLock)
					{
						list2 = exceptionHandlers.ToList();
					}
					if (request.Properties.TryGetValue("__ExceptionHandlerKey", out var value3) && value3 is List<IHttpExceptionHandler> collection2)
					{
						list2.AddRange(collection2);
					}
					foreach (IHttpExceptionHandler item2 in list2)
					{
						flag2 = flag;
						flag = flag2 | await item2.HandleExceptionAsync(new HandleExceptionArgs
						{
							Request = request,
							Exception = lastException,
							TotalTries = NumTries,
							CurrentFailedTry = NumTries - triesRemaining,
							CancellationToken = cancellationToken
						}).ConfigureAwait(continueOnCapturedContext: false);
					}
					if (!flag)
					{
						InstanceLogger.Error(lastException, "Response[{0}] Exception was thrown while executing a HTTP request and it wasn't handled", loggingRequestId);
						throw lastException;
					}
					if (loggable && (LogEvents & LogEventType.ResponseAbnormal) != LogEventType.None)
					{
						InstanceLogger.Debug("Response[{0}] Exception {1} was thrown, but it was handled by an exception handler", loggingRequestId, lastException.Message);
					}
					continue;
				}
				if (loggable)
				{
					if ((LogEvents & LogEventType.ResponseStatus) != LogEventType.None)
					{
						InstanceLogger.Debug("Response[{0}] Response status: {1} '{2}'", loggingRequestId, response.StatusCode, response.ReasonPhrase);
					}
					if ((LogEvents & LogEventType.ResponseHeaders) != LogEventType.None)
					{
						LogHeaders("Response[" + loggingRequestId + "] Headers:", response.Headers, response.Content?.Headers);
					}
					if ((LogEvents & LogEventType.ResponseBody) != LogEventType.None)
					{
						await LogBody("Response[" + loggingRequestId + "] Body: '{0}'", response.Content).ConfigureAwait(continueOnCapturedContext: false);
					}
				}
				if (response.IsSuccessStatusCode)
				{
					triesRemaining = 0;
					continue;
				}
				flag2 = false;
				List<IHttpUnsuccessfulResponseHandler> list3;
				lock (unsuccessfulResponseHandlersLock)
				{
					list3 = unsuccessfulResponseHandlers.ToList();
				}
				if (request.Properties.TryGetValue("__UnsuccessfulResponseHandlerKey", out var value4) && value4 is List<IHttpUnsuccessfulResponseHandler> collection3)
				{
					list3.AddRange(collection3);
				}
				HandleUnsuccessfulResponseArgs handlerArgs = new HandleUnsuccessfulResponseArgs
				{
					Request = request,
					Response = response,
					TotalTries = NumTries,
					CurrentFailedTry = NumTries - triesRemaining,
					CancellationToken = cancellationToken
				};
				bool flag3;
				foreach (IHttpUnsuccessfulResponseHandler item3 in list3)
				{
					try
					{
						flag3 = flag2;
						flag2 = flag3 | await item3.HandleResponseAsync(handlerArgs).ConfigureAwait(continueOnCapturedContext: false);
					}
					catch when (DisposeAndReturnFalse(response))
					{
					}
				}
				flag3 = flag2;
				if (!(flag3 | await CredentialHandleResponseAsync(handlerArgs).ConfigureAwait(continueOnCapturedContext: false)))
				{
					if (FollowRedirect && HandleRedirect(response))
					{
						if (redirectRemaining-- == 0)
						{
							triesRemaining = 0;
						}
						if (loggable && (LogEvents & LogEventType.ResponseAbnormal) != LogEventType.None)
						{
							InstanceLogger.Debug("Response[{0}] Redirect response was handled successfully. Redirect to {1}", loggingRequestId, response.Headers.Location);
						}
					}
					else
					{
						if (loggable && (LogEvents & LogEventType.ResponseAbnormal) != LogEventType.None)
						{
							InstanceLogger.Debug("Response[{0}] An abnormal response wasn't handled. Status code is {1}", loggingRequestId, response.StatusCode);
						}
						triesRemaining = 0;
					}
				}
				else if (loggable && (LogEvents & LogEventType.ResponseAbnormal) != LogEventType.None)
				{
					InstanceLogger.Debug("Response[{0}] An abnormal response was handled by an unsuccessful response handler. Status Code is {1}", loggingRequestId, response.StatusCode);
				}
			}
			while (triesRemaining > 0);
			if (response == null)
			{
				InstanceLogger.Error(lastException, "Request[{0}] Exception was thrown while executing a HTTP request", loggingRequestId);
				throw lastException;
			}
			if (!response.IsSuccessStatusCode && loggable && (LogEvents & LogEventType.ResponseAbnormal) != LogEventType.None)
			{
				InstanceLogger.Debug("Response[{0}] Abnormal response is being returned. Status Code is {1}", loggingRequestId, response.StatusCode);
			}
			return response;
			static bool DisposeAndReturnFalse(IDisposable disposable)
			{
				disposable.Dispose();
				return false;
			}
		}

		private void CheckValidAfterInterceptors(HttpRequestMessage request)
		{
			if (request.Headers.Contains("x-goog-user-project"))
			{
				throw new InvalidOperationException("x-goog-user-project header can only be added through the credential or through the <Product>ClientBuilder.");
			}
		}

		private async Task CredentialInterceptAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			IHttpExecuteInterceptor effectiveCredential = GetEffectiveCredential(request);
			if (effectiveCredential != null)
			{
				await effectiveCredential.InterceptAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		private async Task<bool> CredentialHandleResponseAsync(HandleUnsuccessfulResponseArgs args)
		{
			if (GetEffectiveCredential(args.Request) is IHttpUnsuccessfulResponseHandler httpUnsuccessfulResponseHandler)
			{
				return await httpUnsuccessfulResponseHandler.HandleResponseAsync(args).ConfigureAwait(continueOnCapturedContext: false);
			}
			return false;
		}

		private IHttpExecuteInterceptor GetEffectiveCredential(HttpRequestMessage request)
		{
			if (!request.Properties.TryGetValue("__CredentialKey", out var value) || !(value is IHttpExecuteInterceptor result))
			{
				return Credential;
			}
			return result;
		}

		private bool HandleRedirect(HttpResponseMessage message)
		{
			Uri location = message.Headers.Location;
			if (!message.IsRedirectStatusCode() || location == null)
			{
				return false;
			}
			HttpRequestMessage requestMessage = message.RequestMessage;
			requestMessage.RequestUri = new Uri(requestMessage.RequestUri, location);
			if (message.StatusCode == HttpStatusCode.SeeOther)
			{
				requestMessage.Method = HttpMethod.Get;
			}
			requestMessage.Headers.Remove("Authorization");
			requestMessage.Headers.IfMatch.Clear();
			requestMessage.Headers.IfNoneMatch.Clear();
			requestMessage.Headers.IfModifiedSince = null;
			requestMessage.Headers.IfUnmodifiedSince = null;
			requestMessage.Headers.Remove("If-Range");
			return true;
		}
	}
}
