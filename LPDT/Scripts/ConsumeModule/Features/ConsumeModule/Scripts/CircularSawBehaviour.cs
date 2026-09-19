using Fusion;
using UnityEngine;

namespace Features.ConsumeModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class CircularSawBehaviour : NetworkBehaviour
	{
		public enum MovementType
		{
			Linear = 0,
			Smooth = 1
		}

		[Header("Rotation Settings")]
		[Tooltip("Швидкість обертання пили (градуси за секунду)")]
		public float rotationSpeed = 360f;

		[Tooltip("Вісь обертання (зазвичай Z для 2D або Y для 3D)")]
		public Vector3 rotationAxis = Vector3.forward;

		[Header("Movement Settings")]
		[Tooltip("Точки, між якими рухається пила")]
		public Transform[] waypoints;

		[Tooltip("Швидкість руху пили")]
		public float moveSpeed = 2f;

		[Tooltip("Час паузи на кожній точці (секунди)")]
		public float waitTime;

		[Tooltip("Тип руху")]
		public MovementType movementType;

		[Header("Path Settings")]
		[Tooltip("Чи повертатися назад чи рухатися по колу")]
		public bool loopPath = true;

		[Tooltip("Якщо true, пила рухається туди-сюди. Якщо false - по колу")]
		public bool pingPong;

		private int currentWaypointIndex;

		private bool movingForward = true;

		private float waitTimer;

		private bool isWaiting;

		private void Start()
		{
			if (waypoints == null || waypoints.Length == 0)
			{
				Debug.LogWarning("CircularSaw: Немає точок маршруту! Додайте waypoints у Inspector.");
				base.enabled = false;
			}
			else if (waypoints.Length != 0 && waypoints[0] != null)
			{
				base.transform.position = waypoints[0].position;
			}
		}

		public override void FixedUpdateNetwork()
		{
			base.FixedUpdateNetwork();
			RotateSaw();
			if (waypoints != null && waypoints.Length != 0)
			{
				MoveBetweenWaypoints();
			}
		}

		public void Pause()
		{
			base.enabled = false;
		}

		public void Resume()
		{
			base.enabled = true;
		}

		public void SetRotationSpeed(float speed)
		{
			rotationSpeed = speed;
		}

		public void SetMoveSpeed(float speed)
		{
			moveSpeed = speed;
		}

		private void RotateSaw()
		{
			base.transform.Rotate(rotationAxis, rotationSpeed * base.Object.Runner.DeltaTime);
		}

		private void MoveBetweenWaypoints()
		{
			if (waypoints.Length < 2)
			{
				return;
			}
			if (isWaiting)
			{
				waitTimer -= base.Object.Runner.DeltaTime;
				if (waitTimer <= 0f)
				{
					isWaiting = false;
					MoveToNextWaypoint();
				}
				return;
			}
			Transform transform = waypoints[currentWaypointIndex];
			if (transform == null)
			{
				MoveToNextWaypoint();
				return;
			}
			if (movementType == MovementType.Linear)
			{
				base.transform.position = Vector3.MoveTowards(base.transform.position, transform.position, moveSpeed * base.Object.Runner.DeltaTime);
			}
			else
			{
				float num = Vector3.Distance(base.transform.position, transform.position);
				float t = moveSpeed * base.Object.Runner.DeltaTime / num;
				base.transform.position = Vector3.Lerp(base.transform.position, transform.position, t);
			}
			if (Vector3.Distance(base.transform.position, transform.position) < 0.01f)
			{
				base.transform.position = transform.position;
				if (waitTime > 0f)
				{
					isWaiting = true;
					waitTimer = waitTime;
				}
				else
				{
					MoveToNextWaypoint();
				}
			}
		}

		private void MoveToNextWaypoint()
		{
			if (pingPong)
			{
				if (movingForward)
				{
					currentWaypointIndex++;
					if (currentWaypointIndex >= waypoints.Length)
					{
						currentWaypointIndex = waypoints.Length - 2;
						movingForward = false;
					}
				}
				else
				{
					currentWaypointIndex--;
					if (currentWaypointIndex < 0)
					{
						currentWaypointIndex = 1;
						movingForward = true;
					}
				}
				return;
			}
			currentWaypointIndex++;
			if (currentWaypointIndex >= waypoints.Length)
			{
				if (loopPath)
				{
					currentWaypointIndex = 0;
					return;
				}
				currentWaypointIndex = waypoints.Length - 1;
				base.enabled = false;
			}
		}

		private void OnDrawGizmos()
		{
			if (waypoints == null || waypoints.Length < 2)
			{
				return;
			}
			Gizmos.color = Color.yellow;
			for (int i = 0; i < waypoints.Length; i++)
			{
				if (!(waypoints[i] == null))
				{
					Gizmos.DrawWireSphere(waypoints[i].position, 0.3f);
					int num = (i + 1) % waypoints.Length;
					if ((!pingPong || i != waypoints.Length - 1) && (loopPath || num != 0) && waypoints[num] != null)
					{
						Gizmos.DrawLine(waypoints[i].position, waypoints[num].position);
					}
				}
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
