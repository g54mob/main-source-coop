using System.Collections.Generic;

namespace FishNet.Object
{
	public class NetworkObjectIdComparer : IEqualityComparer<NetworkObject>
	{
		public bool Equals(NetworkObject x, NetworkObject y)
		{
			bool flag = (object)x == null;
			bool flag2 = (object)y == null;
			if (flag != flag2)
			{
				return false;
			}
			if (flag && flag2)
			{
				return true;
			}
			return x.ObjectId == y.ObjectId;
		}

		public int GetHashCode(NetworkObject obj)
		{
			return obj.ObjectId;
		}
	}
}
