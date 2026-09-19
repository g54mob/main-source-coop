using Features.GamePhasesModule.Scripts.Data;
using Features.LevelModule.Scripts;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.GamePhasesModule.Scripts
{
	[CreateAssetMenu(fileName = "GamePhasesConfiguration_Default", menuName = "Configurations/GamePhasesModule/GamePhasesConfiguration")]
	public class GamePhasesConfiguration : ScriptableObject
	{
		public SerializableDictionary<LevelType, GamePhasesData> GamePhasesByLevel = new SerializableDictionary<LevelType, GamePhasesData>();

		public GamePhasesData DefaultGamePhases;
	}
}
