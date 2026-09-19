using System;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.HingeModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class HingeSetupHandler : NetworkBehaviour
	{
		private struct HingeJointSnapshot
		{
			public bool IsCreated;

			public Rigidbody ConnectedBody;

			public ArticulationBody ConnectedArticulationBody;

			public Vector3 Axis;

			public Vector3 Anchor;

			public Vector3 ConnectedAnchor;

			public bool AutoConfigureConnectedAnchor;

			public bool UseSpring;

			public JointSpring Spring;

			public bool UseMotor;

			public JointMotor Motor;

			public bool UseLimits;

			public JointLimits Limits;

			public float BreakForce;

			public float BreakTorque;

			public bool EnableCollision;

			public bool EnablePreprocessing;

			public float MassScale;

			public float ConnectedMassScale;
		}

		[SerializeField]
		private HingeSetupType _hingeSetupType;

		[SerializeField]
		private Rigidbody _targetRigidbody;

		[SerializeField]
		private HingeJoint _hingeJoint;

		[SerializeField]
		private bool _reinitializeJointOnSpawn;

		private HingeSetupConfiguration _configuration;

		private HingeJointSnapshot _hingeJointSnapshot;

		private bool _isStartCalled;

		private bool _isJointWaitingForReinitialize;

		private bool _isJointInitialized;

		public bool IsJointInitialized
		{
			get
			{
				return _isJointInitialized;
			}
			private set
			{
				_isJointInitialized = value;
				if (_isJointInitialized)
				{
					this.OnJointInitialized?.Invoke();
				}
			}
		}

		public event Action OnJointInitialized;

		[Inject]
		private void InjectDependencies(HingeSetupConfiguration configuration)
		{
			_configuration = configuration;
		}

		private void Awake()
		{
			if (_reinitializeJointOnSpawn && _hingeJoint != null)
			{
				_hingeJointSnapshot = CreateSnapshot(_hingeJoint);
			}
		}

		private void Start()
		{
			_isStartCalled = true;
			TryReinitializeJoint();
		}

		public override void Spawned()
		{
			base.Spawned();
			if (_configuration.HingeSetup.TryGetValue(_hingeSetupType, out var value))
			{
				_targetRigidbody.linearDamping = value.LinearDamping;
				_targetRigidbody.angularDamping = value.AngularDamping;
			}
			if (!_reinitializeJointOnSpawn)
			{
				IsJointInitialized = true;
				return;
			}
			if (_hingeJoint != null)
			{
				_hingeJointSnapshot = CreateSnapshot(_hingeJoint);
				UnityEngine.Object.Destroy(_hingeJoint);
				_hingeJoint = null;
			}
			_isJointWaitingForReinitialize = true;
			TryReinitializeJoint();
		}

		private void TryReinitializeJoint()
		{
			if (_isStartCalled && _isJointWaitingForReinitialize && _hingeJointSnapshot.IsCreated)
			{
				_hingeJoint = base.gameObject.AddComponent<HingeJoint>();
				ApplySnapshot(_hingeJoint, _hingeJointSnapshot);
				_isJointWaitingForReinitialize = false;
				IHingeInitializable[] components = GetComponents<IHingeInitializable>();
				for (int i = 0; i < components.Length; i++)
				{
					components[i].InitializeHingeJoint(_hingeJoint);
				}
				IsJointInitialized = true;
			}
		}

		private HingeJointSnapshot CreateSnapshot(HingeJoint hingeJoint)
		{
			return new HingeJointSnapshot
			{
				IsCreated = true,
				ConnectedBody = hingeJoint.connectedBody,
				ConnectedArticulationBody = hingeJoint.connectedArticulationBody,
				Axis = hingeJoint.axis,
				Anchor = hingeJoint.anchor,
				ConnectedAnchor = hingeJoint.connectedAnchor,
				AutoConfigureConnectedAnchor = hingeJoint.autoConfigureConnectedAnchor,
				UseSpring = hingeJoint.useSpring,
				Spring = hingeJoint.spring,
				UseMotor = hingeJoint.useMotor,
				Motor = hingeJoint.motor,
				UseLimits = hingeJoint.useLimits,
				Limits = hingeJoint.limits,
				BreakForce = hingeJoint.breakForce,
				BreakTorque = hingeJoint.breakTorque,
				EnableCollision = hingeJoint.enableCollision,
				EnablePreprocessing = hingeJoint.enablePreprocessing,
				MassScale = hingeJoint.massScale,
				ConnectedMassScale = hingeJoint.connectedMassScale
			};
		}

		private void ApplySnapshot(HingeJoint hingeJoint, HingeJointSnapshot snapshot)
		{
			hingeJoint.connectedBody = snapshot.ConnectedBody;
			hingeJoint.connectedArticulationBody = snapshot.ConnectedArticulationBody;
			hingeJoint.axis = snapshot.Axis;
			hingeJoint.anchor = snapshot.Anchor;
			hingeJoint.autoConfigureConnectedAnchor = snapshot.AutoConfigureConnectedAnchor;
			hingeJoint.connectedAnchor = snapshot.ConnectedAnchor;
			hingeJoint.useSpring = snapshot.UseSpring;
			hingeJoint.spring = snapshot.Spring;
			hingeJoint.useMotor = snapshot.UseMotor;
			hingeJoint.motor = snapshot.Motor;
			hingeJoint.useLimits = snapshot.UseLimits;
			hingeJoint.limits = snapshot.Limits;
			hingeJoint.breakForce = snapshot.BreakForce;
			hingeJoint.breakTorque = snapshot.BreakTorque;
			hingeJoint.enableCollision = snapshot.EnableCollision;
			hingeJoint.enablePreprocessing = snapshot.EnablePreprocessing;
			hingeJoint.massScale = snapshot.MassScale;
			hingeJoint.connectedMassScale = snapshot.ConnectedMassScale;
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
