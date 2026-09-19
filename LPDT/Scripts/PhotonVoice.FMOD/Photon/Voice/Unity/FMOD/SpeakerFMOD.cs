using System;
using FMOD.Studio;
using FMODUnity;
using Photon.Voice.FMOD;
using UnityEngine;

namespace Photon.Voice.Unity.FMOD
{
	[AddComponentMenu("Photon Voice/FMOD/Speaker FMOD")]
	public class SpeakerFMOD : Speaker
	{
		[SerializeField]
		private bool useEvent;

		[SerializeField]
		private EventReference eventReference;

		private EventInstance _instance;

		private bool _initialized;

		public ref EventInstance EventInstance => ref _instance;

		public bool Initialized => _initialized;

		public event Action OnCreateAudioOut;

		protected override void OnDestroy()
		{
			base.OnDestroy();
			this.OnCreateAudioOut = null;
		}

		protected override IAudioOut<float> CreateAudioOut()
		{
			if (useEvent)
			{
				_instance = RuntimeManager.CreateInstance(eventReference);
				if (_instance.isValid())
				{
					RuntimeManager.AttachInstanceToGameObject(_instance, base.transform, GetComponent<Rigidbody>());
					_instance.start();
				}
				_initialized = true;
				this.OnCreateAudioOut?.Invoke();
				return new AudioOutEvent<float>(RuntimeManager.CoreSystem, _instance, playDelayConfig, base.Logger, string.Empty, debugInfo: true);
			}
			return new AudioOut<float>(RuntimeManager.CoreSystem, playDelayConfig, base.Logger, string.Empty, debugInfo: true);
		}
	}
}
