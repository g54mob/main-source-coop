using System;

namespace Zorro.Core
{
	[Serializable]
	public struct BuildVersion
	{
		public int MajorVersion;

		public int MinorVersion;

		public char PatchVersion;

		public BuildVersion(string version)
		{
			string[] array = version.Split('.');
			MajorVersion = int.Parse(array[0]);
			MinorVersion = int.Parse(array[1]);
			PatchVersion = array[2][0];
		}

		public override string ToString()
		{
			return $"{MajorVersion}.{MinorVersion}.{PatchVersion}";
		}

		public string ToMatchmaking()
		{
			return $"{MajorVersion}.{MinorVersion}";
		}
	}
}
