using UnityEngine;

namespace NomadDrive.Features.Rope
{
	[CreateAssetMenu(menuName = "Full Rig/Rope End Object")]
	public class RopeEnd : ScriptableObject
	{
		public float ropeStart;

		public float tangent = 0.01f;

		public float adjustZ;

		public float adjustY;

		public float width = 1f;

		public RopeNum ropeType = RopeNum.All;

		public GameObject prefab;
	}
}
