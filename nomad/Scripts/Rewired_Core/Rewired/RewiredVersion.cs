namespace Rewired
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	internal struct RewiredVersion
	{
		public int version1;

		public int version2;

		public int version3;

		public int version4;

		public string unityVersion;

		public RewiredVersion(int P_0, int P_1, int P_2, int P_3, string P_4)
		{
			version1 = P_0;
			version2 = P_1;
			version3 = P_2;
			version4 = P_3;
			unityVersion = P_4;
		}

		public RewiredVersion(string P_0)
		{
			if (!string.IsNullOrEmpty(P_0))
			{
				string[] array = P_0.Split('.');
				if (array.Length >= 4 && int.TryParse(array[0], out version1) && int.TryParse(array[1], out version2) && int.TryParse(array[2], out version3) && int.TryParse(array[3], out version4))
				{
					if (array.Length > 4)
					{
						unityVersion = array[4];
					}
					else
					{
						unityVersion = string.Empty;
					}
					return;
				}
			}
			version1 = 0;
			version2 = 0;
			version3 = 0;
			version4 = 0;
			unityVersion = string.Empty;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (!(obj is RewiredVersion rewiredVersion))
			{
				return false;
			}
			return this == rewiredVersion;
		}

		public override int GetHashCode()
		{
			return ((((17 * 29 + version1.GetHashCode()) * 29 + version2.GetHashCode()) * 29 + version3.GetHashCode()) * 29 + version4.GetHashCode()) * 29 + unityVersion.GetHashCode();
		}

		public override string ToString()
		{
			string text = version1 + "." + version2 + "." + version3 + "." + version4;
			if (!string.IsNullOrEmpty(unityVersion))
			{
				text = text + "." + unityVersion;
			}
			return text;
		}

		public static bool operator ==(RewiredVersion a, RewiredVersion b)
		{
			if ((object)a == (object)b)
			{
				return true;
			}
			if (a.version1 == b.version1 && a.version2 == b.version2 && a.version3 == b.version3 && a.version4 == b.version4)
			{
				return string.Equals(a.unityVersion, b.unityVersion);
			}
			return false;
		}

		public static bool operator !=(RewiredVersion a, RewiredVersion b)
		{
			return !(a == b);
		}

		public static bool operator >(RewiredVersion a, RewiredVersion b)
		{
			if (a == b)
			{
				return false;
			}
			if (a.version1 > b.version1)
			{
				return true;
			}
			if (a.version1 < b.version1)
			{
				return false;
			}
			if (a.version2 > b.version2)
			{
				return true;
			}
			if (a.version2 < b.version2)
			{
				return false;
			}
			if (a.version3 > b.version3)
			{
				return true;
			}
			if (a.version3 < b.version3)
			{
				return false;
			}
			if (a.version4 > b.version4)
			{
				return true;
			}
			_ = a.version4;
			_ = b.version4;
			return false;
		}

		public static bool operator <(RewiredVersion a, RewiredVersion b)
		{
			if (a == b)
			{
				return false;
			}
			if (a.version1 < b.version1)
			{
				return true;
			}
			if (a.version1 > b.version1)
			{
				return false;
			}
			if (a.version2 < b.version2)
			{
				return true;
			}
			if (a.version2 > b.version2)
			{
				return false;
			}
			if (a.version3 < b.version3)
			{
				return true;
			}
			if (a.version3 > b.version3)
			{
				return false;
			}
			if (a.version4 < b.version4)
			{
				return true;
			}
			_ = a.version4;
			_ = b.version4;
			return false;
		}
	}
}
