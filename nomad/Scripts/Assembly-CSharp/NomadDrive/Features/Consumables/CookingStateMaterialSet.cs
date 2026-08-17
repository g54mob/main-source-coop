using System;
using System.Collections.Generic;
using UnityEngine;

namespace NomadDrive.Features.Consumables
{
	[CreateAssetMenu(menuName = "NomadDrive/Consumables/Cooking State Material Set", fileName = "NewCookingStateMaterialSet")]
	public class CookingStateMaterialSet : ScriptableObject
	{
		[Serializable]
		public struct CookingStateMaterial
		{
			public CookedLevel cookedLevel;

			public Material material;
		}

		[SerializeField]
		private List<CookingStateMaterial> stateMaterials = new List<CookingStateMaterial>
		{
			new CookingStateMaterial
			{
				cookedLevel = CookedLevel.Raw,
				material = null
			},
			new CookingStateMaterial
			{
				cookedLevel = CookedLevel.MidCooked,
				material = null
			},
			new CookingStateMaterial
			{
				cookedLevel = CookedLevel.WellCooked,
				material = null
			},
			new CookingStateMaterial
			{
				cookedLevel = CookedLevel.Burned,
				material = null
			}
		};

		private Dictionary<CookedLevel, Material> _materialCache;

		public IReadOnlyList<CookingStateMaterial> StateMaterials => stateMaterials;

		private void OnEnable()
		{
			BuildCache();
		}

		private void OnValidate()
		{
			BuildCache();
		}

		private void BuildCache()
		{
			_materialCache = new Dictionary<CookedLevel, Material>();
			foreach (CookingStateMaterial stateMaterial in stateMaterials)
			{
				_materialCache[stateMaterial.cookedLevel] = stateMaterial.material;
			}
		}

		public Material GetMaterial(CookedLevel cookedLevel)
		{
			if (_materialCache == null || _materialCache.Count == 0)
			{
				BuildCache();
			}
			if (_materialCache.TryGetValue(cookedLevel, out var value))
			{
				return value;
			}
			return null;
		}
	}
}
