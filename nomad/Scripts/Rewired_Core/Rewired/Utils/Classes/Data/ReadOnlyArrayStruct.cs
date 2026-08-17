namespace Rewired.Utils.Classes.Data
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal struct ReadOnlyArrayStruct<T>
	{
		private T[] fQmANkHpsQYkhmFobwsjBcghZwwl;

		public int Length
		{
			get
			{
				if (fQmANkHpsQYkhmFobwsjBcghZwwl == null)
				{
					return 0;
				}
				return fQmANkHpsQYkhmFobwsjBcghZwwl.Length;
			}
		}

		public T this[int index] => fQmANkHpsQYkhmFobwsjBcghZwwl[index];

		public ReadOnlyArrayStruct(T[] P_0)
		{
			fQmANkHpsQYkhmFobwsjBcghZwwl = P_0;
		}
	}
}
