using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zorro.ControllerSupport;
using pworld.Scripts.Extensions;

public class CursorClicker : MonoBehaviour
{
	public Transform cursor;

	public Vector3 cursorPos;

	public Vector2 clamp;

	private Plane plane;

	private void Start()
	{
		cursorPos = base.transform.position;
	}

	private void LateUpdate()
	{
		Vector3 position = MainCamera.instance.transform.position;
		Vector3 forward = MainCamera.instance.transform.forward;
		if (Vector3.Distance(position, base.transform.position) > 3f)
		{
			return;
		}
		plane = new Plane(base.transform.forward, base.transform.position);
		Ray ray = new Ray(position, forward);
		if (!plane.Raycast(ray, out var enter))
		{
			return;
		}
		cursor.gameObject.SetActive(value: true);
		cursorPos = ray.GetPoint(enter);
		Vector3 position2 = base.transform.InverseTransformPoint(cursorPos);
		if (!(position2.x > 0f - clamp.x) || !(position2.x < clamp.x) || !(position2.y > 0f - clamp.y) || !(position2.y < clamp.y))
		{
			return;
		}
		cursor.gameObject.SetActive(value: true);
		if ((GlobalInputHandler.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.E) || (Gamepad.current != null && Gamepad.current.buttonWest.wasPressedThisFrame)) && PExt.GetUiUnderPos(screenPos: (InputHandler.GetCurrentUsedInputScheme() != InputScheme.Gamepad) ? Input.mousePosition : MainCamera.instance.Cam.WorldToScreenPoint(cursorPos), me: EventSystem.current, hits: out var hits))
		{
			foreach (RaycastResult item in hits)
			{
				Button component = item.gameObject.GetComponent<Button>();
				if (component != null)
				{
					component.onClick?.Invoke();
					Debug.Log("Clicked Button: " + component.gameObject.name + " ", component.gameObject);
				}
			}
		}
		cursorPos = base.transform.TransformPoint(position2);
		Player.localPlayer.data.cantUseItemFor = 0.3f;
		cursor.transform.position = cursorPos;
	}
}
