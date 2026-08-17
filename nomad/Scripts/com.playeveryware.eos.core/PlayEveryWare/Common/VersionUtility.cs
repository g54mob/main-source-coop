using System;

namespace PlayEveryWare.Common
{
	public static class VersionUtility
	{
		public static bool AreVersionsEqual(Version v1, Version v2)
		{
			if (v1.Major != v2.Major)
			{
				return false;
			}
			if (v1.Minor != v2.Minor)
			{
				return false;
			}
			if ((v1.Build != -1 && v2.Build == -1) || (v1.Build == -1 && v2.Build != -1))
			{
				return false;
			}
			if (v1.Build != -1 && v2.Build != -1 && v1.Build != v2.Build)
			{
				return false;
			}
			if ((v1.Revision != -1 && v2.Revision == -1) || (v1.Revision == -1 && v2.Revision != -1))
			{
				return false;
			}
			if (v1.Revision != -1 && v2.Revision != -1 && v1.Revision != v2.Revision)
			{
				return false;
			}
			return true;
		}
	}
}
