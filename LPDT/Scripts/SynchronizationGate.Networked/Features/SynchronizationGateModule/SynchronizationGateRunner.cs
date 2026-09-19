using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;

namespace Features.SynchronizationGateModule
{
	public sealed class SynchronizationGateRunner : IDisposable
	{
		private const string HOST_PREFAB_NAME = "SynchronizationGateObject";

		private static NetworkObject _hostPrefab;

		private readonly NetworkRunner _runner;

		private SynchronizationGateNetworkObject _host;

		private SynchronizationGateParticipant _participant;

		private bool _hostSpawned;

		public bool HasHost => HasLiveHost();

		public SynchronizationGateRunner(NetworkRunner runner)
		{
			_runner = runner;
		}

		public void Tick()
		{
			if (_runner.IsRunning)
			{
				EnsureHost();
				EnsureParticipant();
			}
		}

		public void OpenAt(int lane, int visit, int windowTicks)
		{
			if (HasLiveHost())
			{
				_host.SetWindowTicks(lane, windowTicks);
				_host.SetVisit(lane, visit);
			}
		}

		public bool IsPassed(int lane)
		{
			if (!HasLiveHost())
			{
				return false;
			}
			return _host.IsPassed(lane);
		}

		public void Reconcile(int lane, bool isReady)
		{
			if (HasLiveHost())
			{
				_participant?.Reconcile(lane, isReady);
			}
		}

		public bool IsStepOpen(int step, int epoch)
		{
			if (HasLiveHost())
			{
				return _host.IsStepOpen(step, epoch);
			}
			return false;
		}

		public void RequestStepOpen(int step, int epoch)
		{
			_host?.RequestStepOpen(step, epoch);
		}

		public async UniTask PassAuthorityStepAsync(int step, int epoch, Func<UniTask> work, CancellationToken cancellationToken = default(CancellationToken))
		{
			await UniTask.WaitUntil(() => HasHost, PlayerLoopTiming.Update, cancellationToken);
			bool executed = false;
			while (!IsStepOpen(step, epoch))
			{
				cancellationToken.ThrowIfCancellationRequested();
				if (!executed && _runner.IsSharedModeMasterClient)
				{
					await work();
					RequestStepOpen(step, epoch);
					executed = true;
				}
				await UniTask.Yield(cancellationToken);
			}
		}

		public void Dispose()
		{
			_participant?.Dispose();
			_participant = null;
		}

		private void EnsureHost()
		{
			if (!HasLiveHost())
			{
				if (_participant != null)
				{
					_participant.Dispose();
					_participant = null;
				}
				_host = FindHostForRunner(_runner);
				if (!(_host != null) && _runner.IsSharedModeMasterClient && !_hostSpawned)
				{
					_hostSpawned = true;
					SpawnHostAsync();
				}
			}
		}

		private async void SpawnHostAsync()
		{
			try
			{
				NetworkObject networkObject = await _runner.SpawnAsync(ResolveHostPrefab());
				if (networkObject == null || !networkObject.IsValid)
				{
					_hostSpawned = false;
					return;
				}
				SynchronizationGateNetworkObject component = networkObject.GetComponent<SynchronizationGateNetworkObject>();
				SynchronizationGateNetworkObject synchronizationGateNetworkObject = FindHostForRunner(_runner);
				if (synchronizationGateNetworkObject != null && synchronizationGateNetworkObject != component)
				{
					DespawnOrphan(networkObject);
					_host = synchronizationGateNetworkObject;
				}
				else
				{
					_host = component;
				}
			}
			catch (Exception ex)
			{
				_hostSpawned = false;
				if (_runner.IsRunning)
				{
					Debug.LogWarning("[SynchronizationGate] Async host spawn failed: " + ex.Message);
				}
			}
		}

		private void DespawnOrphan(NetworkObject spawned)
		{
			if (!(spawned == null) && spawned.IsValid && _runner.IsRunning && spawned.HasStateAuthority)
			{
				Debug.LogWarning("[SynchronizationGate] Despawning orphaned gate host — another master spawned one inside our in-flight window.");
				_runner.Despawn(spawned);
			}
		}

		private void EnsureParticipant()
		{
			if (_participant == null && HasLiveHost())
			{
				string ownerId = _runner.LocalPlayer.PlayerId.ToString();
				_participant = new SynchronizationGateParticipant(_runner, _host, ownerId);
			}
		}

		private bool HasLiveHost()
		{
			if (_host != null && _host.Object != null && _host.Object.IsValid)
			{
				return _host.IsReady;
			}
			return false;
		}

		private static SynchronizationGateNetworkObject FindHostForRunner(NetworkRunner runner)
		{
			SynchronizationGateNetworkObject[] array = UnityEngine.Object.FindObjectsByType<SynchronizationGateNetworkObject>(FindObjectsSortMode.None);
			foreach (SynchronizationGateNetworkObject synchronizationGateNetworkObject in array)
			{
				if (synchronizationGateNetworkObject.Runner == runner)
				{
					return synchronizationGateNetworkObject;
				}
			}
			return null;
		}

		private static NetworkObject ResolveHostPrefab()
		{
			if (_hostPrefab == null)
			{
				_hostPrefab = Resources.Load<NetworkObject>("SynchronizationGateObject");
			}
			return _hostPrefab;
		}
	}
}
