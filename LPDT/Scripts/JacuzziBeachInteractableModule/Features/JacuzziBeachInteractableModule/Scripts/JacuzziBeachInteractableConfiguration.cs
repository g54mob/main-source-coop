using UnityEngine;

namespace Features.JacuzziBeachInteractableModule.Scripts
{
	[CreateAssetMenu(fileName = "JacuzziBeachInteractableConfiguration_Default", menuName = "Configurations/JacuzziBeachInteractableModule/JacuzziBeachInteractableConfiguration")]
	public class JacuzziBeachInteractableConfiguration : ScriptableObject
	{
		[Header("Timing")]
		[field: SerializeField]
		[field: Min(0f)]
		public float TimeBeforeReddening { get; private set; } = 3f;

		[field: SerializeField]
		[field: Min(0.01f)]
		public float ReddeningDuration { get; private set; } = 2f;

		[field: SerializeField]
		[field: Min(0.01f)]
		public float RestoreDuration { get; private set; } = 1f;

		[Header("Appearance")]
		[field: SerializeField]
		public Color FlushColor { get; private set; } = new Color(0.85f, 0.25f, 0.25f, 1f);

		[field: SerializeField]
		[field: Min(0f)]
		public float MaxSteamEmissionRate { get; private set; } = 20f;
	}
}
