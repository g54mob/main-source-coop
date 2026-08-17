using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace VContainer.Internal
{
	internal static class TypeAnalyzer
	{
		private static readonly ConcurrentDictionary<Type, InjectTypeInfo> Cache = new ConcurrentDictionary<Type, InjectTypeInfo>();

		[ThreadStatic]
		private static Stack<DependencyInfo> circularDependencyChecker;

		private static readonly Func<Type, InjectTypeInfo> AnalyzeFunc = Analyze;

		public static InjectTypeInfo AnalyzeWithCache(Type type)
		{
			return Cache.GetOrAdd(type, AnalyzeFunc);
		}

		public static InjectTypeInfo Analyze(Type type)
		{
			InjectConstructorInfo injectConstructorInfo = null;
			Type type2 = type;
			TypeInfo typeInfo = type.GetTypeInfo();
			int num = 0;
			int num2 = -1;
			ConstructorInfo[] constructors = typeInfo.GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (ConstructorInfo constructorInfo in constructors)
			{
				if (constructorInfo.IsDefined(typeof(InjectAttribute), inherit: false))
				{
					if (++num > 1)
					{
						throw new VContainerException(type, "Type found multiple [Inject] marked constructors, type: " + type.Name);
					}
					injectConstructorInfo = new InjectConstructorInfo(constructorInfo);
				}
				else if (num <= 0)
				{
					ParameterInfo[] parameters = constructorInfo.GetParameters();
					if (parameters.Length > num2)
					{
						injectConstructorInfo = new InjectConstructorInfo(constructorInfo, parameters);
						num2 = parameters.Length;
					}
				}
			}
			if (injectConstructorInfo == null && !(type.IsEnum | type.IsSubclassOf(typeof(Component))))
			{
				throw new VContainerException(type, "Type does not found injectable constructor, type: " + type.Name);
			}
			List<InjectMethodInfo> list = null;
			List<FieldInfo> list2 = null;
			List<PropertyInfo> list3 = null;
			BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			while (type != null && type != typeof(object))
			{
				MethodInfo[] methods = type.GetMethods(bindingAttr);
				foreach (MethodInfo methodInfo in methods)
				{
					if (!methodInfo.IsDefined(typeof(InjectAttribute), inherit: false))
					{
						continue;
					}
					if (list == null)
					{
						list = new List<InjectMethodInfo>();
					}
					else
					{
						foreach (InjectMethodInfo item in list)
						{
							if (item.MethodInfo.GetBaseDefinition() == methodInfo.GetBaseDefinition())
							{
								goto end_IL_0169;
							}
						}
					}
					list.Add(new InjectMethodInfo(methodInfo));
					continue;
					end_IL_0169:
					break;
				}
				FieldInfo[] fields = type.GetFields(bindingAttr);
				foreach (FieldInfo fieldInfo in fields)
				{
					if (!fieldInfo.IsDefined(typeof(InjectAttribute), inherit: false))
					{
						continue;
					}
					if (list2 == null)
					{
						list2 = new List<FieldInfo>();
					}
					else
					{
						if (Contains(list2, fieldInfo))
						{
							string message = $"Duplicate injection found for field: {fieldInfo}";
							throw new VContainerException(type, message);
						}
						if (list2.Any((FieldInfo x) => x.Name == fieldInfo.Name))
						{
							continue;
						}
					}
					list2.Add(fieldInfo);
				}
				PropertyInfo[] properties = type.GetProperties(bindingAttr);
				foreach (PropertyInfo propertyInfo in properties)
				{
					if (!propertyInfo.IsDefined(typeof(InjectAttribute), inherit: false))
					{
						continue;
					}
					if (list3 == null)
					{
						list3 = new List<PropertyInfo>();
					}
					else
					{
						foreach (PropertyInfo item2 in list3)
						{
							if (item2.Name == propertyInfo.Name)
							{
								goto end_IL_02a6;
							}
						}
					}
					list3.Add(propertyInfo);
					continue;
					end_IL_02a6:
					break;
				}
				type = type.BaseType;
			}
			return new InjectTypeInfo(type2, injectConstructorInfo, list, list2, list3);
		}

		private static bool Contains(List<FieldInfo> fields, FieldInfo field)
		{
			for (int i = 0; i < fields.Count; i++)
			{
				if (fields[i].Name == field.Name)
				{
					return true;
				}
			}
			return false;
		}

		public static void CheckCircularDependency(IReadOnlyList<Registration> registrations, Registry registry)
		{
			if (circularDependencyChecker == null)
			{
				circularDependencyChecker = new Stack<DependencyInfo>();
			}
			for (int i = 0; i < registrations.Count; i++)
			{
				circularDependencyChecker.Clear();
				CheckCircularDependencyRecursive(new DependencyInfo(registrations[i]), registry, circularDependencyChecker);
			}
		}

		private static void CheckCircularDependencyRecursive(DependencyInfo current, Registry registry, Stack<DependencyInfo> stack)
		{
			int num = 0;
			foreach (DependencyInfo item in stack)
			{
				if (current.ImplementationType == item.ImplementationType)
				{
					if (current.Dependency.Provider is FuncInstanceProvider)
					{
						return;
					}
					stack.Push(current);
					string text = string.Join("\n", stack.Take(num + 1).Reverse().Select((DependencyInfo item, int itemIndex) => $"    [{itemIndex + 1}] {item} --> {item.ImplementationType.FullName}"));
					throw new VContainerException(current.Dependency.ImplementationType, "Circular dependency detected!\n" + text);
				}
				num++;
			}
			stack.Push(current);
			if (Cache.TryGetValue(current.ImplementationType, out var value))
			{
				if (value.InjectConstructor != null)
				{
					ParameterInfo[] parameterInfos = value.InjectConstructor.ParameterInfos;
					foreach (ParameterInfo parameterInfo in parameterInfos)
					{
						if (registry.TryGet(parameterInfo.ParameterType, out var registration))
						{
							CheckCircularDependencyRecursive(new DependencyInfo(registration, current.Dependency, value.InjectConstructor.ConstructorInfo, parameterInfo), registry, stack);
						}
					}
				}
				if (value.InjectMethods != null)
				{
					foreach (InjectMethodInfo injectMethod in value.InjectMethods)
					{
						ParameterInfo[] parameterInfos = injectMethod.ParameterInfos;
						foreach (ParameterInfo parameterInfo2 in parameterInfos)
						{
							if (registry.TryGet(parameterInfo2.ParameterType, out var registration2))
							{
								CheckCircularDependencyRecursive(new DependencyInfo(registration2, current.Dependency, injectMethod.MethodInfo, parameterInfo2), registry, stack);
							}
						}
					}
				}
				if (value.InjectFields != null)
				{
					foreach (FieldInfo injectField in value.InjectFields)
					{
						if (registry.TryGet(injectField.FieldType, out var registration3))
						{
							CheckCircularDependencyRecursive(new DependencyInfo(registration3, current.Dependency, injectField), registry, stack);
						}
					}
				}
				if (value.InjectProperties != null)
				{
					foreach (PropertyInfo injectProperty in value.InjectProperties)
					{
						if (registry.TryGet(injectProperty.PropertyType, out var registration4))
						{
							CheckCircularDependencyRecursive(new DependencyInfo(registration4, current.Dependency, injectProperty), registry, stack);
						}
					}
				}
			}
			stack.Pop();
		}
	}
}
