using System;

namespace Zorro.Core.CLI
{
	[AttributeUsage(AttributeTargets.Class)]
	public class ConsoleClassCustomizerAttribute : Attribute
	{
		public string NewDomainName;

		public ConsoleClassCustomizerAttribute(string newDomainName)
		{
			NewDomainName = newDomainName;
		}
	}
}
