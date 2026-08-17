using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Drawing
{
	public struct RedrawScope : IDisposable
	{
		internal GCHandle gizmos;

		internal int id;

		private static int idCounter = 1;

		public bool isValid => id != 0;

		internal RedrawScope(DrawingData gizmos, int id)
		{
			this.gizmos = gizmos.gizmosHandle;
			this.id = id;
		}

		internal RedrawScope(DrawingData gizmos)
		{
			this.gizmos = gizmos.gizmosHandle;
			id = idCounter++;
		}

		internal void Draw()
		{
			if (gizmos.IsAllocated && gizmos.Target is DrawingData drawingData)
			{
				drawingData.Draw(this);
			}
		}

		public void Rewind()
		{
			GameObject associatedGameObject = null;
			if (gizmos.IsAllocated && gizmos.Target is DrawingData drawingData)
			{
				associatedGameObject = drawingData.GetAssociatedGameObject(this);
			}
			Dispose();
			this = DrawingManager.GetRedrawScope(associatedGameObject);
		}

		internal void DrawUntilDispose(GameObject associatedGameObject)
		{
			if (gizmos.Target is DrawingData drawingData)
			{
				drawingData.DrawUntilDisposed(this, associatedGameObject);
			}
		}

		public void Dispose()
		{
			if (gizmos.IsAllocated && gizmos.Target is DrawingData drawingData)
			{
				drawingData.DisposeRedrawScope(this);
			}
			gizmos = default(GCHandle);
			id = 0;
		}
	}
}
