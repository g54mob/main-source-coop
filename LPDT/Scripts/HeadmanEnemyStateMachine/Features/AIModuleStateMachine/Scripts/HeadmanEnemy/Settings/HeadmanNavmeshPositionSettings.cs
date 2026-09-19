using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.HeadmanEnemy.Settings
{
	[CreateAssetMenu(fileName = "HeadmanNavmeshPositionSettings_Default", menuName = "Configurations/AIModuleStateMachine/Headman/HeadmanNavmeshPositionSettings")]
	public class HeadmanNavmeshPositionSettings : ScriptableObject
	{
		[Header("Random safe navmesh sampling")]
		[Min(1f)]
		public int Attempts = 15;

		[Min(0f)]
		public float MinDistanceFromCenter = 3f;

		[Min(0f)]
		public float CenterDistanceWeight = 1.5f;

		[Min(0f)]
		public float AverageAvoidDistanceWeight = 5f;

		[Min(0f)]
		public float TooClosePenaltyRadius = 4f;

		[Min(0f)]
		public float TooClosePenaltyWeight;
	}
}
