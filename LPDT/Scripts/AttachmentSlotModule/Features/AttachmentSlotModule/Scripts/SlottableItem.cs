using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.GrabModule.Scripts;
using Features.GrabModule.Scripts.PhysGrab;
using Features.GrabModule.Scripts.PhysGrab.CartGrabber;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;

namespace Features.AttachmentSlotModule.Scripts
{
	[NetworkBehaviourWeaved(6)]
	public class SlottableItem : NetworkBehaviour
	{
		[SerializeField]
		private AttachmentCategory _category;

		[SerializeField]
		private SimplePointGrabable _grabable;

		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private Transform _mountPoint;

		[Header("Tuning")]
		[SerializeField]
		private float _magnetForce = 200f;

		[SerializeField]
		private float _magnetTorque = 25f;

		[SerializeField]
		private float _magnetAngularDamping = 5f;

		[SerializeField]
		private float _commitDistance = 0.35f;

		[SerializeField]
		private float _magnetRangeBonus;

		[SerializeField]
		private float _detachTurnAngle = 20f;

		[SerializeField]
		private float _detachMoveDistance = 0.35f;

		[WeaverGenerated]
		[DefaultForProperty("IsDockedInternal", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _IsDockedInternal;

		[WeaverGenerated]
		[DefaultForProperty("DockedBearer", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkObject _DockedBearer;

		[WeaverGenerated]
		[DefaultForProperty("DockedSlotIndex", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _DockedSlotIndex;

		[WeaverGenerated]
		[DefaultForProperty("ReservedBearer", 3, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkObject _ReservedBearer;

		[WeaverGenerated]
		[DefaultForProperty("ReservedSlotIndex", 4, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _ReservedSlotIndex;

		[WeaverGenerated]
		[DefaultForProperty("ReservedTick", 5, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _ReservedTick;

		private static readonly List<SlottableItem> _all = new List<SlottableItem>();

		private AttachmentSlot _dockedSlot;

		private Transform _anchor;

		private Vector3 _mountLocalPosition;

		private Quaternion _mountLocalRotation = Quaternion.identity;

		private bool _initialized;

		private bool _dockApplied;

		private Vector3 _detachOffsetBaseline;

		private float _detachInitialAimAngle;

		private int _detachSettleTicks;

		private bool _reattachBlocked;

		private AttachmentSlot _reattachBlockSlot;

		private bool _mountPoseCached;

		private int _undockRequestTick;

		private NetworkObject _isolatedBearer;

		private Collider[] _itemColliders;

		private readonly HashSet<Collider> _isolatedColliders = new HashSet<Collider>();

		private Collider[] _bearerColliders;

		private int _cargoRefreshTicks;

		private int _exitTicks;

		private const int DETACH_SETTLE_TICKS = 8;

		private const int EXIT_MAX_TICKS = 90;

		private const int CARGO_REFRESH_TICKS = 10;

		private const int UNDOCK_REQUEST_RETRY_TICKS = 30;

		[Networked]
		[OnChangedRender("OnDockedChanged")]
		[NetworkedWeaved(0, 1)]
		private unsafe NetworkBool IsDockedInternal
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlottableItem.IsDockedInternal. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlottableItem.IsDockedInternal. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		private unsafe NetworkObject DockedBearer
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlottableItem.DockedBearer. Networked properties can only be accessed when Spawned() has been called.");
				}
				NetworkObject result = null;
				NetworkObject.NetworkUnwrap(base.Runner, *(NetworkId*)(Ptr + 1), ref result);
				return result;
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlottableItem.DockedBearer. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkId*)(Ptr + 1) = NetworkObject.NetworkWrap(value);
			}
		}

		[Networked]
		[NetworkedWeaved(2, 1)]
		private unsafe int DockedSlotIndex
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlottableItem.DockedSlotIndex. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[2];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlottableItem.DockedSlotIndex. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[2] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(3, 1)]
		private unsafe NetworkObject ReservedBearer
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlottableItem.ReservedBearer. Networked properties can only be accessed when Spawned() has been called.");
				}
				NetworkObject result = null;
				NetworkObject.NetworkUnwrap(base.Runner, *(NetworkId*)(Ptr + 3), ref result);
				return result;
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlottableItem.ReservedBearer. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkId*)(Ptr + 3) = NetworkObject.NetworkWrap(value);
			}
		}

		[Networked]
		[NetworkedWeaved(4, 1)]
		private unsafe int ReservedSlotIndex
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlottableItem.ReservedSlotIndex. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[4];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlottableItem.ReservedSlotIndex. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[4] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(5, 1)]
		private unsafe int ReservedTick
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlottableItem.ReservedTick. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[5];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SlottableItem.ReservedTick. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[5] = value;
			}
		}

		public bool IsDocked => IsDockedInternal;

		public AttachmentCategory Category => _category;

		private Transform MountPoint
		{
			get
			{
				if (!(_mountPoint != null))
				{
					return base.transform;
				}
				return _mountPoint;
			}
		}

		private void Reset()
		{
			_grabable = GetComponentInChildren<SimplePointGrabable>();
			_rigidbody = GetComponentInChildren<Rigidbody>();
		}

		public override void Spawned()
		{
			base.Spawned();
			_initialized = true;
			if (!_all.Contains(this))
			{
				_all.Add(this);
			}
			if ((bool)IsDockedInternal)
			{
				ApplyDock();
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_initialized = false;
			_all.Remove(this);
			ClearBearerIsolation();
			if (_dockedSlot != null)
			{
				_dockedSlot.ClearDockedGrabable(_grabable);
			}
		}

		private void FixedUpdate()
		{
			if (!_initialized || _rigidbody == null)
			{
				return;
			}
			bool flag = _grabable.GrabbedByPlayers.Count > 0;
			_grabable.IsGrabPriority = IsDockedInternal;
			if ((bool)IsDockedInternal)
			{
				if (!_dockApplied)
				{
					ApplyDock();
				}
				SetBearerIsolation((_dockedSlot != null) ? _dockedSlot.Bearer : null);
				if (base.HasStateAuthority && _dockedSlot != null && IsSlotTakenByOther(_dockedSlot))
				{
					Undock();
				}
				else
				{
					HandleDocked(flag);
				}
				return;
			}
			AttachmentSlot attachmentSlot = ResolveMagnetSlot();
			UpdateReattachLock(flag);
			UpdateMagnetIsolation(flag, attachmentSlot);
			if (!base.HasStateAuthority)
			{
				return;
			}
			if (flag && attachmentSlot != null)
			{
				UpdateReservation(attachmentSlot);
				if (DistanceToAnchor(attachmentSlot) <= _commitDistance)
				{
					Dock(attachmentSlot);
				}
				else
				{
					ApplyMagnet(attachmentSlot);
				}
			}
			else
			{
				ClearReservation();
			}
		}

		public override void Render()
		{
			if ((bool)IsDockedInternal)
			{
				if (_anchor == null)
				{
					ResolveAnchor();
				}
				if (!(_anchor == null))
				{
					Quaternion quaternion = _anchor.rotation * Quaternion.Inverse(_mountLocalRotation);
					Vector3 position = _anchor.position - quaternion * _mountLocalPosition;
					base.transform.SetPositionAndRotation(position, quaternion);
				}
			}
		}

		private void HandleDocked(bool isGrabbed)
		{
			if (!isGrabbed)
			{
				_detachSettleTicks = 8;
				return;
			}
			PhysGrabber physGrabber = ResolveLocalGrabber();
			if (physGrabber == null || physGrabber.physGrabPointPullerPosition == null || _anchor == null)
			{
				_detachSettleTicks = 8;
				return;
			}
			Vector3 position = physGrabber.GrabberTransform.position;
			Vector3 vector = position - _anchor.position;
			Vector3 vector2 = physGrabber.physGrabPointPullerPosition.position - position;
			if (vector2.sqrMagnitude < 0.0001f || vector.sqrMagnitude < 0.0001f)
			{
				_detachSettleTicks = 8;
				return;
			}
			float num = Vector3.Angle(vector2, -vector);
			if (_detachSettleTicks > 0)
			{
				_detachOffsetBaseline = vector;
				_detachInitialAimAngle = num;
				_detachSettleTicks--;
				return;
			}
			bool num2 = num - _detachInitialAimAngle > _detachTurnAngle;
			bool flag = Vector3.Distance(vector, _detachOffsetBaseline) > _detachMoveDistance;
			if (!num2 && !flag)
			{
				return;
			}
			if (base.HasStateAuthority)
			{
				Undock();
				return;
			}
			int num3 = base.Runner.Tick;
			if (_undockRequestTick == 0 || num3 - _undockRequestTick >= 30)
			{
				_undockRequestTick = num3;
				RequestUndockRPC(base.Runner.LocalPlayer.PlayerId);
			}
		}

		private PhysGrabber ResolveLocalGrabber()
		{
			List<PhysGrabber> grabbers = _grabable.GrabObject.Grabbers;
			int playerId = base.Runner.LocalPlayer.PlayerId;
			for (int i = 0; i < grabbers.Count; i++)
			{
				PhysGrabber physGrabber = grabbers[i];
				if (!(physGrabber == null) && !(physGrabber.Object == null) && physGrabber.PlayerId == playerId)
				{
					return physGrabber;
				}
			}
			return null;
		}

		private AttachmentSlot ResolveMagnetSlot()
		{
			AttachmentSlot result = null;
			float num = float.MaxValue;
			Vector3 position = MountPoint.position;
			foreach (AttachmentSlot item in AttachmentSlot.All)
			{
				if (!(item == null) && item.Accepts(_category) && !IsSlotTakenByOther(item) && (!_reattachBlocked || !(item == _reattachBlockSlot)))
				{
					float num2 = Vector3.Distance(position, item.Anchor.position);
					if (!(num2 > MagnetRange(item)) && !(num2 >= num))
					{
						num = num2;
						result = item;
					}
				}
			}
			return result;
		}

		private void UpdateMagnetIsolation(bool isGrabbed, AttachmentSlot target)
		{
			if (isGrabbed && target != null)
			{
				SetBearerIsolation(target.Bearer);
			}
			else if (!(_isolatedBearer == null))
			{
				if (!isGrabbed || _exitTicks <= 0 || !IsStillLeavingBearer())
				{
					ClearBearerIsolation();
					return;
				}
				_exitTicks--;
				ReassertBearerIsolation();
				RefreshIsolationCargo();
			}
		}

		private bool IsStillLeavingBearer()
		{
			if (!IsWithinBearerBounds())
			{
				return IsInsideBearerVolume();
			}
			return true;
		}

		private bool IsInsideBearerVolume()
		{
			if (_isolatedBearer == null || _grabable.Carts == null)
			{
				return false;
			}
			Transform root = _isolatedBearer.transform.root;
			for (int i = 0; i < _grabable.Carts.Count; i++)
			{
				GameObject gameObject = _grabable.Carts[i];
				if (gameObject != null && gameObject.transform.root == root)
				{
					return true;
				}
			}
			return false;
		}

		private bool IsWithinBearerBounds()
		{
			if (_bearerColliders == null)
			{
				return false;
			}
			if (!TryGetBounds(_bearerColliders, out var bounds))
			{
				return false;
			}
			if (_itemColliders == null)
			{
				_itemColliders = GetComponentsInChildren<Collider>(includeInactive: true);
			}
			if (TryGetBounds(_itemColliders, out var bounds2))
			{
				return bounds.Intersects(bounds2);
			}
			return false;
		}

		private static bool TryGetBounds(Collider[] colliders, out Bounds bounds)
		{
			bounds = default(Bounds);
			bool flag = false;
			foreach (Collider collider in colliders)
			{
				if (IsSolidActiveCollider(collider))
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
			return flag;
		}

		private static bool IsSolidActiveCollider(Collider candidate)
		{
			if (candidate != null && !candidate.isTrigger && candidate.enabled)
			{
				return candidate.gameObject.activeInHierarchy;
			}
			return false;
		}

		private void ApplyMagnet(AttachmentSlot slot)
		{
			if (!_rigidbody.isKinematic)
			{
				EnsureMountLocalPose();
				Quaternion quaternion = slot.Anchor.rotation * Quaternion.Inverse(_mountLocalRotation);
				Vector3 vector = slot.Anchor.position - quaternion * _mountLocalPosition - base.transform.position;
				_rigidbody.AddForce(vector * _magnetForce, ForceMode.Acceleration);
				(quaternion * Quaternion.Inverse(base.transform.rotation)).ToAngleAxis(out var angle, out var axis);
				if (angle > 180f)
				{
					angle -= 360f;
				}
				Vector3 vector2 = ((axis.sqrMagnitude > 0.0001f) ? (axis.normalized * (angle * (MathF.PI / 180f) * _magnetTorque)) : Vector3.zero);
				_rigidbody.AddTorque(vector2 - _rigidbody.angularVelocity * _magnetAngularDamping, ForceMode.Acceleration);
			}
		}

		private float MagnetRange(AttachmentSlot slot)
		{
			return slot.Radius + Mathf.Max(0f, _magnetRangeBonus);
		}

		private float DistanceToAnchor(AttachmentSlot slot)
		{
			return Vector3.Distance(MountPoint.position, slot.Anchor.position);
		}

		private void Dock(AttachmentSlot slot)
		{
			ClearReservation();
			DockedBearer = slot.Bearer;
			DockedSlotIndex = slot.SlotIndex;
			IsDockedInternal = true;
		}

		private void Undock()
		{
			ClearReservation();
			BlockReattach(_dockedSlot);
			IsDockedInternal = false;
		}

		private void BlockReattach(AttachmentSlot slot)
		{
			if (!(slot == null))
			{
				_reattachBlocked = true;
				_reattachBlockSlot = slot;
			}
		}

		private void UpdateReservation(AttachmentSlot slot)
		{
			NetworkObject bearer = slot.Bearer;
			int slotIndex = slot.SlotIndex;
			if (!(ReservedBearer == bearer) || ReservedSlotIndex != slotIndex)
			{
				ReservedBearer = bearer;
				ReservedSlotIndex = slotIndex;
				ReservedTick = base.Runner.Tick;
			}
		}

		private void ClearReservation()
		{
			if (!(ReservedBearer == null))
			{
				ReservedBearer = null;
				ReservedSlotIndex = 0;
				ReservedTick = 0;
			}
		}

		private bool ReservesSlot(AttachmentSlot slot)
		{
			if (ReservedBearer != null && ReservedBearer == slot.Bearer)
			{
				return ReservedSlotIndex == slot.SlotIndex;
			}
			return false;
		}

		private bool DockedAtSlot(AttachmentSlot slot)
		{
			if ((bool)IsDockedInternal && DockedBearer != null && DockedBearer == slot.Bearer)
			{
				return DockedSlotIndex == slot.SlotIndex;
			}
			return false;
		}

		private bool ClaimsSlot(AttachmentSlot slot)
		{
			if (!DockedAtSlot(slot))
			{
				return ReservesSlot(slot);
			}
			return true;
		}

		public static bool IsSlotClaimed(AttachmentSlot slot)
		{
			for (int i = 0; i < _all.Count; i++)
			{
				SlottableItem slottableItem = _all[i];
				if (slottableItem != null && slottableItem.ClaimsSlot(slot))
				{
					return true;
				}
			}
			return false;
		}

		private bool IsSlotTakenByOther(AttachmentSlot slot)
		{
			for (int i = 0; i < _all.Count; i++)
			{
				SlottableItem slottableItem = _all[i];
				if (slottableItem == null || slottableItem == this || !slottableItem.ClaimsSlot(slot))
				{
					continue;
				}
				bool flag = slottableItem.DockedAtSlot(slot);
				bool flag2 = DockedAtSlot(slot);
				if (flag && !flag2)
				{
					return true;
				}
				if (!flag && flag2)
				{
					continue;
				}
				if (flag && flag2)
				{
					if (slottableItem.Object.Id.Raw < base.Object.Id.Raw)
					{
						return true;
					}
					continue;
				}
				if (!ReservesSlot(slot))
				{
					return true;
				}
				if (slottableItem.ReservedTick < ReservedTick)
				{
					return true;
				}
				if (slottableItem.ReservedTick == ReservedTick && slottableItem.Object.Id.Raw < base.Object.Id.Raw)
				{
					return true;
				}
			}
			return false;
		}

		private void OnDockedChanged()
		{
			if ((bool)IsDockedInternal)
			{
				ApplyDock();
			}
			else
			{
				RestoreDock();
			}
		}

		private void ApplyDock()
		{
			if (!_dockApplied)
			{
				ResolveAnchor();
				if (!(_anchor == null))
				{
					EnsureMountLocalPose();
					_grabable.ChangeRigidbodyKinematic = false;
					_rigidbody.linearVelocity = Vector3.zero;
					_rigidbody.angularVelocity = Vector3.zero;
					_rigidbody.isKinematic = true;
					_detachSettleTicks = 8;
					_undockRequestTick = 0;
					_dockApplied = true;
					_dockedSlot.SetDockedGrabable(_grabable);
				}
			}
		}

		private void RestoreDock()
		{
			_grabable.ChangeRigidbodyKinematic = true;
			_rigidbody.isKinematic = !base.HasStateAuthority;
			if (_dockedSlot != null)
			{
				_dockedSlot.ClearDockedGrabable(_grabable);
				BlockReattach(_dockedSlot);
			}
			_dockApplied = false;
			_detachSettleTicks = 8;
			_undockRequestTick = 0;
			_anchor = null;
			_dockedSlot = null;
		}

		private void UpdateReattachLock(bool isGrabbed)
		{
			if (_reattachBlocked)
			{
				if (_reattachBlockSlot == null)
				{
					_reattachBlocked = false;
				}
				else if (!isGrabbed || Vector3.Distance(MountPoint.position, _reattachBlockSlot.Anchor.position) > MagnetRange(_reattachBlockSlot))
				{
					_reattachBlocked = false;
					_reattachBlockSlot = null;
				}
			}
		}

		private void ResolveAnchor()
		{
			if (!(DockedBearer == null))
			{
				AttachmentSlot[] componentsInChildren = DockedBearer.GetComponentsInChildren<AttachmentSlot>(includeInactive: true);
				if (DockedSlotIndex >= 0 && DockedSlotIndex < componentsInChildren.Length)
				{
					_dockedSlot = componentsInChildren[DockedSlotIndex];
					_anchor = _dockedSlot.Anchor;
				}
			}
		}

		private void EnsureMountLocalPose()
		{
			if (!_mountPoseCached)
			{
				if (_mountPoint == null)
				{
					_mountLocalPosition = Vector3.zero;
					_mountLocalRotation = Quaternion.identity;
				}
				else
				{
					_mountLocalPosition = base.transform.InverseTransformPoint(_mountPoint.position);
					_mountLocalRotation = Quaternion.Inverse(base.transform.rotation) * _mountPoint.rotation;
				}
				_mountPoseCached = true;
			}
		}

		private void SetBearerIsolation(NetworkObject bearer)
		{
			if (bearer == null)
			{
				ClearBearerIsolation();
				return;
			}
			_exitTicks = 90;
			if (_isolatedBearer == bearer)
			{
				ReassertBearerIsolation();
				RefreshIsolationCargo();
				return;
			}
			ClearBearerIsolation();
			_isolatedBearer = bearer;
			_exitTicks = 90;
			_bearerColliders = bearer.transform.root.GetComponentsInChildren<Collider>(includeInactive: true);
			IgnoreAll(_bearerColliders);
			_cargoRefreshTicks = 0;
			RefreshIsolationCargo();
		}

		private void ClearBearerIsolation()
		{
			foreach (Collider isolatedCollider in _isolatedColliders)
			{
				SetIgnored(isolatedCollider, ignore: false);
			}
			_isolatedColliders.Clear();
			_isolatedBearer = null;
			_bearerColliders = null;
			_exitTicks = 0;
		}

		private void RefreshIsolationCargo()
		{
			if (_cargoRefreshTicks > 0)
			{
				_cargoRefreshTicks--;
				return;
			}
			_cargoRefreshTicks = 10;
			if (_grabable.GrabbedByPlayers.Count == 0)
			{
				return;
			}
			ICartItemsContainer componentInChildren = _isolatedBearer.transform.root.GetComponentInChildren<ICartItemsContainer>(includeInactive: true);
			if (componentInChildren?.Items == null)
			{
				return;
			}
			foreach (IPointGrabable item in componentInChildren.Items)
			{
				if (item != null && item != _grabable)
				{
					MonoBehaviour monoBehaviour = item as MonoBehaviour;
					if (!(monoBehaviour == null))
					{
						IgnoreAll(monoBehaviour.transform.root.GetComponentsInChildren<Collider>(includeInactive: true));
					}
				}
			}
		}

		private void ReassertBearerIsolation()
		{
			if (_bearerColliders == null)
			{
				return;
			}
			for (int i = 0; i < _bearerColliders.Length; i++)
			{
				Collider collider = _bearerColliders[i];
				if (IsSolidActiveCollider(collider))
				{
					SetIgnored(collider, ignore: true);
				}
			}
		}

		private void IgnoreAll(Collider[] others)
		{
			foreach (Collider collider in others)
			{
				if (!(collider == null) && !collider.isTrigger && !_isolatedColliders.Contains(collider))
				{
					SetIgnored(collider, ignore: true);
					_isolatedColliders.Add(collider);
				}
			}
		}

		private void SetIgnored(Collider other, bool ignore)
		{
			if (other == null)
			{
				return;
			}
			if (_itemColliders == null)
			{
				_itemColliders = GetComponentsInChildren<Collider>(includeInactive: true);
			}
			for (int i = 0; i < _itemColliders.Length; i++)
			{
				Collider collider = _itemColliders[i];
				if (!(collider == null) && !collider.isTrigger)
				{
					Physics.IgnoreCollision(collider, other, ignore);
				}
			}
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 820474378u)]
		private void RequestUndockRPC([RpcPayload(4)] int playerId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(820474378u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AttachmentSlotModule.Scripts.SlottableItem::RequestUndockRPC(System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(playerId, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if ((bool)IsDockedInternal && _grabable.GrabbedByPlayers.Contains(playerId))
			{
				Undock();
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			IsDockedInternal = _IsDockedInternal;
			DockedBearer = _DockedBearer;
			DockedSlotIndex = _DockedSlotIndex;
			ReservedBearer = _ReservedBearer;
			ReservedSlotIndex = _ReservedSlotIndex;
			ReservedTick = _ReservedTick;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_IsDockedInternal = IsDockedInternal;
			_DockedBearer = DockedBearer;
			_DockedSlotIndex = DockedSlotIndex;
			_ReservedBearer = ReservedBearer;
			_ReservedSlotIndex = ReservedSlotIndex;
			_ReservedTick = ReservedTick;
		}

		[NetworkRpcWeavedInvoker(820474378u)]
		[Preserve]
		[WeaverGenerated]
		protected static void RequestUndockRPC_0040Invoker820474378([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out int value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((SlottableItem)context.TargetBehaviour).RequestUndockRPC(value);
		}
	}
}
