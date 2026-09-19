using UnityEngine;

namespace Features.KrakenModule.Scripts
{
	[CreateAssetMenu(fileName = "KrakenHelpThrowConfiguration_Default", menuName = "Configurations/Kraken/KrakenHelpThrowConfiguration")]
	public class KrakenHelpThrowConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public int MaxHelpDeadPartThrowsPerLevel { get; private set; } = 2;

		[field: SerializeField]
		public float MaxBeachDistanceFromExitGate { get; private set; } = 25f;
	}
}
