namespace Fusion
{
	public abstract class DecoratingPropertyAttribute : PropertyAttribute
	{
		public const int DefaultOrder = -10000;

		protected DecoratingPropertyAttribute(bool applyToCollection = false)
			: base(applyToCollection)
		{
			base.order = -10000;
		}

		protected DecoratingPropertyAttribute(int order, bool applyToCollection = false)
			: base(applyToCollection)
		{
			base.order = order;
		}
	}
}
