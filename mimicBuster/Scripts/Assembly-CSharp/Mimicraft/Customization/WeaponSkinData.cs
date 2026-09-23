using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.Customization
{
	public class WeaponSkinData
	{
		public VoxelGrid Grid;

		public Vector3 LeftGrip;

		public Vector3 RightGrip;

		public Vector3 Muzzle;

		public bool HasPoints;

		public Vector3 AdsPos;

		public bool HasAdsPos;

		public WeaponSoundKind Sound;

		public WeaponSkinData()
		{
		}

		public WeaponSkinData(VoxelGrid grid)
		{
			Grid = grid;
		}

		public void SetPoints(Vector3 leftGrip, Vector3 rightGrip, Vector3 muzzle)
		{
			LeftGrip = leftGrip;
			RightGrip = rightGrip;
			Muzzle = muzzle;
			HasPoints = true;
		}

		public void SetAdsPos(Vector3 adsPos)
		{
			AdsPos = adsPos;
			HasAdsPos = true;
		}
	}
}
