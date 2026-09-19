using System;
using UnityEngine;

namespace Fusion
{
	[Serializable]
	public class InterestManagementConfig
	{
		[Range(4f, 128f)]
		public int AreaOfInterestCellSize = 32;

		public Vector3Int AreaOfInterestCellGridSize = new Vector3Int(1024, 1024, 1024);

		internal void Validate()
		{
			if (AreaOfInterestCellSize < 4)
			{
				AreaOfInterestCellSize = 4;
			}
			if (AreaOfInterestCellGridSize.x <= 0)
			{
				AreaOfInterestCellGridSize.x = 32;
			}
			if (AreaOfInterestCellGridSize.y <= 0)
			{
				AreaOfInterestCellGridSize.y = 32;
			}
			if (AreaOfInterestCellGridSize.z <= 0)
			{
				AreaOfInterestCellGridSize.z = 32;
			}
			if (CheckCellHashIntOverflow(AreaOfInterestCellGridSize.x, AreaOfInterestCellGridSize.y, AreaOfInterestCellGridSize.z))
			{
				AreaOfInterestCellGridSize = new Vector3Int(1024, 1024, 1024);
			}
		}

		public static bool CheckCellHashIntOverflow(int x, int y, int z)
		{
			try
			{
				int num = checked(z * x * y + y * x + x + 1);
				return false;
			}
			catch (OverflowException)
			{
				return true;
			}
		}
	}
}
