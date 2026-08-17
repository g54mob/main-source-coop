using System;

namespace VContainer.Internal
{
	internal sealed class FuncNamedParameter : IInjectParameter
	{
		public readonly string Name;

		public readonly Func<IObjectResolver, object> Func;

		public FuncNamedParameter(string name, Func<IObjectResolver, object> func)
		{
			Name = name;
			Func = func;
		}

		public bool Match(Type _, string parameterName)
		{
			return parameterName == Name;
		}

		public object GetValue(IObjectResolver resolver)
		{
			return Func(resolver);
		}
	}
}
