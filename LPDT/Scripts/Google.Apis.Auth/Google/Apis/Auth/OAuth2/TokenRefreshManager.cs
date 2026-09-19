using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Logging;
using Google.Apis.Util;

namespace Google.Apis.Auth.OAuth2
{
	internal sealed class TokenRefreshManager
	{
		private readonly object _lock = new object();

		private readonly IClock _clock;

		private readonly ILogger _logger;

		private readonly Func<CancellationToken, Task<bool>> _refreshAction;

		private TokenResponse _token;

		private Task<TokenResponse> _refreshTask;

		internal static readonly TimeSpan[] RefreshTimeouts = new TimeSpan[3]
		{
			TimeSpan.FromSeconds(12.0),
			TimeSpan.FromMinutes(1.0),
			TimeSpan.FromMinutes(5.0)
		};

		internal TokenResponse Token
		{
			get
			{
				lock (_lock)
				{
					return _token;
				}
			}
			set
			{
				lock (_lock)
				{
					_token = value;
				}
			}
		}

		internal TokenRefreshManager(Func<CancellationToken, Task<bool>> refreshAction, IClock clock, ILogger logger)
		{
			_refreshAction = refreshAction;
			_clock = clock;
			_logger = logger;
		}

		internal async Task<string> GetAccessTokenForRequestAsync(CancellationToken cancellationToken)
		{
			Task<TokenResponse> refreshTask;
			lock (_lock)
			{
				if (_token != null && !_token.IsExpired(_clock))
				{
					return _token.AccessToken;
				}
				if (_refreshTask == null)
				{
					_refreshTask = Task.Run((Func<Task<TokenResponse>>)RefreshTokenAsync);
					_refreshTask.ContinueWith((Func<Task<TokenResponse>, Task>)LogException, TaskContinuationOptions.OnlyOnFaulted);
				}
				if (_token != null && !_token.IsEffectivelyExpired(_clock))
				{
					return _token.AccessToken;
				}
				refreshTask = _refreshTask;
			}
			refreshTask = refreshTask.WithCancellationToken(cancellationToken);
			return (await refreshTask.ConfigureAwait(continueOnCapturedContext: false)).AccessToken;
			async Task LogException(Task task)
			{
				try
				{
					await task.ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception arg)
				{
					_logger.Debug($"An error occured on a background token refresh task.{Environment.NewLine}{arg}");
				}
			}
		}

		private async Task<TokenResponse> RefreshTokenAsync()
		{
			_logger.Debug("Token has expired, trying to get a new one.");
			try
			{
				List<string> errors = null;
				TimeSpan[] refreshTimeouts = RefreshTimeouts;
				for (int i = 0; i < refreshTimeouts.Length; i++)
				{
					TimeSpan timeout = refreshTimeouts[i];
					CancellationToken token = new CancellationTokenSource(timeout).Token;
					try
					{
						if (await _refreshAction(token).ConfigureAwait(continueOnCapturedContext: false))
						{
							_logger.Info("New access token was received successfully");
							return Token;
						}
						List<string> list;
						errors = (list = errors ?? new List<string>());
						list.Add("refresh error");
					}
					catch (OperationCanceledException)
					{
						_logger.Debug("Token refresh time-out after {0} seconds", (int)timeout.TotalSeconds);
						List<string> obj = errors ?? new List<string>();
						List<string> list = obj;
						errors = obj;
						list.Add("timeout");
					}
				}
				throw new InvalidOperationException("The access token has expired and could not be refreshed. Errors: " + string.Join(", ", errors));
			}
			finally
			{
				lock (_lock)
				{
					_refreshTask = null;
				}
			}
		}

		private static T ResultWithUnwrappedExceptions<T>(Task<T> task)
		{
			try
			{
				task.Wait();
			}
			catch (AggregateException ex)
			{
				ExceptionDispatchInfo.Capture(ex.InnerExceptions.FirstOrDefault() ?? ex).Throw();
			}
			return task.Result;
		}
	}
}
