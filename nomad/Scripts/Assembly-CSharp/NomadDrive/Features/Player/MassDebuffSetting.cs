using UnityEngine;

namespace NomadDrive.Features.Player
{
	[CreateAssetMenu(menuName = "NomadDrive/Player/Mass Debuff Setting")]
	public class MassDebuffSetting : ScriptableObject
	{
		[Header("Mass Thresholds")]
		[Tooltip("Items below this mass (kg) cause no speed penalty")]
		[SerializeField]
		[Min(0f)]
		private float noDebuffThreshold = 5f;

		[Tooltip("Mass (kg) at which maximum speed reduction is reached")]
		[SerializeField]
		[Min(0.1f)]
		private float maxDebuffMass = 50f;

		[Tooltip("Mass (kg) above which sprinting is disabled")]
		[SerializeField]
		[Min(0f)]
		private float sprintDisableMass = 10f;

		[Header("Speed Reduction")]
		[Tooltip("Maximum speed reduction factor (0.5 = 50% slower at max mass)")]
		[SerializeField]
		[Range(0f, 1f)]
		private float maxSpeedReduction = 0.5f;

		public float NoDebuffThreshold => noDebuffThreshold;

		public float MaxDebuffMass => maxDebuffMass;

		public float SprintDisableMass => sprintDisableMass;

		public float MaxSpeedReduction => maxSpeedReduction;
	}
}
