namespace Zorro.Core.CLI
{
	public class DomainSuggestion : Suggestion
	{
		public string Domain;

		public DomainSuggestion(string domain)
		{
			Domain = domain;
		}

		public override string GetInputValue()
		{
			return Domain;
		}

		public override bool CanBeSelected()
		{
			return true;
		}

		public override string ToString()
		{
			return "<color=#cccaca>" + Domain;
		}
	}
}
