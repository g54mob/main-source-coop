namespace Rewired.Internal
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	internal interface IPrefetch
	{
		void Prefetch();
	}
}
