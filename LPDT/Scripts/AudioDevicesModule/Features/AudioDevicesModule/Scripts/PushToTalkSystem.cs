using System;
using Features.InputModule.Scripts.Generated;
using RSG.Muffin.InputSubmodule.InputModule.Core.Scripts;
using Zenject;

namespace Features.AudioDevicesModule.Scripts
{
	public class PushToTalkSystem : IInitializable, IDisposable
	{
		private readonly MicrophoneModel _microphoneModel;

		private readonly IInputService _inputService;

		public PushToTalkSystem(MicrophoneModel microphoneModel, IInputService inputService)
		{
			_microphoneModel = microphoneModel;
			_inputService = inputService;
		}

		public void Initialize()
		{
			_microphoneModel.OnPushToTalkEnabledChanged += OnPushToTalkModeChanged;
			_microphoneModel.OnMicrophoneEnabledChanged += RefreshTransmitState;
			_microphoneModel.OnRecorderRegistered += RefreshTransmitState;
			InputDefaultActions pushToTalk = _inputService.PushToTalk;
			pushToTalk.Started = (Action)Delegate.Combine(pushToTalk.Started, new Action(OnPushToTalkPressed));
			InputDefaultActions pushToTalk2 = _inputService.PushToTalk;
			pushToTalk2.Canceled = (Action)Delegate.Combine(pushToTalk2.Canceled, new Action(OnPushToTalkReleased));
			RefreshTransmitState();
		}

		public void Dispose()
		{
			_microphoneModel.OnPushToTalkEnabledChanged -= OnPushToTalkModeChanged;
			_microphoneModel.OnMicrophoneEnabledChanged -= RefreshTransmitState;
			_microphoneModel.OnRecorderRegistered -= RefreshTransmitState;
			InputDefaultActions pushToTalk = _inputService.PushToTalk;
			pushToTalk.Started = (Action)Delegate.Remove(pushToTalk.Started, new Action(OnPushToTalkPressed));
			InputDefaultActions pushToTalk2 = _inputService.PushToTalk;
			pushToTalk2.Canceled = (Action)Delegate.Remove(pushToTalk2.Canceled, new Action(OnPushToTalkReleased));
		}

		private void OnPushToTalkModeChanged()
		{
			RefreshTransmitState();
		}

		private void OnPushToTalkPressed()
		{
			if (_microphoneModel.IsPushToTalkEnabled)
			{
				SetTransmit(enabled: true);
			}
		}

		private void OnPushToTalkReleased()
		{
			if (_microphoneModel.IsPushToTalkEnabled)
			{
				SetTransmit(enabled: false);
			}
		}

		private void RefreshTransmitState()
		{
			if (!(_microphoneModel.PlayerRecorder == null))
			{
				if (!_microphoneModel.IsPushToTalkEnabled)
				{
					SetTransmit(enabled: true);
				}
				else
				{
					SetTransmit(enabled: false);
				}
			}
		}

		private void SetTransmit(bool enabled)
		{
			if (!(_microphoneModel.PlayerRecorder == null))
			{
				_microphoneModel.PlayerRecorder.TransmitEnabled = enabled;
			}
		}
	}
}
