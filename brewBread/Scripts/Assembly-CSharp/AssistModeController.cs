using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AssistModeController : MonoBehaviour
{
	[SerializeField]
	private InputActionReference exit;

	[SerializeField]
	private Toggle _checkPointToggle;

	[SerializeField]
	private Toggle _pullRope;

	private void Awake()
	{
		exit.action.started += Exit;
	}

	private void OnDestroy()
	{
		exit.action.started -= Exit;
	}

	private void Start()
	{
		_checkPointToggle.SetIsOnWithoutNotify(StaticInstance<GameManager>.Instance.GetComponent<CheckpointController>().enabled);
	}

	public void ToggleCheckpoints()
	{
		StaticInstance<AssistModeManager>.Instance.CheckPoints = !StaticInstance<AssistModeManager>.Instance.CheckPoints;
	}

	public void TogglePullRope()
	{
		StaticInstance<AssistModeManager>.Instance.PullingRope = !StaticInstance<AssistModeManager>.Instance.PullingRope;
	}

	public void Exit(InputAction.CallbackContext context)
	{
		EventSystem[] array = Object.FindObjectsOfType<EventSystem>();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].gameObject.scene != base.gameObject.scene)
			{
				array[i].enabled = true;
			}
			else
			{
				array[i].enabled = false;
			}
		}
		ScenesLoadManager.instance.UnloadScene("AssistMode");
	}
}
