using Features.AudioDevicesModule.Scripts;
using Photon.Voice.Unity;
using UnityEngine;
using Zenject;

namespace Features.VoiceSpeakersModule.Scripts
{
	public class PlayerRecorderAutoRegister : MonoBehaviour
	{
		[SerializeField]
		private Recorder _recorder;

		private MicrophoneModel _microphoneModel;

		[Inject]
		public void InjectDependencies(MicrophoneModel microphoneModel)
		{
			_microphoneModel = microphoneModel;
		}

		private void Start()
		{
			RegisterIfNeeded();
		}

		public void RegisterIfNeeded()
		{
			if (_recorder != null)
			{
				_microphoneModel.RegisterRecorder(_recorder);
			}
		}

		private void OnDestroy()
		{
			if (_microphoneModel?.PlayerRecorder == _recorder)
			{
				_microphoneModel.UnRegisterRecorder();
			}
		}
	}
}
