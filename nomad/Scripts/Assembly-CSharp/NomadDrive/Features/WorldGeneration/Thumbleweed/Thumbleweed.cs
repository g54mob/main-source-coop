using UnityEngine;

namespace NomadDrive.Features.WorldGeneration.Thumbleweed
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(SphereCollider))]
	public class Thumbleweed : MonoBehaviour
	{
		private Rigidbody _rigidbody;

		private SphereCollider _sphereCollider;

		private Vector3 _spawnPosition;

		private Vector3 _windForce;

		private float _spawnTime;

		private Vector2Int _chunkCoord;

		private bool _isActiveInWorld;

		public Vector2Int ChunkCoord => _chunkCoord;

		public Vector3 SpawnPosition => _spawnPosition;

		public float ElapsedSeconds => Time.time - _spawnTime;

		public float TraveledDistance => Vector3.Distance(base.transform.position, _spawnPosition);

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody>();
			_sphereCollider = GetComponent<SphereCollider>();
		}

		public void Configure(float mass, float linearDrag, float angularDrag, float colliderRadius, PhysicsMaterial physicsMaterial)
		{
			if (_rigidbody == null)
			{
				_rigidbody = GetComponent<Rigidbody>();
			}
			if (_sphereCollider == null)
			{
				_sphereCollider = GetComponent<SphereCollider>();
			}
			_rigidbody.mass = mass;
			_rigidbody.linearDamping = linearDrag;
			_rigidbody.angularDamping = angularDrag;
			_rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
			_rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
			_rigidbody.useGravity = true;
			_rigidbody.constraints = RigidbodyConstraints.None;
			_sphereCollider.radius = colliderRadius;
			if (physicsMaterial != null)
			{
				_sphereCollider.material = physicsMaterial;
			}
		}

		public void Enable(Vector3 worldPosition, Vector2Int chunkCoord, Vector3 windForce, Vector3 initialSpinTorque)
		{
			_chunkCoord = chunkCoord;
			_spawnPosition = worldPosition;
			_spawnTime = Time.time;
			_windForce = windForce;
			base.transform.position = worldPosition;
			base.transform.rotation = Quaternion.identity;
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(value: true);
			}
			_rigidbody.linearVelocity = Vector3.zero;
			_rigidbody.angularVelocity = Vector3.zero;
			_rigidbody.AddTorque(initialSpinTorque, ForceMode.VelocityChange);
			_isActiveInWorld = true;
		}

		public void SetSimulationActive(bool active)
		{
			if (_isActiveInWorld != active)
			{
				_isActiveInWorld = active;
				if (!active)
				{
					_rigidbody.linearVelocity = Vector3.zero;
					_rigidbody.angularVelocity = Vector3.zero;
				}
				base.gameObject.SetActive(active);
			}
		}

		public void OnOriginShift(Vector3 delta)
		{
			_spawnPosition += delta;
			base.transform.position += delta;
			if (_rigidbody != null)
			{
				_rigidbody.position += delta;
			}
		}

		private void FixedUpdate()
		{
			if (_isActiveInWorld)
			{
				_rigidbody.AddForce(_windForce, ForceMode.Force);
			}
		}

		private void OnDisable()
		{
			if (!(_rigidbody == null))
			{
				_rigidbody.linearVelocity = Vector3.zero;
				_rigidbody.angularVelocity = Vector3.zero;
			}
		}
	}
}
