using UnityEngine;

namespace Obi
{
	public abstract class ObiMeshBasedActorBlueprint : ObiActorBlueprint
	{
		public Mesh inputMesh;

		public Vector3 scale = Vector3.one;

		public Quaternion rotation = Quaternion.identity;
	}
}
