using FMODUnity;
using Photon.Voice.FMOD;
using UnityEngine;

namespace Photon.Voice.Unity.FMOD
{
	[RequireComponent(typeof(Recorder))]
	[AddComponentMenu("Photon Voice/FMOD/FMOD Recorder Setup")]
	public class FMODRecorderSetup : VoiceComponent
	{
		protected override void Awake()
		{
			base.Awake();
			Recorder recorder = GetComponent<Recorder>();
			recorder.SourceType = Recorder.InputSourceType.Factory;
			recorder.InputFactory = delegate
			{
				base.Logger.Log(LogLevel.Info, "Setting recorder's source to FMOD factory with device={0}", recorder.MicrophoneDevice);
				return new AudioInReader<short>(RuntimeManager.CoreSystem, recorder.MicrophoneDevice.IDInt, (int)recorder.SamplingRate, base.Logger);
			};
		}
	}
}
