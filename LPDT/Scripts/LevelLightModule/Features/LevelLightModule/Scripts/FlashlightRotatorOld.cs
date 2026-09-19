using System;
using System.Collections.Generic;
using Features.CameraModelModule;
using Features.GrabModule.Scripts;
using Features.Movement.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.LevelLightModule.Scripts
{
	[NetworkBehaviourWeaved(3)]
	public class FlashlightRotatorOld : NetworkBehaviour
	{
		private static readonly IComparer<RaycastHit> _distanceComparer = Comparer<RaycastHit>.Create((RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance));

		private const int RaycastHitsBufferSize = 16;

		[SerializeField]
		private float _rotationSpeed = 5f;

		[SerializeField]
		private LayerMask _targetDetectionMask;

		private readonly RaycastHit[] _raycastHits = new RaycastHit[16];

		private CameraModel _cameraModel;

		private PlayerMovableModel _playerMovableModel;

		private Vector3 _localTargetForward = Vector3.zero;

		[WeaverGenerated]
		[DefaultForProperty("NetworkedForward", 0, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Vector3 _NetworkedForward;

		[Networked]
		[NetworkedWeaved(0, 3)]
		private unsafe Vector3 NetworkedForward
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing FlashlightRotatorOld.NetworkedForward. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Vector3*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing FlashlightRotatorOld.NetworkedForward. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Vector3*)((byte*)Ptr + 0) = value;
			}
		}

		[Inject]
		private void InjectDependencies(CameraModel cameraModel, PlayerMovableModel playerMovableModel)
		{
			_cameraModel = cameraModel;
			_playerMovableModel = playerMovableModel;
		}

		private void FixedUpdate()
		{
			if (!base.HasInputAuthority || _cameraModel.CameraObject == null || _playerMovableModel.RotationMode != PlayerRotationMode.HeadAndBodyRotation)
			{
				return;
			}
			Ray ray = _cameraModel.CameraObject.ScreenPointToRay(new Vector3((float)Screen.width / 2f, (float)Screen.height / 2f, 0f));
			_localTargetForward = _cameraModel.CameraObject.transform.forward;
			int num = Physics.RaycastNonAlloc(ray, _raycastHits, float.PositiveInfinity, _targetDetectionMask);
			if (num > 1)
			{
				Array.Sort(_raycastHits, 0, num, _distanceComparer);
			}
			for (int i = 0; i < num; i++)
			{
				RaycastHit raycastHit = _raycastHits[i];
				if (!raycastHit.collider.TryGetComponent<SimplePointGrabable>(out var component) || !component.GrabbedByPlayers.Contains(base.Object.StateAuthority.PlayerId))
				{
					_localTargetForward = raycastHit.point - base.transform.position;
					break;
				}
			}
			_localTargetForward.Normalize();
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority && _localTargetForward != Vector3.zero)
			{
				NetworkedForward = _localTargetForward;
			}
		}

		public override void Render()
		{
			Vector3 vector = (base.HasInputAuthority ? _localTargetForward : NetworkedForward);
			if (!(vector == Vector3.zero))
			{
				base.transform.forward = Vector3.Lerp(base.transform.forward, vector, Time.deltaTime * _rotationSpeed);
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			NetworkedForward = _NetworkedForward;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_NetworkedForward = NetworkedForward;
		}
	}
}
