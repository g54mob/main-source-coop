using UnityEngine;

namespace NomadDrive.Features.Consumables
{
	[CreateAssetMenu(menuName = "NomadDrive/Consumables/Medicine", fileName = "MedicineConfig")]
	public class MedicineConfig : ScriptableObject
	{
		[Tooltip("Poison drained from the bar each in-game minute after this medicine is taken (gradual cure).")]
		public float poisonHealRatePerMinute = 5f;

		[Tooltip("Immediate poison reduction applied the moment the medicine is consumed (0 = none, pure gradual cure).")]
		public float instantPoisonReduction;
	}
}
