using Global.SerializableDictionary;
using UnityEngine;

namespace Features.HingeModule.Scripts
{
	[CreateAssetMenu(fileName = "HingeSetupConfiguration_Default", menuName = "Configurations/Hinge/HingeSetupConfiguration")]
	public class HingeSetupConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public SerializableDictionary<HingeSetupType, HingeSetupData> HingeSetup { get; private set; }
	}
}
