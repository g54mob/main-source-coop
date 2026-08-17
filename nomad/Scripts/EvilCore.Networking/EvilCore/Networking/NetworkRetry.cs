using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using EvilCore.EvilPack.EvilLogger;
using UnityEngine;

namespace EvilCore.Networking
{
	public static class NetworkRetry
	{
		public readonly struct Outcome<T>
		{
			public readonly bool Success;

			public readonly T Result;

			public readonly int Attempts;

			public Outcome(bool success, T result, int attempts)
			{
				Success = success;
				Result = result;
				Attempts = attempts;
			}
		}

		public static async UniTask<Outcome<T>> RunAsync<T>(Func<UniTask<T>> attempt, Func<T, bool> isSuccess, Func<T, bool> isRetryable, int maxAttempts = 3, int baseDelayMs = 600, int maxDelayMs = 5000, float multiplier = 2f, float jitter = 0.3f, CancellationToken ct = default(CancellationToken))
		{
			float delay = baseDelayMs;
			T last = default(T);
			for (int i = 1; i <= maxAttempts; i++)
			{
				ct.ThrowIfCancellationRequested();
				bool threw = false;
				try
				{
					last = await attempt();
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception)
				{
					threw = true;
				}
				if (!threw && isSuccess(last))
				{
					return new Outcome<T>(success: true, last, i);
				}
				if ((!threw && !isRetryable(last)) || i >= maxAttempts)
				{
					return new Outcome<T>(success: false, last, i);
				}
				await DelayWithJitterAsync(delay, jitter, ct);
				delay = Mathf.Min(delay * multiplier, maxDelayMs);
			}
			return new Outcome<T>(success: false, last, maxAttempts);
		}

		public static async UniTask<bool> RunUntilNoThrowAsync(Func<UniTask> attempt, int maxAttempts = 3, int baseDelayMs = 600, int maxDelayMs = 5000, float multiplier = 2f, float jitter = 0.3f, Func<Exception, bool> isRetryable = null, CancellationToken ct = default(CancellationToken))
		{
			float delay = baseDelayMs;
			for (int i = 1; i <= maxAttempts; i++)
			{
				ct.ThrowIfCancellationRequested();
				try
				{
					await attempt();
					return true;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (!(isRetryable?.Invoke(ex2) ?? true) || i >= maxAttempts)
					{
						EvilLogger.LogError($"[NetworkRetry] Failed after {i} attempt(s): {ex2.Message}", LogCategory.Network, "RunUntilNoThrowAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\_Core\\NetworkRetry.cs", 124);
						return false;
					}
					await DelayWithJitterAsync(delay, jitter, ct);
					delay = Mathf.Min(delay * multiplier, maxDelayMs);
				}
			}
			return false;
		}

		private static UniTask DelayWithJitterAsync(float delayMs, float jitter, CancellationToken ct)
		{
			float minInclusive = delayMs * (1f - jitter);
			float maxInclusive = delayMs * (1f + jitter);
			return UniTask.Delay(Mathf.Max(1, Mathf.RoundToInt(UnityEngine.Random.Range(minInclusive, maxInclusive))), ignoreTimeScale: false, PlayerLoopTiming.Update, ct);
		}
	}
}
