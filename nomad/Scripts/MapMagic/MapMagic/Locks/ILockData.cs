using Den.Tools.Matrices;
using MapMagic.Nodes;
using UnityEngine;

namespace MapMagic.Locks
{
	public interface ILockData
	{
		void Read(Terrain terrain, Lock lk);

		void WriteInThread(IApplyData applyData);

		void WriteInApply(Terrain terrain, bool resizeTerrain);

		void ApplyHeightDelta(Matrix src, Matrix dst);

		void ResizeFrom(ILockData lockData);
	}
}
