using DG.Tweening;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole
{
	[CreateAssetMenu(fileName = "RatsHoleAbsorbSettings_Default", menuName = "Configurations/AIModuleStateMachine/CoinRobSwarm/RatsHoleAbsorbSettings")]
	public class RatsHoleAbsorbSettings : ScriptableObject
	{
		[field: SerializeField]
		public float AbsorbTriggerDistance { get; private set; } = 1.25f;

		[field: SerializeField]
		public float AbsorbPointRadius { get; private set; } = 0.75f;

		[field: SerializeField]
		public float FlightDuration { get; private set; } = 0.45f;

		[field: SerializeField]
		public float FlightSpeed { get; private set; }

		[field: SerializeField]
		public float MinFlightDuration { get; private set; } = 0.15f;

		[field: SerializeField]
		public float MaxFlightDuration { get; private set; } = 1.2f;

		[field: SerializeField]
		public Ease FlightEase { get; private set; } = Ease.InCubic;

		[field: SerializeField]
		public Ease ScaleEase { get; private set; } = Ease.InQuad;

		[field: SerializeField]
		public float AbsorbEndScale { get; private set; } = 0.2f;
	}
}
