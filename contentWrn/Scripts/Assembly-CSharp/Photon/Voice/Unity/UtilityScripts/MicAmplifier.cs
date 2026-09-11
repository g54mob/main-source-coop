using UnityEngine;

namespace Photon.Voice.Unity.UtilityScripts
{
	[RequireComponent(typeof(Recorder))]
	public class MicAmplifier : VoiceComponent
	{
		private MicrophoneValue m_microphoneValue;

		private void OnEnable()
		{
			if (m_microphoneValue != null)
			{
				m_microphoneValue.Disabled = false;
			}
		}

		private void OnDisable()
		{
			if (m_microphoneValue != null)
			{
				m_microphoneValue.Disabled = true;
			}
		}

		private void PhotonVoiceCreated(PhotonVoiceCreatedParams p)
		{
			if (p.Voice is LocalVoiceAudioShort)
			{
				LocalVoiceAudioShort obj = p.Voice as LocalVoiceAudioShort;
				m_microphoneValue = new MicrophoneValue(p.AudioDesc.SamplingRate);
				obj.AddPostProcessor(m_microphoneValue);
			}
		}
	}
}
