using System;

namespace Zorro.PhotonUtility
{
	public struct ListenerHandle : IComparable<ListenerHandle>
	{
		public Guid id { get; private set; }

		public static ListenerHandle Invalid => new ListenerHandle
		{
			id = Guid.Empty
		};

		public static ListenerHandle Create()
		{
			return new ListenerHandle
			{
				id = Guid.NewGuid()
			};
		}

		public int CompareTo(ListenerHandle other)
		{
			return id.CompareTo(other.id);
		}

		public override string ToString()
		{
			return "ListenerHandle (" + id.ToString() + ")";
		}
	}
}
