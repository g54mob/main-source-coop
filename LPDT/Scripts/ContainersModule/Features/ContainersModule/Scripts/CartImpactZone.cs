using UnityEngine;

namespace Features.ContainersModule.Scripts
{
	public class CartImpactZone : MonoBehaviour
	{
		[SerializeField]
		private Vector3 _size = new Vector3(1.4f, 1.4f, 0.9f);

		[SerializeField]
		private float _minSpeed = 1.5f;

		[SerializeField]
		private float _maxAngleDegrees = 60f;

		[SerializeField]
		private float _sustainSeconds = 1f;

		[SerializeField]
		private float _maxSpeed = 20f;

		private Vector3 _previousCenter;

		private bool _hasPreviousCenter;

		private float _sustainedSeconds;

		public Vector3 Velocity { get; private set; }

		public bool IsArmed => _sustainedSeconds >= _sustainSeconds;

		public float MinSpeed => _minSpeed;

		public Vector3 WorldCenter => base.transform.position;

		public Vector3 HalfExtents => Vector3.Scale(_size, base.transform.lossyScale) * 0.5f;

		public Vector3 StrikeDirection
		{
			get
			{
				Vector3 forward = base.transform.forward;
				forward.y = 0f;
				if (!(forward.sqrMagnitude < 0.0001f))
				{
					return forward.normalized;
				}
				return Vector3.zero;
			}
		}

		public void Evaluate(float deltaTime, bool isDriven)
		{
			Vector3 position = base.transform.position;
			if (!_hasPreviousCenter)
			{
				_previousCenter = position;
				_hasPreviousCenter = true;
				Velocity = Vector3.zero;
				_sustainedSeconds = 0f;
				return;
			}
			Vector3 vector = position - _previousCenter;
			_previousCenter = position;
			vector.y = 0f;
			Velocity = ((deltaTime > 0f) ? (vector / deltaTime) : Vector3.zero);
			if (Velocity.magnitude > _maxSpeed)
			{
				Velocity = Vector3.zero;
				_sustainedSeconds = 0f;
				return;
			}
			Vector3 strikeDirection = StrikeDirection;
			if (!isDriven || strikeDirection == Vector3.zero || Velocity.magnitude < _minSpeed || Vector3.Angle(Velocity, strikeDirection) > _maxAngleDegrees)
			{
				_sustainedSeconds = 0f;
			}
			else
			{
				_sustainedSeconds += deltaTime;
			}
		}

		public void ResetTracking()
		{
			_hasPreviousCenter = false;
			_sustainedSeconds = 0f;
			Velocity = Vector3.zero;
		}

		private void OnDrawGizmosSelected()
		{
			Matrix4x4 matrix = Gizmos.matrix;
			Gizmos.matrix = Matrix4x4.TRS(base.transform.position, base.transform.rotation, HalfExtents * 2f);
			Gizmos.color = new Color(1f, 0.4f, 0f, 0.9f);
			Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
			Gizmos.color = new Color(1f, 0.4f, 0f, 0.15f);
			Gizmos.DrawCube(Vector3.zero, Vector3.one);
			Gizmos.matrix = matrix;
			Gizmos.color = Color.yellow;
			Vector3 vector = base.transform.position + base.transform.forward * (HalfExtents.z + 0.5f);
			Gizmos.DrawLine(base.transform.position, vector);
			Gizmos.DrawWireSphere(vector, 0.08f);
		}
	}
}
