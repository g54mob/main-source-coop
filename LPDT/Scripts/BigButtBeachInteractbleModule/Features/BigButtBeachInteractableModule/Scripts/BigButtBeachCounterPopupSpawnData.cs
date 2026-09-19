using UnityEngine;

namespace Features.BigButtBeachInteractableModule.Scripts
{
	public readonly struct BigButtBeachCounterPopupSpawnData
	{
		public readonly Vector3 Position;

		public readonly Vector3 MovementWorldDirection;

		public BigButtBeachCounterPopupSpawnData(Vector3 position, Vector3 movementWorldDirection)
		{
			Position = position;
			MovementWorldDirection = movementWorldDirection;
		}
	}
}
