namespace Rewired
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	internal class SteamAction
	{
		public readonly string name;

		public readonly ulong handle;

		public SteamAction(string P_0, ulong P_1)
		{
			name = P_0;
			handle = P_1;
		}
	}
}
