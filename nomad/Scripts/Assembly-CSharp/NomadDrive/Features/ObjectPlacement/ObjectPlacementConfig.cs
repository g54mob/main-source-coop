using UnityEngine;

namespace NomadDrive.Features.ObjectPlacement
{
	[CreateAssetMenu(menuName = "NomadDrive/Object Placement/Object Placement Config", fileName = "ObjectPlacementConfig")]
	public class ObjectPlacementConfig : ScriptableObject
	{
		public float snappingOffset;

		public float snappingDistance;

		public float placementShaderRadius;
	}
}
