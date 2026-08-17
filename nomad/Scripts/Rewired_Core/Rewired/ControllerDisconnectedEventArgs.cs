using System;

namespace Rewired
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	internal class ControllerDisconnectedEventArgs : EventArgs
	{
		public readonly int rewiredId;

		public ControllerDisconnectedEventArgs(int P_0)
		{
			rewiredId = P_0;
		}
	}
}
