using System;
using Features.GrabModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.BeachInteractableCommonModule.Scripts
{
	[NetworkBehaviourWeaved(2)]
	public abstract class ButtonInteractReactor : NetworkBehaviour
	{
		[SerializeField]
		private SimplePointGrabable _jacuzziButtonGrabbable;

		[SerializeField]
		private ConfigurableJoint _buttonJoint;

		[SerializeField]
		private float _limitReachDelta = 0.008f;

		[SerializeField]
		private bool _defaultSwitchedOn;

		[WeaverGenerated]
		[DefaultForProperty("IsSwitchedOn", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _IsSwitchedOn;

		[WeaverGenerated]
		[DefaultForProperty("HasInitializedSwitch", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _HasInitializedSwitch;

		private bool _isArmed;

		private bool _didRebindWorldConnectedAnchor;

		[Networked]
		[OnChangedRender("OnIsSwitchedOnRender")]
		[NetworkedWeaved(0, 1)]
		private unsafe NetworkBool IsSwitchedOn
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ButtonInteractReactor.IsSwitchedOn. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ButtonInteractReactor.IsSwitchedOn. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		private unsafe NetworkBool HasInitializedSwitch
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ButtonInteractReactor.HasInitializedSwitch. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(Ptr + 1);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ButtonInteractReactor.HasInitializedSwitch. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 1) = value;
			}
		}

		public bool IsButtonSwitchedOn => IsSwitchedOn;

		public override void Spawned()
		{
			base.Spawned();
			RebindWorldConnectedAnchorIfNeeded();
			if (base.HasStateAuthority && !HasInitializedSwitch)
			{
				HasInitializedSwitch = true;
				IsSwitchedOn = _defaultSwitchedOn;
			}
			OnButtonInteracted(IsSwitchedOn);
		}

		public void SyncSwitchedOnFromExternal(bool isSwitchedOn)
		{
			if (base.HasStateAuthority)
			{
				if (!HasInitializedSwitch)
				{
					HasInitializedSwitch = true;
				}
				if (!(IsSwitchedOn == isSwitchedOn))
				{
					IsSwitchedOn = isSwitchedOn;
					OnButtonInteracted(isSwitchedOn);
				}
			}
		}

		private void FixedUpdate()
		{
			if (base.HasStateAuthority && !(_buttonJoint == null))
			{
				RebindWorldConnectedAnchorIfNeeded();
				float limit = _buttonJoint.linearLimit.limit;
				float offsetAlongMotionAxis = GetOffsetAlongMotionAxis();
				if (_isArmed && offsetAlongMotionAxis <= 0f - limit + _limitReachDelta)
				{
					_isArmed = false;
					bool flag = !IsSwitchedOn;
					IsSwitchedOn = flag;
					OnButtonInteracted(flag);
				}
				else if (!_isArmed && offsetAlongMotionAxis >= limit - _limitReachDelta)
				{
					_isArmed = true;
				}
			}
		}

		protected abstract void OnButtonInteracted(bool isSwitchedOn);

		private void RebindWorldConnectedAnchorIfNeeded()
		{
			if (_didRebindWorldConnectedAnchor || _buttonJoint == null)
			{
				return;
			}
			if (_buttonJoint.connectedBody != null)
			{
				_didRebindWorldConnectedAnchor = true;
				return;
			}
			Vector3 motionAxisWorld = GetMotionAxisWorld();
			Vector3 vector = _buttonJoint.transform.TransformPoint(_buttonJoint.anchor);
			float limit = _buttonJoint.linearLimit.limit;
			if (Mathf.Abs(Vector3.Dot(vector - _buttonJoint.connectedAnchor, motionAxisWorld)) > limit * 2f + 0.05f)
			{
				_buttonJoint.autoConfigureConnectedAnchor = false;
				_buttonJoint.connectedAnchor = vector - motionAxisWorld * limit;
			}
			_didRebindWorldConnectedAnchor = true;
		}

		private float GetOffsetAlongMotionAxis()
		{
			Rigidbody connectedBody = _buttonJoint.connectedBody;
			Vector3 vector = _buttonJoint.transform.TransformPoint(_buttonJoint.anchor);
			Vector3 vector2 = ((connectedBody == null) ? _buttonJoint.connectedAnchor : connectedBody.transform.TransformPoint(_buttonJoint.connectedAnchor));
			return Vector3.Dot(rhs: GetMotionAxisWorld(), lhs: vector - vector2);
		}

		private Vector3 GetMotionAxisWorld()
		{
			Rigidbody connectedBody = _buttonJoint.connectedBody;
			if (connectedBody != null)
			{
				return connectedBody.transform.up;
			}
			Vector3 vector = _buttonJoint.transform.TransformDirection(_buttonJoint.secondaryAxis);
			if (vector.sqrMagnitude < 1E-08f)
			{
				vector = _buttonJoint.transform.up;
			}
			return vector.normalized;
		}

		private void OnIsSwitchedOnRender()
		{
			OnButtonInteracted(IsSwitchedOn);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			IsSwitchedOn = _IsSwitchedOn;
			HasInitializedSwitch = _HasInitializedSwitch;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_IsSwitchedOn = IsSwitchedOn;
			_HasInitializedSwitch = HasInitializedSwitch;
		}
	}
}
