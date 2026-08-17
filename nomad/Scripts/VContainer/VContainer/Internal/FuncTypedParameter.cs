using System;

namespace VContainer.Internal
{
	internal sealed class FuncTypedParameter : IInjectParameter
	{
		public readonly Type Type;

		public readonly Func<IObjectResolver, object> Func;

		public FuncTypedParameter(Type type, Func<IObjectResolver, object> func)
		{
			Type = type;
			Func = func;
		}

		public bool Match(Type parameterType, string _)
		{
			return parameterType == Type;
		}

		public object GetValue(IObjectResolver resolver)
		{
			return Func(resolver);
		}
	}
}
