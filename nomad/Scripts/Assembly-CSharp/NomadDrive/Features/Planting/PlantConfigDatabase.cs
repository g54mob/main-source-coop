using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NomadDrive.Features.Planting
{
	[CreateAssetMenu(menuName = "NomadDrive/Planting/Database", fileName = "PlantConfigDatabase")]
	public class PlantConfigDatabase : SerializedScriptableObject
	{
		[Tooltip("Configuration list for all plant types")]
		[SerializeField]
		private List<PlantConfig> _plantConfigs = new List<PlantConfig>();

		private Dictionary<PlantType, PlantConfig> _configLookup;

		public void Initialize()
		{
			BuildLookupTable();
		}

		private void OnEnable()
		{
			BuildLookupTable();
		}

		private void BuildLookupTable()
		{
			_configLookup = new Dictionary<PlantType, PlantConfig>();
			foreach (PlantConfig plantConfig in _plantConfigs)
			{
				if (!(plantConfig == null) && !_configLookup.ContainsKey(plantConfig.PlantType))
				{
					_configLookup[plantConfig.PlantType] = plantConfig;
				}
			}
		}

		public PlantConfig GetConfig(PlantType plantType)
		{
			if (_configLookup == null)
			{
				BuildLookupTable();
			}
			if (_configLookup.TryGetValue(plantType, out var value))
			{
				return value;
			}
			return null;
		}

		public PlantConfig GetConfigByByte(byte plantTypeByte)
		{
			return GetConfig((PlantType)plantTypeByte);
		}

		public IReadOnlyList<PlantConfig> GetAllConfigs()
		{
			return _plantConfigs.AsReadOnly();
		}

		public bool HasConfig(PlantType plantType)
		{
			if (_configLookup == null)
			{
				BuildLookupTable();
			}
			return _configLookup.ContainsKey(plantType);
		}
	}
}
