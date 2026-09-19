using UnityEngine;

namespace MCPForUnity.Runtime.Helpers
{
	public static class UnityObjectIdCompat
	{
		public static int GetInstanceIDCompat(this Object obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return obj.GetInstanceID();
		}
	}
}
