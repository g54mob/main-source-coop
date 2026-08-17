using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace VContainer.Internal
{
	internal sealed class ReflectionInjector : IInjector
	{
		private readonly InjectTypeInfo injectTypeInfo;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ReflectionInjector Build(Type type)
		{
			return new ReflectionInjector(TypeAnalyzer.AnalyzeWithCache(type));
		}

		private ReflectionInjector(InjectTypeInfo injectTypeInfo)
		{
			this.injectTypeInfo = injectTypeInfo;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Inject(object instance, IObjectResolver resolver, IReadOnlyList<IInjectParameter> parameters)
		{
			InjectFields(instance, resolver, parameters);
			InjectProperties(instance, resolver, parameters);
			InjectMethods(instance, resolver, parameters);
		}

		public object CreateInstance(IObjectResolver resolver, IReadOnlyList<IInjectParameter> parameters)
		{
			ParameterInfo[] parameterInfos = injectTypeInfo.InjectConstructor.ParameterInfos;
			object[] array = CappedArrayPool<object>.Shared8Limit.Rent(parameterInfos.Length);
			try
			{
				for (int i = 0; i < parameterInfos.Length; i++)
				{
					ParameterInfo parameterInfo = parameterInfos[i];
					array[i] = resolver.ResolveOrParameter(parameterInfo.ParameterType, parameterInfo.Name, parameters);
				}
				object obj = injectTypeInfo.InjectConstructor.ConstructorInfo.Invoke(array);
				Inject(obj, resolver, parameters);
				return obj;
			}
			catch (VContainerException ex)
			{
				throw new VContainerException(ex.InvalidType, $"Failed to resolve {injectTypeInfo.Type} : {ex.Message}");
			}
			finally
			{
				CappedArrayPool<object>.Shared8Limit.Return(array);
			}
		}

		private void InjectFields(object obj, IObjectResolver resolver, IReadOnlyList<IInjectParameter> parameters)
		{
			if (injectTypeInfo.InjectFields == null)
			{
				return;
			}
			foreach (FieldInfo injectField in injectTypeInfo.InjectFields)
			{
				object value = resolver.ResolveOrParameter(injectField.FieldType, injectField.Name, parameters);
				injectField.SetValue(obj, value);
			}
		}

		private void InjectProperties(object obj, IObjectResolver resolver, IReadOnlyList<IInjectParameter> parameters)
		{
			if (injectTypeInfo.InjectProperties == null)
			{
				return;
			}
			foreach (PropertyInfo injectProperty in injectTypeInfo.InjectProperties)
			{
				object value = resolver.ResolveOrParameter(injectProperty.PropertyType, injectProperty.Name, parameters);
				injectProperty.SetValue(obj, value);
			}
		}

		private void InjectMethods(object obj, IObjectResolver resolver, IReadOnlyList<IInjectParameter> parameters)
		{
			if (injectTypeInfo.InjectMethods == null)
			{
				return;
			}
			foreach (InjectMethodInfo injectMethod in injectTypeInfo.InjectMethods)
			{
				ParameterInfo[] parameterInfos = injectMethod.ParameterInfos;
				object[] array = CappedArrayPool<object>.Shared8Limit.Rent(parameterInfos.Length);
				try
				{
					for (int i = 0; i < parameterInfos.Length; i++)
					{
						ParameterInfo parameterInfo = parameterInfos[i];
						array[i] = resolver.ResolveOrParameter(parameterInfo.ParameterType, parameterInfo.Name, parameters);
					}
					injectMethod.MethodInfo.Invoke(obj, array);
				}
				catch (VContainerException ex)
				{
					throw new VContainerException(ex.InvalidType, $"Failed to resolve {injectTypeInfo.Type} : {ex.Message}");
				}
				finally
				{
					CappedArrayPool<object>.Shared8Limit.Return(array);
				}
			}
		}
	}
}
