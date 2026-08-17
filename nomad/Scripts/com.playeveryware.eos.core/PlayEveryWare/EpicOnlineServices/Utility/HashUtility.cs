using System;

namespace PlayEveryWare.EpicOnlineServices.Utility
{
	public static class HashUtility
	{
		public static int Combine(params object[] fields)
		{
			return HashCode.Combine(fields);
		}
	}
}
