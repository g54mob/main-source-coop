using UnityEngine;

namespace Obi
{
	public class ObiTriangleMeshHandle : ObiResourceHandle<Mesh>
	{
		public ObiTriangleMeshHandle(Mesh mesh, int index = -1)
			: base(index)
		{
			owner = mesh;
		}
	}
}
