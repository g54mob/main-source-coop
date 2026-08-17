using UnityEngine;

namespace Ami.Extension
{
	public class AudioHighPassFilterProxy : IAudioEffectModifier, IAudioHighPassFilterProxy
	{
		private AudioHighPassFilter _source;

		private bool _isCutoffFrequencyModified;

		private bool _isHighpassResonanceQModified;

		private bool _isEnabledModified;

		public float cutoffFrequency
		{
			get
			{
				return _source.cutoffFrequency;
			}
			set
			{
				_isCutoffFrequencyModified = true;
				_source.cutoffFrequency = value;
			}
		}

		public float highpassResonanceQ
		{
			get
			{
				return _source.highpassResonanceQ;
			}
			set
			{
				_isHighpassResonanceQModified = true;
				_source.highpassResonanceQ = value;
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

		public AudioHighPassFilterProxy(AudioHighPassFilter source)
		{
			_source = source;
		}

		public void TransferValueTo<T>(T target) where T : Behaviour
		{
			if (!(_source == null) && target is AudioHighPassFilter audioHighPassFilter)
			{
				if (_isCutoffFrequencyModified)
				{
					audioHighPassFilter.cutoffFrequency = _source.cutoffFrequency;
				}
				if (_isHighpassResonanceQModified)
				{
					audioHighPassFilter.highpassResonanceQ = _source.highpassResonanceQ;
				}
				if (_isEnabledModified)
				{
					audioHighPassFilter.enabled = _source.enabled;
				}
				_source = audioHighPassFilter;
			}
		}
	}
}
