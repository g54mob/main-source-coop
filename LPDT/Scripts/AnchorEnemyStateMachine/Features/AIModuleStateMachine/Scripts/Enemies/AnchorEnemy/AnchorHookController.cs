using System;
using Features.GrabModule.Scripts;
using Features.GrabModule.Scripts.PhysGrab;
using Fusion;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy
{
	[NetworkBehaviourWeaved(1)]
	public class AnchorHookController : NetworkBehaviour
	{
		public enum AnchorPhase
		{
			Idle = 0,
			Flying = 1,
			Reeling = 2
		}

		[SerializeField]
		private Transform _carrier;

		[SerializeField]
		private PlayerGrabHolder _playerGrabHolder;

		[SerializeField]
		private AnchorItemView _anchorView;

		[SerializeField]
		private Transform _rootBone;

		[SerializeField]
		private float _arriveDistance = 0.2f;

		[SerializeField]
		private float _carrierSmoothTime = 0.06f;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Phase", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private AnchorPhase _Phase;

		private Vector3 _target;

		private bool _flightEnded;

		private float _reelNearSpeed = 28f;

		private float _reelFarSpeed = 6f;

		private float _reelFarDistance = 12f;

		private Vector3 _carrierSmoothVelocity;

		private bool _subscribedRelease;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe AnchorPhase Phase
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing AnchorHookController.Phase. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(AnchorPhase*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing AnchorHookController.Phase. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(AnchorPhase*)((byte*)Ptr + 0) = value;
			}
		}

		public bool HasGrabbedPlayer
		{
			get
			{
				if (_playerGrabHolder != null)
				{
					return _playerGrabHolder.IsGrabbing;
				}
				return false;
			}
		}

		public int GrabbedPlayerId
		{
			get
			{
				SimplePointGrabable simplePointGrabable = ((_playerGrabHolder != null) ? _playerGrabHolder.CurrentGrabable : null);
				if (simplePointGrabable == null || simplePointGrabable.NetworkObject == null)
				{
					return -1;
				}
				return simplePointGrabable.NetworkObject.InputAuthority.PlayerId;
			}
		}

		public float CarrierDistanceToRoot
		{
			get
			{
				if (_rootBone == null || _carrier == null)
				{
					return float.MaxValue;
				}
				return Vector3.Distance(_rootBone.position, _carrier.position);
			}
		}

		public float ArriveDistance => _arriveDistance;

		public float LaunchHeight
		{
			get
			{
				if (!(_rootBone != null))
				{
					return base.transform.position.y;
				}
				return _rootBone.position.y;
			}
		}

		public Vector3 RootWorldPosition
		{
			get
			{
				if (!(_rootBone != null))
				{
					return base.transform.position;
				}
				return _rootBone.position;
			}
		}

		public event Action OnPlayerHit;

		public event Action<AnchorMissReason> OnMissed;

		public event Action<GrabReleaseReason> OnGrabReleased;

		public override void Spawned()
		{
			base.Spawned();
			SubscribeRelease();
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			UnsubscribeRelease();
			base.Despawned(runner, hasState);
		}

		public void Launch(Vector3 targetPosition)
		{
			if (base.HasStateAuthority)
			{
				_target = targetPosition;
				_flightEnded = false;
				_carrierSmoothVelocity = Vector3.zero;
				SnapCarrierToRoot();
				Phase = AnchorPhase.Flying;
			}
		}

		public void TeleportCarrierTo(Vector3 worldPosition)
		{
			if (base.HasStateAuthority && !(_carrier == null))
			{
				_carrierSmoothVelocity = Vector3.zero;
				_carrier.position = worldPosition;
			}
		}

		public void Reel(float nearSpeed, float farSpeed, float farDistance)
		{
			if (base.HasStateAuthority)
			{
				_reelNearSpeed = Mathf.Max(nearSpeed, 0.01f);
				_reelFarSpeed = Mathf.Max(farSpeed, 0.01f);
				_reelFarDistance = Mathf.Max(farDistance, 0.01f);
				_flightEnded = false;
				_carrierSmoothVelocity = Vector3.zero;
				Phase = AnchorPhase.Reeling;
			}
		}

		public void ResetHook()
		{
			if (base.HasStateAuthority)
			{
				if (_playerGrabHolder != null && _playerGrabHolder.IsGrabbing)
				{
					_playerGrabHolder.ReleaseGrab();
				}
				_carrierSmoothVelocity = Vector3.zero;
				SnapCarrierToRoot();
				_flightEnded = false;
				Phase = AnchorPhase.Idle;
			}
		}

		public void ReportPlayerHit(SimplePointGrabable playerGrabable)
		{
			if (base.HasStateAuthority && Phase == AnchorPhase.Flying && !_flightEnded && !(playerGrabable == null) && !(_playerGrabHolder == null) && _playerGrabHolder.CanGrab(playerGrabable))
			{
				Vector3 position = ((_anchorView != null) ? _anchorView.AnchorWorldPosition : playerGrabable.transform.position);
				Transform nearestHandle = playerGrabable.GetNearestHandle(position);
				if (nearestHandle == null)
				{
					nearestHandle = playerGrabable.transform;
				}
				if (_playerGrabHolder.TryGrab(playerGrabable))
				{
					_anchorView?.LatchToPlayer(nearestHandle);
					TeleportCarrierTo(nearestHandle.position);
					EndFlight();
					this.OnPlayerHit?.Invoke();
				}
			}
		}

		public void ReportObstacle()
		{
			if (base.HasStateAuthority && Phase == AnchorPhase.Flying && !_flightEnded)
			{
				EndFlight();
				this.OnMissed?.Invoke(AnchorMissReason.Obstacle);
			}
		}

		private void LateUpdate()
		{
			if (base.HasStateAuthority && !(_carrier == null) && Phase == AnchorPhase.Idle)
			{
				SnapCarrierToRoot();
			}
		}

		public override void FixedUpdateNetwork()
		{
			if (!base.HasStateAuthority || _carrier == null)
			{
				return;
			}
			switch (Phase)
			{
			case AnchorPhase.Reeling:
				if (!(_rootBone == null))
				{
					float carrierDistanceToRoot = CarrierDistanceToRoot;
					if (carrierDistanceToRoot <= _arriveDistance)
					{
						_carrierSmoothVelocity = Vector3.zero;
						SnapCarrierToRoot();
					}
					else
					{
						float t = Mathf.Clamp01(carrierDistanceToRoot / _reelFarDistance);
						float maxSpeed = Mathf.Lerp(_reelNearSpeed, _reelFarSpeed, t);
						MoveCarrierSmooth(_rootBone.position, maxSpeed);
					}
				}
				break;
			case AnchorPhase.Idle:
			case AnchorPhase.Flying:
				break;
			}
		}

		private void MoveCarrierSmooth(Vector3 target, float maxSpeed)
		{
			_carrier.position = Vector3.SmoothDamp(_carrier.position, target, ref _carrierSmoothVelocity, _carrierSmoothTime, maxSpeed, base.Runner.DeltaTime);
		}

		private void EndFlight()
		{
			_flightEnded = true;
		}

		private void SubscribeRelease()
		{
			if (!_subscribedRelease && !(_playerGrabHolder == null))
			{
				_playerGrabHolder.OnReleased += HandleGrabReleased;
				_subscribedRelease = true;
			}
		}

		private void UnsubscribeRelease()
		{
			if (_subscribedRelease && !(_playerGrabHolder == null))
			{
				_playerGrabHolder.OnReleased -= HandleGrabReleased;
				_subscribedRelease = false;
			}
		}

		private void HandleGrabReleased(GrabReleaseReason reason)
		{
			bool num = _anchorView != null && _anchorView.IsLatchedToPlayer;
			_anchorView?.ClearPlayerLatch();
			if (num && Phase == AnchorPhase.Reeling)
			{
				_anchorView?.BeginSpringReel();
			}
			this.OnGrabReleased?.Invoke(reason);
		}

		private void SnapCarrierToRoot()
		{
			if (!(_rootBone == null))
			{
				_carrier.SetPositionAndRotation(_rootBone.position, _rootBone.rotation);
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			Phase = _Phase;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_Phase = Phase;
		}
	}
}
