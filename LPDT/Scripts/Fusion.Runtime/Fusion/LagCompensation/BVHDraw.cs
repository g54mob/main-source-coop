using System.Collections;
using System.Collections.Generic;

namespace Fusion.LagCompensation
{
	public class BVHDraw : IEnumerable<BVHNodeDrawInfo>, IEnumerable
	{
		internal HitboxBuffer _buffer;

		private BVHNodeDrawInfo _drawInfo;

		private Stack<BVHNode> _reusableStack = new Stack<BVHNode>();

		internal BVHDraw(HitboxBuffer buffer)
		{
			_buffer = buffer;
			_drawInfo = new BVHNodeDrawInfo(_buffer);
		}

		public IEnumerator<BVHNodeDrawInfo> GetEnumerator()
		{
			_reusableStack.Clear();
			_reusableStack.Push(_drawInfo.Buffer.BVH.rootBVH);
			while (_reusableStack.Count > 0)
			{
				BVHNode node = _reusableStack.Pop();
				if (node.IsValid)
				{
					yield return _drawInfo.FromBVHNode(ref node);
					if (node.HasLeft)
					{
						_reusableStack.Push(node.GetLeft(_drawInfo.Buffer.BVH));
					}
					if (node.HasRight)
					{
						_reusableStack.Push(node.GetRight(_drawInfo.Buffer.BVH));
					}
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
