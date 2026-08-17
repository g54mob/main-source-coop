using EvilCore;
using EvilCore.DynamicCasting;
using NomadDrive.Features.Player;
using NomadDrive.Features.Vehicle.Networking;
using UnityEngine;

namespace NomadDrive.Features.Vehicle
{
	public class VehicleRescueService : IVehicleRescueService
	{
		private const float VehicleDetectRadius = 30f;

		private const float GroundSearchRadius = 15f;

		private const float RingStep = 2f;

		private const float VerticalProbe = 8f;

		private const float MaxDrop = 20f;

		private const float SettleClearance = 0.4f;

		private const float MaxFootprintHalfHeight = 2f;

		private const float CooldownSeconds = 2f;

		private const float StuckPollInterval = 0.2f;

		private readonly IPlayerService _playerService;

		private readonly ICastingManager _castingManager;

		private float _nextAllowedTime;

		private float _nextStuckEval;

		private bool _cachedCanRescue;

		public bool CanRescueVehicle
		{
			get
			{
				if (Time.unscaledTime >= _nextStuckEval)
				{
					_nextStuckEval = Time.unscaledTime + 0.2f;
					_cachedCanRescue = EvaluateCanRescue();
				}
				return _cachedCanRescue;
			}
		}

		public VehicleRescueService(IPlayerService playerService, ICastingManager castingManager)
		{
			_playerService = playerService;
			_castingManager = castingManager;
		}

		private bool EvaluateCanRescue()
		{
			if (!_playerService.IsPlayerSpawned || _playerService.LocalPlayer == null)
			{
				return false;
			}
			if (_playerService.LocalPlayer.IsPlayerSitting())
			{
				return false;
			}
			NetworkedNWHVehicle networkedNWHVehicle = NetworkedNWHVehicle.FindNearest(_playerService.LocalPlayer.transform.position, 30f, requireNoDriver: true);
			if (networkedNWHVehicle == null)
			{
				return false;
			}
			return VehicleStuckDetector.IsStuck(networkedNWHVehicle, ResolveWorldMask());
		}

		public void RequestRescueVehicle()
		{
			if (!CanRescueVehicle || Time.unscaledTime < _nextAllowedTime)
			{
				return;
			}
			_nextAllowedTime = Time.unscaledTime + 2f;
			NetworkedNWHVehicle networkedNWHVehicle = NetworkedNWHVehicle.FindNearest(_playerService.LocalPlayer.transform.position, 30f, requireNoDriver: true);
			if (!(networkedNWHVehicle == null))
			{
				LayerMask layerMask = ResolveWorldMask();
				if (VehicleStuckDetector.IsStuck(networkedNWHVehicle, layerMask) && TryComputeFootprint(networkedNWHVehicle, out var halfExtents, out var pivotToBottom))
				{
					Collider[] componentsInChildren = networkedNWHVehicle.GetComponentsInChildren<Collider>();
					Vector3 position = networkedNWHVehicle.transform.position;
					Quaternion safeRot = ComputeUprightHeading(networkedNWHVehicle.transform);
					Vector3 groundPoint;
					Vector3 safePos = ((!VehicleRescueLocator.TryFindClearGround(position, halfExtents, layerMask, componentsInChildren, _castingManager, 15f, 2f, 8f, 20f, out groundPoint)) ? (position + Vector3.up * 2f) : (groundPoint + Vector3.up * (pivotToBottom + 0.4f)));
					networkedNWHVehicle.RequestRescueReposition(safePos, safeRot);
				}
			}
		}

		private LayerMask ResolveWorldMask()
		{
			if (_playerService.TryGetCharacterMovement(out var movement))
			{
				return movement.collisionLayers;
			}
			return -5;
		}

		private static Quaternion ComputeUprightHeading(Transform vehicleTransform)
		{
			Vector3 vector = Vector3.ProjectOnPlane(vehicleTransform.forward, Vector3.up);
			if (vector.sqrMagnitude < 0.0001f)
			{
				vector = Vector3.ProjectOnPlane(vehicleTransform.up, Vector3.up);
			}
			if (vector.sqrMagnitude < 0.0001f)
			{
				vector = Vector3.forward;
			}
			return Quaternion.LookRotation(vector.normalized, Vector3.up);
		}

		private static bool TryComputeFootprint(NetworkedNWHVehicle vehicle, out Vector3 halfExtents, out float pivotToBottom)
		{
			halfExtents = Vector3.zero;
			pivotToBottom = 0f;
			Collider[] componentsInChildren = vehicle.GetComponentsInChildren<Collider>();
			bool flag = false;
			Bounds bounds = default(Bounds);
			Collider[] array = componentsInChildren;
			foreach (Collider collider in array)
			{
				if (!(collider == null) && !collider.isTrigger)
				{
					if (!flag)
					{
						bounds = collider.bounds;
						flag = true;
					}
					else
					{
						bounds.Encapsulate(collider.bounds);
					}
				}
			}
			if (!flag)
			{
				return false;
			}
			halfExtents = bounds.extents;
			halfExtents.y = Mathf.Min(halfExtents.y, 2f);
			pivotToBottom = Mathf.Max(0f, vehicle.transform.position.y - bounds.min.y);
			return true;
		}
	}
}
