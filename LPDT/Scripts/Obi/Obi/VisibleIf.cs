using System;
using System.Reflection;

namespace Obi
{
	[AttributeUsage(AttributeTargets.Field)]
	public class VisibleIf : MultiPropertyAttribute
	{
		private MethodInfo eventMethodInfo;

		private FieldInfo fieldInfo;

		private PropertyInfo propertyInfo;

		public string MethodName { get; private set; }

		public bool Negate { get; private set; }

		public VisibleIf(string methodName, bool negate = false)
		{
			MethodName = methodName;
			Negate = negate;
		}
	}
}
