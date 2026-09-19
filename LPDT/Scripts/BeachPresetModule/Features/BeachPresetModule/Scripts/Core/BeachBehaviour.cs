using System;
using UnityEngine;

namespace Features.BeachPresetModule.Scripts.Core
{
	[Serializable]
	public abstract class BeachBehaviour
	{
		[Tooltip("Designer-friendly name shown in the preset inspector.")]
		[SerializeField]
		private string _displayName;

		[Tooltip("When disabled, this behaviour is skipped by Apply and Clear.")]
		[SerializeField]
		private bool _enabled = true;

		public string DisplayName
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(_displayName))
				{
					return _displayName;
				}
				return GetType().Name;
			}
		}

		public bool Enabled => _enabled;

		public virtual int Order => 0;

		public virtual void OnInjected()
		{
		}

		public abstract void Apply(BeachPresetRuntimeContext context);

		public abstract void Clear(BeachPresetRuntimeContext context);

		public abstract BeachValidationResult Validate(BeachPreset preset);
	}
}
