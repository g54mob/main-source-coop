using UnityEngine;

namespace Ami.Extension
{
	public class AudioEchoFilterProxy : IAudioEffectModifier, IAudioEchoFilterProxy
	{
		private AudioEchoFilter _source;

		private bool _isDelayModified;

		private bool _isDecayRatioModified;

		private bool _isDryMixModified;

		private bool _isWetMixModified;

		private bool _isEnabledModified;

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

		public float decayRatio
		{
			get
			{
				return _source.decayRatio;
			}
			set
			{
				_isDecayRatioModified = true;
				_source.decayRatio = value;
			}
		}

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

		public float wetMix
		{
			get
			{
				return _source.wetMix;
			}
			set
			{
				_isWetMixModified = true;
				_source.wetMix = value;
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

		public AudioEchoFilterProxy(AudioEchoFilter source)
		{
			_source = source;
		}

		public void TransferValueTo<T>(T target) where T : Behaviour
		{
			if (!(_source == null) && target is AudioEchoFilter audioEchoFilter)
			{
				if (_isDelayModified)
				{
					audioEchoFilter.delay = _source.delay;
				}
				if (_isDecayRatioModified)
				{
					audioEchoFilter.decayRatio = _source.decayRatio;
				}
				if (_isDryMixModified)
				{
					audioEchoFilter.dryMix = _source.dryMix;
				}
				if (_isWetMixModified)
				{
					audioEchoFilter.wetMix = _source.wetMix;
				}
				if (_isEnabledModified)
				{
					audioEchoFilter.enabled = _source.enabled;
				}
				_source = audioEchoFilter;
			}
		}
	}
}
