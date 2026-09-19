using UnityEngine;
using Zenject;

namespace Features.LevelModule.Scripts.RoomVariations
{
	[DisallowMultipleComponent]
	public class LevelRoomRegistrar : MonoBehaviour
	{
		[Tooltip("Room to register; if empty, uses a LevelRoomEntity on this GameObject.")]
		[SerializeField]
		private LevelRoomEntity _room;

		private LevelRoomsModel _levelRoomsModel;

		private LevelRoomEntity _registeredRoom;

		[Inject]
		private void InjectDependencies(LevelRoomsModel levelRoomsModel)
		{
			_levelRoomsModel = levelRoomsModel;
		}

		private void OnEnable()
		{
			_registeredRoom = ((_room != null) ? _room : GetComponent<LevelRoomEntity>());
			_levelRoomsModel.Register(_registeredRoom);
		}

		private void OnDisable()
		{
			_levelRoomsModel?.Unregister(_registeredRoom);
		}
	}
}
