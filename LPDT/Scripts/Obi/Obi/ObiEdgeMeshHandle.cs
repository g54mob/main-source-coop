using UnityEngine;

namespace Obi
{
	public class ObiEdgeMeshHandle : ObiResourceHandle<EdgeCollider2D>
	{
		public ObiEdgeMeshHandle(EdgeCollider2D collider, int index = -1)
			: base(index)
		{
			owner = collider;
		}
	}
}
