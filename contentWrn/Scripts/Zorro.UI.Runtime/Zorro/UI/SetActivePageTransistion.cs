namespace Zorro.UI
{
	public class SetActivePageTransistion : PageTransistion
	{
		public override void Transistion(PageBase oldSubPage, PageBase newSubPage)
		{
			oldSubPage.OnPageExit();
			oldSubPage.gameObject.SetActive(value: false);
			newSubPage.gameObject.SetActive(value: true);
			newSubPage.OnPageEnter();
			newSubPage.OnPageEntered();
		}
	}
}
