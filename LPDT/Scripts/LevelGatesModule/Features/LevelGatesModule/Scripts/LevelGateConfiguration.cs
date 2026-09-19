using Features.LevelModule.Scripts;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.LevelGatesModule.Scripts
{
	[CreateAssetMenu(fileName = "LevelGateConfiguration_Default", menuName = "Configurations/LevelGates/LevelGateConfiguration")]
	public class LevelGateConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public LayerMask GateLayerMask { get; private set; }

		[field: SerializeField]
		public float DistanceToMaxGateGain { get; private set; }

		[field: SerializeField]
		public float GateActivationTime { get; private set; }

		[field: SerializeField]
		public SerializableDictionary<LevelType, PlayerTerritoryMode> TerritoryModeByLevel { get; private set; }

		public PlayerTerritoryMode GetTerritoryMode(LevelType level)
		{
			if (TerritoryModeByLevel != null && TerritoryModeByLevel.TryGetValue(level, out var value))
			{
				return value;
			}
			return PlayerTerritoryMode.ExitGateDot;
		}
	}
}
