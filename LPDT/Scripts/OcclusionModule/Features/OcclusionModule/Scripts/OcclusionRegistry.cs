using System.Collections.Generic;

namespace Features.OcclusionModule.Scripts
{
	public static class OcclusionRegistry
	{
		private static readonly HashSet<OcclusionRoom> _rooms = new HashSet<OcclusionRoom>();

		private static readonly HashSet<OcclusionPortal> _portals = new HashSet<OcclusionPortal>();

		private static readonly List<OcclusionRoom> _uniqueRooms = new List<OcclusionRoom>();

		private static readonly HashSet<int> _seenRoots = new HashSet<int>();

		public static RoomOcclusionSceneMarker ActiveMarker { get; private set; }

		public static int RoomCount => _rooms.Count;

		public static void Register(OcclusionRoom room)
		{
			_rooms.Add(room);
		}

		public static void Unregister(OcclusionRoom room)
		{
			_rooms.Remove(room);
		}

		public static void Register(OcclusionPortal portal)
		{
			_portals.Add(portal);
		}

		public static void Unregister(OcclusionPortal portal)
		{
			_portals.Remove(portal);
		}

		public static void SetMarker(RoomOcclusionSceneMarker marker)
		{
			ActiveMarker = marker;
		}

		public static void ClearMarker(RoomOcclusionSceneMarker marker)
		{
			if (ActiveMarker == marker)
			{
				ActiveMarker = null;
			}
		}

		public static OcclusionRoom[] GetRooms()
		{
			_uniqueRooms.Clear();
			_seenRoots.Clear();
			foreach (OcclusionRoom room in _rooms)
			{
				if (room != null && _seenRoots.Add(room.gameObject.GetInstanceID()))
				{
					_uniqueRooms.Add(room);
				}
			}
			return _uniqueRooms.ToArray();
		}

		public static OcclusionPortal[] GetPortals()
		{
			OcclusionPortal[] array = new OcclusionPortal[_portals.Count];
			_portals.CopyTo(array);
			return array;
		}
	}
}
