using System.Collections.Generic;
using Features.CameraModelModule;
using Features.GrabModule.Scripts.PhysGrab.CartGrabber;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.GrabModule.Scripts.PhysGrab
{
	[NetworkBehaviourWeaved(0)]
	public class GrabObject : GrabObjectBase
	{
		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private List<PhysGrabber> _playerGrabbing = new List<PhysGrabber>();

		[SerializeField]
		private float _yAxisDependsOnCameraTorqueStrength = 10f;

		[SerializeField]
		private CartItemsGrabber _cartItemsGrabber;

		[SerializeField]
		private CartV2CargoRegistry _cargoRegistry;

		[SerializeField]
		private float _grabStrengthMultiplier = 1f;

		[SerializeField]
		private AnimationCurve _grabStrengthCurve;

		public bool IsFreezeZRepositioning;

		private CameraModel _cameraModel;

		private IRiderCarrierVolume _riderCarrier;

		public override Rigidbody Rigidbody => _rigidbody;

		public override List<PhysGrabber> Grabbers => _playerGrabbing;

		public override float GrabStrengthMultiplier
		{
			get
			{
				return _grabStrengthMultiplier;
			}
			set
			{
				_grabStrengthMultiplier = value;
			}
		}

		public override ICartItemsContainer CartItemsGrabber
		{
			get
			{
				if (!(_cargoRegistry != null))
				{
					return _cartItemsGrabber;
				}
				return _cargoRegistry;
			}
		}

		public override bool GrabbingPhysicsBlocked { get; set; }

		public float MassOverride { get; set; }

		[Inject]
		private void InjectDependencies(CameraModel cameraModel)
		{
			_cameraModel = cameraModel;
		}

		private void Awake()
		{
			_riderCarrier = GetComponent<IRiderCarrierVolume>();
		}

		private void FixedUpdate()
		{
			GrabbingPhysics();
		}

		private void GrabbingPhysics()
		{
			if (GrabbingPhysicsBlocked || _cameraModel == null || _cameraModel.CameraObject == null || _playerGrabbing.Count <= 0 || base.Object == null || _playerGrabbing[0].Object == null || base.Object.StateAuthority != _playerGrabbing[0].Object.StateAuthority)
			{
				return;
			}
			if (CartItemsGrabber is CartItemsGrabber cartItemsGrabber)
			{
				foreach (IPointGrabable item in cartItemsGrabber.Items)
				{
					if (item != null && !(item.NetworkObject == null) && !cartItemsGrabber.ParentGrabbers.ContainsKey(item) && item.NetworkObject.StateAuthority != _playerGrabbing[0].Object.StateAuthority)
					{
						return;
					}
				}
			}
			Vector3 zero = Vector3.zero;
			float num = ((MassOverride > 0f) ? MassOverride : _rigidbody.mass);
			foreach (PhysGrabber item2 in _playerGrabbing)
			{
				if (!IsSelfRiderGrabber(item2))
				{
					Vector3 vector = Vector3.zero;
					if (item2.physGrabPoints.ContainsKey(this) && item2.physGrabPoints[this] != null)
					{
						vector = item2.physGrabPoints[this].position;
					}
					if (item2.UseOwnPositionAsGrabPoint || !item2.IsProcessPhysGrabbing || !item2.physGrabPoints.ContainsKey(this) || item2.physGrabPoints[this] == null)
					{
						vector = base.transform.position;
					}
					float forceMax = item2.forceMax;
					Vector3 position = item2.physGrabPointPullerPosition.position;
					Vector3 vector2 = Vector3.ClampMagnitude(Vector3.ClampMagnitude(position - vector, forceMax) * 10f, forceMax);
					Vector3 pointVelocity = _rigidbody.GetPointVelocity(vector);
					Vector3 vector3 = Vector3.ClampMagnitude(vector2 * item2.springConstant - pointVelocity * item2.dampingConstant, forceMax) * 2f / num;
					float num2 = item2.grabStrength * GrabStrengthMultiplier;
					float num3 = 10f;
					float num4 = Mathf.Min(num2, 30f);
					float b = num2 / (1f + num4);
					float t = Mathf.Min((num2 - 1f) / num3, 0.9f);
					num2 = Mathf.Lerp(num2, b, t);
					Vector3 force = vector3 * num2 * item2.forceConstant * _grabStrengthCurve.Evaluate(_playerGrabbing.Count);
					float num5 = Mathf.Min(Vector3.Distance(position, vector) * 10f, 1f);
					force *= num5;
					if (IsFreezeZRepositioning)
					{
						force = new Vector3(force.x, force.y, 0f);
					}
					force = item2.ApplyForceAxisMask(force);
					Vector3 force2 = Vector3.Lerp(item2.currentGrabForce, force, 0.8f);
					item2.currentGrabForce = force;
					_rigidbody.AddForceAtPosition(force2, vector, ForceMode.Force);
					zero += force;
				}
			}
			PhysGrabber physGrabber = FirstNonRiderGrabber();
			if (physGrabber != null)
			{
				if (physGrabber.physGrabPoints.ContainsKey(this) && physGrabber.physGrabPoints[this] != null)
				{
					Vector3 normalized = (_rigidbody.centerOfMass - physGrabber.physGrabPoints[this].localPosition).normalized;
					Vector3 vector4 = base.transform.rotation * normalized;
					Vector3 forward = _cameraModel.AimTransform.forward;
					Vector3 torque = new Vector3(0f, ((base.transform.up.y < 0f) ? Vector3.Cross(forward, vector4) : Vector3.Cross(vector4, forward)).y * _yAxisDependsOnCameraTorqueStrength, 0f);
					_rigidbody.AddTorque(torque, ForceMode.Force);
				}
				if (!_rigidbody.isKinematic)
				{
					_rigidbody.angularVelocity *= 0.8f;
				}
			}
			if (zero.magnitude > 0f && !_rigidbody.isKinematic)
			{
				_rigidbody.linearVelocity *= 0.9f;
			}
		}

		private bool IsSelfRiderGrabber(PhysGrabber grabber)
		{
			if (_riderCarrier != null && grabber != null && grabber.Object != null)
			{
				return _riderCarrier.IsRiddenByPlayer(grabber.Object.StateAuthority.PlayerId);
			}
			return false;
		}

		private PhysGrabber FirstNonRiderGrabber()
		{
			foreach (PhysGrabber item in _playerGrabbing)
			{
				if (item != null && !IsSelfRiderGrabber(item))
				{
					return item;
				}
			}
			return null;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}
	}
}
