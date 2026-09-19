using System.Collections.Generic;
using Features.BeachPresetModule.Scripts.Core.Interfaces;
using Features.BeachPresetModule.Scripts.Data;
using UnityEngine;

namespace Features.BeachPresetModule.Scripts.Services
{
	public class BeachMaterialSwapService : IBeachMaterialSwapService
	{
		private readonly BeachMaterialSwapModel _materialSwapModel;

		public BeachMaterialSwapService(BeachMaterialSwapModel materialSwapModel)
		{
			_materialSwapModel = materialSwapModel;
		}

		public BeachMaterialSwapHandle Apply(IReadOnlyList<BeachMaterialSwap> swaps)
		{
			BeachMaterialSwapHandle beachMaterialSwapHandle = new BeachMaterialSwapHandle();
			Dictionary<string, Material> dictionary = BuildReplacementMap(swaps);
			if (dictionary.Count == 0)
			{
				return beachMaterialSwapHandle;
			}
			foreach (Renderer registeredRenderer in _materialSwapModel.RegisteredRenderers)
			{
				if (registeredRenderer == null)
				{
					continue;
				}
				Material[] sharedMaterials = registeredRenderer.sharedMaterials;
				Material[] array = (Material[])sharedMaterials.Clone();
				bool flag = false;
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != null && dictionary.TryGetValue(array[i].name, out var value))
					{
						array[i] = value;
						flag = true;
					}
				}
				if (flag)
				{
					beachMaterialSwapHandle.RendererSnapshots[registeredRenderer] = sharedMaterials;
					registeredRenderer.sharedMaterials = array;
				}
			}
			return beachMaterialSwapHandle;
		}

		public void Restore(BeachMaterialSwapHandle handle)
		{
			if (handle == null)
			{
				return;
			}
			foreach (KeyValuePair<Renderer, Material[]> rendererSnapshot in handle.RendererSnapshots)
			{
				if (rendererSnapshot.Key != null)
				{
					rendererSnapshot.Key.sharedMaterials = rendererSnapshot.Value;
				}
			}
			handle.RendererSnapshots.Clear();
		}

		private static Dictionary<string, Material> BuildReplacementMap(IReadOnlyList<BeachMaterialSwap> swaps)
		{
			Dictionary<string, Material> dictionary = new Dictionary<string, Material>();
			foreach (BeachMaterialSwap swap in swaps)
			{
				if (swap.SourceMaterial != null && swap.TargetMaterial != null)
				{
					dictionary[swap.SourceMaterial.name] = swap.TargetMaterial;
				}
			}
			return dictionary;
		}
	}
}
