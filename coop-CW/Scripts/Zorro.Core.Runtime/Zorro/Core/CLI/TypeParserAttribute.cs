using System;

namespace Zorro.Core.CLI
{
	[AttributeUsage(AttributeTargets.Class)]
	public class TypeParserAttribute : Attribute
	{
		public Type ParseType;

		public TypeParserAttribute(Type parseType)
		{
			ParseType = parseType;
		}
	}
}
