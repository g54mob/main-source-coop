using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class MenuButtonsArrangement : MonoBehaviour
{
	public Transform[] buttons;

	public EventSystem eventSystem;

	[SerializeField]
	private Camera _pauseCamera;

	private UniversalAdditionalCameraData _data;

	private void Start()
	{
		if ((bool)_pauseCamera)
		{
			_data = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().GetUniversalAdditionalCameraData();
			if (_data != null)
			{
				_data.cameraStack.Add(_pauseCamera);
			}
		}
	}

	public void ButtonSelected()
	{
		int num = buttons.Length;
		for (int i = 0; i < buttons.Length; i++)
		{
			if (buttons[i].GetChild(0).gameObject.activeInHierarchy)
			{
				Vector3 localPosition = buttons[i].localPosition;
				if (buttons[i].GetComponentInChildren<Button>().gameObject == eventSystem.currentSelectedGameObject)
				{
					localPosition.y = 2.3f - 2.3f * (float)i;
					buttons[i].localPosition = localPosition;
					num = i;
				}
				else if (num > i)
				{
					localPosition.y = 2.3f - 2.3f * (float)i + 1f;
					buttons[i].localPosition = localPosition;
				}
				else
				{
					localPosition.y = 2.3f - 2.3f * (float)i - 1f;
					buttons[i].localPosition = localPosition;
				}
			}
		}
	}
}
