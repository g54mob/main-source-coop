using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Ami.Extension
{
	public class AudioSourceProxy : IDisposable, IAudioSourceProxy
	{
		private AudioSource _source;

		private bool _isVolumeModified;

		private bool _isPitchModified;

		private bool _isTimeModified;

		private bool _isTimeSamplesModified;

		private bool _isOutputAudioMixerGroupModified;

		private bool _isLoopModified;

		private bool _isIgnoreListenerVolumeModified;

		private bool _isPlayOnAwakeModified;

		private bool _isIgnoreListenerPauseModified;

		private bool _isVelocityUpdateModeModified;

		private bool _isPanStereoModified;

		private bool _isSpatialBlendModified;

		private bool _isSpatializeModified;

		private bool _isSpatializePostEffectsModified;

		private bool _isReverbZoneMixModified;

		private bool _isBypassEffectsModified;

		private bool _isBypassListenerEffectsModified;

		private bool _isBypassReverbZonesModified;

		private bool _isDopplerLevelModified;

		private bool _isSpreadModified;

		private bool _isPriorityModified;

		private bool _isMuteModified;

		private bool _isMinDistanceModified;

		private bool _isMaxDistanceModified;

		private bool _isRolloffModeModified;

		private bool _isClipModified;

		public float volume
		{
			get
			{
				return _source.volume;
			}
			set
			{
				_isVolumeModified = true;
				_source.volume = value;
			}
		}

		public float pitch
		{
			get
			{
				return _source.pitch;
			}
			set
			{
				_isPitchModified = true;
				_source.pitch = value;
			}
		}

		public float time
		{
			get
			{
				return _source.time;
			}
			set
			{
				_isTimeModified = true;
				_source.time = value;
			}
		}

		public int timeSamples
		{
			get
			{
				return _source.timeSamples;
			}
			set
			{
				_isTimeSamplesModified = true;
				_source.timeSamples = value;
			}
		}

		public AudioMixerGroup outputAudioMixerGroup
		{
			get
			{
				return _source.outputAudioMixerGroup;
			}
			set
			{
				_isOutputAudioMixerGroupModified = true;
				_source.outputAudioMixerGroup = value;
			}
		}

		public bool loop
		{
			get
			{
				return _source.loop;
			}
			set
			{
				_isLoopModified = true;
				_source.loop = value;
			}
		}

		public bool ignoreListenerVolume
		{
			get
			{
				return _source.ignoreListenerVolume;
			}
			set
			{
				_isIgnoreListenerVolumeModified = true;
				_source.ignoreListenerVolume = value;
			}
		}

		public bool playOnAwake
		{
			get
			{
				return _source.playOnAwake;
			}
			set
			{
				_isPlayOnAwakeModified = true;
				_source.playOnAwake = value;
			}
		}

		public bool ignoreListenerPause
		{
			get
			{
				return _source.ignoreListenerPause;
			}
			set
			{
				_isIgnoreListenerPauseModified = true;
				_source.ignoreListenerPause = value;
			}
		}

		public AudioVelocityUpdateMode velocityUpdateMode
		{
			get
			{
				return _source.velocityUpdateMode;
			}
			set
			{
				_isVelocityUpdateModeModified = true;
				_source.velocityUpdateMode = value;
			}
		}

		public float panStereo
		{
			get
			{
				return _source.panStereo;
			}
			set
			{
				_isPanStereoModified = true;
				_source.panStereo = value;
			}
		}

		public float spatialBlend
		{
			get
			{
				return _source.spatialBlend;
			}
			set
			{
				_isSpatialBlendModified = true;
				_source.spatialBlend = value;
			}
		}

		public bool spatialize
		{
			get
			{
				return _source.spatialize;
			}
			set
			{
				_isSpatializeModified = true;
				_source.spatialize = value;
			}
		}

		public bool spatializePostEffects
		{
			get
			{
				return _source.spatializePostEffects;
			}
			set
			{
				_isSpatializePostEffectsModified = true;
				_source.spatializePostEffects = value;
			}
		}

		public float reverbZoneMix
		{
			get
			{
				return _source.reverbZoneMix;
			}
			set
			{
				_isReverbZoneMixModified = true;
				_source.reverbZoneMix = value;
			}
		}

		public bool bypassEffects
		{
			get
			{
				return _source.bypassEffects;
			}
			set
			{
				_isBypassEffectsModified = true;
				_source.bypassEffects = value;
			}
		}

		public bool bypassListenerEffects
		{
			get
			{
				return _source.bypassListenerEffects;
			}
			set
			{
				_isBypassListenerEffectsModified = true;
				_source.bypassListenerEffects = value;
			}
		}

		public bool bypassReverbZones
		{
			get
			{
				return _source.bypassReverbZones;
			}
			set
			{
				_isBypassReverbZonesModified = true;
				_source.bypassReverbZones = value;
			}
		}

		public float dopplerLevel
		{
			get
			{
				return _source.dopplerLevel;
			}
			set
			{
				_isDopplerLevelModified = true;
				_source.dopplerLevel = value;
			}
		}

		public float spread
		{
			get
			{
				return _source.spread;
			}
			set
			{
				_isSpreadModified = true;
				_source.spread = value;
			}
		}

		public int priority
		{
			get
			{
				return _source.priority;
			}
			set
			{
				_isPriorityModified = true;
				_source.priority = value;
			}
		}

		public bool mute
		{
			get
			{
				return _source.mute;
			}
			set
			{
				_isMuteModified = true;
				_source.mute = value;
			}
		}

		public float minDistance
		{
			get
			{
				return _source.minDistance;
			}
			set
			{
				_isMinDistanceModified = true;
				_source.minDistance = value;
			}
		}

		public float maxDistance
		{
			get
			{
				return _source.maxDistance;
			}
			set
			{
				_isMaxDistanceModified = true;
				_source.maxDistance = value;
			}
		}

		public AudioRolloffMode rolloffMode
		{
			get
			{
				return _source.rolloffMode;
			}
			set
			{
				_isRolloffModeModified = true;
				_source.rolloffMode = value;
			}
		}

		public AudioClip clip
		{
			get
			{
				return _source.clip;
			}
			set
			{
				_isClipModified = true;
				_source.clip = value;
			}
		}

		public AnimationCurve GetCustomCurve(AudioSourceCurveType type)
		{
			return _source.GetCustomCurve(type);
		}

		public void SetCustomCurve(AudioSourceCurveType type, AnimationCurve curve)
		{
			_source.SetCustomCurve(type, curve);
		}

		public bool GetAmbisonicDecoderFloat(int index, out float value)
		{
			return _source.GetAmbisonicDecoderFloat(index, out value);
		}

		public bool SetAmbisonicDecoderFloat(int index, float value)
		{
			return _source.SetAmbisonicDecoderFloat(index, value);
		}

		public bool GetSpatializerFloat(int index, out float value)
		{
			return _source.GetSpatializerFloat(index, out value);
		}

		public bool SetSpatializerFloat(int index, float value)
		{
			return _source.SetSpatializerFloat(index, value);
		}

		public AudioSourceProxy(AudioSource source)
		{
			_source = source;
		}

		public void Dispose()
		{
			if (_isVolumeModified)
			{
				_source.volume = 1f;
				_isVolumeModified = false;
			}
			if (_isPitchModified)
			{
				_source.pitch = 1f;
				_isPitchModified = false;
			}
			if (_isTimeModified)
			{
				_source.time = 0f;
				_isTimeModified = false;
			}
			if (_isTimeSamplesModified)
			{
				_source.timeSamples = 0;
				_isTimeSamplesModified = false;
			}
			if (_isOutputAudioMixerGroupModified)
			{
				_source.outputAudioMixerGroup = null;
				_isOutputAudioMixerGroupModified = false;
			}
			if (_isLoopModified)
			{
				_source.loop = false;
				_isLoopModified = false;
			}
			if (_isIgnoreListenerVolumeModified)
			{
				_source.ignoreListenerVolume = false;
				_isIgnoreListenerVolumeModified = false;
			}
			if (_isPlayOnAwakeModified)
			{
				_source.playOnAwake = true;
				_isPlayOnAwakeModified = false;
			}
			if (_isIgnoreListenerPauseModified)
			{
				_source.ignoreListenerPause = false;
				_isIgnoreListenerPauseModified = false;
			}
			if (_isVelocityUpdateModeModified)
			{
				_source.velocityUpdateMode = AudioVelocityUpdateMode.Auto;
				_isVelocityUpdateModeModified = false;
			}
			if (_isPanStereoModified)
			{
				_source.panStereo = 0f;
				_isPanStereoModified = false;
			}
			if (_isSpatialBlendModified)
			{
				_source.spatialBlend = 0f;
				_isSpatialBlendModified = false;
			}
			if (_isSpatializeModified)
			{
				_source.spatialize = false;
				_isSpatializeModified = false;
			}
			if (_isSpatializePostEffectsModified)
			{
				_source.spatializePostEffects = false;
				_isSpatializePostEffectsModified = false;
			}
			if (_isReverbZoneMixModified)
			{
				_source.reverbZoneMix = 1f;
				_isReverbZoneMixModified = false;
			}
			if (_isBypassEffectsModified)
			{
				_source.bypassEffects = false;
				_isBypassEffectsModified = false;
			}
			if (_isBypassListenerEffectsModified)
			{
				_source.bypassListenerEffects = false;
				_isBypassListenerEffectsModified = false;
			}
			if (_isBypassReverbZonesModified)
			{
				_source.bypassReverbZones = false;
				_isBypassReverbZonesModified = false;
			}
			if (_isDopplerLevelModified)
			{
				_source.dopplerLevel = 1f;
				_isDopplerLevelModified = false;
			}
			if (_isSpreadModified)
			{
				_source.spread = 0f;
				_isSpreadModified = false;
			}
			if (_isPriorityModified)
			{
				_source.priority = 128;
				_isPriorityModified = false;
			}
			if (_isMuteModified)
			{
				_source.mute = false;
				_isMuteModified = false;
			}
			if (_isMinDistanceModified)
			{
				_source.minDistance = 1f;
				_isMinDistanceModified = false;
			}
			if (_isMaxDistanceModified)
			{
				_source.maxDistance = 500f;
				_isMaxDistanceModified = false;
			}
			if (_isRolloffModeModified)
			{
				_source.rolloffMode = AudioRolloffMode.Logarithmic;
				_isRolloffModeModified = false;
			}
			if (_isClipModified)
			{
				_source.clip = null;
				_isClipModified = false;
			}
		}
	}
}
