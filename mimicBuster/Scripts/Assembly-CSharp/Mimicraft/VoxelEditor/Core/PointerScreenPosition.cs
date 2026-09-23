using UnityEngine;
using UnityEngine.InputSystem;

namespace Mimicraft.VoxelEditor.Core
{
	public static class PointerScreenPosition
	{
		public static bool TryGet(out Vector2 position)
		{
			position = default(Vector2);
			if (Mouse.current == null)
			{
				return false;
			}
			Vector2 vector = Mouse.current.position.ReadValue();
			if (!IsFinite(vector))
			{
				return false;
			}
			position = vector;
			return true;
		}

		private static bool IsFinite(Vector2 v)
		{
			if (!float.IsNaN(v.x) && !float.IsInfinity(v.x) && !float.IsNaN(v.y))
			{
				return !float.IsInfinity(v.y);
			}
			return false;
		}
	}
}
