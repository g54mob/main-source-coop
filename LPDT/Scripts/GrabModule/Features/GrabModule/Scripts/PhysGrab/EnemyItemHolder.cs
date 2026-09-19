using System;
using Fusion;
using UnityEngine;

namespace Features.GrabModule.Scripts.PhysGrab
{
	[NetworkBehaviourWeaved(1)]
	public class EnemyItemHolder : NetworkBehaviour, IStateAuthorityChanged, IPublicFacingInterface
	{
		[SerializeField]
		private PhysGrabber _physGrabber;

		[SerializeField]
		private Transform _carryAnchor;

		[SerializeField]
		private float _maxCarryDistance = 2.5f;

		[SerializeField]
		private bool _requireNotGrabbedByOthers;

		[SerializeField]
		private bool _blockOthersGrabWhileHeld;

		[SerializeField]
		private bool _releaseWhenGrabbedByOther = true;

		[SerializeField]
		private float _postReleaseCollisionImmunityDuration = 2f;

		[WeaverGenerated]
		[DefaultForProperty("GrabbedObjectId", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkId _GrabbedObjectId;

		private SimplePointGrabable _grabbedGrabable;

		private Transform _grabbedTransform;

		private bool _hasBlockedGrab;

		private bool _hasExternalReservation;

		private bool _isCarrySetupPending;

		private int _externalHolderId = -1;

		private readonly EnemyCarryController _carryController = new EnemyCarryController();

		[Networked]
		[OnChangedRender("OnGrabbedObjectIdChanged")]
		[NetworkedWeaved(0, 1)]
		private unsafe NetworkId GrabbedObjectId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing EnemyItemHolder.GrabbedObjectId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkId*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing EnemyItemHolder.GrabbedObjectId. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkId*)((byte*)Ptr + 0) = value;
			}
		}

		public bool IsGrabbing => _grabbedTransform != null;

		public SimplePointGrabable CurrentGrabable => _grabbedGrabable;

		public Transform GrabbedTransform => _grabbedTransform;

		public Transform CarryAnchor => _carryAnchor;

		public event Action<GrabReleaseReason> OnReleased;

		public override void Spawned()
		{
			base.Spawned();
			_physGrabber.physGrabPointPullerPosition = _carryAnchor;
			_physGrabber.IsProcessPhysGrabbing = true;
			RestoreGrabStateFromNetwork();
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			if (hasState)
			{
				ReleaseGrabInternal(GrabReleaseReason.Explicit, clearNetworkState: false);
			}
			else
			{
				ClearLocalGrabState();
			}
			base.Despawned(runner, hasState);
		}

		public void StateAuthorityChanged()
		{
			RestoreGrabStateFromNetwork();
		}

		private void OnGrabbedObjectIdChanged()
		{
			RestoreGrabStateFromNetwork();
		}

		private void RestoreGrabStateFromNetwork()
		{
			if (!GrabbedObjectId.IsValid || base.Runner == null || !base.Runner.TryFindObject(GrabbedObjectId, out var networkObject))
			{
				ClearLocalGrabState();
				return;
			}
			if (!networkObject.TryGetComponent<SimplePointGrabable>(out var component) || component.GrabObject == null)
			{
				ClearLocalGrabState();
				return;
			}
			if (_grabbedGrabable != component)
			{
				ClearLocalGrabState();
			}
			_grabbedGrabable = component;
			_grabbedTransform = component.transform;
			if (!base.HasStateAuthority)
			{
				ClearAuthorityOnlyCarryState();
				return;
			}
			ReserveGrabable(component);
			component.AddIgnoreItemsCollisionRequest(base.gameObject.GetHashCode());
			if (component.NetworkObject != null && component.NetworkObject.IsValid && component.NetworkObject.StateAuthority.PlayerId != base.Object.StateAuthority.PlayerId)
			{
				component.RequestStateAuthorityRPC(base.Object.StateAuthority.PlayerId);
			}
			if (HasItemStateAuthority(component))
			{
				SetupCarryLink(component);
			}
			else
			{
				_isCarrySetupPending = true;
			}
		}

		private void FixedUpdate()
		{
			if (!base.HasStateAuthority || _grabbedGrabable == null)
			{
				return;
			}
			if (_grabbedGrabable.NetworkObject == null)
			{
				ReleaseGrabInternal(GrabReleaseReason.Explicit);
			}
			else if (_grabbedGrabable.NetworkObject.HasStateAuthority)
			{
				if (_isCarrySetupPending)
				{
					SetupCarryLink(_grabbedGrabable);
				}
				if (_carryController.IsLockedCarryActive)
				{
					_carryController.ApplyPose(_carryAnchor);
				}
				if (_grabbedGrabable.InCart)
				{
					ReleaseGrabInternal(GrabReleaseReason.InCart);
				}
				else if (Vector3.Distance(_carryAnchor.position, _grabbedGrabable.transform.position) > _maxCarryDistance)
				{
					ReleaseGrabInternal(GrabReleaseReason.MaxDistance);
				}
				else if (_releaseWhenGrabbedByOther && _grabbedGrabable.GrabbedByPlayersCount > 0)
				{
					ReleaseGrabInternal(GrabReleaseReason.TakenByOther);
				}
			}
		}

		public bool CanGrab(SimplePointGrabable grabable)
		{
			if (grabable == null || grabable.NetworkObject == null)
			{
				return false;
			}
			if (_grabbedGrabable != null)
			{
				return false;
			}
			if (grabable.GrabObject == null)
			{
				return false;
			}
			if (grabable.GrabBlocked)
			{
				return false;
			}
			if (grabable.GrabbedByExternalsCount > 0)
			{
				return false;
			}
			if (grabable.GrabbedBySomethingCount > 0)
			{
				return false;
			}
			if (_requireNotGrabbedByOthers && grabable.GrabbedByPlayersCount > 0)
			{
				return false;
			}
			return true;
		}

		public bool TryGrab(SimplePointGrabable grabable)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			if (!CanGrab(grabable))
			{
				return false;
			}
			_grabbedGrabable = grabable;
			GrabbedObjectId = grabable.NetworkObject.Id;
			_grabbedTransform = grabable.transform;
			ReserveGrabable(grabable);
			grabable.AddIgnoreItemsCollisionRequest(base.gameObject.GetHashCode());
			if (HasItemStateAuthority(grabable))
			{
				SetupCarryLink(grabable);
			}
			else
			{
				_isCarrySetupPending = true;
			}
			return true;
		}

		public void ReleaseGrab()
		{
			ReleaseGrabInternal(GrabReleaseReason.Explicit);
		}

		private void ReleaseGrabInternal(GrabReleaseReason reason, bool clearNetworkState = true)
		{
			if (!(_grabbedGrabable == null))
			{
				if (_hasBlockedGrab)
				{
					_grabbedGrabable.EnableGrabRPC();
					_hasBlockedGrab = false;
				}
				ReleaseGrabableReservation(_grabbedGrabable);
				_carryController.Release();
				_grabbedGrabable.SuppressCollisionDamageFor(_postReleaseCollisionImmunityDuration);
				_grabbedGrabable.RemoveIgnoreItemsCollisionRequest(base.gameObject.GetHashCode());
				_grabbedGrabable = null;
				_grabbedTransform = null;
				if (clearNetworkState)
				{
					GrabbedObjectId = default(NetworkId);
				}
				this.OnReleased?.Invoke(reason);
			}
		}

		private void SetupCarryLink(SimplePointGrabable grabable)
		{
			if (!(grabable == null) && !(grabable.GrabObject == null))
			{
				_carryController.Setup(grabable, _carryAnchor, _physGrabber);
				_isCarrySetupPending = false;
			}
		}

		private bool HasItemStateAuthority(SimplePointGrabable grabable)
		{
			if (grabable == null || grabable.NetworkObject == null)
			{
				return false;
			}
			return grabable.NetworkObject.HasStateAuthority;
		}

		private void ReserveGrabable(SimplePointGrabable grabable)
		{
			if (grabable == null)
			{
				return;
			}
			if (_externalHolderId < 0)
			{
				_externalHolderId = base.Object.Id.GetHashCode();
			}
			if (!_hasExternalReservation)
			{
				grabable.GrabbedByExternal(_externalHolderId, base.Object.StateAuthority.PlayerId);
				_hasExternalReservation = true;
			}
			if (_blockOthersGrabWhileHeld)
			{
				if (!grabable.GrabBlocked)
				{
					grabable.BlockGrabRPC();
				}
				_hasBlockedGrab = true;
			}
		}

		private void ReleaseGrabableReservation(SimplePointGrabable grabable)
		{
			if (!(grabable == null) && _hasExternalReservation)
			{
				grabable.UnGrabbedByExternal(_externalHolderId);
				_hasExternalReservation = false;
			}
		}

		private void ClearLocalGrabState()
		{
			if (base.HasStateAuthority && _hasBlockedGrab && _grabbedGrabable != null)
			{
				_grabbedGrabable.EnableGrabRPC();
			}
			if (base.HasStateAuthority)
			{
				ReleaseGrabableReservation(_grabbedGrabable);
			}
			_carryController.Release();
			if (_grabbedGrabable != null)
			{
				_grabbedGrabable.RemoveIgnoreItemsCollisionRequest(base.gameObject.GetHashCode());
			}
			_grabbedGrabable = null;
			_grabbedTransform = null;
			_hasBlockedGrab = false;
			_hasExternalReservation = false;
			_isCarrySetupPending = false;
		}

		private void ClearAuthorityOnlyCarryState()
		{
			_carryController.Release();
			if (_grabbedGrabable != null)
			{
				_grabbedGrabable.RemoveIgnoreItemsCollisionRequest(base.gameObject.GetHashCode());
			}
			_hasBlockedGrab = false;
			_hasExternalReservation = false;
			_isCarrySetupPending = false;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			GrabbedObjectId = _GrabbedObjectId;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_GrabbedObjectId = GrabbedObjectId;
		}
	}
}
