using System;
using Features.ChestScreamerModule.Scripts.Presets;
using UnityEngine;

namespace Features.ChestScreamerModule.Scripts
{
	[CreateAssetMenu(fileName = "ChestScreamerConfiguration_Default", menuName = "Configurations/Chest/ChestScreamerConfiguration")]
	public class ChestScreamerConfiguration : ScriptableObject
	{
		[SerializeField]
		private ChestScreamerPreset[] _presets = Array.Empty<ChestScreamerPreset>();

		[SerializeField]
		[Range(0f, 1f)]
		private float _procChance = 0.03f;

		private float _runtimeProcChanceOverride = -1f;

		public ChestScreamerPreset[] Presets => _presets;

		public float ProcChance
		{
			get
			{
				if (!(_runtimeProcChanceOverride >= 0f))
				{
					return _procChance;
				}
				return _runtimeProcChanceOverride;
			}
		}

		public void SetRuntimeProcChanceOverride(float chance)
		{
			_runtimeProcChanceOverride = Mathf.Clamp01(chance);
		}
	}
}
