using System.Collections.Generic;

namespace Fusion
{
	internal class NetworkInterestedObjectList
	{
		private HashSet<NetworkObjectConnectionData> _active = new HashSet<NetworkObjectConnectionData>();

		public HashSet<NetworkObjectConnectionData>.Enumerator Active => _active.GetEnumerator();

		public void SetIdle(NetworkObjectConnectionData item)
		{
			if (_active.Contains(item))
			{
				_active.Remove(item);
			}
		}

		public void SetActive(NetworkObjectConnectionData item, NetworkObjectMeta meta)
		{
			_active.Add(item);
		}

		public void Remove(NetworkObjectConnectionData item)
		{
			_active.Remove(item);
		}

		public bool IsActive(NetworkObjectConnectionData data)
		{
			return _active.Contains(data);
		}
	}
}
