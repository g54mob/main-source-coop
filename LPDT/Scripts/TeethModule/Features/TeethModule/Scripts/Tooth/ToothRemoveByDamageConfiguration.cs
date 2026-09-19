using UnityEngine;

namespace Features.TeethModule.Scripts.Tooth
{
	[CreateAssetMenu(fileName = "ToothRemoveByDamageConfiguration_Default", menuName = "Configurations/ToothModule/ToothRemoveByDamageConfiguration")]
	public class ToothRemoveByDamageConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public float MinToothAffectDamage { get; set; }

		[field: SerializeField]
		public int MaxToothAffectCount { get; set; }

		[field: SerializeField]
		public float TeethRemoveDamage { get; set; }
	}
}
