using UnityEngine;

namespace Features.NavigationModule.Scripts
{
	[CreateAssetMenu(fileName = "NavigationConfiguration_Default", menuName = "Configurations/Navigation/NavigationConfiguration")]
	public class NavigationConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public float PointProjectionDistance { get; private set; }

		[field: SerializeField]
		public LayerMask FloorLayerMask { get; private set; }

		[field: SerializeField]
		public float PointEqualityApproximation { get; private set; }

		[field: SerializeField]
		public float PointProjectionRadius { get; private set; }
	}
}
