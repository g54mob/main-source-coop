using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace.Petter.TitleCard
{
	public class MouseBrush : MonoBehaviour
	{
		public GameObject visual;

		private TitleCardItemInstance titleCardInstace;

		private TitleCardCanvas titleCardCanvas;

		public BoxCollider boxCollider;

		private bool insideCanvas;

		private bool show;

		public bool Show
		{
			get
			{
				return show = true;
			}
			set
			{
				show = value;
				if (!show)
				{
					titleCardCanvas.FinishDrawing();
				}
			}
		}

		private void Awake()
		{
			titleCardInstace = GetComponentInParent<TitleCardItemInstance>();
			titleCardCanvas = GetComponent<TitleCardCanvas>();
			Show = true;
		}

		public Vector2 PointToUv(Vector3 point)
		{
			Vector3 vector = base.transform.InverseTransformPoint(point);
			Vector3 size = boxCollider.size;
			Vector2 vector2 = new Vector2(vector.x / size.x, vector.y / size.y);
			return new Vector2(1f - (vector2.x + 0.5f), 1f - (vector2.y + 0.5f));
		}

		public void Update()
		{
			if (!show || !titleCardInstace.isHeldByMe)
			{
				return;
			}
			if (Mouse.current.delta.value != Vector2.zero)
			{
				Camera main = Camera.main;
				Ray ray = main.ScreenPointToRay(Mouse.current.position.value);
				if (Vector3.Angle(main.transform.forward, boxCollider.transform.forward) > 90f)
				{
					return;
				}
				insideCanvas = boxCollider.Raycast(ray, out var hitInfo, 50f);
				if (insideCanvas)
				{
					visual.transform.position = hitInfo.point;
				}
			}
			if (Gamepad.current != null)
			{
				Vector2 value = Gamepad.current.leftStick.value;
				if (value != Vector2.zero)
				{
					float deltaTime = Time.deltaTime;
					Vector3 localPosition = visual.transform.localPosition;
					localPosition.x = Mathf.Clamp(localPosition.x + value.x * deltaTime, -0.5f, 0.5f);
					localPosition.y = Mathf.Clamp(localPosition.y + value.y * deltaTime, -0.5f, 0.5f);
					visual.transform.localPosition = localPosition;
					insideCanvas = true;
				}
			}
			if ((Mouse.current.leftButton.isPressed || (Gamepad.current != null && Gamepad.current.rightTrigger.isPressed)) && insideCanvas)
			{
				Vector3 position = visual.transform.position;
				Vector2 uvCoord = PointToUv(position);
				titleCardCanvas.Draw(uvCoord, position);
			}
			if (Mouse.current.leftButton.wasReleasedThisFrame || (Gamepad.current != null && Gamepad.current.rightTrigger.wasReleasedThisFrame))
			{
				titleCardCanvas.FinishDrawing();
			}
		}
	}
}
