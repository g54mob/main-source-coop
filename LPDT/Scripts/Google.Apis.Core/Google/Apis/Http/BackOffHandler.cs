using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Logging;
using Google.Apis.Util;

namespace Google.Apis.Http
{
	public class BackOffHandler : IHttpUnsuccessfulResponseHandler, IHttpExceptionHandler
	{
		public class Initializer
		{
			public static readonly Func<HttpResponseMessage, bool> DefaultHandleUnsuccessfulResponseFunc = (HttpResponseMessage r) => r.StatusCode == HttpStatusCode.ServiceUnavailable;

			public static readonly Func<Exception, bool> DefaultHandleExceptionFunc = (Exception ex) => !(ex is TaskCanceledException) && !(ex is OperationCanceledException);

			public IBackOff BackOff { get; private set; }

			public TimeSpan MaxTimeSpan { get; set; }

			public Func<HttpResponseMessage, bool> HandleUnsuccessfulResponseFunc { get; set; }

			public Func<Exception, bool> HandleExceptionFunc { get; set; }

			public Initializer(IBackOff backOff)
			{
				BackOff = backOff;
				HandleExceptionFunc = DefaultHandleExceptionFunc;
				HandleUnsuccessfulResponseFunc = DefaultHandleUnsuccessfulResponseFunc;
				MaxTimeSpan = TimeSpan.FromSeconds(16.0);
			}
		}

		private static readonly ILogger Logger = ApplicationContext.Logger.ForType<BackOffHandler>();

		public IBackOff BackOff { get; private set; }

		public TimeSpan MaxTimeSpan { get; private set; }

		public Func<HttpResponseMessage, bool> HandleUnsuccessfulResponseFunc { get; private set; }

		public Func<Exception, bool> HandleExceptionFunc { get; private set; }

		public BackOffHandler(IBackOff backOff)
			: this(new Initializer(backOff))
		{
		}

		public BackOffHandler(Initializer initializer)
		{
			BackOff = initializer.BackOff;
			MaxTimeSpan = initializer.MaxTimeSpan;
			HandleExceptionFunc = initializer.HandleExceptionFunc;
			HandleUnsuccessfulResponseFunc = initializer.HandleUnsuccessfulResponseFunc;
		}

		public virtual async Task<bool> HandleResponseAsync(HandleUnsuccessfulResponseArgs args)
		{
			if (HandleUnsuccessfulResponseFunc != null && HandleUnsuccessfulResponseFunc(args.Response))
			{
				return await HandleAsync(args.SupportsRetry, args.CurrentFailedTry, args.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			return false;
		}

		public virtual async Task<bool> HandleExceptionAsync(HandleExceptionArgs args)
		{
			if (HandleExceptionFunc != null && HandleExceptionFunc(args.Exception))
			{
				return await HandleAsync(args.SupportsRetry, args.CurrentFailedTry, args.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			return false;
		}

		private async Task<bool> HandleAsync(bool supportsRetry, int currentFailedTry, CancellationToken cancellationToken)
		{
			if (!supportsRetry || BackOff.MaxNumOfRetries < currentFailedTry)
			{
				return false;
			}
			TimeSpan ts = BackOff.GetNextBackOff(currentFailedTry);
			if (ts > MaxTimeSpan || ts < TimeSpan.Zero)
			{
				return false;
			}
			await Wait(ts, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			Logger.Debug("Back-Off handled the error. Waited {0}ms before next retry...", ts.TotalMilliseconds);
			return true;
		}

		protected virtual async Task Wait(TimeSpan ts, CancellationToken cancellationToken)
		{
			await Task.Delay(ts, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
	}
}
