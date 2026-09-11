using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Zorro.ControllerSupport;
using Zorro.Core;
using Zorro.UI;

public class EscapeMenu : Singleton<EscapeMenu>
{
	public InputActionReference OpenMenuAction;

	public InputActionReference MenuBackAction;

	[SerializeField]
	private EscapeMenuUIHandler m_uiHandler;

	[SerializeField]
	private GameObject m_menu;

	public bool Open { get; private set; }

	private void OnEnable()
	{
		OpenMenuAction.action.performed += Toggle;
	}

	private void OnDisable()
	{
		OpenMenuAction.action.performed -= Toggle;
	}

	private void Toggle(InputAction.CallbackContext _)
	{
		Toggle();
	}

	private void Close(InputAction.CallbackContext _)
	{
		if (Open)
		{
			StartCoroutine(CloseDelayed());
		}
	}

	private IEnumerator CloseDelayed()
	{
		yield return null;
		Toggle();
	}

	public void Toggle()
	{
		if (!(RetrievableResourceSingleton<Modal>.Instance != null) || !RetrievableResourceSingleton<Modal>.Instance.Open)
		{
			if (m_uiHandler.currentPage is IHaveParentPage haveParentPage)
			{
				var (page, pageTransistion) = haveParentPage.GetParentPage();
				m_uiHandler.TransistionToPage(page, pageTransistion);
			}
			else
			{
				Open = !Open;
				m_menu.SetActive(Open);
			}
			if (Open)
			{
				m_uiHandler.TransistionToPage<EscapeMenuMainPage>();
			}
			else
			{
				InputHandler.AddInputBlock();
			}
		}
	}
}
