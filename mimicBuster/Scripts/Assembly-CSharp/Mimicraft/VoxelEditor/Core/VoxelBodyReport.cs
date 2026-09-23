using System.Collections.Generic;
using UnityEngine;

namespace Mimicraft.VoxelEditor.Core
{
	public struct VoxelBodyReport
	{
		public bool HasVoxels;

		public Bounds Bounds;

		public Vector3 Extent;

		public bool BelowMin;

		public bool AboveMax;

		public List<int> StrandedPieces;

		public List<int> OverlappingPieces;

		public List<int> SmallPieces;

		public List<int> FragmentedPieces;

		public bool HasInvalidPieces
		{
			get
			{
				if ((StrandedPieces == null || StrandedPieces.Count <= 0) && (OverlappingPieces == null || OverlappingPieces.Count <= 0) && (SmallPieces == null || SmallPieces.Count <= 0))
				{
					if (FragmentedPieces != null)
					{
						return FragmentedPieces.Count > 0;
					}
					return false;
				}
				return true;
			}
		}

		public bool IsValid
		{
			get
			{
				if (HasVoxels && !BelowMin && !AboveMax)
				{
					return !HasInvalidPieces;
				}
				return false;
			}
		}
	}
}
