using UnityEngine;

namespace Photon.Voice.Unity.UtilityScripts
{
	[RequireComponent(typeof(Recorder))]
	public class MicAmplifier : VoiceComponent
	{
		[SerializeField]
		private float amplificationFactor = 1f;

		private MicAmplifierFloat floatProcessor;

		private MicAmplifierShort shortProcessor;

		public float AmplificationFactor
		{
			get
			{
				return amplificationFactor;
			}
			set
			{
				if (!amplificationFactor.Equals(value))
				{
					amplificationFactor = value;
					if (floatProcessor != null)
					{
						floatProcessor.AmplificationFactor = amplificationFactor;
					}
					if (shortProcessor != null)
					{
						shortProcessor.AmplificationFactor = amplificationFactor;
					}
				}
			}
		}

		private void OnEnable()
		{
			if (floatProcessor != null)
			{
				floatProcessor.Disabled = false;
			}
			if (shortProcessor != null)
			{
				shortProcessor.Disabled = false;
			}
		}

		private void OnDisable()
		{
			if (floatProcessor != null)
			{
				floatProcessor.Disabled = true;
			}
			if (shortProcessor != null)
			{
				shortProcessor.Disabled = true;
			}
		}

		private void PhotonVoiceCreated(PhotonVoiceCreatedParams p)
		{
			if (p.Voice is LocalVoiceAudioFloat)
			{
				LocalVoiceAudioFloat obj = p.Voice as LocalVoiceAudioFloat;
				floatProcessor = new MicAmplifierFloat(AmplificationFactor);
				obj.AddPostProcessor(floatProcessor);
			}
			else if (p.Voice is LocalVoiceAudioShort)
			{
				LocalVoiceAudioShort obj2 = p.Voice as LocalVoiceAudioShort;
				shortProcessor = new MicAmplifierShort(AmplificationFactor);
				obj2.AddPostProcessor(shortProcessor);
			}
			else
			{
				base.Logger.Log(LogLevel.Error, "LocalVoice object has unexpected value/type: {0}", (p.Voice == null) ? "null" : p.Voice.GetType().ToString());
			}
		}
	}
}
