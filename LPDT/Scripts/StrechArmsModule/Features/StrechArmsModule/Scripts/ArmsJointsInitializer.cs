using System;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.StrechArmsModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class ArmsJointsInitializer : NetworkBehaviour
	{
		[SerializeField]
		private Joint _leftArmJoint;

		[SerializeField]
		private Joint _rightArmJoint;

		[SerializeField]
		private float _ragdollLinearLimitDamping = 20f;

		private ArmStartsModel _armStartsModel;

		[Inject]
		public void InjectDependencies(ArmStartsModel armStartsModel)
		{
			_armStartsModel = armStartsModel;
		}

		public override void Spawned()
		{
			base.Spawned();
			if (!(base.Object == null) && !(_leftArmJoint == null) && !(_rightArmJoint == null))
			{
				IArmEndEntity end = _armStartsModel.GetEnd(base.Object.InputAuthority, Arm.Left);
				IArmEndEntity end2 = _armStartsModel.GetEnd(base.Object.InputAuthority, Arm.Right);
				if (end != null)
				{
					ConfigureArmJoint(_leftArmJoint, end.Transform.GetComponent<Rigidbody>());
				}
				else
				{
					ArmStartsModel armStartsModel = _armStartsModel;
					armStartsModel.OnArmEndAdded = (Action<PlayerRef, IArmEndEntity, Arm>)Delegate.Combine(armStartsModel.OnArmEndAdded, new Action<PlayerRef, IArmEndEntity, Arm>(InitializeArmJointsLeft));
				}
				if (end2 != null)
				{
					ConfigureArmJoint(_rightArmJoint, end2.Transform.GetComponent<Rigidbody>());
					return;
				}
				ArmStartsModel armStartsModel2 = _armStartsModel;
				armStartsModel2.OnArmEndAdded = (Action<PlayerRef, IArmEndEntity, Arm>)Delegate.Combine(armStartsModel2.OnArmEndAdded, new Action<PlayerRef, IArmEndEntity, Arm>(InitializeArmJointsRight));
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			ArmStartsModel armStartsModel = _armStartsModel;
			armStartsModel.OnArmEndAdded = (Action<PlayerRef, IArmEndEntity, Arm>)Delegate.Remove(armStartsModel.OnArmEndAdded, new Action<PlayerRef, IArmEndEntity, Arm>(InitializeArmJointsLeft));
			ArmStartsModel armStartsModel2 = _armStartsModel;
			armStartsModel2.OnArmEndAdded = (Action<PlayerRef, IArmEndEntity, Arm>)Delegate.Remove(armStartsModel2.OnArmEndAdded, new Action<PlayerRef, IArmEndEntity, Arm>(InitializeArmJointsRight));
		}

		private void InitializeArmJointsRight(PlayerRef playerRef, IArmEndEntity _, Arm arm)
		{
			if (!(playerRef != base.Object.InputAuthority) && arm == Arm.Right)
			{
				ConfigureArmJoint(_rightArmJoint, _armStartsModel.GetEnd(base.Object.InputAuthority, Arm.Right).Transform.GetComponent<Rigidbody>());
				ArmStartsModel armStartsModel = _armStartsModel;
				armStartsModel.OnArmEndAdded = (Action<PlayerRef, IArmEndEntity, Arm>)Delegate.Remove(armStartsModel.OnArmEndAdded, new Action<PlayerRef, IArmEndEntity, Arm>(InitializeArmJointsRight));
			}
		}

		private void InitializeArmJointsLeft(PlayerRef playerRef, IArmEndEntity _, Arm arm)
		{
			if (!(playerRef != base.Object.InputAuthority) && arm == Arm.Left)
			{
				ConfigureArmJoint(_leftArmJoint, _armStartsModel.GetEnd(base.Object.InputAuthority, Arm.Left).Transform.GetComponent<Rigidbody>());
				ArmStartsModel armStartsModel = _armStartsModel;
				armStartsModel.OnArmEndAdded = (Action<PlayerRef, IArmEndEntity, Arm>)Delegate.Remove(armStartsModel.OnArmEndAdded, new Action<PlayerRef, IArmEndEntity, Arm>(InitializeArmJointsLeft));
			}
		}

		private void ConfigureArmJoint(Joint joint, Rigidbody connectedBody)
		{
			joint.autoConfigureConnectedAnchor = false;
			joint.connectedAnchor = Vector3.zero;
			joint.connectedBody = connectedBody;
			if (joint is ConfigurableJoint { linearLimit: var linearLimit } configurableJoint)
			{
				linearLimit.bounciness = 0f;
				configurableJoint.linearLimit = linearLimit;
				SoftJointLimitSpring linearLimitSpring = configurableJoint.linearLimitSpring;
				linearLimitSpring.damper = Mathf.Max(linearLimitSpring.damper, _ragdollLinearLimitDamping);
				configurableJoint.linearLimitSpring = linearLimitSpring;
			}
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
