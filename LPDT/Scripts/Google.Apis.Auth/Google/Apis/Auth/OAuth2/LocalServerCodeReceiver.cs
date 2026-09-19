using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Logging;
using Google.Apis.Util;

namespace Google.Apis.Auth.OAuth2
{
	public class LocalServerCodeReceiver : ICodeReceiver
	{
		public enum CallbackUriChooserStrategy
		{
			Default = 0,
			ForceLoopbackIp = 1,
			ForceLocalhost = 2
		}

		internal class LimitedLocalhostHttpServer : IDisposable
		{
			public class ServerException : Exception
			{
				public ServerException(string msg)
					: base(msg)
				{
				}
			}

			private const int MaxRequestLineLength = 16384;

			private const int MaxHeadersLength = 65536;

			private const int NetworkReadBufferSize = 1024;

			private static ILogger Logger = ApplicationContext.Logger.ForType<LimitedLocalhostHttpServer>();

			private readonly TcpListener _listener;

			private readonly CancellationTokenSource _cts;

			private readonly string _closePageResponse;

			public int Port { get; }

			public static LimitedLocalhostHttpServer Start(string url, string closePageResponse)
			{
				Uri uri = new Uri(url);
				if (!uri.IsLoopback)
				{
					throw new ArgumentException("Url must be loopback, but given: '" + url + "'", "url");
				}
				return new LimitedLocalhostHttpServer(new TcpListener(IPAddress.Loopback, uri.Port), closePageResponse);
			}

			private LimitedLocalhostHttpServer(TcpListener listener, string closePageResponse)
			{
				_listener = listener;
				_closePageResponse = closePageResponse;
				_cts = new CancellationTokenSource();
				_listener.Start();
				Port = ((IPEndPoint)_listener.LocalEndpoint).Port;
			}

			public async Task<Dictionary<string, string>> GetQueryParamsAsync(CancellationToken cancellationToken = default(CancellationToken))
			{
				using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, cancellationToken);
				using (cts.Token.Register(_listener.Stop))
				{
					_ = 1;
					try
					{
						using TcpClient client = await _listener.AcceptTcpClientAsync().ConfigureAwait(continueOnCapturedContext: false);
						_ = 1;
						try
						{
							return await GetQueryParamsFromClientAsync(client, cts.Token).ConfigureAwait(continueOnCapturedContext: false);
						}
						catch (ServerException ex)
						{
							Logger.Warning("{0}", ex.Message);
							throw;
						}
					}
					catch (ObjectDisposedException) when (cts.IsCancellationRequested)
					{
						cts.Token.ThrowIfCancellationRequested();
						throw;
					}
				}
			}

			private async Task<Dictionary<string, string>> GetQueryParamsFromClientAsync(TcpClient client, CancellationToken cancellationToken)
			{
				NetworkStream stream = client.GetStream();
				using (cancellationToken.Register(delegate
				{
					stream.Dispose();
				}))
				{
					byte[] buffer = new byte[1024];
					int bufferOfs = 0;
					int bufferSize = 0;
					Func<Task<char?>> getChar = async delegate
					{
						if (bufferOfs == bufferSize)
						{
							try
							{
								bufferSize = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(continueOnCapturedContext: false);
							}
							catch (Exception ex) when (ex is ObjectDisposedException || ex is IOException)
							{
								throw new OperationCanceledException(cancellationToken);
							}
							cancellationToken.ThrowIfCancellationRequested();
							if (bufferSize == 0)
							{
								return (char?)null;
							}
							bufferOfs = 0;
						}
						return (char)buffer[bufferOfs++];
					};
					Dictionary<string, string> requestParams = ValidateAndGetRequestParams(await ReadRequestLine(getChar).ConfigureAwait(continueOnCapturedContext: false));
					await WaitForAllHeaders(getChar).ConfigureAwait(continueOnCapturedContext: false);
					await WriteResponse(stream, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
					return requestParams;
				}
			}

			private async Task<string> ReadRequestLine(Func<Task<char?>> getChar)
			{
				StringBuilder requestLine = new StringBuilder(16384);
				do
				{
					if (requestLine.Length >= 16384)
					{
						throw new ServerException($"Request line too long: > {16384} bytes.");
					}
					char? c = await getChar().ConfigureAwait(continueOnCapturedContext: false);
					if (!c.HasValue)
					{
						throw new ServerException("Unexpected end of network stream reading request line.");
					}
					requestLine.Append(c);
				}
				while (requestLine.Length < 2 || requestLine[requestLine.Length - 2] != '\r' || requestLine[requestLine.Length - 1] != '\n');
				requestLine.Length -= 2;
				return requestLine.ToString();
			}

			private Dictionary<string, string> ValidateAndGetRequestParams(string requestLine)
			{
				string[] array = requestLine.Split(new char[1] { ' ' });
				if (array.Length != 3)
				{
					throw new ServerException("Request line ill-formatted. Should be '<request-method> <request-path> HTTP/1.1'");
				}
				string text = array[0];
				if (text != "GET")
				{
					throw new ServerException("Expected 'GET' request, got '" + text + "'");
				}
				string text2 = array[1];
				if (!text2.StartsWith("/authorize/"))
				{
					throw new ServerException("Expected request path to start '/authorize/', got '" + text2 + "'");
				}
				string[] array2 = text2.Split(new char[1] { '?' });
				if (array2.Length == 1)
				{
					return new Dictionary<string, string>();
				}
				if (array2.Length != 2)
				{
					throw new ServerException("Expected a single '?' in request path, got '" + text2 + "'");
				}
				return array2[1].Split(new char[1] { '&' }, StringSplitOptions.RemoveEmptyEntries).Select(delegate(string param)
				{
					string[] array3 = param.Split(new char[1] { '=' });
					if (array3.Length > 2)
					{
						throw new ServerException("Invalid query parameter: '" + param + "'");
					}
					string key = WebUtility.UrlDecode(array3[0]);
					string value = ((array3.Length == 2) ? WebUtility.UrlDecode(array3[1]) : "");
					return new { key, value };
				}).ToDictionary(x => x.key, x => x.value);
			}

			private async Task WaitForAllHeaders(Func<Task<char?>> getChar)
			{
				int byteCount = 0;
				int lineLength = 0;
				char c1 = '\0';
				while (true)
				{
					if (byteCount > 65536)
					{
						throw new ServerException($"Headers too long: > {65536} bytes.");
					}
					char? c2 = await getChar().ConfigureAwait(continueOnCapturedContext: false);
					if (!c2.HasValue)
					{
						throw new ServerException("Unexpected end of network stream waiting for headers.");
					}
					char num = c1;
					c1 = c2.Value;
					lineLength++;
					byteCount++;
					if (num == '\r' && c1 == '\n')
					{
						if (lineLength == 2)
						{
							break;
						}
						lineLength = 0;
					}
				}
			}

			private async Task WriteResponse(NetworkStream stream, CancellationToken cancellationToken)
			{
				string s = "HTTP/1.1 200 OK\r\n\r\n" + _closePageResponse;
				byte[] bytes = Encoding.ASCII.GetBytes(s);
				await stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				await stream.FlushAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}

			public void Dispose()
			{
				_cts.Cancel();
				_listener.Stop();
			}
		}

		internal class CallbackUriChooser
		{
			private class UriStatistics
			{
				private readonly TimeSpan _timeouts;

				private readonly IClock _clock;

				public string Uri { get; }

				public DateTimeOffset FirstServedAt { get; private set; }

				public bool IsKnownToSucceed { get; private set; }

				public bool IsKnownToFail { get; private set; }

				public int TotalResets { get; private set; }

				public bool IsTimedOut
				{
					get
					{
						if (!IsKnownToSucceed && !IsKnownToFail)
						{
							return FirstServedAt.Add(_timeouts) <= _clock.UtcNow;
						}
						return false;
					}
				}

				public bool CanBeUsed
				{
					get
					{
						if (!IsKnownToSucceed)
						{
							if (!IsKnownToFail)
							{
								return !IsTimedOut;
							}
							return false;
						}
						return true;
					}
				}

				public UriStatistics(string uri, TimeSpan timeoutsAfter, IClock clock)
				{
					_timeouts = timeoutsAfter;
					_clock = clock;
					Uri = uri;
					FirstServedAt = new DateTimeOffset(_clock.UtcNow);
					IsKnownToSucceed = false;
					IsKnownToFail = false;
					TotalResets = 0;
				}

				public void Succeeded()
				{
					IsKnownToSucceed = true;
				}

				public void Failed()
				{
					IsKnownToFail = true;
				}

				public void Reset()
				{
					TotalResets++;
					FirstServedAt = new DateTimeOffset(_clock.UtcNow);
					IsKnownToFail = false;
				}
			}

			internal static readonly string CallbackUriTemplateLocalhost = "http://localhost:{0}/authorize/";

			internal static readonly string CallbackUriTemplate127001 = "http://127.0.0.1:{0}/authorize/";

			private readonly IClock _clock;

			private readonly TimeSpan _timeout;

			private readonly Func<string, bool> _listenerFailsFor;

			private readonly object _lock = new object();

			private UriStatistics _loopbackIp;

			private UriStatistics _localhost;

			public static CallbackUriChooser Default { get; } = new CallbackUriChooser();

			public CallbackUriChooser()
				: this(SystemClock.Default, TimeSpan.FromMinutes(1.0), FailsHttpListener)
			{
			}

			internal CallbackUriChooser(IClock clock, TimeSpan timeout, Func<string, bool> listenerFailsFor)
			{
				_clock = clock;
				_timeout = timeout;
				_listenerFailsFor = listenerFailsFor;
			}

			internal string GetUriTemplate(CallbackUriChooserStrategy strategy)
			{
				lock (_lock)
				{
					switch (strategy)
					{
					case CallbackUriChooserStrategy.ForceLoopbackIp:
						InitUriStatisticsIfNeeded(ref _loopbackIp, CallbackUriTemplate127001, checkListener: false);
						return _loopbackIp.Uri;
					case CallbackUriChooserStrategy.ForceLocalhost:
						InitUriStatisticsIfNeeded(ref _localhost, CallbackUriTemplateLocalhost, checkListener: false);
						return _localhost.Uri;
					default:
					{
						InitUriStatisticsIfNeeded(ref _loopbackIp, CallbackUriTemplate127001, checkListener: true);
						if (_loopbackIp.CanBeUsed)
						{
							return _loopbackIp.Uri;
						}
						InitUriStatisticsIfNeeded(ref _localhost, CallbackUriTemplateLocalhost, checkListener: true);
						if (_localhost.CanBeUsed)
						{
							return _localhost.Uri;
						}
						int totalResets = _loopbackIp.TotalResets;
						UriStatistics uriStatistics = ((totalResets < _localhost.TotalResets) ? _loopbackIp : ((totalResets > _localhost.TotalResets) ? _localhost : (_loopbackIp.IsTimedOut ? _loopbackIp : ((!_localhost.IsTimedOut) ? _loopbackIp : _localhost))));
						uriStatistics.Reset();
						return uriStatistics.Uri;
					}
					}
				}
				void InitUriStatisticsIfNeeded(ref UriStatistics statistics, string uri, bool checkListener)
				{
					if (statistics == null)
					{
						statistics = new UriStatistics(uri, _timeout, _clock);
						if (checkListener && _listenerFailsFor(statistics.Uri))
						{
							statistics.Failed();
						}
					}
				}
			}

			public void ReportSuccess(string uri)
			{
				GetStatisticsFor(uri).Succeeded();
			}

			public void ReportFailure(string uri)
			{
				GetStatisticsFor(uri).Failed();
			}

			private UriStatistics GetStatisticsFor(string uri)
			{
				if (!(uri == CallbackUriTemplate127001))
				{
					if (!(uri == CallbackUriTemplateLocalhost))
					{
						throw new ArgumentOutOfRangeException("uri");
					}
					return _localhost;
				}
				return _loopbackIp;
			}

			private static bool FailsHttpListener(string uri)
			{
				try
				{
					using HttpListener httpListener = new HttpListener();
					httpListener.Prefixes.Add(string.Format(uri, GetRandomUnusedPort()));
					httpListener.Start();
				}
				catch (HttpListenerException ex) when (ex.ErrorCode == 5)
				{
					return true;
				}
				catch
				{
				}
				return false;
			}
		}

		private static readonly ILogger Logger = ApplicationContext.Logger.ForType<LocalServerCodeReceiver>();

		internal const string LoopbackCallbackPath = "/authorize/";

		internal const string DefaultClosePageResponse = "<html>\r\n  <head><title>OAuth 2.0 Authentication Token Received</title></head>\r\n  <body>\r\n    Received verification code. You may now close this window.\r\n    <script type='text/javascript'>\r\n      // This doesn't work on every browser.\r\n      window.setTimeout(function() {\r\n          this.focus();\r\n          window.opener = this;\r\n          window.open('', '_self', ''); \r\n          window.close(); \r\n        }, 1000);\r\n      //if (window.opener) { window.opener.checkToken(); }\r\n    </script>\r\n  </body>\r\n</html>";

		private string _callbackUriTemplate;

		private readonly string _closePageResponse;

		private string redirectUri;

		public string RedirectUri
		{
			get
			{
				if (string.IsNullOrEmpty(redirectUri))
				{
					redirectUri = string.Format(_callbackUriTemplate, GetRandomUnusedPort());
				}
				return redirectUri;
			}
		}

		public LocalServerCodeReceiver()
			: this("<html>\r\n  <head><title>OAuth 2.0 Authentication Token Received</title></head>\r\n  <body>\r\n    Received verification code. You may now close this window.\r\n    <script type='text/javascript'>\r\n      // This doesn't work on every browser.\r\n      window.setTimeout(function() {\r\n          this.focus();\r\n          window.opener = this;\r\n          window.open('', '_self', ''); \r\n          window.close(); \r\n        }, 1000);\r\n      //if (window.opener) { window.opener.checkToken(); }\r\n    </script>\r\n  </body>\r\n</html>", CallbackUriChooserStrategy.Default)
		{
		}

		public LocalServerCodeReceiver(string closePageResponse)
			: this(closePageResponse, CallbackUriChooserStrategy.Default)
		{
		}

		public LocalServerCodeReceiver(string closePageResponse, CallbackUriChooserStrategy strategy)
		{
			_closePageResponse = closePageResponse;
			_callbackUriTemplate = CallbackUriChooser.Default.GetUriTemplate(strategy);
		}

		public async Task<AuthorizationCodeResponseUrl> ReceiveCodeAsync(AuthorizationCodeRequestUrl url, CancellationToken taskCancellationToken)
		{
			string absoluteUri = url.Build().AbsoluteUri;
			using HttpListener listener = StartListener();
			Logger.Debug("Open a browser with \"{0}\" URL", absoluteUri);
			bool flag;
			try
			{
				flag = OpenBrowser(absoluteUri);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to launch browser with \"{0}\" for authorization", absoluteUri);
				throw new NotSupportedException("Failed to launch browser with \"" + absoluteUri + "\" for authorization. See inner exception for details.", ex);
			}
			if (!flag)
			{
				Logger.Error("Failed to launch browser with \"{0}\" for authorization; platform not supported.", absoluteUri);
				throw new NotSupportedException("Failed to launch browser with \"" + absoluteUri + "\" for authorization; platform not supported.");
			}
			return await GetResponseFromListener(listener, taskCancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		private static int GetRandomUnusedPort()
		{
			TcpListener tcpListener = new TcpListener(IPAddress.Loopback, 0);
			try
			{
				tcpListener.Start();
				return ((IPEndPoint)tcpListener.LocalEndpoint).Port;
			}
			finally
			{
				tcpListener.Stop();
			}
		}

		private HttpListener StartListener()
		{
			try
			{
				HttpListener httpListener = new HttpListener();
				httpListener.Prefixes.Add(RedirectUri);
				httpListener.Start();
				return httpListener;
			}
			catch
			{
				CallbackUriChooser.Default.ReportFailure(_callbackUriTemplate);
				throw;
			}
		}

		private async Task<AuthorizationCodeResponseUrl> GetResponseFromListener(HttpListener listener, CancellationToken ct)
		{
			HttpListenerContext context;
			using (ct.Register(listener.Stop))
			{
				try
				{
					context = await listener.GetContextAsync().ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception) when (ct.IsCancellationRequested)
				{
					ct.ThrowIfCancellationRequested();
					throw new InvalidOperationException();
				}
				catch
				{
					CallbackUriChooser.Default.ReportFailure(_callbackUriTemplate);
					throw;
				}
				CallbackUriChooser.Default.ReportSuccess(_callbackUriTemplate);
			}
			NameValueCollection coll = context.Request.QueryString;
			byte[] bytes = Encoding.UTF8.GetBytes(_closePageResponse);
			context.Response.ContentLength64 = bytes.Length;
			context.Response.SendChunked = false;
			context.Response.KeepAlive = false;
			Stream output = context.Response.OutputStream;
			await output.WriteAsync(bytes, 0, bytes.Length).ConfigureAwait(continueOnCapturedContext: false);
			await output.FlushAsync().ConfigureAwait(continueOnCapturedContext: false);
			output.Close();
			context.Response.Close();
			return new AuthorizationCodeResponseUrl(coll.AllKeys.ToDictionary((string k) => k, (string k) => coll[k]));
		}

		protected virtual bool OpenBrowser(string url)
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				url = Regex.Replace(url, "(\\\\*)\"", "$1$1\\\"");
				url = Regex.Replace(url, "(\\\\+)$", "$1$1");
				Process.Start(new ProcessStartInfo("cmd", "/c start \"\" \"" + url + "\"")
				{
					CreateNoWindow = true
				});
				return true;
			}
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				Process.Start("xdg-open", url);
				return true;
			}
			if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				Process.Start("open", url);
				return true;
			}
			return false;
		}
	}
}
