using System;
using System.Collections.Generic;
using System.Linq;
using Features.GrabModule.Scripts;
using Features.GrabModule.Scripts.PhysGrab.CartGrabber;
using Features.Movement.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.CollectingModule.Scripts.New
{
	[RequireComponent(typeof(Rigidbody))]
	[NetworkBehaviourWeaved(0)]
	public class PhysicsItemRotationLimiter : NetworkBehaviour
	{
		[Header("X Axis Rotation Limits")]
		[Tooltip("Rotation limit mode for X axis")]
		public RotationLimitMode XLimitMode = RotationLimitMode.MinMax;

		[Tooltip("Minimum rotation angle on X axis (local)")]
		public float MinX = -45f;

		[Tooltip("Maximum rotation angle on X axis (local)")]
		public float MaxX = 45f;

		[Header("Y Axis Rotation Limits")]
		[Tooltip("Rotation limit mode for Y axis")]
		public RotationLimitMode YLimitMode = RotationLimitMode.MinMax;

		[Tooltip("Minimum rotation angle on Y axis (local)")]
		public float MinY = -45f;

		[Tooltip("Maximum rotation angle on Y axis (local)")]
		public float MaxY = 45f;

		[Header("Z Axis Rotation Limits")]
		[Tooltip("Rotation limit mode for Z axis")]
		public RotationLimitMode ZLimitMode = RotationLimitMode.MinMax;

		[Tooltip("Minimum rotation angle on Z axis (local)")]
		public float MinZ = -45f;

		[Tooltip("Maximum rotation angle on Z axis (local)")]
		public float MaxZ = 45f;

		[Header("Behavior")]
		[Tooltip("How strongly to push the object back within limits")]
		[Range(0.1f, 100f)]
		public float CorrectionStrength = 2f;

		[Tooltip("Strength of holder-based rotation correction")]
		[Range(0.1f, 100f)]
		public float HolderCorrectionStrength = 5f;

		[Tooltip("Use local rotation (relative to parent) instead of world rotation")]
		public bool UseLocalRotation = true;

		[SerializeField]
		private SimplePointGrabable _simplePointGrabable;

		[SerializeField]
		private CartItemsGrabber _cartItemsGrabber;

		[SerializeField]
		private LayerMask _detectionLayer;

		private Rigidbody _rb;

		private Transform _parentTransform;

		private SpawnedPlayersModel _spawnedPlayersModel;

		private bool _initialized;

		public bool DisableRotation;

		[Inject]
		public void InjectDependencies(SpawnedPlayersModel spawnedPlayersModel)
		{
			_spawnedPlayersModel = spawnedPlayersModel;
		}

		public override void Spawned()
		{
			_initialized = true;
		}

		private void Start()
		{
			_rb = GetComponent<Rigidbody>();
			_parentTransform = base.transform.parent;
		}

		private void FixedUpdate()
		{
			if (!_initialized)
			{
				return;
			}
			if (_cartItemsGrabber != null)
			{
				foreach (IPointGrabable item in _cartItemsGrabber.Items)
				{
					if (item != null && !_cartItemsGrabber.ParentGrabbers.ContainsKey(item) && item.NetworkObject.StateAuthority != base.Object.StateAuthority)
					{
						return;
					}
				}
			}
			if (!(_rb == null) && _simplePointGrabable.GrabbedByPlayers.Contains(base.Runner.LocalPlayer.PlayerId))
			{
				if (XLimitMode == RotationLimitMode.ByRaycast || YLimitMode == RotationLimitMode.ByRaycast || ZLimitMode == RotationLimitMode.ByRaycast)
				{
					ApplyRaycastBasedRotation();
				}
				if (XLimitMode == RotationLimitMode.MinMax || YLimitMode == RotationLimitMode.MinMax || ZLimitMode == RotationLimitMode.MinMax)
				{
					ApplyMinMaxRotation();
				}
				if (XLimitMode == RotationLimitMode.ByCamera || YLimitMode == RotationLimitMode.ByCamera || ZLimitMode == RotationLimitMode.ByCamera)
				{
					ApplyByCameraRotation();
				}
			}
		}

		private void ApplyMinMaxRotation()
		{
			Vector3 angles = ((UseLocalRotation && _parentTransform != null) ? base.transform.localEulerAngles : base.transform.eulerAngles);
			Vector3 vector = NormalizeAngles(angles);
			Vector3 vector2 = vector;
			if (XLimitMode == RotationLimitMode.MinMax)
			{
				vector2.x = Mathf.Clamp(vector.x, MinX, MaxX);
			}
			if (YLimitMode == RotationLimitMode.MinMax)
			{
				vector2.y = Mathf.Clamp(vector.y, MinY, MaxY);
			}
			if (ZLimitMode == RotationLimitMode.MinMax)
			{
				vector2.z = Mathf.Clamp(vector.z, MinZ, MaxZ);
			}
			if (vector2 != vector)
			{
				Quaternion quaternion = Quaternion.Euler(vector2);
				if (UseLocalRotation && _parentTransform != null)
				{
					Quaternion targetRotation = _parentTransform.rotation * quaternion;
					ApplyRotationCorrection(targetRotation, CorrectionStrength);
				}
				else
				{
					ApplyRotationCorrection(quaternion, CorrectionStrength);
				}
			}
		}

		private void ApplyRaycastBasedRotation()
		{
			if (_simplePointGrabable == null || _simplePointGrabable.GrabbedByPlayers == null || _simplePointGrabable.GrabbedByPlayers.Count == 0)
			{
				return;
			}
			KeyValuePair<PlayerRef, PlayerDataHolder> keyValuePair = _spawnedPlayersModel.Players.FirstOrDefault((KeyValuePair<PlayerRef, PlayerDataHolder> p) => p.Key.PlayerId == _simplePointGrabable.GrabbedByPlayers[0]);
			if (keyValuePair.Value != null)
			{
				Vector3 normalized = (keyValuePair.Value.NetworkObject.GetComponent<PlayerLookDetection>().GetRaycastPosition(_detectionLayer) - base.transform.position).normalized;
				if (!(normalized.magnitude < 0.01f))
				{
					Vector3 eulerAngles = Quaternion.LookRotation(normalized, Vector3.up).eulerAngles;
					ApplyRotation(eulerAngles, RotationLimitMode.ByRaycast);
				}
			}
		}

		private void ApplyByCameraRotation()
		{
			if (!(_simplePointGrabable == null) && _simplePointGrabable.GrabbedByPlayers != null && _simplePointGrabable.GrabbedByPlayers.Count != 0)
			{
				KeyValuePair<PlayerRef, PlayerDataHolder> keyValuePair = _spawnedPlayersModel.Players.FirstOrDefault((KeyValuePair<PlayerRef, PlayerDataHolder> p) => p.Key.PlayerId == _simplePointGrabable.GrabbedByPlayers[0]);
				if (keyValuePair.Value != null)
				{
					Vector3 eulerAngles = keyValuePair.Value.NetworkObject.GetComponent<PlayerLookDetection>().GetPlayerLookTransform().rotation.eulerAngles;
					ApplyRotation(eulerAngles, RotationLimitMode.ByCamera);
				}
			}
		}

		private void ApplyRotation(Vector3 targetEuler, RotationLimitMode rotationLimitMode)
		{
			Vector3 eulerAngles = base.transform.eulerAngles;
			if (XLimitMode == rotationLimitMode)
			{
				float num = NormalizeAngle(eulerAngles.x);
				float target = NormalizeAngle(targetEuler.x);
				float num2 = Mathf.DeltaAngle(num, target);
				targetEuler.x = num + num2;
			}
			else
			{
				targetEuler.x = eulerAngles.x;
			}
			if (YLimitMode == rotationLimitMode)
			{
				float num3 = NormalizeAngle(eulerAngles.y);
				float target2 = NormalizeAngle(targetEuler.y);
				float num4 = Mathf.DeltaAngle(num3, target2);
				targetEuler.y = num3 + num4;
			}
			else
			{
				targetEuler.y = eulerAngles.y;
			}
			if (ZLimitMode == rotationLimitMode)
			{
				float num5 = NormalizeAngle(eulerAngles.z);
				float target3 = NormalizeAngle(targetEuler.z);
				float num6 = Mathf.DeltaAngle(num5, target3);
				targetEuler.z = num5 + num6;
			}
			else
			{
				targetEuler.z = eulerAngles.z;
			}
			Quaternion targetRotation = Quaternion.Euler(targetEuler);
			ApplyRotationCorrection(targetRotation, HolderCorrectionStrength);
		}

		private void ApplyRotationCorrection(Quaternion targetRotation, float strength)
		{
			(targetRotation * Quaternion.Inverse(base.transform.rotation)).ToAngleAxis(out var angle, out var axis);
			if (angle > 180f)
			{
				angle -= 360f;
			}
			if (angle != 0f && !float.IsNaN(axis.x))
			{
				Vector3 b = axis.normalized * (angle * (MathF.PI / 180f) * strength);
				_rb.angularVelocity = Vector3.Lerp(_rb.angularVelocity, b, Time.fixedDeltaTime * 10f);
			}
		}

		private Vector3 NormalizeAngles(Vector3 angles)
		{
			angles.x = NormalizeAngle(angles.x);
			angles.y = NormalizeAngle(angles.y);
			angles.z = NormalizeAngle(angles.z);
			return angles;
		}

		private float NormalizeAngle(float angle)
		{
			while (angle > 180f)
			{
				angle -= 360f;
			}
			while (angle < -180f)
			{
				angle += 360f;
			}
			return angle;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
