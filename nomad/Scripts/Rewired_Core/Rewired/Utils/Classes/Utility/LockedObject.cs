using System;
using System.Threading;

namespace Rewired.Utils.Classes.Utility
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class LockedObject<T> : IDisposable
	{
		public T item;

		private readonly object vApxoHJmCMxqGCTcyvHWyccDDSlT;

		private bool WJECeIWdRjgfbjqfsiHADMpZIEEJA;

		public LockedObject()
		{
			vApxoHJmCMxqGCTcyvHWyccDDSlT = new object();
		}

		public LockedObject(object P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("lockObject");
			}
			vApxoHJmCMxqGCTcyvHWyccDDSlT = P_0;
		}

		public void Lock()
		{
			if (WJECeIWdRjgfbjqfsiHADMpZIEEJA)
			{
				throw new Exception("Already locked. Dispose must be called before Lock can be called again.");
			}
			Monitor.Enter(vApxoHJmCMxqGCTcyvHWyccDDSlT);
			WJECeIWdRjgfbjqfsiHADMpZIEEJA = true;
		}

		public void Unlock()
		{
			if (!WJECeIWdRjgfbjqfsiHADMpZIEEJA)
			{
				throw new Exception("Not locked. Lock must be called before Dispose.");
			}
			Monitor.Exit(vApxoHJmCMxqGCTcyvHWyccDDSlT);
			WJECeIWdRjgfbjqfsiHADMpZIEEJA = false;
		}

		void IDisposable.Dispose()
		{
			Unlock();
		}
	}
}
