namespace Fusion
{
	public class TickRateDivisorAttribute : PropertyAttribute
	{
		public string PropertyName { get; }

		public string ObsoleteIndexName { get; }

		public bool ForceOne { get; }

		public TickRateDivisorAttribute(string propertyName, string obsoleteIndexName, bool forceOne = false)
		{
			PropertyName = propertyName;
			ObsoleteIndexName = obsoleteIndexName;
			ForceOne = forceOne;
			base._002Ector();
		}
	}
}
