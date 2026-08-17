using UnityEngine;

namespace Ami.Extension
{
	public class AudioDistortionFilterProxy : IAudioEffectModifier, IAudioDistortionFilterProxy
	{
		private AudioDistortionFilter _source;

		private bool _isDistortionLevelModified;

		private bool _isEnabledModified;

		public float distortionLevel
		{
			get
			{
				return _source.distortionLevel;
			}
			set
			{
				_isDistortionLevelModified = true;
				_source.distortionLevel = value;
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

		public AudioDistortionFilterProxy(AudioDistortionFilter source)
		{
			_source = source;
		}

		public void TransferValueTo<T>(T target) where T : Behaviour
		{
			if (!(_source == null) && target is AudioDistortionFilter audioDistortionFilter)
			{
				if (_isDistortionLevelModified)
				{
					audioDistortionFilter.distortionLevel = _source.distortionLevel;
				}
				if (_isEnabledModified)
				{
					audioDistortionFilter.enabled = _source.enabled;
				}
				_source = audioDistortionFilter;
			}
		}
	}
}
