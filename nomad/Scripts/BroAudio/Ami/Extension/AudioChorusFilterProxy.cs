using UnityEngine;

namespace Ami.Extension
{
	public class AudioChorusFilterProxy : IAudioEffectModifier, IAudioChorusFilterProxy
	{
		private AudioChorusFilter _source;

		private bool _isDryMixModified;

		private bool _isWetMix1Modified;

		private bool _isWetMix2Modified;

		private bool _isWetMix3Modified;

		private bool _isDelayModified;

		private bool _isRateModified;

		private bool _isDepthModified;

		private bool _isEnabledModified;

		public float dryMix
		{
			get
			{
				return _source.dryMix;
			}
			set
			{
				_isDryMixModified = true;
				_source.dryMix = value;
			}
		}

		public float wetMix1
		{
			get
			{
				return _source.wetMix1;
			}
			set
			{
				_isWetMix1Modified = true;
				_source.wetMix1 = value;
			}
		}

		public float wetMix2
		{
			get
			{
				return _source.wetMix2;
			}
			set
			{
				_isWetMix2Modified = true;
				_source.wetMix2 = value;
			}
		}

		public float wetMix3
		{
			get
			{
				return _source.wetMix3;
			}
			set
			{
				_isWetMix3Modified = true;
				_source.wetMix3 = value;
			}
		}

		public float delay
		{
			get
			{
				return _source.delay;
			}
			set
			{
				_isDelayModified = true;
				_source.delay = value;
			}
		}

		public float rate
		{
			get
			{
				return _source.rate;
			}
			set
			{
				_isRateModified = true;
				_source.rate = value;
			}
		}

		public float depth
		{
			get
			{
				return _source.depth;
			}
			set
			{
				_isDepthModified = true;
				_source.depth = value;
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

		public AudioChorusFilterProxy(AudioChorusFilter source)
		{
			_source = source;
		}

		public void TransferValueTo<T>(T target) where T : Behaviour
		{
			if (!(_source == null) && target is AudioChorusFilter audioChorusFilter)
			{
				if (_isDryMixModified)
				{
					audioChorusFilter.dryMix = _source.dryMix;
				}
				if (_isWetMix1Modified)
				{
					audioChorusFilter.wetMix1 = _source.wetMix1;
				}
				if (_isWetMix2Modified)
				{
					audioChorusFilter.wetMix2 = _source.wetMix2;
				}
				if (_isWetMix3Modified)
				{
					audioChorusFilter.wetMix3 = _source.wetMix3;
				}
				if (_isDelayModified)
				{
					audioChorusFilter.delay = _source.delay;
				}
				if (_isRateModified)
				{
					audioChorusFilter.rate = _source.rate;
				}
				if (_isDepthModified)
				{
					audioChorusFilter.depth = _source.depth;
				}
				if (_isEnabledModified)
				{
					audioChorusFilter.enabled = _source.enabled;
				}
				_source = audioChorusFilter;
			}
		}
	}
}
