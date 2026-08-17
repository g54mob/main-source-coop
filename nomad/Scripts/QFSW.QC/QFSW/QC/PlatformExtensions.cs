using UnityEngine;

namespace QFSW.QC
{
	public static class PlatformExtensions
	{
		public static Platform ToPlatform(this RuntimePlatform pl)
		{
			return (Platform)(1L << (int)pl);
		}
	}
}
