using UnityEngine;

public class SFXCanvasFadeSound : MonoBehaviour
{
	public CanvasGroup cGroup;

	public SFX_Instance on;

	public SFX_Instance off;

	private bool t;

	private void Update()
	{
		if (cGroup.interactable && !t)
		{
			t = true;
			on.Play();
		}
		if (!cGroup.interactable && t)
		{
			t = false;
			off.Play();
		}
	}
}
