using UnityEngine;

namespace ECM2
{
	public interface IColliderFilter
	{
		bool Filter(Collider otherCollider);
	}
}
