namespace Zorro.Core.CLI
{
	public abstract class Suggestion
	{
		public abstract string GetInputValue();

		public abstract bool CanBeSelected();
	}
}
