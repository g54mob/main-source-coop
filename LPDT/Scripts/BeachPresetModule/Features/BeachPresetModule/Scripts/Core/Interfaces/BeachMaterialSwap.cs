using UnityEngine;

namespace Features.BeachPresetModule.Scripts.Core.Interfaces
{
	public readonly struct BeachMaterialSwap
	{
		public readonly Material SourceMaterial;

		public readonly Material TargetMaterial;

		public BeachMaterialSwap(Material sourceMaterial, Material targetMaterial)
		{
			SourceMaterial = sourceMaterial;
			TargetMaterial = targetMaterial;
		}
	}
}
