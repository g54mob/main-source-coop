using UnityEngine;

namespace Fusion
{
	internal static class ObjectExtensions
	{
		public static int GetObjectId(this Object obj)
		{
			return obj.GetInstanceID();
		}
	}
}
