using System;
using System.Threading;

namespace Rewired.Utils.Classes.Utility
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal struct Locker : IDisposable
	{
		private object QMmaBzbGhLRYBQhUiAbwUcRQwInF;

		public Locker(object P_0)
		{
			QMmaBzbGhLRYBQhUiAbwUcRQwInF = P_0;
			if (P_0 != null)
			{
				Monitor.Enter(P_0);
			}
		}

		public void Dispose()
		{
			if (QMmaBzbGhLRYBQhUiAbwUcRQwInF != null)
			{
				Monitor.Exit(QMmaBzbGhLRYBQhUiAbwUcRQwInF);
				QMmaBzbGhLRYBQhUiAbwUcRQwInF = null;
			}
		}
	}
}
