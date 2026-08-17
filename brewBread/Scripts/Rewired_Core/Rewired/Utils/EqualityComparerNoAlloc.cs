using System;
using System.Collections;
using System.Collections.Generic;

namespace Rewired.Utils
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal static class EqualityComparerNoAlloc<T>
	{
		private class grfdsoINuCRxugqWrFSetQwseIat : IEqualityComparer, IEqualityComparer<int>
		{
			private static grfdsoINuCRxugqWrFSetQwseIat mJiiUbHNLBekzUQakGJdNUzzeWhy;

			public static grfdsoINuCRxugqWrFSetQwseIat vhJlHqbDISDRgELdbflITCiWLdGrA => mJiiUbHNLBekzUQakGJdNUzzeWhy ?? (mJiiUbHNLBekzUQakGJdNUzzeWhy = new grfdsoINuCRxugqWrFSetQwseIat());

			public bool Equals(int x, int y)
			{
				return x == y;
			}

			public int GetHashCode(int obj)
			{
				return obj.GetHashCode();
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

		private class ZhLAixnYiQsnQYAPmEqGgDCMXUPb : IEqualityComparer, IEqualityComparer<ulong>
		{
			private static ZhLAixnYiQsnQYAPmEqGgDCMXUPb mJiiUbHNLBekzUQakGJdNUzzeWhy;

			public static ZhLAixnYiQsnQYAPmEqGgDCMXUPb vhJlHqbDISDRgELdbflITCiWLdGrA => mJiiUbHNLBekzUQakGJdNUzzeWhy ?? (mJiiUbHNLBekzUQakGJdNUzzeWhy = new ZhLAixnYiQsnQYAPmEqGgDCMXUPb());

			public bool Equals(ulong x, ulong y)
			{
				return x == y;
			}

			public int GetHashCode(ulong obj)
			{
				return obj.GetHashCode();
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

		private class PLTxrKfAIpWTVoptIOmUTMOoxBNO : IEqualityComparer, IEqualityComparer<uint>
		{
			private static PLTxrKfAIpWTVoptIOmUTMOoxBNO mJiiUbHNLBekzUQakGJdNUzzeWhy;

			public static PLTxrKfAIpWTVoptIOmUTMOoxBNO vhJlHqbDISDRgELdbflITCiWLdGrA => mJiiUbHNLBekzUQakGJdNUzzeWhy ?? (mJiiUbHNLBekzUQakGJdNUzzeWhy = new PLTxrKfAIpWTVoptIOmUTMOoxBNO());

			public bool Equals(uint x, uint y)
			{
				return x == y;
			}

			public int GetHashCode(uint obj)
			{
				return obj.GetHashCode();
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

		private class urPdLpVCDoehBcPoVKZKrQBcTeis : IEqualityComparer, IEqualityComparer<ulong>
		{
			private static urPdLpVCDoehBcPoVKZKrQBcTeis mJiiUbHNLBekzUQakGJdNUzzeWhy;

			public static urPdLpVCDoehBcPoVKZKrQBcTeis vhJlHqbDISDRgELdbflITCiWLdGrA => mJiiUbHNLBekzUQakGJdNUzzeWhy ?? (mJiiUbHNLBekzUQakGJdNUzzeWhy = new urPdLpVCDoehBcPoVKZKrQBcTeis());

			public bool Equals(ulong x, ulong y)
			{
				return x == y;
			}

			public int GetHashCode(ulong obj)
			{
				return obj.GetHashCode();
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

		private class rIfdJBejMupBidriDZqDoVYcivqHc : IEqualityComparer, IEqualityComparer<float>
		{
			private static rIfdJBejMupBidriDZqDoVYcivqHc mJiiUbHNLBekzUQakGJdNUzzeWhy;

			public static rIfdJBejMupBidriDZqDoVYcivqHc vhJlHqbDISDRgELdbflITCiWLdGrA => mJiiUbHNLBekzUQakGJdNUzzeWhy ?? (mJiiUbHNLBekzUQakGJdNUzzeWhy = new rIfdJBejMupBidriDZqDoVYcivqHc());

			public bool Equals(float x, float y)
			{
				return x == y;
			}

			public int GetHashCode(float obj)
			{
				return obj.GetHashCode();
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

		private class iWAKEFHBJywKnolOesMOkkSJcRsN : IEqualityComparer, IEqualityComparer<double>
		{
			private static iWAKEFHBJywKnolOesMOkkSJcRsN mJiiUbHNLBekzUQakGJdNUzzeWhy;

			public static iWAKEFHBJywKnolOesMOkkSJcRsN vhJlHqbDISDRgELdbflITCiWLdGrA => mJiiUbHNLBekzUQakGJdNUzzeWhy ?? (mJiiUbHNLBekzUQakGJdNUzzeWhy = new iWAKEFHBJywKnolOesMOkkSJcRsN());

			public bool Equals(double x, double y)
			{
				return x == y;
			}

			public int GetHashCode(double obj)
			{
				return obj.GetHashCode();
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

		private class wArbswEROYSIldLWMJeAXCdoxbzuA : IEqualityComparer, IEqualityComparer<byte>
		{
			private static wArbswEROYSIldLWMJeAXCdoxbzuA mJiiUbHNLBekzUQakGJdNUzzeWhy;

			public static wArbswEROYSIldLWMJeAXCdoxbzuA vhJlHqbDISDRgELdbflITCiWLdGrA => mJiiUbHNLBekzUQakGJdNUzzeWhy ?? (mJiiUbHNLBekzUQakGJdNUzzeWhy = new wArbswEROYSIldLWMJeAXCdoxbzuA());

			public bool Equals(byte x, byte y)
			{
				return x == y;
			}

			public int GetHashCode(byte obj)
			{
				return obj.GetHashCode();
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

		private class pgPVDfNdBEzptuRxNJcIMFzQPxtU : IEqualityComparer, IEqualityComparer<sbyte>
		{
			private static pgPVDfNdBEzptuRxNJcIMFzQPxtU mJiiUbHNLBekzUQakGJdNUzzeWhy;

			public static pgPVDfNdBEzptuRxNJcIMFzQPxtU vhJlHqbDISDRgELdbflITCiWLdGrA => mJiiUbHNLBekzUQakGJdNUzzeWhy ?? (mJiiUbHNLBekzUQakGJdNUzzeWhy = new pgPVDfNdBEzptuRxNJcIMFzQPxtU());

			public bool Equals(sbyte x, sbyte y)
			{
				return x == y;
			}

			public int GetHashCode(sbyte obj)
			{
				return obj.GetHashCode();
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

		private class IcWrOSiMDDhNzufmVeKWGAVZosCw : IEqualityComparer, IEqualityComparer<bool>
		{
			private static IcWrOSiMDDhNzufmVeKWGAVZosCw mJiiUbHNLBekzUQakGJdNUzzeWhy;

			public static IcWrOSiMDDhNzufmVeKWGAVZosCw vhJlHqbDISDRgELdbflITCiWLdGrA => mJiiUbHNLBekzUQakGJdNUzzeWhy ?? (mJiiUbHNLBekzUQakGJdNUzzeWhy = new IcWrOSiMDDhNzufmVeKWGAVZosCw());

			public bool Equals(bool x, bool y)
			{
				return x == y;
			}

			public int GetHashCode(bool obj)
			{
				return obj.GetHashCode();
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

		private class WWHkXhMltDeCdMtPRcvAkhcyfHjbA : IEqualityComparer, IEqualityComparer<IntPtr>
		{
			private static WWHkXhMltDeCdMtPRcvAkhcyfHjbA mJiiUbHNLBekzUQakGJdNUzzeWhy;

			public static WWHkXhMltDeCdMtPRcvAkhcyfHjbA vhJlHqbDISDRgELdbflITCiWLdGrA => mJiiUbHNLBekzUQakGJdNUzzeWhy ?? (mJiiUbHNLBekzUQakGJdNUzzeWhy = new WWHkXhMltDeCdMtPRcvAkhcyfHjbA());

			public bool Equals(IntPtr x, IntPtr y)
			{
				return x == y;
			}

			public int GetHashCode(IntPtr obj)
			{
				return obj.GetHashCode();
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

		public static IEqualityComparer<T> Default
		{
			get
			{
				Type typeFromHandle = typeof(T);
				if ((object)typeFromHandle == typeof(int))
				{
					return (IEqualityComparer<T>)grfdsoINuCRxugqWrFSetQwseIat.vhJlHqbDISDRgELdbflITCiWLdGrA;
				}
				if ((object)typeFromHandle == typeof(long))
				{
					return (IEqualityComparer<T>)ZhLAixnYiQsnQYAPmEqGgDCMXUPb.vhJlHqbDISDRgELdbflITCiWLdGrA;
				}
				if ((object)typeFromHandle == typeof(uint))
				{
					return (IEqualityComparer<T>)PLTxrKfAIpWTVoptIOmUTMOoxBNO.vhJlHqbDISDRgELdbflITCiWLdGrA;
				}
				if ((object)typeFromHandle == typeof(ulong))
				{
					return (IEqualityComparer<T>)urPdLpVCDoehBcPoVKZKrQBcTeis.vhJlHqbDISDRgELdbflITCiWLdGrA;
				}
				if ((object)typeFromHandle == typeof(float))
				{
					return (IEqualityComparer<T>)rIfdJBejMupBidriDZqDoVYcivqHc.vhJlHqbDISDRgELdbflITCiWLdGrA;
				}
				if ((object)typeFromHandle == typeof(double))
				{
					return (IEqualityComparer<T>)iWAKEFHBJywKnolOesMOkkSJcRsN.vhJlHqbDISDRgELdbflITCiWLdGrA;
				}
				if ((object)typeFromHandle == typeof(byte))
				{
					return (IEqualityComparer<T>)wArbswEROYSIldLWMJeAXCdoxbzuA.vhJlHqbDISDRgELdbflITCiWLdGrA;
				}
				if ((object)typeFromHandle == typeof(sbyte))
				{
					return (IEqualityComparer<T>)pgPVDfNdBEzptuRxNJcIMFzQPxtU.vhJlHqbDISDRgELdbflITCiWLdGrA;
				}
				if ((object)typeFromHandle == typeof(bool))
				{
					return (IEqualityComparer<T>)IcWrOSiMDDhNzufmVeKWGAVZosCw.vhJlHqbDISDRgELdbflITCiWLdGrA;
				}
				if ((object)typeFromHandle == typeof(IntPtr))
				{
					return (IEqualityComparer<T>)WWHkXhMltDeCdMtPRcvAkhcyfHjbA.vhJlHqbDISDRgELdbflITCiWLdGrA;
				}
				return EqualityComparer<T>.Default;
			}
		}
	}
}
