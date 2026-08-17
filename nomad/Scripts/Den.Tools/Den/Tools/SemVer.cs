using System;
using System.Runtime.InteropServices;

namespace Den.Tools
{
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	public struct SemVer
	{
		public enum PRT
		{
			Release = 4,
			RC = 3,
			Beta = 2,
			Alpha = 1
		}

		[FieldOffset(0)]
		public byte major;

		[FieldOffset(1)]
		public byte minor;

		[FieldOffset(2)]
		public byte prt;

		[FieldOffset(3)]
		public byte patch;

		[FieldOffset(0)]
		private int hash;

		[FieldOffset(0)]
		private uint summary;

		public string PRTtoString => prt switch
		{
			3 => "RC", 
			2 => "B", 
			1 => "A", 
			_ => "", 
		};

		public SemVer(byte major, byte minor, PRT prt, byte patch)
		{
			hash = 0;
			summary = 0u;
			this.major = major;
			this.minor = minor;
			this.prt = (byte)prt;
			this.patch = patch;
		}

		public SemVer(byte major, byte minor, byte patch)
			: this(major, minor, PRT.Release, patch)
		{
		}

		public override string ToString()
		{
			return $"{major}.{minor}.{PRTtoString}{patch}";
		}

		public bool Equals(SemVer obj)
		{
			return summary == obj.summary;
		}

		public override bool Equals(object obj)
		{
			return summary == ((SemVer)obj).summary;
		}

		public override int GetHashCode()
		{
			return hash;
		}

		public static bool operator ==(SemVer v1, SemVer v2)
		{
			return v1.summary == v2.summary;
		}

		public static bool operator !=(SemVer v1, SemVer v2)
		{
			return v1.summary != v2.summary;
		}

		public static bool operator <(SemVer v1, SemVer v2)
		{
			return v1.summary < v2.summary;
		}

		public static bool operator >(SemVer v1, SemVer v2)
		{
			return v1.summary > v2.summary;
		}

		public static bool operator <=(SemVer v1, SemVer v2)
		{
			return v1.summary <= v2.summary;
		}

		public static bool operator >=(SemVer v1, SemVer v2)
		{
			return v1.summary >= v2.summary;
		}
	}
}
