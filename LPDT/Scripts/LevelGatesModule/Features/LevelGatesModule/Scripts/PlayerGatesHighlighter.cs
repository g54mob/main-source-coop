using System.Collections.Generic;
using Features.CameraModelModule;
using Features.LevelGatesModule.Data;
using Features.LevelModule.Scripts;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using UnityEngine;
using Zenject;

namespace Features.LevelGatesModule.Scripts
{
	public class PlayerGatesHighlighter : MonoBehaviour
	{
		[SerializeField]
		private PlayerCharacterMovableBase _playerCharacterMovable;

		[SerializeField]
		private LayerMask _gateLayerMask;

		[SerializeField]
		[Min(0f)]
		[Tooltip("Rebuild the highlighted gate path only after the body moves this far. Territory is still evaluated every frame.")]
		private float _pathRecalculationDistance = 0.5f;

		private PlayersGatesModelSynchronizedModel _playersGatesModelSynchronizedModel;

		private IGateNavigationService _gateNavigationService;

		private LevelGatesModel _levelGatesModel;

		private MultiplayerModel _multiplayerModel;

		private CameraModel _cameraModel;

		private LevelGateConfiguration _levelGateConfiguration;

		private LevelModel _levelModel;

		private RoomTerritoryModel _roomTerritoryModel;

		private List<GatePathData> _prevGates = new List<GatePathData>();

		private List<GatePathData> _pathGates = new List<GatePathData>();

		private Vector3 _lastPathBodyPosition;

		private IGate _lastClosestExitGate;

		private bool _lastPathInsideGate;

		private bool _hasCachedPath;

		[Inject]
		public void InjectDependencies(IGateNavigationService gateNavigationService, LevelGatesModel levelGatesModel, PlayersGatesModelSynchronizedModel playersGatesModelSynchronizedModel, MultiplayerModel multiplayerModel, CameraModel cameraModel, LevelGateConfiguration levelGateConfiguration, LevelModel levelModel, RoomTerritoryModel roomTerritoryModel)
		{
			_gateNavigationService = gateNavigationService;
			_levelGatesModel = levelGatesModel;
			_playersGatesModelSynchronizedModel = playersGatesModelSynchronizedModel;
			_multiplayerModel = multiplayerModel;
			_cameraModel = cameraModel;
			_levelGateConfiguration = levelGateConfiguration;
			_levelModel = levelModel;
			_roomTerritoryModel = roomTerritoryModel;
		}

		private void OnEnable()
		{
			_levelGatesModel.OnGateRegistered += InvalidatePathCache;
			_levelGatesModel.OnGateUnRegistered += RemoveStaleGate;
		}

		private void OnDisable()
		{
			_levelGatesModel.OnGateRegistered -= InvalidatePathCache;
			_levelGatesModel.OnGateUnRegistered -= RemoveStaleGate;
		}

		private void Update()
		{
			if (!_playerCharacterMovable.HasInputAuthority)
			{
				return;
			}
			Vector3 position = _playerCharacterMovable.GetPosition();
			if (!TryEvaluateTerritory(position, out var isInside))
			{
				return;
			}
			_levelGatesModel.IsLocalPlayerInsideGate = isInside;
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			if (!_playersGatesModelSynchronizedModel.TryGetPlayerState(playerId, out var state) || state.PlayerInsideGate != isInside)
			{
				_playersGatesModelSynchronizedModel.SetPlayerInsideGate(playerId, isInside);
			}
			Camera cameraObject = _cameraModel.CameraObject;
			if (cameraObject == null)
			{
				return;
			}
			Vector3 position2 = cameraObject.transform.position;
			if (!_gateNavigationService.TryGetClosestExitGate(position2, out var closestGate) || !ShouldRebuildPath(position, closestGate, isInside))
			{
				return;
			}
			_lastPathBodyPosition = position;
			_lastClosestExitGate = closestGate;
			_lastPathInsideGate = isInside;
			_hasCachedPath = true;
			_gateNavigationService.ConstructPathToGate(position2, closestGate, projectPoints: true, _pathGates);
			if (!isInside)
			{
				RemoveGate(_pathGates, closestGate);
			}
			for (int i = 0; i < _prevGates.Count; i++)
			{
				GatePathData gatePathData = _prevGates[i];
				if (gatePathData.PathGate.IsActivated && !_gateNavigationService.PathContainsGate(_pathGates, gatePathData.PathGate))
				{
					gatePathData.PathGate.DeactivateGate();
				}
			}
			for (int j = 0; j < _pathGates.Count; j++)
			{
				GatePathData gatePathData2 = _pathGates[j];
				if (!_gateNavigationService.PathContainsGate(_prevGates, gatePathData2.PathGate) || !gatePathData2.PathGate.IsActivated)
				{
					gatePathData2.PathGate.ActivateGate(gatePathData2.IntersectionStart);
				}
			}
			List<GatePathData> prevGates = _prevGates;
			_prevGates = _pathGates;
			_pathGates = prevGates;
		}

		private bool TryEvaluateTerritory(Vector3 bodyPosition, out bool isInside)
		{
			PlayerTerritoryMode territoryMode = _levelGateConfiguration.GetTerritoryMode(_levelModel.CurrentLevel);
			if (territoryMode != PlayerTerritoryMode.ExitGateDot && territoryMode == PlayerTerritoryMode.RoomFootprints)
			{
				isInside = _roomTerritoryModel.IsInsideInteriorTerritory(bodyPosition);
				return true;
			}
			if (!_gateNavigationService.TryGetClosestExitGate(bodyPosition, out var closestGate))
			{
				isInside = false;
				return false;
			}
			Vector3 lhs = closestGate.Position - bodyPosition;
			isInside = Vector3.Dot(lhs, closestGate.Forward) >= 0f;
			return true;
		}

		private bool ShouldRebuildPath(Vector3 bodyPosition, IGate closestExitGate, bool isLocalPlayerInsideGate)
		{
			if (!_hasCachedPath || closestExitGate != _lastClosestExitGate || isLocalPlayerInsideGate != _lastPathInsideGate)
			{
				return true;
			}
			float pathRecalculationDistance = _pathRecalculationDistance;
			if (pathRecalculationDistance <= 0f)
			{
				return true;
			}
			return (bodyPosition - _lastPathBodyPosition).sqrMagnitude >= pathRecalculationDistance * pathRecalculationDistance;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (ShouldDeactivatePoiGate(other.gameObject, out var gate))
			{
				gate.DeactivateGate();
			}
		}

		private bool ShouldDeactivatePoiGate(GameObject gateObject, out IGate gate)
		{
			gate = null;
			if (_playerCharacterMovable.HasInputAuthority && IsInLayerMask(gateObject.layer, _gateLayerMask) && gateObject.TryGetComponent<IGate>(out gate) && gate.GateGroup == GateGroup.Poi)
			{
				return gate.IsActivated;
			}
			return false;
		}

		private bool IsInLayerMask(int layer, LayerMask layerMask)
		{
			return (layerMask.value & (1 << layer)) != 0;
		}

		private void RemoveStaleGate(IGate gate)
		{
			RemoveGate(_prevGates, gate);
			InvalidatePathCache(gate);
		}

		private void InvalidatePathCache(IGate _)
		{
			_hasCachedPath = false;
		}

		private static void RemoveGate(List<GatePathData> pathGates, IGate gate)
		{
			for (int num = pathGates.Count - 1; num >= 0; num--)
			{
				if (pathGates[num].PathGate == gate)
				{
					pathGates.RemoveAt(num);
				}
			}
		}
	}
}
