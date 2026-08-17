using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TimerDemo : MonoBehaviour
{
	public float timer;

	public bool timerEnabled = true;

	public Image image;

	private void Update()
	{
		if (timerEnabled)
		{
			timer -= Time.deltaTime;
		}
		if (Keyboard.current.leftCtrlKey.isPressed && Keyboard.current.f2Key.wasPressedThisFrame)
		{
			timerEnabled = !timerEnabled;
		}
		image.fillAmount = timer / 720f;
		if (timer <= 0f)
		{
			Object.Destroy(this);
		}
	}
}
