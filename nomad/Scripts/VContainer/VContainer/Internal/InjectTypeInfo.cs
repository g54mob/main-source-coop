using System;
using System.Collections.Generic;
using System.Reflection;

namespace VContainer.Internal
{
	internal sealed class InjectTypeInfo
	{
		public readonly Type Type;

		public readonly InjectConstructorInfo InjectConstructor;

		public readonly IReadOnlyList<InjectMethodInfo> InjectMethods;

		public readonly IReadOnlyList<FieldInfo> InjectFields;

		public readonly IReadOnlyList<PropertyInfo> InjectProperties;

		public InjectTypeInfo(Type type, InjectConstructorInfo injectConstructor, IReadOnlyList<InjectMethodInfo> injectMethods, IReadOnlyList<FieldInfo> injectFields, IReadOnlyList<PropertyInfo> injectProperties)
		{
			Type = type;
			InjectConstructor = injectConstructor;
			InjectFields = injectFields;
			InjectProperties = injectProperties;
			InjectMethods = injectMethods;
		}
	}
}
