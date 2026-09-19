using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Discovery;
using Google.Apis.Http;
using Google.Apis.Logging;
using Google.Apis.Requests.Parameters;
using Google.Apis.Services;
using Google.Apis.Testing;
using Google.Apis.Util;

namespace Google.Apis.Requests
{
	public abstract class ClientServiceRequest
	{
		protected List<IHttpUnsuccessfulResponseHandler> _unsuccessfulResponseHandlers;

		protected List<IHttpExceptionHandler> _exceptionHandlers;

		protected List<IHttpExecuteInterceptor> _executeInterceptors;

		public IHttpExecuteInterceptor Credential { get; set; }

		public void AddUnsuccessfulResponseHandler(IHttpUnsuccessfulResponseHandler handler)
		{
			handler.ThrowIfNull("handler");
			if (_unsuccessfulResponseHandlers == null)
			{
				_unsuccessfulResponseHandlers = new List<IHttpUnsuccessfulResponseHandler>();
			}
			_unsuccessfulResponseHandlers.Add(handler);
		}

		public void AddExceptionHandler(IHttpExceptionHandler handler)
		{
			handler.ThrowIfNull("handler");
			if (_exceptionHandlers == null)
			{
				_exceptionHandlers = new List<IHttpExceptionHandler>();
			}
			_exceptionHandlers.Add(handler);
		}

		public void AddExecuteInterceptor(IHttpExecuteInterceptor handler)
		{
			handler.ThrowIfNull("handler");
			if (_executeInterceptors == null)
			{
				_executeInterceptors = new List<IHttpExecuteInterceptor>();
			}
			_executeInterceptors.Add(handler);
		}
	}
	public abstract class ClientServiceRequest<TResponse> : ClientServiceRequest, IClientServiceRequest<TResponse>, IClientServiceRequest
	{
		private static readonly ILogger Logger = ApplicationContext.Logger.ForType<ClientServiceRequest<TResponse>>();

		private readonly IClientService service;

		public ETagAction ETagAction { get; set; }

		public Action<HttpRequestMessage> ModifyRequest { get; set; }

		public bool? ValidateParameters { get; set; }

		public abstract string MethodName { get; }

		public abstract string RestPath { get; }

		public abstract string HttpMethod { get; }

		public IDictionary<string, IParameter> RequestParameters { get; private set; }

		public IClientService Service => service;

		protected ClientServiceRequest(IClientService service)
		{
			this.service = service;
		}

		protected virtual void InitParameters()
		{
			RequestParameters = new Dictionary<string, IParameter>();
		}

		public TResponse Execute()
		{
			try
			{
				using HttpResponseMessage response = ExecuteUnparsedAsync(CancellationToken.None).Result;
				return ParseResponse(response).Result;
			}
			catch (AggregateException ex)
			{
				ExceptionDispatchInfo.Capture(ex.InnerException ?? ex).Throw();
				throw;
			}
		}

		public Stream ExecuteAsStream()
		{
			try
			{
				return ExecuteUnparsedAsync(CancellationToken.None).Result.Content.ReadAsStreamAsync().Result;
			}
			catch (AggregateException ex)
			{
				throw ex.InnerException;
			}
			catch (Exception ex2)
			{
				throw ex2;
			}
		}

		public async Task<TResponse> ExecuteAsync()
		{
			return await ExecuteAsync(CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<TResponse> ExecuteAsync(CancellationToken cancellationToken)
		{
			using HttpResponseMessage response = await ExecuteUnparsedAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			cancellationToken.ThrowIfCancellationRequested();
			return await ParseResponse(response).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<Stream> ExecuteAsStreamAsync()
		{
			return await ExecuteAsStreamAsync(CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<Stream> ExecuteAsStreamAsync(CancellationToken cancellationToken)
		{
			HttpResponseMessage obj = await ExecuteUnparsedAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			cancellationToken.ThrowIfCancellationRequested();
			return await obj.Content.ReadAsStreamAsync().ConfigureAwait(continueOnCapturedContext: false);
		}

		private async Task<HttpResponseMessage> ExecuteUnparsedAsync(CancellationToken cancellationToken)
		{
			using HttpRequestMessage request = CreateRequest();
			return await service.HttpClient.SendAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		private async Task<TResponse> ParseResponse(HttpResponseMessage response)
		{
			if (response.IsSuccessStatusCode)
			{
				return await service.DeserializeResponse<TResponse>(response).ConfigureAwait(continueOnCapturedContext: false);
			}
			RequestError error = await service.DeserializeError(response).ConfigureAwait(continueOnCapturedContext: false);
			throw new GoogleApiException(service.Name)
			{
				Error = error,
				HttpStatusCode = response.StatusCode
			};
		}

		public HttpRequestMessage CreateRequest(bool? overrideGZipEnabled = null)
		{
			HttpRequestMessage httpRequestMessage = CreateBuilder().CreateRequest();
			object body = GetBody();
			httpRequestMessage.SetRequestSerailizedContent(service, body, overrideGZipEnabled.HasValue ? overrideGZipEnabled.Value : service.GZipEnabled);
			AddETag(httpRequestMessage);
			if (_unsuccessfulResponseHandlers != null)
			{
				httpRequestMessage.Properties.Add("__UnsuccessfulResponseHandlerKey", _unsuccessfulResponseHandlers);
			}
			if (_exceptionHandlers != null)
			{
				httpRequestMessage.Properties.Add("__ExceptionHandlerKey", _exceptionHandlers);
			}
			if (_executeInterceptors != null)
			{
				httpRequestMessage.Properties.Add("__ExecuteInterceptorKey", _executeInterceptors);
			}
			if (base.Credential != null)
			{
				httpRequestMessage.Properties.Add("__CredentialKey", base.Credential);
			}
			ModifyRequest?.Invoke(httpRequestMessage);
			return httpRequestMessage;
		}

		private RequestBuilder CreateBuilder()
		{
			RequestBuilder requestBuilder = new RequestBuilder
			{
				BaseUri = new Uri(Service.BaseUri),
				Path = RestPath,
				Method = HttpMethod
			};
			if (service.ApiKey != null)
			{
				requestBuilder.AddParameter(RequestParameterType.Query, "key", service.ApiKey);
			}
			IDictionary<string, object> dictionary = ParameterUtils.CreateParameterDictionary(this);
			AddParameters(requestBuilder, ParameterCollection.FromDictionary(dictionary));
			return requestBuilder;
		}

		protected string GenerateRequestUri()
		{
			return CreateBuilder().BuildUri().AbsoluteUri;
		}

		protected virtual object GetBody()
		{
			return null;
		}

		private void AddETag(HttpRequestMessage request)
		{
			if (GetBody() is IDirectResponseSchema directResponseSchema && !string.IsNullOrEmpty(directResponseSchema.ETag))
			{
				string eTag = directResponseSchema.ETag;
				switch ((ETagAction == ETagAction.Default) ? GetDefaultETagAction(HttpMethod) : ETagAction)
				{
				case ETagAction.IfMatch:
					request.Headers.TryAddWithoutValidation("If-Match", eTag);
					break;
				case ETagAction.IfNoneMatch:
					request.Headers.TryAddWithoutValidation("If-None-Match", eTag);
					break;
				}
			}
		}

		[VisibleForTestOnly]
		public static ETagAction GetDefaultETagAction(string httpMethod)
		{
			switch (httpMethod)
			{
			case "GET":
				return ETagAction.IfNoneMatch;
			case "PUT":
			case "POST":
			case "PATCH":
			case "DELETE":
				return ETagAction.IfMatch;
			default:
				return ETagAction.Ignore;
			}
		}

		private void AddParameters(RequestBuilder requestBuilder, ParameterCollection inputParameters)
		{
			bool flag = ValidateParameters ?? (Service as BaseClientService)?.ValidateParameters ?? true;
			foreach (KeyValuePair<string, string> inputParameter in inputParameters)
			{
				if (!RequestParameters.TryGetValue(inputParameter.Key, out var value))
				{
					throw new GoogleApiException(Service.Name, "Invalid parameter \"" + inputParameter.Key + "\" was specified");
				}
				string text = inputParameter.Value;
				if (flag && !ParameterValidator.ValidateParameter(value, text, out var error))
				{
					throw new GoogleApiException(Service.Name, "Parameter validation failed for \"" + value.Name + "\" : " + error);
				}
				if (text == null)
				{
					text = value.DefaultValue;
				}
				switch (value.ParameterType)
				{
				case "path":
					requestBuilder.AddParameter(RequestParameterType.Path, inputParameter.Key, text);
					break;
				case "query":
					if (!object.Equals(text, value.DefaultValue) || value.IsRequired)
					{
						requestBuilder.AddParameter(RequestParameterType.Query, inputParameter.Key, text);
					}
					break;
				default:
					throw new GoogleApiException(service.Name, "Unsupported parameter type \"" + value.ParameterType + "\" for \"" + value.Name + "\"");
				}
			}
			foreach (IParameter value2 in RequestParameters.Values)
			{
				if (value2.IsRequired && !inputParameters.ContainsKey(value2.Name))
				{
					throw new GoogleApiException(service.Name, "Parameter \"" + value2.Name + "\" is missing");
				}
			}
		}
	}
}
