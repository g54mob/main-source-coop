using UnityEngine;

namespace EvilCore.Extensions
{
	public static class CustomDirectionExtensions
	{
		public static Vector3 GetGlobalDirectionVector(this CustomDirection direction)
		{
			return direction switch
			{
				CustomDirection.Right => Vector3.right, 
				CustomDirection.Left => Vector3.left, 
				CustomDirection.Up => Vector3.up, 
				CustomDirection.Down => Vector3.down, 
				CustomDirection.Forward => Vector3.forward, 
				CustomDirection.Backward => Vector3.back, 
				_ => Vector3.zero, 
			};
		}

		public static Vector3 GetTransformDirectionVector(this CustomDirection direction, Transform t)
		{
			return direction switch
			{
				CustomDirection.Right => t.right, 
				CustomDirection.Left => -t.right, 
				CustomDirection.Up => t.up, 
				CustomDirection.Down => -t.up, 
				CustomDirection.Forward => t.forward, 
				CustomDirection.Backward => -t.forward, 
				_ => Vector3.zero, 
			};
		}
	}
}
