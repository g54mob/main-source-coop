using System;

namespace VContainer.Internal
{
	internal sealed class TypedParameter : IInjectParameter
	{
		public readonly Type Type;

		public readonly object Value;

		public TypedParameter(Type type, object value)
		{
			Type = type;
			Value = value;
		}

		public bool Match(Type parameterType, string _)
		{
			return parameterType == Type;
		}

		public object GetValue(IObjectResolver _)
		{
			return Value;
		}
	}
}
