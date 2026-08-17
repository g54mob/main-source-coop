using UnityEngine;

namespace Ami.Extension
{
	public class AudioLowPassFilterProxy : IAudioEffectModifier, IAudioLowPassFilterProxy
	{
		private AudioLowPassFilter _source;

		private bool _isCustomCutoffCurveModified;

		private bool _isCutoffFrequencyModified;

		private bool _isLowpassResonanceQModified;

		private bool _isEnabledModified;

		public AnimationCurve customCutoffCurve
		{
			get
			{
				return _source.customCutoffCurve;
			}
			set
			{
				_isCustomCutoffCurveModified = true;
				_source.customCutoffCurve = value;
			}
		}

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

		public float lowpassResonanceQ
		{
			get
			{
				return _source.lowpassResonanceQ;
			}
			set
			{
				_isLowpassResonanceQModified = true;
				_source.lowpassResonanceQ = value;
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

		public AudioLowPassFilterProxy(AudioLowPassFilter source)
		{
			_source = source;
		}

		public void TransferValueTo<T>(T target) where T : Behaviour
		{
			if (!(_source == null) && target is AudioLowPassFilter audioLowPassFilter)
			{
				if (_isCustomCutoffCurveModified)
				{
					audioLowPassFilter.customCutoffCurve = _source.customCutoffCurve;
				}
				if (_isCutoffFrequencyModified)
				{
					audioLowPassFilter.cutoffFrequency = _source.cutoffFrequency;
				}
				if (_isLowpassResonanceQModified)
				{
					audioLowPassFilter.lowpassResonanceQ = _source.lowpassResonanceQ;
				}
				if (_isEnabledModified)
				{
					audioLowPassFilter.enabled = _source.enabled;
				}
				_source = audioLowPassFilter;
			}
		}
	}
}
