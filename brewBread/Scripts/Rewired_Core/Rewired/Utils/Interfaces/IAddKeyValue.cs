namespace Rewired.Utils.Interfaces
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal interface IAddKeyValue<TKey, TValue>
	{
		void Add(TKey key, TValue value);
	}
}
