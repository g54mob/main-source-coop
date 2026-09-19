using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.Movement.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class PlayerLookDetection : NetworkBehaviour
	{
		[SerializeField]
		private Transform _playerCamera;

		[SerializeField]
		private float _detectionRadius = 50f;

		[SerializeField]
		private float _lookAngleThreshold = 30f;

		[SerializeField]
		private LayerMask _detectionLayer;

		private PlayerMovableModel _playerMovableModel;

		[Inject]
		public void InjectDependencies(PlayerMovableModel playerMovableModel)
		{
			_playerMovableModel = playerMovableModel;
		}

		public override void Spawned()
		{
			base.Spawned();
			if (base.Runner.LocalPlayer == base.Object.InputAuthority)
			{
				_playerMovableModel.PlayerLookDetection = this;
			}
		}

		public bool IsLookingAtObject(Transform targetTransform, bool angleCullEnabled = true, float hardDetectionDistance = 0f, float raysThreshold = 1f)
		{
			if (targetTransform == null || _playerCamera == null)
			{
				return false;
			}
			if (hardDetectionDistance != 0f && Vector3.Distance(_playerCamera.position, targetTransform.position) <= hardDetectionDistance)
			{
				return true;
			}
			Vector3 directionToTarget = targetTransform.position - _playerCamera.position;
			if (directionToTarget.magnitude > _detectionRadius)
			{
				return false;
			}
			float num = Vector3.Dot(_playerCamera.forward, directionToTarget.normalized);
			float num2 = Mathf.Cos(_lookAngleThreshold * (MathF.PI / 180f));
			Vector3 position = _playerCamera.position;
			Vector3 normalized = _playerCamera.forward.normalized;
			float lookAngleThreshold = _lookAngleThreshold;
			Vector3 vector = Quaternion.AngleAxis(0f - lookAngleThreshold, Vector3.up) * normalized;
			Vector3 vector2 = Quaternion.AngleAxis(lookAngleThreshold, Vector3.up) * normalized;
			Debug.DrawLine(position, position + vector * _detectionRadius, Color.yellow);
			Debug.DrawLine(position, position + vector2 * _detectionRadius, Color.yellow);
			if (angleCullEnabled && num < num2 - 0.0001f)
			{
				return false;
			}
			foreach (Vector3 item in new List<Vector3>
			{
				_playerCamera.position,
				_playerCamera.position + new Vector3(0f, raysThreshold),
				_playerCamera.position + new Vector3(0f, 0f - raysThreshold),
				_playerCamera.position + new Vector3(raysThreshold, 0f),
				_playerCamera.position + new Vector3(0f - raysThreshold, 0f)
			})
			{
				if (CheckRaycast(targetTransform, directionToTarget, item))
				{
					return true;
				}
			}
			return false;
		}

		public Transform GetPlayerLookTransform()
		{
			return _playerCamera;
		}

		public Vector3 GetRaycastPosition(LayerMask? detectionLayer = null)
		{
			if (_playerCamera == null)
			{
				return Vector3.zero;
			}
			if (Physics.Raycast(layerMask: detectionLayer.HasValue ? detectionLayer.Value : _detectionLayer, origin: _playerCamera.position, direction: _playerCamera.forward, hitInfo: out var hitInfo, maxDistance: _detectionRadius))
			{
				Debug.DrawRay(hitInfo.point, Vector3.up, Color.red);
				return hitInfo.point;
			}
			Vector3 vector = _playerCamera.position + _playerCamera.forward * _detectionRadius;
			Debug.DrawRay(vector, Vector3.up, Color.yellow);
			return vector;
		}

		private bool CheckRaycast(Transform targetTransform, Vector3 directionToTarget, Vector3 position)
		{
			Debug.DrawRay(position, directionToTarget.normalized * 30f, Color.cyan);
			if (Physics.Raycast(position, directionToTarget.normalized, out var hitInfo, _detectionRadius, _detectionLayer))
			{
				return hitInfo.collider.transform == targetTransform;
			}
			return false;
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
