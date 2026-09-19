using System.Collections;
using System.Collections.Generic;

namespace Fusion.LagCompensation
{
	public class HitboxColliderContainerDraw : IEnumerable<ColliderDrawInfo>, IEnumerable
	{
		internal HitboxBuffer.HitboxSnapshot _container;

		private ColliderDrawInfo _drawInfo = new ColliderDrawInfo();

		public IEnumerator<ColliderDrawInfo> GetEnumerator()
		{
			_drawInfo.SetContainer(_container);
			for (int i = 1; i <= _container.CollidersCount; i++)
			{
				if (_container.GetCollider(i).Active && _container.GetCollider(i).Used)
				{
					yield return _drawInfo.FromHitboxCollider(i);
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
