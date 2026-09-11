using System;

namespace Photon.Voice
{
	public struct DeviceInfo
	{
		private DeviceFeatures features;

		private bool useStringID;

		public static readonly DeviceInfo Default = new DeviceInfo(isDefault: true, -128, "", "[Default]");

		public bool IsDefault { get; private set; }

		public int IDInt { get; private set; }

		public string IDString { get; private set; }

		public string Name { get; private set; }

		public DeviceFeatures Features
		{
			get
			{
				if (features != null)
				{
					return features;
				}
				return DeviceFeatures.Default;
			}
		}

		private DeviceInfo(bool isDefault, int idInt, string idString, string name, DeviceFeatures features = null)
		{
			IsDefault = isDefault;
			IDInt = idInt;
			IDString = idString;
			Name = name;
			useStringID = false;
			this.features = features;
		}

		public DeviceInfo(int id, string name, DeviceFeatures features = null)
		{
			IsDefault = false;
			IDInt = id;
			IDString = "";
			Name = name;
			useStringID = false;
			this.features = features;
		}

		public DeviceInfo(string id, string name, DeviceFeatures features = null)
		{
			IsDefault = false;
			IDInt = 0;
			IDString = id;
			Name = name;
			useStringID = true;
			this.features = features;
		}

		public DeviceInfo(string name, DeviceFeatures features = null)
		{
			IsDefault = false;
			IDInt = 0;
			IDString = name;
			Name = name;
			useStringID = true;
			this.features = features;
		}

		public static bool operator ==(DeviceInfo d1, DeviceInfo d2)
		{
			return d1.Equals(d2);
		}

		public static bool operator !=(DeviceInfo d1, DeviceInfo d2)
		{
			return !d1.Equals(d2);
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			if (useStringID)
			{
				return ((Name == null) ? "" : Name) + ((IDString == null || IDString == Name) ? "" : (" (" + IDString.Substring(0, Math.Min(10, IDString.Length)) + ")"));
			}
			return $"{Name} ({IDInt})";
		}
	}
}
