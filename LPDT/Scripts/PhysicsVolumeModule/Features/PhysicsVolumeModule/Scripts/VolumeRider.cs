using Features.Movement.Scripts;
using Features.RagdollModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.PhysicsVolumeModule.Scripts
{
	[RequireComponent(typeof(Rigidbody))]
	public class VolumeRider : MonoBehaviour
	{
		private Rigidbody _rigidbody;

		private NetworkObject _networkObject;

		private Transform _carrier;

		private PhysicsInfluenceVolume _volume;

		private PlayerCharacterMovableBase _player;

		private Vector3 _lastCarrierPosition;

		private Quaternion _lastCarrierRotation;

		private float _linearTransfer = 1f;

		private float _angularTransfer = 1f;

		private const float FLOOR_SETTLE_TOLERANCE = 0.12f;

		private const float FLOOR_SETTLE_STEP = 0.04f;

		private const float FLOOR_SETTLE_MAX_UP_VELOCITY = 1f;

		private bool _isSlotted;

		private Vector3 _slotLocalPosition;

		private float _tiltOverAngleSeconds;

		private const float TILT_RELEASE_HOLD_SECONDS = 0.35f;

		private const float RIDER_COLUMN_HEIGHT = 2.5f;

		private RagdollEntity _ragdoll;

		public bool IsSlotted => _isSlotted;

		public string LastSlotRelease { get; private set; } = "none";

		public int RagdollReleases { get; private set; }

		public int JumpReleases { get; private set; }

		public int TiltReleases { get; private set; }

		public float LastCarrierTiltDeg { get; private set; }

		public float MaxCarrierTiltDeg { get; private set; }

		private bool HasControl
		{
			get
			{
				if (!(_networkObject == null))
				{
					return _networkObject.HasInputAuthority;
				}
				return true;
			}
		}

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody>();
			_networkObject = GetComponent<NetworkObject>();
		}

		public void SetCarrier(Transform carrier, PhysicsInfluenceVolume volume, float linearTransfer, float angularTransfer, PlayerCharacterMovableBase player)
		{
			_carrier = carrier;
			_volume = volume;
			_player = player;
			_linearTransfer = linearTransfer;
			_angularTransfer = angularTransfer;
			_volume.GetCarrierRenderPose(out _lastCarrierPosition, out _lastCarrierRotation);
			_isSlotted = false;
			_ragdoll = ((player != null) ? player.GetComponentInChildren<RagdollEntity>(includeInactive: true) : null);
		}

		public void ClearCarrier(Transform carrier)
		{
			if (!(_carrier != carrier))
			{
				_carrier = null;
				_isSlotted = false;
			}
		}

		private void FixedUpdate()
		{
			if (!(_carrier == null) && HasControl && _isSlotted)
			{
				_volume.GetCarrierRenderPose(out var position, out var rotation);
				if (StillSlotted(rotation))
				{
					Matrix4x4 matrix4x = Matrix4x4.TRS(position, rotation, _carrier.lossyScale);
					_rigidbody.position = matrix4x.MultiplyPoint3x4(_slotLocalPosition);
					_rigidbody.linearVelocity = Vector3.zero;
				}
			}
		}

		private void Update()
		{
			if (_carrier == null || !HasControl)
			{
				return;
			}
			_volume.GetCarrierRenderPose(out var position, out var rotation);
			Quaternion quaternion = rotation * Quaternion.Inverse(_lastCarrierRotation);
			Vector3 vector = position - _lastCarrierPosition;
			Vector3 vector2 = _rigidbody.position - _lastCarrierPosition;
			Vector3 position2 = _lastCarrierPosition + quaternion * vector2 + vector * _linearTransfer;
			_rigidbody.position = position2;
			if (!TrySlotToCarrierFloor(position, rotation))
			{
				SettleOnFloor();
			}
			if (_networkObject != null)
			{
				_rigidbody.PublishTransform();
			}
			if (_player != null)
			{
				if (_angularTransfer > 0f)
				{
					float num = Mathf.DeltaAngle(_lastCarrierRotation.eulerAngles.y, rotation.eulerAngles.y);
					_player.AddCarrierYaw(num * _angularTransfer);
				}
				Matrix4x4 matrix4x = Matrix4x4.TRS(position, rotation, _carrier.lossyScale);
				_player.StagePlatformAnchor(matrix4x.inverse.MultiplyPoint3x4(_rigidbody.position));
			}
			_lastCarrierPosition = position;
			_lastCarrierRotation = rotation;
		}

		private bool TrySlotToCarrierFloor(Vector3 carrierPosition, Quaternion carrierRotation)
		{
			if (_volume == null || !_volume.SlotsRidersToFloor || _player == null || _player.BodyCollider == null)
			{
				return false;
			}
			if (!StillSlotted(carrierRotation))
			{
				return false;
			}
			Matrix4x4 carrierMatrix = Matrix4x4.TRS(carrierPosition, carrierRotation, _carrier.lossyScale);
			if (!_isSlotted)
			{
				if (!_volume.ContainsRiderInShape(_rigidbody, 2.5f))
				{
					if (!_volume.TryGetCarrierFloorY(_rigidbody, out var floorY))
					{
						return false;
					}
					if (Mathf.Abs(_player.BodyCollider.bounds.min.y - floorY) > 0.12f)
					{
						return false;
					}
				}
				if (!TryResolveSeatLocal(carrierMatrix, out _slotLocalPosition))
				{
					return false;
				}
				_isSlotted = true;
			}
			_rigidbody.position = carrierMatrix.MultiplyPoint3x4(_slotLocalPosition);
			_rigidbody.linearVelocity = Vector3.zero;
			return true;
		}

		private bool TryResolveSeatLocal(Matrix4x4 carrierMatrix, out Vector3 seatLocal)
		{
			seatLocal = Vector3.zero;
			if (!_volume.TryGetRiderSeatWorld(out var seatWorld))
			{
				return false;
			}
			seatWorld.y += _rigidbody.position.y - _player.BodyCollider.bounds.min.y;
			seatLocal = carrierMatrix.inverse.MultiplyPoint3x4(seatWorld);
			return true;
		}

		private bool StillSlotted(Quaternion carrierRotation)
		{
			float num = (LastCarrierTiltDeg = Vector3.Angle(carrierRotation * Vector3.up, Vector3.up));
			if (num > MaxCarrierTiltDeg)
			{
				MaxCarrierTiltDeg = num;
			}
			if (_ragdoll != null && (_ragdoll.IsSimulated || _ragdoll.IsRagdollSimulated || _ragdoll.IsBlendingOut))
			{
				if (_isSlotted)
				{
					RagdollReleases++;
					LastSlotRelease = "ragdoll";
				}
				_isSlotted = false;
				return false;
			}
			if (_player != null && _player.IsJumping)
			{
				if (_isSlotted)
				{
					JumpReleases++;
					LastSlotRelease = "jump";
				}
				_isSlotted = false;
				return false;
			}
			if (_volume != null && num > _volume.RiderSlotReleaseAngle)
			{
				_tiltOverAngleSeconds += Time.deltaTime;
				if (_tiltOverAngleSeconds >= 0.35f)
				{
					if (_isSlotted)
					{
						TiltReleases++;
						LastSlotRelease = $"tilt {num:F0}deg held {_tiltOverAngleSeconds:F2}s";
					}
					_isSlotted = false;
					return false;
				}
			}
			else
			{
				_tiltOverAngleSeconds = 0f;
			}
			return true;
		}

		private void SettleOnFloor()
		{
			if (_player == null || _player.BodyCollider == null || _rigidbody.linearVelocity.y > 1f || !_volume.TryGetCarrierFloorY(_rigidbody, out var floorY))
			{
				return;
			}
			float num = _player.BodyCollider.bounds.min.y - floorY;
			if (!(Mathf.Abs(num) > 0.12f))
			{
				float target = _rigidbody.position.y - num;
				Vector3 position = _rigidbody.position;
				position.y = Mathf.MoveTowards(position.y, target, 0.04f);
				_rigidbody.position = position;
				Vector3 linearVelocity = _rigidbody.linearVelocity;
				if (linearVelocity.y < 0f)
				{
					linearVelocity.y = 0f;
					_rigidbody.linearVelocity = linearVelocity;
				}
			}
		}
	}
}
