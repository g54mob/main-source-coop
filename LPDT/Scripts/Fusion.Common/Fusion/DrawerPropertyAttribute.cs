namespace Fusion
{
	public abstract class DrawerPropertyAttribute : PropertyAttribute
	{
		protected DrawerPropertyAttribute(bool applyToCollection = false)
			: base(applyToCollection)
		{
		}
	}
}
