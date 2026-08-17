namespace Rewired.Utils.Classes.Data
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal struct ReadOnlyArrayStruct<T>
	{
		private T[] nGXUNsriBWFFnaAvKCPXkGJVjEVR;

		public int Length
		{
			get
			{
				if (nGXUNsriBWFFnaAvKCPXkGJVjEVR == null)
				{
					return 0;
				}
				return nGXUNsriBWFFnaAvKCPXkGJVjEVR.Length;
			}
		}

		public T this[int index] => nGXUNsriBWFFnaAvKCPXkGJVjEVR[index];

		public ReadOnlyArrayStruct(T[] P_0)
		{
			nGXUNsriBWFFnaAvKCPXkGJVjEVR = P_0;
		}
	}
}
