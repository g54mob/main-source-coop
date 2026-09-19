using System.Collections.Generic;
using Features.Movement.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.StoreModule.Scripts
{
	public class StoreStatsRotator : MonoBehaviour
	{
		[SerializeField]
		private Transform _rotationTransform;

		private PlayerMovableModel _playerMovableModel;

		private bool _isInitialized;

		private int _playerId;

		private PlayerCharacterMovableBase _targteMovable;

		[Inject]
		private void InjectDependencies(PlayerMovableModel playerMovableModel)
		{
			_playerMovableModel = playerMovableModel;
		}

		public void LateUpdate()
		{
			if (!_isInitialized)
			{
				return;
			}
			if (_targteMovable == null)
			{
				foreach (KeyValuePair<PlayerRef, PlayerCharacterMovableBase> allCharacterMovable in _playerMovableModel.AllCharacterMovables)
				{
					if (allCharacterMovable.Key.PlayerId == _playerId)
					{
						_targteMovable = allCharacterMovable.Value;
					}
				}
			}
			if (!(_targteMovable == null) && !(_targteMovable.RotatoblePart == null))
			{
				Vector3 forward = _targteMovable.RotatoblePart.forward;
				forward.y = 1f;
				base.transform.forward = forward.normalized;
			}
		}

		public void SetPlayerId(int playerId)
		{
			_playerId = playerId;
			_isInitialized = true;
		}
	}
}
