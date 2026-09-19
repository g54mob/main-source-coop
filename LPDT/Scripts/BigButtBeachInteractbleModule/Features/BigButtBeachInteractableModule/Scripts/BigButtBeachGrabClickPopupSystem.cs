using System;
using System.Collections.Generic;
using System.Linq;
using Features.CameraModelModule;
using Features.GrabModule.Scripts;
using Features.WorldTokenModule.Scripts;
using Features.WorldTokenModule.Scripts.Views;
using PlayerCustomization;
using UnityEngine;
using Zenject;

namespace Features.BigButtBeachInteractableModule.Scripts
{
	public class BigButtBeachGrabClickPopupSystem : IInitializable, IDisposable
	{
		private static readonly Vector3 FallbackOffset = Vector3.up;

		private readonly BigButtBeachGrabClickedEventClass _grabClickedEventClass;

		private readonly IWorldTokenService _worldTokenService;

		private readonly CameraModel _cameraModel;

		private readonly PlayerCustomizationModel _playerCustomizationModel;

		private readonly List<BigButtWorldTokenPresenter> _worldTokenPool = new List<BigButtWorldTokenPresenter>();

		public BigButtBeachGrabClickPopupSystem(BigButtBeachGrabClickedEventClass grabClickedEventClass, IWorldTokenService worldTokenService, CameraModel cameraModel, PlayerCustomizationModel playerCustomizationModel)
		{
			_grabClickedEventClass = grabClickedEventClass;
			_worldTokenService = worldTokenService;
			_cameraModel = cameraModel;
			_playerCustomizationModel = playerCustomizationModel;
		}

		public void Initialize()
		{
			_grabClickedEventClass.OnGrabClicked += SpawnClickToken;
		}

		public void Dispose()
		{
			_grabClickedEventClass.OnGrabClicked -= SpawnClickToken;
			_worldTokenPool.Clear();
		}

		private void SpawnClickToken(BigButtBeachInteractable source, SimplePointGrabable grabable, int playerId)
		{
			if (!(source == null) && _worldTokenService.IsReady)
			{
				BigButtBeachCounterPopupSpawnData grabPopupSpawnData = GetGrabPopupSpawnData(source, grabable, playerId);
				BigButtWorldTokenPresenter worldTokenFromPool = GetWorldTokenFromPool(grabPopupSpawnData.Position);
				worldTokenFromPool.SetColor(GetPlayerPrimaryColor(playerId));
				worldTokenFromPool.SetMovementDirection(GetTokenMovementDirection(grabPopupSpawnData.MovementWorldDirection));
				worldTokenFromPool.SetCameraToLookAt(_cameraModel.CameraObject);
				worldTokenFromPool.StartMovement();
			}
		}

		private Color GetPlayerPrimaryColor(int playerId)
		{
			return _playerCustomizationModel.Slots.FirstOrDefault((PlayerCustomizationSlotData slot) => slot.PlayerId == playerId)?.PrimaryColor ?? Color.white;
		}

		private static BigButtBeachCounterPopupSpawnData GetGrabPopupSpawnData(BigButtBeachInteractable source, SimplePointGrabable grabable, int playerId)
		{
			if (source.TryGetGrabClickPopupSpawnData(grabable, playerId, out var spawnData))
			{
				return spawnData;
			}
			if (grabable != null && grabable.Handles != null && grabable.Handles.Count > 0 && grabable.Handles[0] != null)
			{
				return new BigButtBeachCounterPopupSpawnData(grabable.Handles[0].position, Vector3.up);
			}
			return new BigButtBeachCounterPopupSpawnData(source.transform.position + FallbackOffset, Vector3.up);
		}

		private Vector2 GetTokenMovementDirection(Vector3 movementWorldDirection)
		{
			if (_cameraModel.CameraObject == null)
			{
				return Vector2.up;
			}
			Vector3 vector = _cameraModel.CameraObject.transform.InverseTransformDirection(movementWorldDirection);
			Vector2 vector2 = new Vector2(vector.x, vector.y);
			if (!(vector2.sqrMagnitude > 0.0001f))
			{
				return Vector2.up;
			}
			return vector2.normalized;
		}

		private BigButtWorldTokenPresenter GetWorldTokenFromPool(Vector3 position)
		{
			for (int num = _worldTokenPool.Count - 1; num >= 0; num--)
			{
				if (!_worldTokenPool[num].IsActive())
				{
					BigButtWorldTokenPresenter bigButtWorldTokenPresenter = _worldTokenPool[num];
					bigButtWorldTokenPresenter.SetPosition(position);
					return bigButtWorldTokenPresenter;
				}
			}
			BigButtWorldTokenPresenter bigButtWorldTokenPresenter2 = _worldTokenService.CreateBigButtWWorldToken(WorldTokenType.BigButtBeachGrabClickIcon, position);
			_worldTokenPool.Add(bigButtWorldTokenPresenter2);
			return bigButtWorldTokenPresenter2;
		}
	}
}
