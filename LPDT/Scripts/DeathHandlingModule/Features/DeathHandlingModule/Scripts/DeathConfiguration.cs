using FMODUnity;
using UnityEngine;

namespace Features.DeathHandlingModule.Scripts
{
	[CreateAssetMenu(fileName = "DeathConfiguration_Default", menuName = "Configurations/DeathHandling/DeathConfiguration")]
	public class DeathConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public float TimeToDie { get; private set; }

		[field: SerializeField]
		public Color DeathVignetteColor { get; private set; }

		[field: SerializeField]
		public float DeathVignetteIntensity { get; private set; }

		[field: SerializeField]
		public float DeathVignetteSmoothness { get; private set; }

		[field: SerializeField]
		public EventReference DeathEventInstance { get; private set; }
	}
}
