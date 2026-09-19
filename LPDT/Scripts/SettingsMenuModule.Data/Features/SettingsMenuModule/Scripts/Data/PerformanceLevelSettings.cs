using System;
using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.Data
{
	[Serializable]
	public class PerformanceLevelSettings
	{
		[field: SerializeField]
		public int ProcessorCount { get; private set; }

		[field: SerializeField]
		public int MemorySize { get; private set; }

		[field: SerializeField]
		public int GraphicsMemorySize { get; private set; }
	}
}
