namespace MessagePack.Internal
{
	internal interface IAsymmetricEqualityComparer<TKey1, TKey2>
	{
		int GetHashCode(TKey1 key1);

		int GetHashCode(TKey2 key2);

		bool Equals(TKey1 x, TKey1 y);

		bool Equals(TKey1 x, TKey2 y);
	}
}
