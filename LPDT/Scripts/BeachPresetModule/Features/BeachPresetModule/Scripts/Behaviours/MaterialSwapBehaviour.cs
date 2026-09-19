using System;
using System.Collections.Generic;
using Features.BeachPresetModule.Scripts.Core;
using Features.BeachPresetModule.Scripts.Core.Interfaces;
using UnityEngine;
using Zenject;

namespace Features.BeachPresetModule.Scripts.Behaviours
{
	[Serializable]
	public class MaterialSwapBehaviour : BeachBehaviour
	{
		[Serializable]
		private class MaterialSwapEntry
		{
			[Tooltip("Material currently used by the beach renderers.")]
			[SerializeField]
			private Material _sourceMaterial;

			[Tooltip("Material that should replace the source material while the preset is active.")]
			[SerializeField]
			private Material _targetMaterial;

			public Material SourceMaterial => _sourceMaterial;

			public Material TargetMaterial => _targetMaterial;

			public BeachMaterialSwap ToServiceSwap()
			{
				return new BeachMaterialSwap(_sourceMaterial, _targetMaterial);
			}

			public void Validate(BeachValidationResult result)
			{
				if (_sourceMaterial == null)
				{
					result.AddError("Material swap entry is missing a source material.");
				}
				if (_targetMaterial == null)
				{
					result.AddError("Material swap entry is missing a target material.");
				}
			}
		}

		[Tooltip("Material replacements applied while this preset is active.")]
		[SerializeField]
		private List<MaterialSwapEntry> _materialSwaps = new List<MaterialSwapEntry>();

		private IBeachMaterialSwapService _materialSwapService;

		private BeachMaterialSwapHandle _handle;

		public override int Order => 50;

		[Inject]
		public void InjectDependencies(IBeachMaterialSwapService materialSwapService)
		{
			_materialSwapService = materialSwapService;
		}

		public override void Apply(BeachPresetRuntimeContext context)
		{
			_materialSwapService.Restore(_handle);
			_handle = _materialSwapService.Apply(BuildServiceSwaps());
		}

		public override void Clear(BeachPresetRuntimeContext context)
		{
			_materialSwapService.Restore(_handle);
			_handle = null;
		}

		public override BeachValidationResult Validate(BeachPreset preset)
		{
			BeachValidationResult beachValidationResult = new BeachValidationResult();
			if (_materialSwaps.Count == 0)
			{
				beachValidationResult.AddWarning("MaterialSwapBehaviour has no material swap entries.");
			}
			foreach (MaterialSwapEntry materialSwap in _materialSwaps)
			{
				materialSwap?.Validate(beachValidationResult);
			}
			return beachValidationResult;
		}

		private IReadOnlyList<BeachMaterialSwap> BuildServiceSwaps()
		{
			List<BeachMaterialSwap> list = new List<BeachMaterialSwap>();
			foreach (MaterialSwapEntry materialSwap in _materialSwaps)
			{
				if (materialSwap != null)
				{
					list.Add(materialSwap.ToServiceSwap());
				}
			}
			return list;
		}
	}
}
