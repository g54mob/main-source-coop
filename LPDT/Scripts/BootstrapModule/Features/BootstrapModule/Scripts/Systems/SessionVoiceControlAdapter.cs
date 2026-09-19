using Features.AudioDevicesModule.Scripts;
using Features.SessionManagementModule.Models;

namespace Features.BootstrapModule.Scripts.Systems
{
	public sealed class SessionVoiceControlAdapter : ISessionVoiceControl
	{
		private readonly MicrophoneModel _microphoneModel;

		public SessionVoiceControlAdapter(MicrophoneModel microphoneModel)
		{
			_microphoneModel = microphoneModel;
		}

		public void RestartRecording()
		{
			_microphoneModel.RestartRecording();
		}
	}
}
