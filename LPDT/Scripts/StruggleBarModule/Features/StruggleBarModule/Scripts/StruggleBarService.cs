using Features.MultiplayerSessionServices.Scripts;
using UnityEngine;

namespace Features.StruggleBarModule.Scripts
{
	public class StruggleBarService : IStruggleBarService
	{
		private readonly StruggleBarModel _struggleBarModel;

		private readonly StruggleBarConfiguration _struggleBarConfiguration;

		private readonly StruggleBarCompletedNetworkEvent _struggleBarCompletedNetworkEvent;

		private readonly StruggleBarFailedNetworkEvent _struggleBarFailedNetworkEvent;

		private readonly MultiplayerModel _multiplayerModel;

		private float _activeDrainPerSecond;

		private float _activeBoostPerPress;

		public bool IsActive => _struggleBarModel.IsActive;

		public StruggleBarService(StruggleBarModel struggleBarModel, StruggleBarConfiguration struggleBarConfiguration, StruggleBarCompletedNetworkEvent struggleBarCompletedNetworkEvent, StruggleBarFailedNetworkEvent struggleBarFailedNetworkEvent, MultiplayerModel multiplayerModel)
		{
			_struggleBarModel = struggleBarModel;
			_struggleBarConfiguration = struggleBarConfiguration;
			_struggleBarCompletedNetworkEvent = struggleBarCompletedNetworkEvent;
			_struggleBarFailedNetworkEvent = struggleBarFailedNetworkEvent;
			_multiplayerModel = multiplayerModel;
		}

		public void Start()
		{
			Start(_struggleBarConfiguration.DrainPerSecond, _struggleBarConfiguration.BoostPerPress, _struggleBarConfiguration.StartNormalized);
		}

		public void Start(float drainPerSecond, float boostPerPress, float startNormalized)
		{
			_activeDrainPerSecond = Mathf.Max(0f, drainPerSecond);
			_activeBoostPerPress = Mathf.Max(0f, boostPerPress);
			_struggleBarModel.Start(Mathf.Clamp01(startNormalized));
		}

		public void Cancel()
		{
			_struggleBarModel.Stop();
		}

		public void TickDrain(float deltaTime)
		{
			if (_struggleBarModel.IsActive)
			{
				float num = _struggleBarModel.CurrentFill - _activeDrainPerSecond * deltaTime;
				if (num <= 0f)
				{
					FailStruggle();
				}
				else
				{
					_struggleBarModel.SetFill(num);
				}
			}
		}

		public void ApplyBoost()
		{
			if (_struggleBarModel.IsActive)
			{
				float fill = _struggleBarModel.CurrentFill + _activeBoostPerPress;
				_struggleBarModel.SetFill(fill);
				_struggleBarModel.NotifyBoostApplied();
				if (!(_struggleBarModel.CurrentFill < 1f))
				{
					CompleteSuccessfully();
				}
			}
		}

		private void CompleteSuccessfully()
		{
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			_struggleBarModel.Stop();
			_struggleBarCompletedNetworkEvent.SendEvent(playerId);
		}

		private void FailStruggle()
		{
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			_struggleBarModel.Stop();
			_struggleBarFailedNetworkEvent.SendEvent(playerId);
		}
	}
}
