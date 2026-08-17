using UnityEngine;

namespace Mirror.Examples.BilliardsPredicted
{
	public class WhiteBallPredicted : NetworkBehaviour
	{
		public LineRenderer dragIndicator;

		public float dragTolerance = 1f;

		public Rigidbody rigidBody;

		public float forceMultiplier = 2f;

		public float maxForce = 40f;

		internal Vector3 startPosition;

		private bool draggingStartedOverObject;

		private bool MouseToWorld(out Vector3 position)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (new Plane(Vector3.up, base.transform.position).Raycast(ray, out var enter))
			{
				position = ray.GetPoint(enter);
				return true;
			}
			position = default(Vector3);
			return false;
		}

		private void Awake()
		{
			startPosition = base.transform.position;
		}

		[ClientCallback]
		private void Update()
		{
			if (!NetworkClient.active)
			{
				return;
			}
			if (Input.GetMouseButtonDown(0))
			{
				if (MouseToWorld(out var position) && Vector3.Distance(position, base.transform.position) <= dragTolerance)
				{
					dragIndicator.SetPosition(0, base.transform.position);
					dragIndicator.SetPosition(1, base.transform.position);
					dragIndicator.gameObject.SetActive(value: true);
					draggingStartedOverObject = true;
				}
			}
			else if (Input.GetMouseButton(0))
			{
				if (draggingStartedOverObject && MouseToWorld(out var position2))
				{
					dragIndicator.SetPosition(0, base.transform.position);
					dragIndicator.SetPosition(1, position2);
				}
			}
			else if (Input.GetMouseButtonUp(0))
			{
				if (draggingStartedOverObject && MouseToWorld(out var position3))
				{
					Vector3 position4 = base.transform.position;
					Debug.DrawLine(position4, position3, Color.white, 2f);
					Vector3 vector = (position4 - position3) * forceMultiplier;
					vector = Vector3.ClampMagnitude(vector, maxForce);
					NetworkClient.localPlayer.GetComponent<PlayerPredicted>().OnDraggedBall(vector);
					dragIndicator.gameObject.SetActive(value: false);
				}
				draggingStartedOverObject = false;
			}
		}

		[ClientCallback]
		private void OnGUI()
		{
			if (NetworkClient.active && GUI.Button(new Rect(10f, 150f, 200f, 20f), "Hit!"))
			{
				Vector3 force = Vector3.ClampMagnitude(new Vector3(10f, 0f, 600f), maxForce);
				NetworkClient.localPlayer.GetComponent<PlayerPredicted>().OnDraggedBall(force);
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
