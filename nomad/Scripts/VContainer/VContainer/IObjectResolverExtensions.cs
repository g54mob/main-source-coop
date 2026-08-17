using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VContainer
{
	public static class IObjectResolverExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Resolve<T>(this IObjectResolver resolver)
		{
			return (T)resolver.Resolve(typeof(T));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryResolve<T>(this IObjectResolver resolver, out T resolved)
		{
			if (resolver.TryResolve(typeof(T), out var resolved2))
			{
				resolved = (T)resolved2;
				return true;
			}
			resolved = default(T);
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T ResolveOrDefault<T>(this IObjectResolver resolver, T defaultValue = default(T))
		{
			if (resolver.TryResolve(typeof(T), out var resolved))
			{
				return (T)resolved;
			}
			return defaultValue;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Preserve]
		public static object ResolveNonGeneric(this IObjectResolver resolve, Type type)
		{
			return resolve.Resolve(type);
		}

		public static object ResolveOrParameter(this IObjectResolver resolver, Type parameterType, string parameterName, IReadOnlyList<IInjectParameter> parameters)
		{
			if (parameters != null)
			{
				for (int i = 0; i < parameters.Count; i++)
				{
					IInjectParameter injectParameter = parameters[i];
					if (injectParameter.Match(parameterType, parameterName))
					{
						return injectParameter.GetValue(resolver);
					}
				}
			}
			return resolver.Resolve(parameterType);
		}
	}
}
