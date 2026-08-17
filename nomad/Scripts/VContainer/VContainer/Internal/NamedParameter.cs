using System;

namespace VContainer.Internal
{
	internal sealed class NamedParameter : IInjectParameter
	{
		public readonly string Name;

		public readonly object Value;

		public NamedParameter(string name, object value)
		{
			Name = name;
			Value = value;
		}

		public bool Match(Type _, string parameterName)
		{
			return parameterName == Name;
		}

		public object GetValue(IObjectResolver _)
		{
			return Value;
		}
	}
}
