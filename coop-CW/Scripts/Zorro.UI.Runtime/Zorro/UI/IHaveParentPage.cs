namespace Zorro.UI
{
	public interface IHaveParentPage
	{
		(UIPage, PageTransistion) GetParentPage();

		bool OnAttemptGoToParent()
		{
			return true;
		}
	}
}
