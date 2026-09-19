using System;
using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.Data
{
	[Serializable]
	public class PingVisual
	{
		[field: SerializeField]
		public Color Color { get; set; }

		[field: SerializeField]
		public float TargetValue { get; set; }
	}
}
