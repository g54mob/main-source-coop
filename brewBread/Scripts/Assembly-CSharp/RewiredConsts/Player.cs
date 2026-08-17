using Rewired.Dev;

namespace RewiredConsts
{
	public static class Player
	{
		[PlayerIdFieldInfo(friendlyName = "System")]
		public const int System = 9999999;

		[PlayerIdFieldInfo(friendlyName = "Bread")]
		public const int Bread = 0;

		[PlayerIdFieldInfo(friendlyName = "Fred")]
		public const int Fred = 1;

		[PlayerIdFieldInfo(friendlyName = "Greg")]
		public const int Greg = 2;

		[PlayerIdFieldInfo(friendlyName = "UI")]
		public const int UI = 3;
	}
}
