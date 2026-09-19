using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Features.MultiplayerSessionServices.Scripts;
using Features.SessionManagementModule.Models;
using Features.SynchronizationGateModule;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.SessionManagementModule.Gate
{
	public sealed class SessionGate : ISynchronizationGate, ISessionAuthorityGate, ITickable, IDisposable
	{
		private const double STEP_STUCK_WARN_SECONDS = 8.0;

		private const double AUTHORITY_WORK_SLOW_SECONDS = 4.0;

		private readonly MultiplayerModel _multiplayerModel;

		private SynchronizationGateRunner _synchronizationGateRunner;

		private SessionAuthorityLane _inFlightLane;

		private int _inFlightEpoch = -1;

		private double _inFlightEnteredAt;

		private bool _inFlightIsAuthority;

		private bool _hasWarnedInFlightStep;

		public SessionAuthorityLane InFlightLane => _inFlightLane;

		public int InFlightEpoch => _inFlightEpoch;

		public SessionGate(MultiplayerModel multiplayerModel)
		{
			_multiplayerModel = multiplayerModel;
		}

		public void Tick()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner.IsRunning)
			{
				if (_synchronizationGateRunner == null)
				{
					_synchronizationGateRunner = new SynchronizationGateRunner(networkRunner);
				}
				_synchronizationGateRunner.Tick();
				WarnIfStepStuck(networkRunner);
			}
		}

		public void OpenOnEntry(SynchronizationGateKey key, int epoch, int windowTicks)
		{
			if (key != SynchronizationGateKey.None)
			{
				_synchronizationGateRunner?.OpenAt((int)key, epoch, windowTicks);
			}
		}

		public bool IsPassed(SynchronizationGateKey key)
		{
			if (key == SynchronizationGateKey.None)
			{
				return true;
			}
			if (_synchronizationGateRunner == null)
			{
				return false;
			}
			return _synchronizationGateRunner.IsPassed((int)key);
		}

		public void Reconcile(SynchronizationGateKey key, bool isReady)
		{
			int lane = (int)((key == SynchronizationGateKey.None) ? ((SynchronizationGateKey)(-1)) : key);
			_synchronizationGateRunner?.Reconcile(lane, isReady);
		}

		public async UniTask WaitUntilPassedAsync(SynchronizationGateKey key)
		{
			if (key == SynchronizationGateKey.None)
			{
				Reconcile(key, isReady: false);
				return;
			}
			double arrivedAt = Time.realtimeSinceStartupAsDouble;
			Debug.Log($"[GateTrace] -> arrive cohort={key} (waiting for cohort)");
			while (!IsPassed(key))
			{
				Reconcile(key, isReady: true);
				await UniTask.Yield();
			}
			Reconcile(key, isReady: false);
			Debug.Log($"[GateTrace] <- pass   cohort={key} waited {Time.realtimeSinceStartupAsDouble - arrivedAt:F2}s");
		}

		public bool IsStepOpen(int step, int epoch)
		{
			if (_synchronizationGateRunner != null)
			{
				return _synchronizationGateRunner.IsStepOpen(step, epoch);
			}
			return false;
		}

		public async UniTask PassAsync(int step, int epoch, Func<UniTask> work, CancellationToken cancellationToken = default(CancellationToken))
		{
			await UniTask.WaitUntil(() => _synchronizationGateRunner != null, PlayerLoopTiming.Update, cancellationToken);
			EnterStep(step, epoch);
			try
			{
				await _synchronizationGateRunner.PassAuthorityStepAsync(step, epoch, () => RunTimedAuthorityWorkAsync(step, epoch, work), cancellationToken);
			}
			finally
			{
				ExitStep();
			}
		}

		private async UniTask RunTimedAuthorityWorkAsync(int step, int epoch, Func<UniTask> work)
		{
			double startedAt = Time.realtimeSinceStartupAsDouble;
			await work();
			double num = Time.realtimeSinceStartupAsDouble - startedAt;
			if (!(num < 4.0))
			{
				Debug.LogWarning($"[AuthorityWork] lane={(SessionAuthorityLane)step} epoch={epoch} authority work ran {num:F1}s " + $"(threshold {4.0:F0}s) — excessive step duration on the master.");
			}
		}

		public void Dispose()
		{
			_synchronizationGateRunner?.Dispose();
			_synchronizationGateRunner = null;
		}

		private void EnterStep(int step, int epoch)
		{
			_inFlightLane = (SessionAuthorityLane)step;
			_inFlightEpoch = epoch;
			_inFlightEnteredAt = Time.realtimeSinceStartupAsDouble;
			_inFlightIsAuthority = _multiplayerModel.NetworkRunner.IsSharedModeMasterClient;
			_hasWarnedInFlightStep = false;
			Debug.Log($"[GateTrace] -> arrive lane={_inFlightLane} epoch={epoch} (authority={_inFlightIsAuthority})");
		}

		private void ExitStep()
		{
			double num = Time.realtimeSinceStartupAsDouble - _inFlightEnteredAt;
			Debug.Log($"[GateTrace] <- pass   lane={_inFlightLane} epoch={_inFlightEpoch} held {num:F2}s " + $"(authority={_inFlightIsAuthority})");
			_inFlightLane = SessionAuthorityLane.None;
			_inFlightEpoch = -1;
			_hasWarnedInFlightStep = false;
		}

		private void WarnIfStepStuck(NetworkRunner runner)
		{
			if (_inFlightLane != SessionAuthorityLane.None && !_hasWarnedInFlightStep)
			{
				double num = Time.realtimeSinceStartupAsDouble - _inFlightEnteredAt;
				if (!(num < 8.0))
				{
					_hasWarnedInFlightStep = true;
					Debug.LogWarning($"[AuthorityStep] held at lane={_inFlightLane} epoch={_inFlightEpoch} for {num:F1}s " + $"(isAuthority={runner.IsSharedModeMasterClient}) — the authority is stuck or disconnected at this step.");
				}
			}
		}
	}
}
