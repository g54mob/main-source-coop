using System;

namespace Fusion.Analyzer
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
	public class GenerateIntrusiveLinkedListAttribute : Attribute
	{
		public string GlobalAlias;

		public bool GenerateAsClass;

		public string FreeMethod;

		public string NodeField { get; }

		public GenerateIntrusiveLinkedListAttribute(string nodeField = "_node")
		{
			NodeField = nodeField;
			base._002Ector();
		}
	}
}
