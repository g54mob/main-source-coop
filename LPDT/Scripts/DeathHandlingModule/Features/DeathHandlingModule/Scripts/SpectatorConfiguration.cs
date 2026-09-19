using UnityEngine;

namespace Features.DeathHandlingModule.Scripts
{
	[CreateAssetMenu(fileName = "SpectatorConfiguration_Default", menuName = "Configurations/DeathHandling/SpectatorConfiguration")]
	public class SpectatorConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public float TimeToSpectator { get; private set; }
	}
}
