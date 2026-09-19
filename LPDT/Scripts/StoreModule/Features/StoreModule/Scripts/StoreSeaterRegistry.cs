using Cysharp.Threading.Tasks;
using Features.GameCycle.Scripts.SessionCleanup;
using UnityEngine;

namespace Features.StoreModule.Scripts
{
	public sealed class StoreSeaterRegistry : ISessionCleanup
	{
		private const float SEATER_REGISTRATION_WAIT_SECONDS = 5f;

		private IStoreSeater _seater;

		public void Register(IStoreSeater seater)
		{
			_seater = seater;
		}

		public void Unregister(IStoreSeater seater)
		{
			if (_seater == seater)
			{
				_seater = null;
			}
		}

		public async UniTask ReleaseGrabsAsync()
		{
			(await WaitForSeaterAsync()).ReleaseGrabs();
		}

		public async UniTask EnsureBodyAuthorityAsync()
		{
			await (await WaitForSeaterAsync()).EnsureBodyAuthorityAsync();
		}

		public async UniTask ResetRagdollAsync()
		{
			(await WaitForSeaterAsync()).ResetRagdoll();
		}

		public async UniTask SeatAtStoreSeatAsync()
		{
			await (await WaitForSeaterAsync()).SeatAtStoreSeatAsync();
		}

		public void ClearSeatRequest()
		{
			_seater?.CancelSeating();
		}

		public void RestorePreStoreState()
		{
			_seater?.RestorePreStoreState();
		}

		public void Cleanup()
		{
			_seater?.CancelSeating();
		}

		private async UniTask<IStoreSeater> WaitForSeaterAsync()
		{
			if (_seater != null)
			{
				return _seater;
			}
			Debug.Log("[ShopEnterTrace] WaitForSeater: no seater registered, holding");
			float deadline = Time.unscaledTime + 5f;
			while (Time.unscaledTime < deadline)
			{
				await UniTask.Yield(PlayerLoopTiming.Update);
				if (_seater != null)
				{
					Debug.Log("[ShopEnterTrace] WaitForSeater: seater registered");
					return _seater;
				}
			}
			throw new StoreSeatingStepFailedException($"Shop entry: the store seater did not register within {5f}s — " + "the scene's store table is absent, so no shop-entry step can enforce anything.");
		}
	}
}
