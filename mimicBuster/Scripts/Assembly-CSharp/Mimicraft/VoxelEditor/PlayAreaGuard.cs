using Mimicraft.Gameplay;
using Mimicraft.Localization;
using Mimicraft.UI;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.VoxelEditor
{
	public static class PlayAreaGuard
	{
		private const float WarnCooldownSeconds = 3f;

		private static float lastWarned = float.NegativeInfinity;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			lastWarned = float.NegativeInfinity;
		}

		public static bool Allows(Transform space, VoxelGrid grid)
		{
			if (space == null || grid == null || !MapBounds.AnyBaked)
			{
				return true;
			}
			return BodyClearance.CountOutsideCorners(PieceShape.Of(grid), space.localToWorldMatrix) == 0;
		}

		public static void Refuse()
		{
			if (!(Time.unscaledTime - lastWarned < 3f))
			{
				lastWarned = Time.unscaledTime;
				if (ToastView.Instance != null)
				{
					ToastView.Instance.Show(Loc.Get("Tool.CannotLeaveMap"));
				}
			}
		}
	}
}
