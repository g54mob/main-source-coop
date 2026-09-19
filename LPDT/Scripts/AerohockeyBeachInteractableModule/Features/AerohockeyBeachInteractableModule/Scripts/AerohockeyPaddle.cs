using Features.CustomSynchronizersModule.Scripts;
using Features.GrabModule.Scripts;
using Features.GrabModule.Scripts.PhysGrab;
using Fusion;
using UnityEngine;

namespace Features.AerohockeyBeachInteractableModule.Scripts
{
	public class AerohockeyPaddle : MonoBehaviour
	{
		private const float PuckContactSkin = 0.012f;

		[SerializeField]
		private AerohockeySide _side;

		[SerializeField]
		private AerohockeyBeachInteractableBehaviour _table;

		[SerializeField]
		private SimplePointGrabable _simplePointGrabable;

		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private Collider _hitCollider;

		[SerializeField]
		private Collider _authorityClaimCollider;

		[SerializeField]
		private Transform _driveTransform;

		[SerializeField]
		private PhysicsSynchronizer _physicsSynchronizer;

		[SerializeField]
		private float _puckBlockSphereRadius = 0.08f;

		private readonly RaycastHit[] _puckCastHits = new RaycastHit[8];

		private bool _wasLocalPlayerGrabbing;

		private Vector3 _restLocalPosition;

		private Quaternion _restLocalRotation;

		private bool _restCaptured;

		private Vector3 _lastWorldPosition;

		private Vector3 _worldVelocity;

		private bool _hasLastWorldPosition;

		private float _moveSpeed = 25f;

		private bool _spawnNetworkPosePublished;

		public SimplePointGrabable SimplePointGrabable => _simplePointGrabable;

		public Transform DriveTransform => _driveTransform;

		public Collider HitCollider => _hitCollider;

		public Collider AuthorityClaimCollider => _authorityClaimCollider;

		public AerohockeySide Side => _side;

		public Vector3 WorldVelocity
		{
			get
			{
				if (_physicsSynchronizer != null && _physicsSynchronizer.Object != null && _physicsSynchronizer.Object.IsValid && !_physicsSynchronizer.HasStateAuthority)
				{
					return _physicsSynchronizer.LinearVelocity;
				}
				return _worldVelocity;
			}
		}

		public bool IsLocalPlayerDriving
		{
			get
			{
				if (_simplePointGrabable == null || !_simplePointGrabable.Initialized)
				{
					return false;
				}
				NetworkObject networkObject = _simplePointGrabable.Object;
				if (networkObject == null || !networkObject.IsValid || !networkObject.HasStateAuthority)
				{
					return false;
				}
				NetworkRunner runner = _simplePointGrabable.Runner;
				if (runner == null)
				{
					return false;
				}
				int playerId = runner.LocalPlayer.PlayerId;
				return _simplePointGrabable.GrabbedByPlayers.Contains(playerId);
			}
		}

		public void BindTable(AerohockeyBeachInteractableBehaviour table, float moveSpeed)
		{
			_table = table;
			_moveSpeed = moveSpeed;
		}

		public void CaptureRestPose()
		{
			_restLocalPosition = _driveTransform.localPosition;
			_restLocalRotation = _driveTransform.localRotation;
			_restCaptured = true;
			ConfigureStaticGrabRigidbody();
		}

		public bool PublishSpawnNetworkPose()
		{
			if (_spawnNetworkPosePublished)
			{
				return true;
			}
			if (_physicsSynchronizer == null || _physicsSynchronizer.Object == null || !_physicsSynchronizer.Object.IsValid || !_physicsSynchronizer.Object.HasStateAuthority)
			{
				return false;
			}
			Vector3 position = _driveTransform.position;
			Quaternion rotation = _driveTransform.rotation;
			_rigidbody.position = position;
			_rigidbody.rotation = rotation;
			_physicsSynchronizer.Teleport(position, rotation);
			_spawnNetworkPosePublished = true;
			return true;
		}

		public void ApplyLocalXZ(float localX, float localZ, float velocityLocalX, float velocityLocalZ)
		{
			Vector3 position = (_restCaptured ? _restLocalPosition : _driveTransform.localPosition);
			position.x = localX;
			position.z = localZ;
			Transform playfieldTransform = _table.PlayfieldTransform;
			float y = position.y;
			Vector3 vector = playfieldTransform.TransformPoint(position);
			Vector3 velocitySamplePos = playfieldTransform.TransformPoint(new Vector3(velocityLocalX, y, velocityLocalZ));
			Quaternion rot = playfieldTransform.rotation * (_restCaptured ? _restLocalRotation : _driveTransform.localRotation);
			RecordWorldVelocity(vector, velocitySamplePos);
			_rigidbody.isKinematic = true;
			_rigidbody.MovePosition(vector);
			_rigidbody.MoveRotation(rot);
			_rigidbody.linearVelocity = _worldVelocity;
			_rigidbody.angularVelocity = Vector3.zero;
		}

		private void RecordWorldVelocity(Vector3 actualWorldPos, Vector3 velocitySamplePos)
		{
			if (!Time.inFixedTimeStep)
			{
				_lastWorldPosition = actualWorldPos;
				_hasLastWorldPosition = true;
				return;
			}
			float fixedDeltaTime = Time.fixedDeltaTime;
			if (_hasLastWorldPosition && fixedDeltaTime > 0.0001f)
			{
				_worldVelocity = (velocitySamplePos - _lastWorldPosition) / fixedDeltaTime;
			}
			else
			{
				_worldVelocity = Vector3.zero;
			}
			_lastWorldPosition = actualWorldPos;
			_hasLastWorldPosition = true;
		}

		public void InitializeGrabCallbacks()
		{
			_simplePointGrabable.OnGrabbedPlayersChanged -= OnGrabbedPlayersChanged;
			_simplePointGrabable.OnGrabbedPlayersChanged += OnGrabbedPlayersChanged;
			ConfigureStaticGrabRigidbody();
			EnsureGrabObjectAllowsPhysics();
		}

		public void DisposeGrabCallbacks()
		{
			_simplePointGrabable.OnGrabbedPlayersChanged -= OnGrabbedPlayersChanged;
		}

		private void ConfigureStaticGrabRigidbody()
		{
			_rigidbody.isKinematic = true;
			_rigidbody.useGravity = false;
			_rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
			_rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
		}

		private void EnsureGrabObjectAllowsPhysics()
		{
			if (!(_simplePointGrabable.GrabObject == null))
			{
				_simplePointGrabable.GrabObject.GrabbingPhysicsBlocked = false;
			}
		}

		private void OnGrabbedPlayersChanged()
		{
			if (_simplePointGrabable.Runner == null)
			{
				return;
			}
			int playerId = _simplePointGrabable.Runner.LocalPlayer.PlayerId;
			bool flag = _simplePointGrabable.GrabbedByPlayers.Contains(playerId);
			if (flag && !_wasLocalPlayerGrabbing)
			{
				if (!_table.TryClaimPaddle(_side, playerId))
				{
					ReleaseLocalGrab(playerId);
					_wasLocalPlayerGrabbing = false;
					return;
				}
			}
			else if (!flag && _wasLocalPlayerGrabbing)
			{
				_table.ReleasePaddleClaim(_side, playerId);
			}
			_wasLocalPlayerGrabbing = flag;
			EnsureGrabObjectAllowsPhysics();
			ConfigureStaticGrabRigidbody();
		}

		private void FixedUpdate()
		{
			EnsureGrabObjectAllowsPhysics();
			if (!_simplePointGrabable.Initialized)
			{
				return;
			}
			NetworkObject networkObject = _simplePointGrabable.Object;
			if (networkObject == null || !networkObject.IsValid || !networkObject.HasStateAuthority)
			{
				return;
			}
			NetworkRunner runner = _simplePointGrabable.Runner;
			if (runner == null)
			{
				return;
			}
			int playerId = runner.LocalPlayer.PlayerId;
			if (_simplePointGrabable.GrabbedByPlayers.Contains(playerId) && !(_simplePointGrabable.GrabObject == null) && _simplePointGrabable.GrabObject.Grabbers.Count != 0)
			{
				PhysGrabber physGrabber = _simplePointGrabable.GrabObject.Grabbers[0];
				if (!(physGrabber == null) && !(physGrabber.physGrabPointPullerPosition == null))
				{
					Transform playfieldTransform = _table.PlayfieldTransform;
					Vector3 vector = playfieldTransform.InverseTransformPoint(physGrabber.physGrabPointPullerPosition.position);
					_table.ClampPaddleXZ(_side, vector.x, vector.z, out var clampedX, out var clampedZ);
					Vector3 vector2 = playfieldTransform.InverseTransformPoint(_driveTransform.position);
					float maxDelta = _moveSpeed * Time.fixedDeltaTime;
					float localX = Mathf.MoveTowards(vector2.x, clampedX, maxDelta);
					float localZ = Mathf.MoveTowards(vector2.z, clampedZ, maxDelta);
					float velocityLocalX = localX;
					float velocityLocalZ = localZ;
					ResolveAgainstPuck(ref localX, ref localZ);
					ApplyLocalXZ(localX, localZ, velocityLocalX, velocityLocalZ);
				}
			}
		}

		private void ResolveAgainstPuck(ref float localX, ref float localZ)
		{
			AerohockeyPuck aerohockeyPuck = ((_table != null) ? _table.Puck : null);
			if (aerohockeyPuck == null || aerohockeyPuck.Collider == null || _hitCollider == null || !aerohockeyPuck.gameObject.activeInHierarchy)
			{
				return;
			}
			Transform playfieldTransform = _table.PlayfieldTransform;
			Vector3 obj = (_restCaptured ? _restLocalPosition : _driveTransform.localPosition);
			float y = obj.y;
			Quaternion rotationA = playfieldTransform.rotation * (_restCaptured ? _restLocalRotation : _driveTransform.localRotation);
			Rigidbody rigidbody = aerohockeyPuck.Rigidbody;
			Vector3 vector = ((rigidbody != null) ? rigidbody.position : aerohockeyPuck.Collider.transform.position);
			Quaternion rotationB = ((rigidbody != null) ? rigidbody.rotation : aerohockeyPuck.Collider.transform.rotation);
			Vector3 vector2 = playfieldTransform.InverseTransformPoint(_driveTransform.position);
			Vector3 vector3 = playfieldTransform.TransformPoint(new Vector3(vector2.x, y, vector2.z));
			Vector3 vector4 = playfieldTransform.TransformPoint(new Vector3(localX, y, localZ));
			Vector3 vector5 = vector4 - vector3;
			float magnitude = vector5.magnitude;
			if (magnitude > 0.0001f)
			{
				float radius = Mathf.Max(0.01f, _puckBlockSphereRadius);
				Vector3 vector6 = vector5 / magnitude;
				int num = Physics.SphereCastNonAlloc(vector3, radius, vector6, _puckCastHits, magnitude, -1, QueryTriggerInteraction.Ignore);
				float num2 = float.MaxValue;
				for (int i = 0; i < num; i++)
				{
					RaycastHit raycastHit = _puckCastHits[i];
					if (!(raycastHit.collider != aerohockeyPuck.Collider) && !(raycastHit.distance >= num2))
					{
						num2 = raycastHit.distance;
					}
				}
				if (num2 < float.MaxValue)
				{
					vector4 = vector3 + vector6 * Mathf.Max(0f, num2 - 0.012f);
				}
			}
			if (Physics.ComputePenetration(_hitCollider, vector4, rotationA, aerohockeyPuck.Collider, vector, rotationB, out var direction, out var distance))
			{
				Vector3 vector7 = Vector3.ProjectOnPlane(direction, playfieldTransform.up);
				if (vector7.sqrMagnitude < 0.0001f)
				{
					vector7 = Vector3.ProjectOnPlane(vector4 - vector, playfieldTransform.up);
				}
				if (vector7.sqrMagnitude > 0.0001f)
				{
					vector4 += vector7.normalized * (distance + 0.012f);
				}
			}
			Vector3 vector8 = playfieldTransform.InverseTransformPoint(vector4);
			localX = vector8.x;
			localZ = vector8.z;
			_table.ClampPaddleXZ(_side, localX, localZ, out localX, out localZ);
		}

		private void ReleaseLocalGrab(int playerId)
		{
			if (_simplePointGrabable.GrabbedByPlayers.Contains(playerId))
			{
				_simplePointGrabable.UnGrabbedByPlayer(playerId);
			}
		}
	}
}
