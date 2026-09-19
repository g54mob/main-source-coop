using UnityEngine;

namespace Features.BasketballHoopModule.Scripts
{
	[CreateAssetMenu(fileName = "BasketballConfiguration_Default", menuName = "Configurations/BasketballHoopModule/BasketballConfiguration")]
	public class BasketballConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public float MaxDistanceFromSpawnBeforeReset { get; private set; } = 20f;

		[field: SerializeField]
		public float OutOfZoneResetDelaySeconds { get; private set; } = 10f;
	}
}
