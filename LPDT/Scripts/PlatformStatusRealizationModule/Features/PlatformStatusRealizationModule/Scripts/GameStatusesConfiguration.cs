using Global.SerializableDictionary;
using UnityEngine;

namespace Features.PlatformStatusRealizationModule.Scripts
{
	[CreateAssetMenu(fileName = "GameStatusesConfiguration_Default", menuName = "Configurations/PlatformStatusRealizationModule/GameStatusesConfiguration")]
	public class GameStatusesConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public SerializableDictionary<GameStatusId, GameStatusData> GameStatusesData { get; private set; }

		[field: SerializeField]
		public SerializableDictionary<GameStatusParameterId, GameStatusParameterData> GameStatusParametersData { get; private set; }
	}
}
