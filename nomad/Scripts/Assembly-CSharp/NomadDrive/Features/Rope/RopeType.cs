using UnityEngine;

namespace NomadDrive.Features.Rope
{
	[CreateAssetMenu(menuName = "Full Rig/Rope Type")]
	public class RopeType : ScriptableObject
	{
		public Mesh mesh;

		public Material material;

		public RopeNum ropeNum = RopeNum.One;
	}
}
