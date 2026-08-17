using System;
using System.Collections.Generic;

namespace Rewired.Utils.Classes.Utility
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class ObjectInstanceTracker : IDisposable
	{
		[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
		[CustomObfuscation(rename = false)]
		public class Wrapper<T> : IDisposable where T : class
		{
			public readonly T instance;

			public readonly uint instanceId;

			private readonly ObjectInstanceTracker rPmAbhCtLNMndubZWkWpZIqzCVzVA;

			private bool AeWaeWamxRrERkciQkpFDWbRfZMkA;

			public Wrapper(T P_0)
				: this(P_0, Default)
			{
			}

			public Wrapper(T P_0, ObjectInstanceTracker P_1)
			{
				if (P_0 == null)
				{
					throw new ArgumentNullException("instance");
				}
				if (P_1 == null)
				{
					throw new ArgumentNullException("tracker");
				}
				instance = P_0;
				rPmAbhCtLNMndubZWkWpZIqzCVzVA = P_1;
				instanceId = P_1.Register(P_0);
			}

			public void Dispose()
			{
				Dispose(disposing: true);
				GC.SuppressFinalize(this);
			}

			~Wrapper()
			{
				Dispose(disposing: false);
			}

			protected virtual void Dispose(bool disposing)
			{
				if (!AeWaeWamxRrERkciQkpFDWbRfZMkA)
				{
					if (rPmAbhCtLNMndubZWkWpZIqzCVzVA != null)
					{
						rPmAbhCtLNMndubZWkWpZIqzCVzVA.Unregister(instanceId);
					}
					AeWaeWamxRrERkciQkpFDWbRfZMkA = true;
				}
			}
		}

		private static ObjectInstanceTracker sKlAxiBmvyBxAjrDeOltNvBiEdkP;

		private readonly Dictionary<uint, object> uLVFtSGVlWpKOAXAzHqzFEiYcoKMA = new Dictionary<uint, object>();

		private readonly object hGYDsRExQHitzTdLrtalwgFHamjpA = new object();

		private uint yvHmjtsrJoGkbQczHLFlXRLXbdpX;

		private int CpmIjziOpPELRjKVLKljMysvOoke;

		private bool RpkeFpZxPdgBKAWcRCmHCNgWumJ;

		public static ObjectInstanceTracker Default => sKlAxiBmvyBxAjrDeOltNvBiEdkP ?? (sKlAxiBmvyBxAjrDeOltNvBiEdkP = new ObjectInstanceTracker());

		public uint Register(object instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			CpmIjziOpPELRjKVLKljMysvOoke++;
			uint num = yvHmjtsrJoGkbQczHLFlXRLXbdpX++;
			uLVFtSGVlWpKOAXAzHqzFEiYcoKMA.Add(num, instance);
			return num;
		}

		public void Unregister(uint instanceId)
		{
			CpmIjziOpPELRjKVLKljMysvOoke--;
			if (CpmIjziOpPELRjKVLKljMysvOoke < 0)
			{
				CpmIjziOpPELRjKVLKljMysvOoke = 0;
			}
			lock (hGYDsRExQHitzTdLrtalwgFHamjpA)
			{
				uLVFtSGVlWpKOAXAzHqzFEiYcoKMA.Remove(instanceId);
			}
		}

		public bool TryGetInstance<T>(uint instanceId, out T instance) where T : class
		{
			lock (hGYDsRExQHitzTdLrtalwgFHamjpA)
			{
				if (!uLVFtSGVlWpKOAXAzHqzFEiYcoKMA.TryGetValue(instanceId, out var value))
				{
					instance = null;
					return false;
				}
				if (value is T)
				{
					instance = (T)value;
					return true;
				}
				instance = null;
				return false;
			}
		}

		public void Dispose()
		{
			oTWKNdZEHnDTTJoUPKlJFjuBkKdNb(true);
			GC.SuppressFinalize(this);
		}

		private void oTWKNdZEHnDTTJoUPKlJFjuBkKdNb(bool P_0)
		{
			if (!RpkeFpZxPdgBKAWcRCmHCNgWumJ)
			{
				if (this == sKlAxiBmvyBxAjrDeOltNvBiEdkP)
				{
					sKlAxiBmvyBxAjrDeOltNvBiEdkP = null;
				}
				RpkeFpZxPdgBKAWcRCmHCNgWumJ = true;
			}
		}

		~ObjectInstanceTracker()
		{
			oTWKNdZEHnDTTJoUPKlJFjuBkKdNb(false);
		}
	}
}
