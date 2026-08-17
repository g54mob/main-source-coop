using GameKit.Utilities;

namespace FishNet.Broadcast.Helping
{
	internal static class BroadcastHelper
	{
		public static ushort GetKey<T>()
		{
			return typeof(T).FullName.GetStableHashU16();
		}
	}
}
