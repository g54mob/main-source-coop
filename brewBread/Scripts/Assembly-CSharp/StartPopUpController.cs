using Rewired;
using UnityEngine;

public class StartPopUpController : MonoBehaviour
{
	private Player _playerUI;

	[SerializeField]
	private GameObject _uiCamera;

	private void Start()
	{
		if (StaticInstance<DataBetweenScenes>.Instance.loadGame)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		StaticInstance<Bread>.Instance.DisableInput();
		StaticInstance<Fred>.Instance.DisableInput();
		_playerUI = ReInput.players.GetPlayer(3);
		_playerUI.AddInputEventDelegate(ClosePopUp, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, 18);
	}

	private void OnDestroy()
	{
		if (_playerUI != null)
		{
			_playerUI.RemoveInputEventDelegate(ClosePopUp);
		}
	}

	private void ClosePopUp(InputActionEventData obj)
	{
		if ((bool)_uiCamera)
		{
			_uiCamera.SetActive(value: false);
		}
		StaticInstance<Bread>.Instance.EnableInput();
		StaticInstance<Fred>.Instance.EnableInput();
		Object.Destroy(base.gameObject);
	}
}
