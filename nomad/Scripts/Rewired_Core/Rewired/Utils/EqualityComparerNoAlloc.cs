using System;
using System.Collections;
using System.Collections.Generic;
using Rewired.Utils.Classes.Data;

namespace Rewired.Utils
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal static class EqualityComparerNoAlloc<T>
	{
		private class WiRkHcuSNiASnEEvBqYyAQpWEGlhA : IEqualityComparer, IEqualityComparer<int>
		{
			private static WiRkHcuSNiASnEEvBqYyAQpWEGlhA BVMnFjRKYpUsNdkUxNnnKUFrEThG;

			public static WiRkHcuSNiASnEEvBqYyAQpWEGlhA sJUZrHZMMRSUFAShWYGzyvvBcDcf => BVMnFjRKYpUsNdkUxNnnKUFrEThG ?? (BVMnFjRKYpUsNdkUxNnnKUFrEThG = new WiRkHcuSNiASnEEvBqYyAQpWEGlhA());

			public bool Equals(int x, int y)
			{
				return x == y;
			}

			bool IEqualityComparer<int>.Equals(int x, int y)
			{
				//ILSpy generated this explicit interface implementation from .override directive in Equals
				return this.Equals(x, y);
			}

			public int GetHashCode(int obj)
			{
				return obj.GetHashCode();
			}

			int IEqualityComparer<int>.GetHashCode(int obj)
			{
				//ILSpy generated this explicit interface implementation from .override directive in GetHashCode
				return this.GetHashCode(obj);
			}

			bool IEqualityComparer.Equals(object x, object y)
			{
				if (x == null)
				{
					if (y == null)
					{
						return true;
					}
					return false;
				}
				if (!(x is int) || !(y is int))
				{
					return false;
				}
				return Equals((int)x, (int)y);
			}

			int IEqualityComparer.GetHashCode(object obj)
			{
				if (obj == null || !(obj is int))
				{
					return 0;
				}
				return GetHashCode((int)obj);
			}
		}

		private class jkfAxedPjmtZisiflFUwEHMmeTJoA : IEqualityComparer, IEqualityComparer<ulong>
		{
			private static jkfAxedPjmtZisiflFUwEHMmeTJoA rXHGPrzVlxaWltfrgYczPsRMgiPu;

			public static jkfAxedPjmtZisiflFUwEHMmeTJoA BjbIWmXOSHTjoFkWZQupmVQDoUKc => rXHGPrzVlxaWltfrgYczPsRMgiPu ?? (rXHGPrzVlxaWltfrgYczPsRMgiPu = new jkfAxedPjmtZisiflFUwEHMmeTJoA());

			public bool Equals(ulong x, ulong y)
			{
				return x == y;
			}

			bool IEqualityComparer<ulong>.Equals(ulong x, ulong y)
			{
				//ILSpy generated this explicit interface implementation from .override directive in Equals
				return this.Equals(x, y);
			}

			public int GetHashCode(ulong obj)
			{
				return obj.GetHashCode();
			}

			int IEqualityComparer<ulong>.GetHashCode(ulong obj)
			{
				//ILSpy generated this explicit interface implementation from .override directive in GetHashCode
				return this.GetHashCode(obj);
			}

			bool IEqualityComparer.Equals(object x, object y)
			{
				if (x == null)
				{
					if (y == null)
					{
						return true;
					}
					return false;
				}
				if (!(x is ulong) || !(y is ulong))
				{
					return false;
				}
				return Equals((ulong)x, (ulong)y);
			}

			int IEqualityComparer.GetHashCode(object obj)
			{
				if (obj == null || !(obj is ulong))
				{
					return 0;
				}
				return GetHashCode((ulong)obj);
			}
		}

		private class xYhGXATpwZiUHAEAssHQqFyYRpAd : IEqualityComparer, IEqualityComparer<uint>
		{
			private static xYhGXATpwZiUHAEAssHQqFyYRpAd frKIxqvyuzTOtrwqMVaokruDaYx;

			public static xYhGXATpwZiUHAEAssHQqFyYRpAd xZVcuDHLfCVChizvmdaQZRYbjjgy => frKIxqvyuzTOtrwqMVaokruDaYx ?? (frKIxqvyuzTOtrwqMVaokruDaYx = new xYhGXATpwZiUHAEAssHQqFyYRpAd());

			public bool Equals(uint x, uint y)
			{
				return x == y;
			}

			bool IEqualityComparer<uint>.Equals(uint x, uint y)
			{
				//ILSpy generated this explicit interface implementation from .override directive in Equals
				return this.Equals(x, y);
			}

			public int GetHashCode(uint obj)
			{
				return obj.GetHashCode();
			}

			int IEqualityComparer<uint>.GetHashCode(uint obj)
			{
				//ILSpy generated this explicit interface implementation from .override directive in GetHashCode
				return this.GetHashCode(obj);
			}

			bool IEqualityComparer.Equals(object x, object y)
			{
				if (x == null)
				{
					if (y == null)
					{
						return true;
					}
					return false;
				}
				if (!(x is uint) || !(y is uint))
				{
					return false;
				}
				return Equals((uint)x, (uint)y);
			}

			int IEqualityComparer.GetHashCode(object obj)
			{
				if (obj == null || !(obj is uint))
				{
					return 0;
				}
				return GetHashCode((uint)obj);
			}
		}

		private class UczGKldrgOTKOUbRrgHOHOISuivTA : IEqualityComparer, IEqualityComparer<ulong>
		{
			private static UczGKldrgOTKOUbRrgHOHOISuivTA etSECIJBbHWQqRbHUNkvaDgiRRUHc;

			public static UczGKldrgOTKOUbRrgHOHOISuivTA IONWYrByzebBexXwjjVCDxmUJQym => etSECIJBbHWQqRbHUNkvaDgiRRUHc ?? (etSECIJBbHWQqRbHUNkvaDgiRRUHc = new UczGKldrgOTKOUbRrgHOHOISuivTA());

			public bool Equals(ulong x, ulong y)
			{
				return x == y;
			}

			bool IEqualityComparer<ulong>.Equals(ulong x, ulong y)
			{
				//ILSpy generated this explicit interface implementation from .override directive in Equals
				return this.Equals(x, y);
			}

			public int GetHashCode(ulong obj)
			{
				return obj.GetHashCode();
			}

			int IEqualityComparer<ulong>.GetHashCode(ulong obj)
			{
				//ILSpy generated this explicit interface implementation from .override directive in GetHashCode
				return this.GetHashCode(obj);
			}

			bool IEqualityComparer.Equals(object x, object y)
			{
				if (x == null)
				{
					if (y == null)
					{
						return true;
					}
					return false;
				}
				if (!(x is ulong) || !(y is ulong))
				{
					return false;
				}
				return Equals((ulong)x, (ulong)y);
			}

			int IEqualityComparer.GetHashCode(object obj)
			{
				if (obj == null || !(obj is ulong))
				{
					return 0;
				}
				return GetHashCode((ulong)obj);
			}
		}

		private class NJokPVZdSkcjkXRlKeHCyJGEhzrA : IEqualityComparer, IEqualityComparer<float>
		{
			private static NJokPVZdSkcjkXRlKeHCyJGEhzrA bbGJNrmmbJzoHEdRZXhEyKAZkOMk;

			public static NJokPVZdSkcjkXRlKeHCyJGEhzrA fFWVHvpTrRDCoGfGOeYDhUmiaZdu => bbGJNrmmbJzoHEdRZXhEyKAZkOMk ?? (bbGJNrmmbJzoHEdRZXhEyKAZkOMk = new NJokPVZdSkcjkXRlKeHCyJGEhzrA());

			public bool Equals(float x, float y)
			{
				return x == y;
			}

			bool IEqualityComparer<float>.Equals(float x, float y)
			{
				//ILSpy generated this explicit interface implementation from .override directive in Equals
				return this.Equals(x, y);
			}

			public int GetHashCode(float obj)
			{
				return obj.GetHashCode();
			}

			int IEqualityComparer<float>.GetHashCode(float obj)
			{
				//ILSpy generated this explicit interface implementation from .override directive in GetHashCode
				return this.GetHashCode(obj);
			}

			bool IEqualityComparer.Equals(object x, object y)
			{
				if (x == null)
				{
					if (y == null)
					{
						return true;
					}
					return false;
				}
				if (!(x is float) || !(y is float))
				{
					return false;
				}
				return Equals((float)x, (float)y);
			}

			int IEqualityComparer.GetHashCode(object obj)
			{
				if (obj == null || !(obj is float))
				{
					return 0;
				}
				return GetHashCode((float)obj);
			}
		}

		private class StaAxRnssEvVcQGdEHWYNkXjTSrK : IEqualityComparer, IEqualityComparer<double>
		{
			private static StaAxRnssEvVcQGdEHWYNkXjTSrK LkWzjSmboDvbWqyeqqJdSbsxUjRF;

			public static StaAxRnssEvVcQGdEHWYNkXjTSrK UWSmAwlndjOkNrJFNEiXiHDjFKYqA => LkWzjSmboDvbWqyeqqJdSbsxUjRF ?? (LkWzjSmboDvbWqyeqqJdSbsxUjRF = new StaAxRnssEvVcQGdEHWYNkXjTSrK());

			public bool Equals(double x, double y)
			{
				return x == y;
			}

			bool IEqualityComparer<double>.Equals(double x, double y)
			{
				//ILSpy generated this explicit interface implementation from .override directive in Equals
				return this.Equals(x, y);
			}

			public int GetHashCode(double obj)
			{
				return obj.GetHashCode();
			}

			int IEqualityComparer<double>.GetHashCode(double obj)
			{
				//ILSpy generated this explicit interface implementation from .override directive in GetHashCode
				return this.GetHashCode(obj);
			}

			bool IEqualityComparer.Equals(object x, object y)
			{
				if (x == null)
				{
					if (y == null)
					{
						return true;
					}
					return false;
				}
				if (!(x is double) || !(y is double))
				{
					return false;
				}
				return Equals((double)x, (double)y);
			}

			int IEqualityComparer.GetHashCode(object obj)
			{
				if (obj == null || !(obj is double))
				{
					return 0;
				}
				return GetHashCode((double)obj);
			}
		}

		private class WTHRGstifqrjytfIaiCWlCeEzMof : IEqualityComparer, IEqualityComparer<byte>
		{
			private static WTHRGstifqrjytfIaiCWlCeEzMof zOcnjiqlNPOnfrYJulrdrynwdCdw;

			public static WTHRGstifqrjytfIaiCWlCeEzMof EDigCYCteJznCWVkkqLjmmhqrkBeA => zOcnjiqlNPOnfrYJulrdrynwdCdw ?? (zOcnjiqlNPOnfrYJulrdrynwdCdw = new WTHRGstifqrjytfIaiCWlCeEzMof());

			public bool Equals(byte x, byte y)
			{
				return x == y;
			}

			bool IEqualityComparer<byte>.Equals(byte x, byte y)
			{
				//ILSpy generated this explicit interface implementation from .override directive in Equals
				return this.Equals(x, y);
			}

			public int GetHashCode(byte obj)
			{
				return obj.GetHashCode();
			}

			int IEqualityComparer<byte>.GetHashCode(byte obj)
			{
				//ILSpy generated this explicit interface implementation from .override directive in GetHashCode
				return this.GetHashCode(obj);
			}

			bool IEqualityComparer.Equals(object x, object y)
			{
				if (x == null)
				{
					if (y == null)
					{
						return true;
					}
					return false;
				}
				if (!(x is byte) || !(y is byte))
				{
					return false;
				}
				return Equals((byte)x, (byte)y);
			}

			int IEqualityComparer.GetHashCode(object obj)
			{
				if (obj == null || !(obj is byte))
				{
					return 0;
				}
				return GetHashCode((byte)obj);
			}
		}

		private class RSduRztikeSlaGEMtigQrcgktPoe : IEqualityComparer, IEqualityComparer<sbyte>
		{
			private static RSduRztikeSlaGEMtigQrcgktPoe tGXEgrAZnGLnFlGgWnmepfOkZDpO;

			public static RSduRztikeSlaGEMtigQrcgktPoe hrmiidggxtakMCFZNHTFKcGjVTXbA => tGXEgrAZnGLnFlGgWnmepfOkZDpO ?? (tGXEgrAZnGLnFlGgWnmepfOkZDpO = new RSduRztikeSlaGEMtigQrcgktPoe());

			public bool Equals(sbyte x, sbyte y)
			{
				return x == y;
			}

			bool IEqualityComparer<sbyte>.Equals(sbyte x, sbyte y)
			{
				//ILSpy generated this explicit interface implementation from .override directive in Equals
				return this.Equals(x, y);
			}

			public int GetHashCode(sbyte obj)
			{
				return obj.GetHashCode();
			}

			int IEqualityComparer<sbyte>.GetHashCode(sbyte obj)
			{
				//ILSpy generated this explicit interface implementation from .override directive in GetHashCode
				return this.GetHashCode(obj);
			}

			bool IEqualityComparer.Equals(object x, object y)
			{
				if (x == null)
				{
					if (y == null)
					{
						return true;
					}
					return false;
				}
				if (!(x is sbyte) || !(y is sbyte))
				{
					return false;
				}
				return Equals((sbyte)x, (sbyte)y);
			}

			int IEqualityComparer.GetHashCode(object obj)
			{
				if (obj == null || !(obj is sbyte))
				{
					return 0;
				}
				return GetHashCode((sbyte)obj);
			}
		}

		private class uuymSKUXgfLAoAiFbSUUrVAhbcPM : IEqualityComparer, IEqualityComparer<bool>
		{
			private static uuymSKUXgfLAoAiFbSUUrVAhbcPM jfGjtiXvMMjoENsWigjOrwCvevUk;

			public static uuymSKUXgfLAoAiFbSUUrVAhbcPM KGgUYwohHElFblYDmsuqfJknUWzL => jfGjtiXvMMjoENsWigjOrwCvevUk ?? (jfGjtiXvMMjoENsWigjOrwCvevUk = new uuymSKUXgfLAoAiFbSUUrVAhbcPM());

			public bool Equals(bool x, bool y)
			{
				return x == y;
			}

			bool IEqualityComparer<bool>.Equals(bool x, bool y)
			{
				//ILSpy generated this explicit interface implementation from .override directive in Equals
				return this.Equals(x, y);
			}

			public int GetHashCode(bool obj)
			{
				return obj.GetHashCode();
			}

			int IEqualityComparer<bool>.GetHashCode(bool obj)
			{
				//ILSpy generated this explicit interface implementation from .override directive in GetHashCode
				return this.GetHashCode(obj);
			}

			bool IEqualityComparer.Equals(object x, object y)
			{
				if (x == null)
				{
					if (y == null)
					{
						return true;
					}
					return false;
				}
				if (!(x is bool) || !(y is bool))
				{
					return false;
				}
				return Equals((bool)x, (bool)y);
			}

			int IEqualityComparer.GetHashCode(object obj)
			{
				if (obj == null || !(obj is bool))
				{
					return 0;
				}
				return GetHashCode((bool)obj);
			}
		}

		private class atxaexmzMndhmkXqtpzYPUfKvNkt : IEqualityComparer, IEqualityComparer<IntPtr>
		{
			private static atxaexmzMndhmkXqtpzYPUfKvNkt ywMDlJhvqheWNeMTCjovyUQIPgywB;

			public static atxaexmzMndhmkXqtpzYPUfKvNkt OmCAyQbAOeUClkwFqhEvrLQynZnwA => ywMDlJhvqheWNeMTCjovyUQIPgywB ?? (ywMDlJhvqheWNeMTCjovyUQIPgywB = new atxaexmzMndhmkXqtpzYPUfKvNkt());

			public bool Equals(IntPtr x, IntPtr y)
			{
				return x == y;
			}

			bool IEqualityComparer<IntPtr>.Equals(IntPtr x, IntPtr y)
			{
				//ILSpy generated this explicit interface implementation from .override directive in Equals
				return this.Equals(x, y);
			}

			public int GetHashCode(IntPtr obj)
			{
				return obj.GetHashCode();
			}

			int IEqualityComparer<IntPtr>.GetHashCode(IntPtr obj)
			{
				//ILSpy generated this explicit interface implementation from .override directive in GetHashCode
				return this.GetHashCode(obj);
			}

			bool IEqualityComparer.Equals(object x, object y)
			{
				if (x == null)
				{
					if (y == null)
					{
						return true;
					}
					return false;
				}
				if (!(x is IntPtr) || !(y is IntPtr))
				{
					return false;
				}
				return Equals((IntPtr)x, (IntPtr)y);
			}

			int IEqualityComparer.GetHashCode(object obj)
			{
				if (obj == null || !(obj is IntPtr))
				{
					return 0;
				}
				return GetHashCode((IntPtr)obj);
			}
		}

		private class sWiLjHVUAGXxgkTXTorxyprWXfMG : IEqualityComparer, IEqualityComparer<Guid>
		{
			private static sWiLjHVUAGXxgkTXTorxyprWXfMG DZqbgOUzYgYFpdLmYwWnMQQoePjb;

			public static sWiLjHVUAGXxgkTXTorxyprWXfMG IbAoOvXFpACQOzxmaiycOcPmYkNo => DZqbgOUzYgYFpdLmYwWnMQQoePjb ?? (DZqbgOUzYgYFpdLmYwWnMQQoePjb = new sWiLjHVUAGXxgkTXTorxyprWXfMG());

			public bool Equals(Guid x, Guid y)
			{
				return x == y;
			}

			bool IEqualityComparer<Guid>.Equals(Guid x, Guid y)
			{
				//ILSpy generated this explicit interface implementation from .override directive in Equals
				return this.Equals(x, y);
			}

			public int GetHashCode(Guid obj)
			{
				return obj.GetHashCode();
			}

			int IEqualityComparer<Guid>.GetHashCode(Guid obj)
			{
				//ILSpy generated this explicit interface implementation from .override directive in GetHashCode
				return this.GetHashCode(obj);
			}

			bool IEqualityComparer.Equals(object x, object y)
			{
				if (x == null)
				{
					if (y == null)
					{
						return true;
					}
					return false;
				}
				if (!(x is Guid) || !(y is Guid))
				{
					return false;
				}
				return Equals((Guid)x, (Guid)y);
			}

			int IEqualityComparer.GetHashCode(object obj)
			{
				if (obj == null || !(obj is Guid))
				{
					return 0;
				}
				return GetHashCode((Guid)obj);
			}
		}

		private class DEGFHKIYpHKiZLsxvDnIUmncmLst : IEqualityComparer, IEqualityComparer<Bytes20>
		{
			private static DEGFHKIYpHKiZLsxvDnIUmncmLst fGtzXanXEuwldLCfQLitYUCpLzkf;

			public static DEGFHKIYpHKiZLsxvDnIUmncmLst EahAIndjuwabpQvxCxVqmjWtgLeA => fGtzXanXEuwldLCfQLitYUCpLzkf ?? (fGtzXanXEuwldLCfQLitYUCpLzkf = new DEGFHKIYpHKiZLsxvDnIUmncmLst());

			public bool Equals(Bytes20 x, Bytes20 y)
			{
				return x == y;
			}

			bool IEqualityComparer<Bytes20>.Equals(Bytes20 x, Bytes20 y)
			{
				//ILSpy generated this explicit interface implementation from .override directive in Equals
				return this.Equals(x, y);
			}

			public int GetHashCode(Bytes20 obj)
			{
				return obj.GetHashCode();
			}

			int IEqualityComparer<Bytes20>.GetHashCode(Bytes20 obj)
			{
				//ILSpy generated this explicit interface implementation from .override directive in GetHashCode
				return this.GetHashCode(obj);
			}

			bool IEqualityComparer.Equals(object x, object y)
			{
				if (x == null)
				{
					if (y == null)
					{
						return true;
					}
					return false;
				}
				if (!(x is Bytes20) || !(y is Bytes20))
				{
					return false;
				}
				return Equals((Bytes20)x, (Bytes20)y);
			}

			int IEqualityComparer.GetHashCode(object obj)
			{
				if (obj == null || !(obj is Bytes20))
				{
					return 0;
				}
				return GetHashCode((Bytes20)obj);
			}
		}

		public static IEqualityComparer<T> Default
		{
			get
			{
				Type typeFromHandle = typeof(T);
				if ((object)typeFromHandle == typeof(int))
				{
					return (IEqualityComparer<T>)WiRkHcuSNiASnEEvBqYyAQpWEGlhA.sJUZrHZMMRSUFAShWYGzyvvBcDcf;
				}
				if ((object)typeFromHandle == typeof(long))
				{
					return (IEqualityComparer<T>)jkfAxedPjmtZisiflFUwEHMmeTJoA.BjbIWmXOSHTjoFkWZQupmVQDoUKc;
				}
				if ((object)typeFromHandle == typeof(uint))
				{
					return (IEqualityComparer<T>)xYhGXATpwZiUHAEAssHQqFyYRpAd.xZVcuDHLfCVChizvmdaQZRYbjjgy;
				}
				if ((object)typeFromHandle == typeof(ulong))
				{
					return (IEqualityComparer<T>)UczGKldrgOTKOUbRrgHOHOISuivTA.IONWYrByzebBexXwjjVCDxmUJQym;
				}
				if ((object)typeFromHandle == typeof(float))
				{
					return (IEqualityComparer<T>)NJokPVZdSkcjkXRlKeHCyJGEhzrA.fFWVHvpTrRDCoGfGOeYDhUmiaZdu;
				}
				if ((object)typeFromHandle == typeof(double))
				{
					return (IEqualityComparer<T>)StaAxRnssEvVcQGdEHWYNkXjTSrK.UWSmAwlndjOkNrJFNEiXiHDjFKYqA;
				}
				if ((object)typeFromHandle == typeof(byte))
				{
					return (IEqualityComparer<T>)WTHRGstifqrjytfIaiCWlCeEzMof.EDigCYCteJznCWVkkqLjmmhqrkBeA;
				}
				if ((object)typeFromHandle == typeof(sbyte))
				{
					return (IEqualityComparer<T>)RSduRztikeSlaGEMtigQrcgktPoe.hrmiidggxtakMCFZNHTFKcGjVTXbA;
				}
				if ((object)typeFromHandle == typeof(bool))
				{
					return (IEqualityComparer<T>)uuymSKUXgfLAoAiFbSUUrVAhbcPM.KGgUYwohHElFblYDmsuqfJknUWzL;
				}
				if ((object)typeFromHandle == typeof(IntPtr))
				{
					return (IEqualityComparer<T>)atxaexmzMndhmkXqtpzYPUfKvNkt.OmCAyQbAOeUClkwFqhEvrLQynZnwA;
				}
				if ((object)typeFromHandle == typeof(Guid))
				{
					return (IEqualityComparer<T>)sWiLjHVUAGXxgkTXTorxyprWXfMG.IbAoOvXFpACQOzxmaiycOcPmYkNo;
				}
				if ((object)typeFromHandle == typeof(Bytes20))
				{
					return (IEqualityComparer<T>)DEGFHKIYpHKiZLsxvDnIUmncmLst.EahAIndjuwabpQvxCxVqmjWtgLeA;
				}
				return EqualityComparer<T>.Default;
			}
		}
	}
}
