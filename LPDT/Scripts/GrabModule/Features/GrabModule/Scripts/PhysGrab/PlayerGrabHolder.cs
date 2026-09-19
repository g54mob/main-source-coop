using System;
using Fusion;
using UnityEngine;

namespace Features.GrabModule.Scripts.PhysGrab
{
	[NetworkBehaviourWeaved(1)]
	public class PlayerGrabHolder : NetworkBehaviour
	{
		[SerializeField]
		private PhysGrabber _physGrabber;

		[SerializeField]
		private Transform _carryAnchor;

		[SerializeField]
		private float _maxCarryDistance = 2.5f;

		[WeaverGenerated]
		[DefaultForProperty("GrabbedObjectId", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkId _GrabbedObjectId;

		private SimplePointGrabable _grabbedGrabable;

		private Transform _grabbedTransform;

		private int _externalHolderId = -1;

		[Networked]
		[OnChangedRender("OnGrabbedObjectIdChanged")]
		[NetworkedWeaved(0, 1)]
		private unsafe NetworkId GrabbedObjectId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerGrabHolder.GrabbedObjectId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkId*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerGrabHolder.GrabbedObjectId. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkId*)((byte*)Ptr + 0) = value;
			}
		}

		public bool IsGrabbing => _grabbedTransform != null;

		public SimplePointGrabable CurrentGrabable => _grabbedGrabable;

		public Transform GrabbedTransform => _grabbedTransform;

		public Transform CarryAnchor => _carryAnchor;

		public event Action<GrabReleaseReason> OnReleased;

		public event Action OnVictimLost;

		public override void Spawned()
		{
			base.Spawned();
			_physGrabber.physGrabPointPullerPosition = _carryAnchor;
			_physGrabber.IsProcessPhysGrabbing = true;
			OnGrabbedObjectIdChanged();
		}

		private void OnGrabbedObjectIdChanged()
		{
			if (GrabbedObjectId.IsValid && base.Runner.TryFindObject(GrabbedObjectId, out var networkObject))
			{
				_grabbedTransform = networkObject.transform;
			}
			else
			{
				_grabbedTransform = null;
			}
		}

		private void FixedUpdate()
		{
			if (!base.HasStateAuthority)
			{
				return;
			}
			if (_grabbedGrabable == null)
			{
				if (HasLocalGrabState())
				{
					ReleaseGrabInternal(GrabReleaseReason.VictimLost);
				}
				return;
			}
			NetworkObject networkObject = _grabbedGrabable.NetworkObject;
			if (networkObject == null || !networkObject.IsValid)
			{
				ReleaseGrabInternal(GrabReleaseReason.VictimLost);
			}
			else if (!IsPlayerActive(networkObject.InputAuthority.PlayerId))
			{
				ReleaseGrabInternal(GrabReleaseReason.VictimLost);
			}
			else if (HasOtherGrabber())
			{
				ReleaseGrabInternal(GrabReleaseReason.TakenByOther);
			}
			else if (networkObject.HasStateAuthority && Vector3.Distance(_carryAnchor.position, _grabbedGrabable.transform.position) > _maxCarryDistance)
			{
				ReleaseGrabInternal(GrabReleaseReason.MaxDistance);
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
			if (grabable.GrabbedByExternalsCount > 0)
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
			_externalHolderId = base.Object.Id.GetHashCode();
			int playerId = base.Object.StateAuthority.PlayerId;
			grabable.GrabbedByExternal(_externalHolderId, playerId);
			grabable.InvokeOnGrab();
			grabable.GrabObject.Grabbers.Add(_physGrabber);
			_physGrabber.physGrabPoints.Add(grabable.GrabObject, grabable.GetNearestHandle(_carryAnchor.position));
			_grabbedGrabable = grabable;
			GrabbedObjectId = grabable.NetworkObject.Id;
			return true;
		}

		public void ReleaseGrab()
		{
			ReleaseGrabInternal(GrabReleaseReason.Explicit);
		}

		private void ReleaseGrabInternal(GrabReleaseReason reason)
		{
			if (!HasLocalGrabState())
			{
				return;
			}
			SimplePointGrabable grabbedGrabable = _grabbedGrabable;
			if (grabbedGrabable != null)
			{
				grabbedGrabable.UnGrabbedByExternal(_externalHolderId);
				grabbedGrabable.InvokeOnUnGrab();
				if (grabbedGrabable.GrabObject != null)
				{
					grabbedGrabable.GrabObject.Grabbers.Remove(_physGrabber);
					_physGrabber.physGrabPoints.Remove(grabbedGrabable.GrabObject);
				}
			}
			_grabbedGrabable = null;
			_externalHolderId = -1;
			GrabbedObjectId = default(NetworkId);
			_grabbedTransform = null;
			if (reason == GrabReleaseReason.VictimLost)
			{
				this.OnVictimLost?.Invoke();
			}
			this.OnReleased?.Invoke(reason);
		}

		private bool HasLocalGrabState()
		{
			if (!(_grabbedGrabable != null) && !GrabbedObjectId.IsValid)
			{
				return _externalHolderId >= 0;
			}
			return true;
		}

		private bool HasOtherGrabber()
		{
			return _grabbedGrabable.GrabbedByPlayersCount > 0;
		}

		private bool IsPlayerActive(int playerId)
		{
			if (base.Runner == null || playerId < 0)
			{
				return false;
			}
			foreach (PlayerRef activePlayer in base.Runner.ActivePlayers)
			{
				if (activePlayer.PlayerId == playerId)
				{
					return true;
				}
			}
			return false;
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
