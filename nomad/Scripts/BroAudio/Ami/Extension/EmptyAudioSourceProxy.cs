using UnityEngine;
using UnityEngine.Audio;

namespace Ami.Extension
{
	public class EmptyAudioSourceProxy : IAudioSourceProxy
	{
		public float volume
		{
			get
			{
				return 1f;
			}
			set
			{
			}
		}

		public float pitch
		{
			get
			{
				return 1f;
			}
			set
			{
			}
		}

		public float time
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public int timeSamples
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public AudioMixerGroup outputAudioMixerGroup
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public bool loop
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public bool ignoreListenerVolume
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public bool playOnAwake
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		public bool ignoreListenerPause
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public AudioVelocityUpdateMode velocityUpdateMode
		{
			get
			{
				return AudioVelocityUpdateMode.Auto;
			}
			set
			{
			}
		}

		public float panStereo
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public float spatialBlend
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public bool spatialize
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public bool spatializePostEffects
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public float reverbZoneMix
		{
			get
			{
				return 1f;
			}
			set
			{
			}
		}

		public bool bypassEffects
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public bool bypassListenerEffects
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public bool bypassReverbZones
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public float dopplerLevel
		{
			get
			{
				return 1f;
			}
			set
			{
			}
		}

		public float spread
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public int priority
		{
			get
			{
				return 128;
			}
			set
			{
			}
		}

		public bool mute
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public float minDistance
		{
			get
			{
				return 1f;
			}
			set
			{
			}
		}

		public float maxDistance
		{
			get
			{
				return 500f;
			}
			set
			{
			}
		}

		public AudioRolloffMode rolloffMode
		{
			get
			{
				return AudioRolloffMode.Logarithmic;
			}
			set
			{
			}
		}

		public AudioClip clip
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public AnimationCurve GetCustomCurve(AudioSourceCurveType type)
		{
			return null;
		}

		public void SetCustomCurve(AudioSourceCurveType type, AnimationCurve curve)
		{
		}

		public bool GetAmbisonicDecoderFloat(int index, out float value)
		{
			value = 0f;
			return false;
		}

		public bool SetAmbisonicDecoderFloat(int index, float value)
		{
			return false;
		}

		public bool GetSpatializerFloat(int index, out float value)
		{
			value = 0f;
			return false;
		}

		public bool SetSpatializerFloat(int index, float value)
		{
			return false;
		}
	}
}
