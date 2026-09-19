using System;
using System.Collections.Generic;
using Features.CameraModelModule;
using Features.WorldTokenModule.Scripts;
using Features.WorldTokenModule.Scripts.Views;
using UnityEngine;
using Zenject;

namespace Features.BigButtBeachInteractableModule.Scripts
{
	public class BigButtBeachCounterPopupSystem : IInitializable, IDisposable
	{
		private const int MAX_ACTIVE_TOKENS = 10;

		private static readonly Vector3 TokenOffset = Vector3.up;

		private readonly BigButtBeachCounterChangedEventClass _counterChangedEventClass;

		private readonly IWorldTokenService _worldTokenService;

		private readonly CameraModel _cameraModel;

		private readonly List<BigButtWorldTokenPresenter> _worldTokenPool = new List<BigButtWorldTokenPresenter>();

		public BigButtBeachCounterPopupSystem(BigButtBeachCounterChangedEventClass counterChangedEventClass, IWorldTokenService worldTokenService, CameraModel cameraModel)
		{
			_counterChangedEventClass = counterChangedEventClass;
			_worldTokenService = worldTokenService;
			_cameraModel = cameraModel;
		}

		public void Initialize()
		{
			_counterChangedEventClass.OnCounterIncreased += SpawnCounterToken;
		}

		public void Dispose()
		{
			_counterChangedEventClass.OnCounterIncreased -= SpawnCounterToken;
			_worldTokenPool.Clear();
		}

		private void SpawnCounterToken(BigButtBeachInteractable source, long previousValue, long currentValue)
		{
			if (!(source == null) && _worldTokenService.IsReady && GetActiveTokenCount() < 10)
			{
				BigButtBeachCounterPopupSpawnData counterPopupSpawnData = source.GetCounterPopupSpawnData(TokenOffset);
				BigButtWorldTokenPresenter worldTokenFromPool = GetWorldTokenFromPool(counterPopupSpawnData.Position);
				worldTokenFromPool.SetMovementDirection(GetTokenMovementDirection(counterPopupSpawnData.MovementWorldDirection));
				worldTokenFromPool.SetCameraToLookAt(_cameraModel.CameraObject);
				worldTokenFromPool.StartMovement();
			}
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

		private int GetActiveTokenCount()
		{
			int num = 0;
			for (int i = 0; i < _worldTokenPool.Count; i++)
			{
				if (_worldTokenPool[i].IsActive())
				{
					num++;
				}
			}
			return num;
		}

		private BigButtWorldTokenPresenter GetWorldTokenFromPool(Vector3 position)
		{
			float h = UnityEngine.Random.Range(0f, 1f);
			float s = Mathf.Lerp(0.5f, 1f, UnityEngine.Random.Range(0f, 1f));
			float v = Mathf.Lerp(0.7f, 1f, UnityEngine.Random.Range(0f, 1f));
			Color color = Color.HSVToRGB(h, s, v);
			for (int num = _worldTokenPool.Count - 1; num >= 0; num--)
			{
				if (!_worldTokenPool[num].IsActive())
				{
					BigButtWorldTokenPresenter bigButtWorldTokenPresenter = _worldTokenPool[num];
					bigButtWorldTokenPresenter.SetPosition(position);
					bigButtWorldTokenPresenter.SetColor(color);
					return bigButtWorldTokenPresenter;
				}
			}
			BigButtWorldTokenPresenter bigButtWorldTokenPresenter2 = _worldTokenService.CreateBigButtWWorldToken(WorldTokenType.BigButtBeachCounterIcon, position);
			bigButtWorldTokenPresenter2.SetColor(color);
			_worldTokenPool.Add(bigButtWorldTokenPresenter2);
			return bigButtWorldTokenPresenter2;
		}
	}
}
