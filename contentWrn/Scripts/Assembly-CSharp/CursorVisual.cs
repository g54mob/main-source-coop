using UnityEngine;

public class CursorVisual : MonoBehaviour
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
		float enter = 0f;
		Ray ray = new Ray(position, forward);
		if (plane.Raycast(ray, out enter))
		{
			cursorPos = ray.GetPoint(enter);
			if (GlobalInputHandler.GetKeyDown(KeyCode.Mouse0))
			{
				Player.localPlayer.refs.interaction.PressE();
			}
			Player.localPlayer.data.cantUseItemFor = 0.3f;
		}
		Vector3 position2 = base.transform.InverseTransformPoint(cursorPos);
		position2.x = Mathf.Clamp(position2.x, 0f - clamp.x, clamp.x);
		position2.y = Mathf.Clamp(position2.y, 0f - clamp.y, clamp.y);
		cursorPos = base.transform.TransformPoint(position2);
		cursor.transform.position = cursorPos;
	}
}
