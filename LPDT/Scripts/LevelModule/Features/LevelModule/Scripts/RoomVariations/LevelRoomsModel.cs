using System;
using System.Collections.Generic;

namespace Features.LevelModule.Scripts.RoomVariations
{
	public class LevelRoomsModel
	{
		private static readonly IReadOnlyList<LevelRoomEntity> _emptyRooms = Array.Empty<LevelRoomEntity>();

		private readonly List<LevelRoomEntity> _rooms = new List<LevelRoomEntity>();

		private readonly Dictionary<RoomType, List<LevelRoomEntity>> _roomsByType = new Dictionary<RoomType, List<LevelRoomEntity>>();

		public IReadOnlyList<LevelRoomEntity> Rooms => _rooms;

		public event Action<LevelRoomEntity> OnRoomRegistered;

		public event Action<LevelRoomEntity> OnRoomUnregistered;

		public void Register(LevelRoomEntity room)
		{
			if (!(room == null) && room.RoomType != RoomType.None && !_rooms.Contains(room))
			{
				_rooms.Add(room);
				if (!_roomsByType.TryGetValue(room.RoomType, out var value))
				{
					value = new List<LevelRoomEntity>();
					_roomsByType[room.RoomType] = value;
				}
				value.Add(room);
				this.OnRoomRegistered?.Invoke(room);
			}
		}

		public void Unregister(LevelRoomEntity room)
		{
			if (!(room == null) && _rooms.Remove(room))
			{
				if (_roomsByType.TryGetValue(room.RoomType, out var value))
				{
					value.Remove(room);
				}
				this.OnRoomUnregistered?.Invoke(room);
			}
		}

		public IReadOnlyList<LevelRoomEntity> GetRooms(RoomType roomType)
		{
			if (!_roomsByType.TryGetValue(roomType, out var value))
			{
				return _emptyRooms;
			}
			return value;
		}

		public bool TryGetRooms(RoomType roomType, out IReadOnlyList<LevelRoomEntity> rooms)
		{
			if (_roomsByType.TryGetValue(roomType, out var value) && value.Count > 0)
			{
				rooms = value;
				return true;
			}
			rooms = _emptyRooms;
			return false;
		}

		public void Reset()
		{
			_rooms.Clear();
			_roomsByType.Clear();
		}
	}
}
