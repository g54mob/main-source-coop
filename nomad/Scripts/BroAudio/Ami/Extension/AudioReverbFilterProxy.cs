using UnityEngine;

namespace Ami.Extension
{
	public class AudioReverbFilterProxy : IAudioEffectModifier, IAudioReverbFilterProxy
	{
		private AudioReverbFilter _source;

		private bool _isReverbPresetModified;

		private bool _isDryLevelModified;

		private bool _isRoomModified;

		private bool _isRoomHFModified;

		private bool _isDecayTimeModified;

		private bool _isDecayHFRatioModified;

		private bool _isReflectionsLevelModified;

		private bool _isReflectionsDelayModified;

		private bool _isReverbLevelModified;

		private bool _isReverbDelayModified;

		private bool _isDiffusionModified;

		private bool _isDensityModified;

		private bool _isHfReferenceModified;

		private bool _isRoomLFModified;

		private bool _isLfReferenceModified;

		private bool _isEnabledModified;

		public AudioReverbPreset reverbPreset
		{
			get
			{
				return _source.reverbPreset;
			}
			set
			{
				_isReverbPresetModified = true;
				_source.reverbPreset = value;
			}
		}

		public float dryLevel
		{
			get
			{
				return _source.dryLevel;
			}
			set
			{
				_isDryLevelModified = true;
				_source.dryLevel = value;
			}
		}

		public float room
		{
			get
			{
				return _source.room;
			}
			set
			{
				_isRoomModified = true;
				_source.room = value;
			}
		}

		public float roomHF
		{
			get
			{
				return _source.roomHF;
			}
			set
			{
				_isRoomHFModified = true;
				_source.roomHF = value;
			}
		}

		public float decayTime
		{
			get
			{
				return _source.decayTime;
			}
			set
			{
				_isDecayTimeModified = true;
				_source.decayTime = value;
			}
		}

		public float decayHFRatio
		{
			get
			{
				return _source.decayHFRatio;
			}
			set
			{
				_isDecayHFRatioModified = true;
				_source.decayHFRatio = value;
			}
		}

		public float reflectionsLevel
		{
			get
			{
				return _source.reflectionsLevel;
			}
			set
			{
				_isReflectionsLevelModified = true;
				_source.reflectionsLevel = value;
			}
		}

		public float reflectionsDelay
		{
			get
			{
				return _source.reflectionsDelay;
			}
			set
			{
				_isReflectionsDelayModified = true;
				_source.reflectionsDelay = value;
			}
		}

		public float reverbLevel
		{
			get
			{
				return _source.reverbLevel;
			}
			set
			{
				_isReverbLevelModified = true;
				_source.reverbLevel = value;
			}
		}

		public float reverbDelay
		{
			get
			{
				return _source.reverbDelay;
			}
			set
			{
				_isReverbDelayModified = true;
				_source.reverbDelay = value;
			}
		}

		public float diffusion
		{
			get
			{
				return _source.diffusion;
			}
			set
			{
				_isDiffusionModified = true;
				_source.diffusion = value;
			}
		}

		public float density
		{
			get
			{
				return _source.density;
			}
			set
			{
				_isDensityModified = true;
				_source.density = value;
			}
		}

		public float hfReference
		{
			get
			{
				return _source.hfReference;
			}
			set
			{
				_isHfReferenceModified = true;
				_source.hfReference = value;
			}
		}

		public float roomLF
		{
			get
			{
				return _source.roomLF;
			}
			set
			{
				_isRoomLFModified = true;
				_source.roomLF = value;
			}
		}

		public float lfReference
		{
			get
			{
				return _source.lfReference;
			}
			set
			{
				_isLfReferenceModified = true;
				_source.lfReference = value;
			}
		}

		public bool enabled
		{
			get
			{
				return _source.enabled;
			}
			set
			{
				_isEnabledModified = true;
				_source.enabled = value;
			}
		}

		public AudioReverbFilterProxy(AudioReverbFilter source)
		{
			_source = source;
		}

		public void TransferValueTo<T>(T target) where T : Behaviour
		{
			if (!(_source == null) && target is AudioReverbFilter audioReverbFilter)
			{
				if (_isReverbPresetModified)
				{
					audioReverbFilter.reverbPreset = _source.reverbPreset;
				}
				if (_isDryLevelModified)
				{
					audioReverbFilter.dryLevel = _source.dryLevel;
				}
				if (_isRoomModified)
				{
					audioReverbFilter.room = _source.room;
				}
				if (_isRoomHFModified)
				{
					audioReverbFilter.roomHF = _source.roomHF;
				}
				if (_isDecayTimeModified)
				{
					audioReverbFilter.decayTime = _source.decayTime;
				}
				if (_isDecayHFRatioModified)
				{
					audioReverbFilter.decayHFRatio = _source.decayHFRatio;
				}
				if (_isReflectionsLevelModified)
				{
					audioReverbFilter.reflectionsLevel = _source.reflectionsLevel;
				}
				if (_isReflectionsDelayModified)
				{
					audioReverbFilter.reflectionsDelay = _source.reflectionsDelay;
				}
				if (_isReverbLevelModified)
				{
					audioReverbFilter.reverbLevel = _source.reverbLevel;
				}
				if (_isReverbDelayModified)
				{
					audioReverbFilter.reverbDelay = _source.reverbDelay;
				}
				if (_isDiffusionModified)
				{
					audioReverbFilter.diffusion = _source.diffusion;
				}
				if (_isDensityModified)
				{
					audioReverbFilter.density = _source.density;
				}
				if (_isHfReferenceModified)
				{
					audioReverbFilter.hfReference = _source.hfReference;
				}
				if (_isRoomLFModified)
				{
					audioReverbFilter.roomLF = _source.roomLF;
				}
				if (_isLfReferenceModified)
				{
					audioReverbFilter.lfReference = _source.lfReference;
				}
				if (_isEnabledModified)
				{
					audioReverbFilter.enabled = _source.enabled;
				}
				_source = audioReverbFilter;
			}
		}
	}
}
