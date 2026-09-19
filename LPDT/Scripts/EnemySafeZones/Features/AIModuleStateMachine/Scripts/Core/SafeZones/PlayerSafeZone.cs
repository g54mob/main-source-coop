using System.Collections.Generic;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Core.SafeZones
{
	public class PlayerSafeZone : MonoBehaviour
	{
		private static readonly List<PlayerSafeZone> _activeZones = new List<PlayerSafeZone>();

		[SerializeField]
		private SafeZoneType _zoneType;

		[SerializeField]
		private List<Transform> _interactionPoints;

		public static IReadOnlyList<PlayerSafeZone> ActiveZones => _activeZones;

		public SafeZoneType SafeZoneType => _zoneType;

		public IReadOnlyList<Transform> InteractionPoints => _interactionPoints;

		private void OnEnable()
		{
			_activeZones.Add(this);
		}

		private void OnDisable()
		{
			_activeZones.Remove(this);
		}
	}
}
