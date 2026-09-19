using System.Collections.Generic;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using TMPro;
using UnityEngine;
using Zenject;

namespace Features.StoreModule.Scripts
{
	public class StoreCustomizeView : MonoBehaviour
	{
		[SerializeField]
		private List<TMP_Text> _nickNames;

		[SerializeField]
		private List<Transform> _containersTransforms;

		private IStoreSeatingService _storeSeatingService;

		private MultiplayerModel _multiplayerModel;

		private int _cachedLocalIndex = -1;

		private PlayerMovableModel _playerMovableModel;

		[Inject]
		private void InjectDependencies(IStoreSeatingService storeSeatingService, MultiplayerModel multiplayerModel, PlayerMovableModel playerMovableModel)
		{
			_storeSeatingService = storeSeatingService;
			_multiplayerModel = multiplayerModel;
			_playerMovableModel = playerMovableModel;
		}

		private void OnEnable()
		{
			_cachedLocalIndex = _storeSeatingService.GetSeatIndex(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
			for (int i = 0; i < _nickNames.Count; i++)
			{
				int playerIdBySeat = _storeSeatingService.GetPlayerIdBySeat(i);
				_nickNames[i].SetText((playerIdBySeat < 0) ? string.Empty : (_storeSeatingService.GetHandle(playerIdBySeat) ?? string.Empty));
			}
		}

		private void Update()
		{
			if (_cachedLocalIndex != -1 && !(_playerMovableModel.LocalMovable == null) && _cachedLocalIndex < _containersTransforms.Count && !(_containersTransforms[_cachedLocalIndex] == null))
			{
				Vector3 forward = _playerMovableModel.LocalMovable.transform.position - _containersTransforms[_cachedLocalIndex].position;
				forward.y = 0f;
				forward.Normalize();
				_containersTransforms[_cachedLocalIndex].forward = forward;
			}
		}
	}
}
