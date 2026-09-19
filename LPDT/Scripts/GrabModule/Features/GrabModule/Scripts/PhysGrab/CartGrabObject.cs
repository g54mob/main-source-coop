using System;
using System.Collections.Generic;
using Features.GrabModule.Scripts.PhysGrab.CartGrabber;
using Fusion;
using Global.SerializableDictionary;
using UnityEngine;

namespace Features.GrabModule.Scripts.PhysGrab
{
	[NetworkBehaviourWeaved(0)]
	public class CartGrabObject : GrabObjectBase
	{
		public enum FacingDirection
		{
			Forward = 0,
			Back = 1,
			Left = 2,
			Right = 3
		}

		[Serializable]
		public class GrabPointData
		{
			public Transform Transform;

			public float Offset;
		}

		[SerializeField]
		private float _rotationStrength = 10f;

		[SerializeField]
		private Global.SerializableDictionary.SerializableDictionary<FacingDirection, GrabPointData> _playerGrabPoints;

		[SerializeField]
		private float _velocityLerpSpeed;

		[SerializeField]
		private SimplePointGrabable _grabable;

		[SerializeField]
		private Rigidbody _rb;

		[SerializeField]
		private List<PhysGrabber> _playerGrabbing = new List<PhysGrabber>();

		[SerializeField]
		private Transform _heightCheckPointForward;

		[SerializeField]
		private Transform _heightCheckPointBack;

		[SerializeField]
		private float _heightCheckDistance = 5f;

		[SerializeField]
		private LayerMask _groundMask;

		private bool _hasCarrierFloor;

		private float _carrierFloorY;

		[SerializeField]
		private float _offset;

		[SerializeField]
		private float _yForceMultiplier = 5f;

		[SerializeField]
		private float _stiffness = 100f;

		[SerializeField]
		private float _damping = 5f;

		[SerializeField]
		private float _maxSpeed;

		[Range(0.1f, 100f)]
		[SerializeField]
		private float _forceMultiplier = 10f;

		[SerializeField]
		private MonoBehaviour _cartItemsGrabber;

		[SerializeField]
		private float _dampingStrenth = 2f;

		private float _objectsMass;

		private FacingDirection _direction;

		private bool _enableYGrab;

		private bool _forceInGrabPoint;

		private bool _terrainFollow;

		public override Rigidbody Rigidbody => _rb;

		public override List<PhysGrabber> Grabbers => _playerGrabbing;

		public override float GrabStrengthMultiplier { get; set; }

		public override ICartItemsContainer CartItemsGrabber => _cartItemsGrabber as ICartItemsContainer;

		public override bool GrabbingPhysicsBlocked { get; set; }

		public void FixedUpdate()
		{
			GrabbingPhysics();
		}

		public void SetObjectsMass(float mass)
		{
			_objectsMass = mass;
		}

		public bool TryGetHeldDirection(PhysGrabber grabber, out FacingDirection direction)
		{
			GrabPointData grabPointData;
			return TryGetHeldGrabPoint(grabber, out direction, out grabPointData);
		}

		public bool TryGetHeldGrabPoint(PhysGrabber grabber, out FacingDirection direction, out GrabPointData grabPointData)
		{
			direction = FacingDirection.Forward;
			grabPointData = null;
			if (grabber == null || !grabber.physGrabPoints.TryGetValue(this, out var value) || value == null)
			{
				return false;
			}
			foreach (KeyValuePair<FacingDirection, GrabPointData> playerGrabPoint in _playerGrabPoints)
			{
				if (!(playerGrabPoint.Value.Transform != value))
				{
					direction = playerGrabPoint.Key;
					grabPointData = playerGrabPoint.Value;
					return true;
				}
			}
			return false;
		}

		public bool TryGetGrabberOfPlayer(int playerId, out PhysGrabber grabber)
		{
			foreach (PhysGrabber item in _playerGrabbing)
			{
				if (!(item == null) && item.PlayerId == playerId)
				{
					grabber = item;
					return true;
				}
			}
			grabber = null;
			return false;
		}

		public void EnableYGrab(bool enable)
		{
			_enableYGrab = enable;
		}

		public void SetForceInGrabPoint(bool forceInGrabPoint)
		{
			_forceInGrabPoint = forceInGrabPoint;
		}

		public void SetTerrainFollow(bool value)
		{
			_terrainFollow = value;
		}

		private void GrabbingPhysics()
		{
			if (GrabbingPhysicsBlocked || Grabbers.Count == 0)
			{
				return;
			}
			PhysGrabber physGrabber = DrivingGrabber();
			if (physGrabber == null)
			{
				return;
			}
			if (_cartItemsGrabber is CartItemsGrabber cartItemsGrabber)
			{
				foreach (IPointGrabable item in cartItemsGrabber.Items)
				{
					if (item != null && !(item.NetworkObject == null) && !cartItemsGrabber.ParentGrabbers.ContainsKey(item) && item.NetworkObject.StateAuthority != physGrabber.Object.StateAuthority)
					{
						return;
					}
				}
			}
			Vector3 position = physGrabber.physGrabPointPullerPosition.position;
			if (!_enableYGrab && !_terrainFollow)
			{
				Vector3 eulerAngles = _rb.rotation.eulerAngles;
				_rb.rotation = Quaternion.Lerp(_rb.rotation, Quaternion.Euler(0f, eulerAngles.y, 0f), Time.deltaTime * 5f);
				float offset = _offset;
				if (TryGetHeldGrabPoint(physGrabber, out var _, out var grabPointData))
				{
					offset = grabPointData.Offset;
				}
				if (TryGetGroundPoint(out var groundPoint))
				{
					float y = groundPoint.y + offset;
					position.y = y;
				}
				else
				{
					position.y = base.transform.position.y;
				}
			}
			if (!(physGrabber.physGrabPoints[this] == null))
			{
				if (!_enableYGrab && _terrainFollow)
				{
					position.y = physGrabber.physGrabPoints[this].position.y;
				}
				Vector3 vector = position - physGrabber.physGrabPoints[this].position;
				vector = Vector3.ClampMagnitude(vector, 2f);
				float num = _rb.mass + _objectsMass;
				Vector3 vector2 = new Vector3(vector.x * _stiffness - _rb.linearVelocity.x * _damping, vector.y * _stiffness * _yForceMultiplier - _rb.linearVelocity.y * _damping, vector.z * _stiffness - _rb.linearVelocity.z * _damping);
				if (!_enableYGrab && _terrainFollow)
				{
					vector2.y = 0f;
				}
				Vector3 force = vector2 * num;
				float num2 = num * _forceMultiplier;
				if (force.magnitude > num2)
				{
					force = force.normalized * num2;
				}
				if (_rb.linearVelocity.magnitude > _maxSpeed)
				{
					force *= 0.5f;
				}
				if (_forceInGrabPoint)
				{
					_rb.AddForceAtPosition(force, physGrabber.physGrabPoints[this].position, ForceMode.Force);
				}
				else
				{
					_rb.AddForce(force, ForceMode.Force);
				}
				ProcessRotation();
			}
		}

		private PhysGrabber DrivingGrabber()
		{
			foreach (PhysGrabber item in _playerGrabbing)
			{
				if (item != null && item.Object != null && item.Object.StateAuthority == base.Object.StateAuthority)
				{
					return item;
				}
			}
			return null;
		}

		private void ProcessRotation()
		{
			if (!base.HasStateAuthority || _grabable == null || _grabable.GrabbedByPlayers.Count == 0)
			{
				return;
			}
			PhysGrabber physGrabber = DrivingGrabber();
			if (physGrabber == null)
			{
				return;
			}
			if (TryGetHeldDirection(physGrabber, out var direction))
			{
				_direction = direction;
			}
			Transform playerLookTransform = physGrabber.PlayerLookDetection.GetPlayerLookTransform();
			if (!(playerLookTransform == null))
			{
				Vector3 vector = playerLookTransform.forward;
				vector.y = 0f;
				vector.Normalize();
				switch (_direction)
				{
				case FacingDirection.Back:
					vector = -vector;
					break;
				case FacingDirection.Left:
					vector = -playerLookTransform.right;
					vector.y = 0f;
					vector.Normalize();
					break;
				case FacingDirection.Right:
					vector = playerLookTransform.right;
					vector.y = 0f;
					vector.Normalize();
					break;
				}
				Quaternion target = Quaternion.LookRotation(vector, Vector3.up);
				ApplyYawRotation(target);
			}
		}

		private void ApplyYawRotation(Quaternion target)
		{
			(target * Quaternion.Inverse(base.transform.rotation)).ToAngleAxis(out var angle, out var axis);
			if (angle > 180f)
			{
				angle -= 360f;
			}
			if (!float.IsNaN(axis.y))
			{
				float num = angle * axis.y * (MathF.PI / 180f) * _rotationStrength;
				float num2 = _rb.angularVelocity.y * _dampingStrenth;
				float b = num - num2;
				Vector3 angularVelocity = _rb.angularVelocity;
				angularVelocity.y = Mathf.Lerp(angularVelocity.y, b, base.Runner.DeltaTime * _velocityLerpSpeed);
				_rb.angularVelocity = angularVelocity;
			}
		}

		public void SetCarrierFloor(float worldY)
		{
			_carrierFloorY = worldY;
			_hasCarrierFloor = true;
		}

		public void ClearCarrierFloor()
		{
			_hasCarrierFloor = false;
		}

		private bool TryGetGroundPoint(out Vector3 groundPoint)
		{
			if (_hasCarrierFloor)
			{
				groundPoint = new Vector3(base.transform.position.x, _carrierFloorY, base.transform.position.z);
				return true;
			}
			RaycastHit hitInfo;
			bool flag = Physics.Raycast(_heightCheckPointForward.position, Vector3.down, out hitInfo, _heightCheckDistance, _groundMask);
			RaycastHit hitInfo2;
			bool flag2 = Physics.Raycast(_heightCheckPointBack.position, Vector3.down, out hitInfo2, _heightCheckDistance, _groundMask);
			if (flag && flag2)
			{
				if (hitInfo.point.y > hitInfo2.point.y)
				{
					groundPoint = hitInfo.point;
				}
				else
				{
					groundPoint = hitInfo2.point;
				}
				return true;
			}
			if (flag)
			{
				groundPoint = hitInfo.point;
				return true;
			}
			if (flag2)
			{
				groundPoint = hitInfo2.point;
				return true;
			}
			groundPoint = Vector3.zero;
			return false;
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
