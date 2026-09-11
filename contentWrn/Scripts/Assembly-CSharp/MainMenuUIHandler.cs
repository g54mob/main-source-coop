using Portningsbolaget;
using UnityEngine.InputSystem;
using Zorro.Core;
using Zorro.UI;

public class MainMenuUIHandler : UIPageHandler
{
	public InputActionReference MenuBackAction;

	protected override void OnTransistionedToPage(UIPage newPage)
	{
		base.OnTransistionedToPage(newPage);
		_ = newPage is MainMenuPage;
	}

	private void Update()
	{
		if (MenuBackAction.action.WasPressedThisFrame() && !RetrievableResourceSingleton<Modal>.Instance.Open && (!(currentPage is MainMenuSettingsPage) || !(BlocklistPopup.Instance != null) || !BlocklistPopup.Instance.IsVisible) && currentPage is IHaveParentPage haveParentPage)
		{
			var (page, pageTransistion) = haveParentPage.GetParentPage();
			TransistionToPage(page, pageTransistion);
		}
	}
}
