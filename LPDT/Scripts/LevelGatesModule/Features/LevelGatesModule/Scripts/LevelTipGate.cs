using System;
using System.Collections;
using System.Collections.Generic;
using Features.LevelModule.Scripts;
using Features.Movement.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.LevelGatesModule.Scripts
{
	public class LevelTipGate : MonoBehaviour, IGate, IPlayerReboundListener
	{
		[SerializeField]
		private GateType _gateType;

		[SerializeField]
		private GateGroup _gateGroup;

		[SerializeField]
		private List<Transform> _gateLightTransforms;

		[SerializeField]
		private Light _gateLight;

		[SerializeField]
		private bool _isInitiallyActivated;

		private float _originalLightIntensity;

		private Light _cachedLight;

		private Transform _gateTransform;

		private Coroutine _activationRoutine;

		private PlayerMovableModel _playerMovableModel;

		private LevelGateConfiguration _levelGateConfiguration;

		private PlayerReboundModel _playerReboundModel;

		private LevelModel _levelModel;

		private RoomTerritoryModel _roomTerritoryModel;

		private Light CachedLight
		{
			get
			{
				if ((object)_cachedLight == null)
				{
					_cachedLight = UnityEngine.Object.Instantiate(_gateLight, base.transform);
				}
				return _cachedLight;
			}
		}

		public Vector3 Position => base.transform.position;

		public Vector3 Forward
		{
			get
			{
				Transform transform = null;
				transform = ((_gateTransform == null) ? _gateLightTransforms[0] : _gateTransform);
				return transform.position - new Vector3(base.transform.position.x, transform.position.y, base.transform.position.z);
			}
		}

		public GateType GateType => _gateType;

		public GateGroup GateGroup => _gateGroup;

		public bool IsActivated { get; private set; }

		[Inject]
		public void InjectDependencies(PlayerMovableModel playerMovableModel, LevelGateConfiguration levelGateConfiguration, PlayerReboundModel playerReboundModel, LevelModel levelModel, RoomTerritoryModel roomTerritoryModel)
		{
			_playerMovableModel = playerMovableModel;
			_levelGateConfiguration = levelGateConfiguration;
			_playerReboundModel = playerReboundModel;
			_levelModel = levelModel;
			_roomTerritoryModel = roomTerritoryModel;
		}

		private void Start()
		{
			_playerReboundModel.OnPlayerRebound += HandlePlayerRebound;
			TryInitializeGate();
		}

		private void OnDestroy()
		{
			_playerReboundModel.OnPlayerRebound -= HandlePlayerRebound;
		}

		private void HandlePlayerRebound(PlayerRef playerRef, NetworkObject avatar)
		{
			OnPlayerRebound(playerRef, avatar);
		}

		public void OnPlayerRebound(PlayerRef playerRef, NetworkObject avatar)
		{
			_gateTransform = null;
			if (avatar == null || !avatar.IsValid)
			{
				DeactivateGate(immediate: true);
			}
			else
			{
				TryInitializeGate();
			}
		}

		private void TryInitializeGate()
		{
			if (!(_playerMovableModel.LocalMovable == null))
			{
				_originalLightIntensity = CachedLight.intensity;
				if (_isInitiallyActivated)
				{
					ActivateGate(_playerMovableModel.LocalMovable.GetPosition(), immediate: true);
				}
				else
				{
					DeactivateGate(immediate: true);
				}
			}
		}

		private void Update()
		{
			if (IsActivated && _activationRoutine == null && !(_playerMovableModel.LocalMovable == null))
			{
				Vector3 position = _playerMovableModel.LocalMovable.GetPosition();
				position.y = Position.y;
				CachedLight.intensity = CalculateIntensityByDistance(position);
			}
		}

		public void ActivateGate(Vector3 observationPoint, bool immediate = false)
		{
			_gateTransform = SelectLightTransform(observationPoint);
			if (_gateTransform == null)
			{
				return;
			}
			IsActivated = true;
			if (_activationRoutine != null)
			{
				StopCoroutine(_activationRoutine);
			}
			CachedLight.transform.position = _gateTransform.position;
			CachedLight.transform.rotation = _gateTransform.rotation;
			if (_playerMovableModel.LocalMovable == null)
			{
				return;
			}
			if (immediate)
			{
				CachedLight.intensity = CalculateIntensityByDistance(_playerMovableModel.LocalMovable.GetPosition());
				return;
			}
			_activationRoutine = StartCoroutine(ActivationRoutine(_levelGateConfiguration.GateActivationTime, () => CalculateIntensityByDistance(_playerMovableModel.LocalMovable.GetPosition())));
		}

		public void DeactivateGate(bool immediate = false)
		{
			IsActivated = false;
			if (_activationRoutine != null)
			{
				StopCoroutine(_activationRoutine);
			}
			if (immediate)
			{
				CachedLight.intensity = 0f;
			}
			else
			{
				_activationRoutine = StartCoroutine(ActivationRoutine(_levelGateConfiguration.GateActivationTime, () => 0f));
			}
			_gateTransform = null;
		}

		private Transform SelectLightTransform(Vector3 observationPoint)
		{
			if (_gateLightTransforms == null || _gateLightTransforms.Count == 0)
			{
				return null;
			}
			Transform transform = null;
			if (_levelGateConfiguration != null && _levelModel != null && _levelGateConfiguration.GetTerritoryMode(_levelModel.CurrentLevel) == PlayerTerritoryMode.RoomFootprints)
			{
				transform = SelectLightTransformByRoom(observationPoint);
			}
			if (transform == null)
			{
				transform = SelectLightTransformByOppositeDot(observationPoint);
			}
			if (transform == null)
			{
				transform = _gateLightTransforms[0];
			}
			return transform;
		}

		private Transform SelectLightTransformByRoom(Vector3 observationPoint)
		{
			Vector3 vector = observationPoint;
			if (_playerMovableModel != null && _playerMovableModel.LocalMovable != null)
			{
				vector = _playerMovableModel.LocalMovable.GetPosition();
			}
			if (_gateType == GateType.Exit && _roomTerritoryModel != null)
			{
				Transform transform = FirstContainerWhere((Transform container) => !_roomTerritoryModel.IsInsideInteriorTerritory(container.position));
				Transform transform2 = FirstContainerWhere((Transform container) => _roomTerritoryModel.IsInsideInteriorTerritory(container.position));
				if (transform != null && transform2 != null)
				{
					return transform;
				}
			}
			if (_roomTerritoryModel != null && _roomTerritoryModel.TryGetContainingInterior(vector, out var playerRoom))
			{
				Transform transform3 = FirstContainerWhere((Transform container) => !playerRoom.Contains(container.position));
				if (transform3 != null)
				{
					return transform3;
				}
			}
			return SelectLightTransformByOppositeDot(vector);
		}

		private Transform SelectLightTransformByOppositeDot(Vector3 observationPoint)
		{
			Vector3 gatePosition = base.transform.position;
			Vector3 vector = new Vector3(observationPoint.x, gatePosition.y, observationPoint.z) - gatePosition;
			if (vector.sqrMagnitude < 0.0001f)
			{
				return null;
			}
			Vector3 observerDirection = vector.normalized;
			return FirstContainerWhere(delegate(Transform container)
			{
				Vector3 vector2 = container.position - new Vector3(gatePosition.x, container.position.y, gatePosition.z);
				return !(vector2.sqrMagnitude < 0.0001f) && Vector3.Dot(observerDirection, vector2.normalized) < 0f;
			});
		}

		private Transform FirstContainerWhere(Func<Transform, bool> predicate)
		{
			for (int i = 0; i < _gateLightTransforms.Count; i++)
			{
				Transform transform = _gateLightTransforms[i];
				if (!(transform == null) && predicate(transform))
				{
					return transform;
				}
			}
			return null;
		}

		private IEnumerator ActivationRoutine(float activationTime, Func<float> intensityAction)
		{
			float timer = 0f;
			float cachedLightIntensity = CachedLight.intensity;
			while (timer < activationTime)
			{
				float t = Mathf.Clamp01(timer / activationTime);
				CachedLight.intensity = Mathf.Lerp(cachedLightIntensity, intensityAction(), t);
				timer += Time.deltaTime;
				yield return null;
			}
			CachedLight.intensity = intensityAction();
			_activationRoutine = null;
		}

		private float CalculateIntensityByDistance(Vector3 playerPosition)
		{
			return Mathf.Lerp(0f, _originalLightIntensity, Mathf.Clamp01(Vector3.Distance(playerPosition, Position) / _levelGateConfiguration.DistanceToMaxGateGain));
		}
	}
}
