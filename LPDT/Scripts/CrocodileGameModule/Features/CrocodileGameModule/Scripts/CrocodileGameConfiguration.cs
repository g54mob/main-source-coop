using FMODUnity;
using UnityEngine;

namespace Features.CrocodileGameModule.Scripts
{
	[CreateAssetMenu(fileName = "CrocodileGameConfiguration_Default", menuName = "Configurations/CrocodileGameModule/CrocodileGameConfiguration")]
	public class CrocodileGameConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public float BiteDamage { get; private set; } = 25f;

		[field: SerializeField]
		[field: Min(1f)]
		public int BadTeethCount { get; private set; } = 1;

		[Header("Sounds (FMOD)")]
		[field: SerializeField]
		public EventReference BiteSound { get; private set; }

		[field: SerializeField]
		public EventReference ToothPressSound { get; private set; }

		[field: SerializeField]
		public EventReference MouthOpenSound { get; private set; }

		[field: SerializeField]
		public EventReference MouthCloseSound { get; private set; }
	}
}
