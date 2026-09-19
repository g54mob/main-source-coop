using Global.SerializableDictionary;
using UnityEngine;

namespace Features.TeethModule.Scripts.Tooth
{
	[CreateAssetMenu(fileName = "ToothCustomizationConfig_Default", menuName = "Configurations/ToothModule/ToothCustomizationConfig")]
	public class ToothCustomizationConfig : ScriptableObject
	{
		[field: SerializeField]
		public SerializableDictionary<ToothPreset, ToothCustomizationData> MinToothAffectDamage { get; set; }
	}
}
