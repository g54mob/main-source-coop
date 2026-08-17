using System;
using System.Collections.Generic;

namespace Rewired.Utils.Classes.Utility
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class ObjectInstanceTracker : IDisposable
	{
		[CustomObfuscation(rename = false)]
		[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
		public class Wrapper<T> : IDisposable where T : class
		{
			public readonly T instance;

			public readonly uint instanceId;

			private readonly ObjectInstanceTracker HeCwfLiBAmhoAWcUetszDsTLMMcA;

			private bool JTnenNbhrKrYSPMtzKOWWuzEIJoyA;

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
				HeCwfLiBAmhoAWcUetszDsTLMMcA = P_1;
				instanceId = P_1.Register(P_0);
			}

			public void Dispose()
			{
				Dispose(disposing: true);
				GC.SuppressFinalize(this);
			}

			void IDisposable.Dispose()
			{
				//ILSpy generated this explicit interface implementation from .override directive in Dispose
				this.Dispose();
			}

			~Wrapper()
			{
				Dispose(disposing: false);
			}

			protected virtual void Dispose(bool disposing)
			{
				if (!JTnenNbhrKrYSPMtzKOWWuzEIJoyA)
				{
					if (HeCwfLiBAmhoAWcUetszDsTLMMcA != null)
					{
						HeCwfLiBAmhoAWcUetszDsTLMMcA.Unregister(instanceId);
					}
					JTnenNbhrKrYSPMtzKOWWuzEIJoyA = true;
				}
			}
		}

		private static ObjectInstanceTracker rzgbbUrRjKQAuTkbgeYIhcaCXeRdA;

		private readonly Dictionary<uint, object> HplvMlYJVvLucVAOHSaHFdwVJzIf = new Dictionary<uint, object>();

		private readonly object FiaQYLRmuoGDNKrGzUfkAdCVtWdl = new object();

		private uint mvVWMRzSgHIVmPyhXCKAWhRpPZqP;

		private int HaCjvIpykrTjQMIEnXJRiwliThXc;

		private bool hxefoAVzYOsoMbWpFoYYhrfrgiRP;

		public static ObjectInstanceTracker Default => rzgbbUrRjKQAuTkbgeYIhcaCXeRdA ?? (rzgbbUrRjKQAuTkbgeYIhcaCXeRdA = new ObjectInstanceTracker());

		public uint Register(object instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			HaCjvIpykrTjQMIEnXJRiwliThXc++;
			uint num = mvVWMRzSgHIVmPyhXCKAWhRpPZqP++;
			HplvMlYJVvLucVAOHSaHFdwVJzIf.Add(num, instance);
			return num;
		}

		public void Unregister(uint instanceId)
		{
			HaCjvIpykrTjQMIEnXJRiwliThXc--;
			if (HaCjvIpykrTjQMIEnXJRiwliThXc < 0)
			{
				HaCjvIpykrTjQMIEnXJRiwliThXc = 0;
			}
			lock (FiaQYLRmuoGDNKrGzUfkAdCVtWdl)
			{
				HplvMlYJVvLucVAOHSaHFdwVJzIf.Remove(instanceId);
			}
		}

		public bool TryGetInstance<T>(uint instanceId, out T instance) where T : class
		{
			lock (FiaQYLRmuoGDNKrGzUfkAdCVtWdl)
			{
				if (!HplvMlYJVvLucVAOHSaHFdwVJzIf.TryGetValue(instanceId, out var value))
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
			gXoObCcLCNYDNJuFMPpbqaMiAZgu(true);
			GC.SuppressFinalize(this);
		}

		void IDisposable.Dispose()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Dispose
			this.Dispose();
		}

		private void gXoObCcLCNYDNJuFMPpbqaMiAZgu(bool P_0)
		{
			if (!hxefoAVzYOsoMbWpFoYYhrfrgiRP)
			{
				if (this == rzgbbUrRjKQAuTkbgeYIhcaCXeRdA)
				{
					rzgbbUrRjKQAuTkbgeYIhcaCXeRdA = null;
				}
				hxefoAVzYOsoMbWpFoYYhrfrgiRP = true;
			}
		}

		~ObjectInstanceTracker()
		{
			gXoObCcLCNYDNJuFMPpbqaMiAZgu(false);
		}
	}
}
