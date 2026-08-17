using System;
using System.Threading;

namespace Rewired.Utils.Classes.Utility
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal class LockedObject<T> : IDisposable
	{
		public T item;

		private readonly object hGYDsRExQHitzTdLrtalwgFHamjpA;

		private bool azEbMXJVPUbjKiSxhuFTpIAHlwEdE;

		public LockedObject()
		{
			hGYDsRExQHitzTdLrtalwgFHamjpA = new object();
		}

		public LockedObject(object P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("lockObject");
			}
			hGYDsRExQHitzTdLrtalwgFHamjpA = P_0;
		}

		public void Lock()
		{
			if (azEbMXJVPUbjKiSxhuFTpIAHlwEdE)
			{
				throw new Exception("Already locked. Dispose must be called before Lock can be called again.");
			}
			Monitor.Enter(hGYDsRExQHitzTdLrtalwgFHamjpA);
			azEbMXJVPUbjKiSxhuFTpIAHlwEdE = true;
		}

		public void Unlock()
		{
			if (!azEbMXJVPUbjKiSxhuFTpIAHlwEdE)
			{
				throw new Exception("Not locked. Lock must be called before Dispose.");
			}
			Monitor.Exit(hGYDsRExQHitzTdLrtalwgFHamjpA);
			azEbMXJVPUbjKiSxhuFTpIAHlwEdE = false;
		}

		void IDisposable.Dispose()
		{
			Unlock();
		}
	}
}
