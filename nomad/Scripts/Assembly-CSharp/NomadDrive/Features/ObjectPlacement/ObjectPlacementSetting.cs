using UnityEngine;

namespace NomadDrive.Features.ObjectPlacement
{
	[CreateAssetMenu(menuName = "NomadDrive/Object Placement/Object Placement Setting", fileName = "ObjectPlacementSetting")]
	public class ObjectPlacementSetting : ScriptableObject
	{
		public float defaultObjectDistanceToCamera = 1.5f;

		public float objectMovingScrollDensity = 0.2f;

		public float minObjectDistanceToCamera = 0.75f;

		public float maxObjectDistanceToCamera = 2.25f;

		public float objectLerpingDensity = 25f;

		public float objectRotationDensity = 25f;
	}
}
