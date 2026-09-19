using System.Collections;
using System.Collections.Generic;

namespace Fusion.LagCompensation
{
	public class SnapshotHistoryDraw : IEnumerable<HitboxColliderContainerDraw>, IEnumerable
	{
		private HitboxBuffer _buffer;

		private HitboxColliderContainerDraw _containerDraw = new HitboxColliderContainerDraw();

		internal SnapshotHistoryDraw(HitboxBuffer buffer)
		{
			_buffer = buffer;
		}

		public IEnumerator<HitboxColliderContainerDraw> GetEnumerator()
		{
			for (int i = 0; i < _buffer.Length; i++)
			{
				_containerDraw._container = _buffer._buffer[i];
				yield return _containerDraw;
			}
			_containerDraw._container = null;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
