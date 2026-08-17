using System;
using System.Linq;
using UnityEngine;

namespace VisualDesignCafe.Packages
{
	[Serializable]
	public struct Version
	{
		[SerializeField]
		private string _bundleIdentifier;

		[SerializeField]
		private string _versionNumber;

		[SerializeField]
		private int _buildNumber;

		public bool IsValid => !string.IsNullOrEmpty(BundleIdentifier) && !string.IsNullOrEmpty(VersionNumber);

		public string BundleIdentifier => _bundleIdentifier;

		public string VersionNumber => _versionNumber;

		[Obsolete]
		public int BuildNumber => _buildNumber;

		public Version(string identifier, string versionNumber, int buildNumber)
		{
			_bundleIdentifier = identifier;
			_versionNumber = versionNumber;
			_buildNumber = buildNumber;
		}

		public override string ToString()
		{
			return BundleIdentifier + "." + VersionNumber + "." + _buildNumber;
		}

		public int CompareTo(in Version other)
		{
			if (string.IsNullOrEmpty(VersionNumber) && string.IsNullOrEmpty(other.VersionNumber))
			{
				return 0;
			}
			if (string.IsNullOrEmpty(VersionNumber))
			{
				return -1;
			}
			if (string.IsNullOrEmpty(other.VersionNumber))
			{
				return 1;
			}
			int[] array = (from s in VersionNumber.Split(new char[1] { '.' })
				select int.Parse(s)).ToArray();
			int[] array2 = (from s in other.VersionNumber.Split(new char[1] { '.' })
				select int.Parse(s)).ToArray();
			int num = Math.Min(array.Length, array2.Length);
			for (int num2 = 0; num2 < num; num2++)
			{
				if (array[num2] > array2[num2])
				{
					return 1;
				}
				if (array[num2] < array2[num2])
				{
					return -1;
				}
			}
			if (BuildNumber > 0 && other.BuildNumber > 0)
			{
				return BuildNumber.CompareTo(other.BuildNumber);
			}
			return 0;
		}
	}
}
