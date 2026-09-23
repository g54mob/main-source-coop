using UnityEngine;

namespace Mimicraft.VoxelEditor
{
	public struct ClearanceResult
	{
		public float Burial;

		public int OutsideCorners;

		public int OutsideCentres;

		public Collider Blocker;

		public bool IsClear
		{
			get
			{
				if (Burial <= 0f)
				{
					return OutsideCorners == 0;
				}
				return false;
			}
		}

		public bool NoWorseThan(ClearanceResult reference)
		{
			if (Burial <= Mathf.Max(0f, reference.Burial) + 0.0005f)
			{
				return OutsideCorners <= reference.OutsideCorners;
			}
			return false;
		}
	}
}
