using UnityEngine;

namespace Features.OcclusionModule.Scripts
{
	public static class OcclusionRoomQuery
	{
		private const float CONTAINMENT_GEOMETRY_MARGIN = 3f;

		public static bool IsPointInsideRoom(OcclusionRoom room, Bounds roomBounds, Vector3 point)
		{
			if (!roomBounds.Contains(point))
			{
				return false;
			}
			return room.SqrDistanceToNearestRenderer(point) <= 9f;
		}

		public static bool TryGetContainingRoom(Vector3 point, out OcclusionRoom containingRoom)
		{
			containingRoom = null;
			float num = float.MaxValue;
			OcclusionRoom[] rooms = OcclusionRegistry.GetRooms();
			foreach (OcclusionRoom occlusionRoom in rooms)
			{
				if (!(occlusionRoom == null) && occlusionRoom.HasBounds && IsPointInsideRoom(occlusionRoom, occlusionRoom.RendererBounds, point))
				{
					float sqrMagnitude = (occlusionRoom.transform.position - point).sqrMagnitude;
					if (!(sqrMagnitude >= num))
					{
						num = sqrMagnitude;
						containingRoom = occlusionRoom;
					}
				}
			}
			return containingRoom != null;
		}

		public static bool TryGetFloorHeight(Vector3 point, out float floorHeight)
		{
			if (!TryGetContainingRoom(point, out var containingRoom))
			{
				floorHeight = 0f;
				return false;
			}
			floorHeight = containingRoom.RendererBounds.min.y;
			return true;
		}
	}
}
