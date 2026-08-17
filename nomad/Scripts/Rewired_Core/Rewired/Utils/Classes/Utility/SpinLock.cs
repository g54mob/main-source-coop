using System;
using System.Threading;

namespace Rewired.Utils.Classes.Utility
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal sealed class SpinLock : IDisposable
	{
		private const int xbLflesvIEhPLCfxcFINbXzPVdoCA = 1;

		private const int ZUADtWegoianmfGEjPiWESzThuBPA = 0;

		private int vXdBHScSlnRzadDjgcPIiCGvIzof;

		void IDisposable.Dispose()
		{
			VkpAoDSgnnvyAwbHlHehvWKrBUWI();
		}

		private void jNiCdeUTncDsTJbhHgTwkStVnICCb()
		{
			while (Interlocked.Exchange(ref vXdBHScSlnRzadDjgcPIiCGvIzof, 1) != 0)
			{
				Thread.SpinWait(1);
			}
		}

		private void VkpAoDSgnnvyAwbHlHehvWKrBUWI()
		{
			Interlocked.Exchange(ref vXdBHScSlnRzadDjgcPIiCGvIzof, 0);
		}

		public SpinLock Lock()
		{
			jNiCdeUTncDsTJbhHgTwkStVnICCb();
			return this;
		}
	}
}
