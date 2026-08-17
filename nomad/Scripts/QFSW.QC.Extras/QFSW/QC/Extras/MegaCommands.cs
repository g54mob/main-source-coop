using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using QFSW.QC.Utilities;

namespace QFSW.QC.Extras
{
	public static class MegaCommands
	{
		private static readonly QuantumSerializer Serializer = new QuantumSerializer();

		private static readonly QuantumParser Parser = new QuantumParser();

		private static MethodInfo[] ExtractMethods(Type type, string name)
		{
			MethodInfo[] array = (from x in type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod)
				where x.Name == name
				select x).ToArray();
			if (!array.Any())
			{
				PropertyInfo property = type.GetProperty(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod);
				if (property != null)
				{
					array = new MethodInfo[2] { property.GetMethod, property.SetMethod }.Where((MethodInfo x) => x != null).ToArray();
					if (array.Length != 0)
					{
						return array;
					}
				}
				throw new ArgumentException("No method or property named " + name + " could be found in class " + Serializer.SerializeFormatted(type));
			}
			return array;
		}

		private static string GenerateSignature(MethodInfo method)
		{
			IEnumerable<string> values = from x in method.GetParameters()
				select (Name: x.Name, ParameterType: x.ParameterType) into x
				select x.ParameterType.GetDisplayName() + " " + x.Name;
			string text = string.Join(", ", values);
			return method.Name + "(" + text + ")";
		}

		private static MethodInfo GetIdealOverload(MethodInfo[] methods, bool isStatic, int argc)
		{
			methods = methods.Where((MethodInfo x) => x.IsStatic == isStatic).ToArray();
			if (methods.Length == 0)
			{
				throw new ArgumentException("No " + (isStatic ? "static" : "non-static") + " overloads could be found.");
			}
			if (methods.Length == 1)
			{
				return methods[0];
			}
			methods = methods.Where((MethodInfo x) => !x.IsGenericMethod).ToArray();
			if (methods.Length == 0)
			{
				throw new ArgumentException("Generic methods are not supported.");
			}
			MethodInfo[] array = methods.Where((MethodInfo x) => x.GetParameters().Length == argc).ToArray();
			if (array.Length == 1)
			{
				return array[0];
			}
			if (array.Length == 0)
			{
				IEnumerable<string> values = methods.Select(GenerateSignature);
				string arg = string.Join("\n", values);
				throw new ArgumentException($"No overloads with {argc} arguments were found. the following overloads are available:\n{arg}");
			}
			IEnumerable<string> values2 = array.Select(GenerateSignature);
			string text = string.Join("\n", values2);
			throw new ArgumentException("Multiple overloads with the same argument count were found: please specify the types explicitly.\n" + text);
		}

		private static MethodInfo GetIdealOverload(MethodInfo[] methods, bool isStatic, Type[] argTypes)
		{
			MethodInfo[] array = methods;
			foreach (MethodInfo methodInfo in array)
			{
				if (methodInfo.IsStatic == isStatic && (from x in methodInfo.GetParameters()
					select x.ParameterType).SequenceEqual(argTypes))
				{
					return methodInfo;
				}
			}
			array = methods;
			foreach (MethodInfo methodInfo2 in array)
			{
				if (methodInfo2.IsStatic == isStatic)
				{
					ParameterInfo[] parameters = methodInfo2.GetParameters();
					if (parameters.Length == argTypes.Length && parameters.Select((ParameterInfo x) => x.ParameterType).Zip(argTypes, (Type x, Type y) => (x: x, y: y)).All(((Type x, Type y) pair) => pair.x.IsAssignableFrom(pair.y)))
					{
						return methodInfo2;
					}
				}
			}
			throw new ArgumentException("No overload with the supplied argument types could be found.");
		}

		private static object[] CreateArgs(MethodInfo method, string[] rawArgs)
		{
			Type[] argTypes = (from x in method.GetParameters()
				select x.ParameterType).ToArray();
			return CreateArgs(method, argTypes, rawArgs);
		}

		private static object[] CreateArgs(MethodInfo method, Type[] argTypes, string[] rawArgs)
		{
			ParameterInfo[] parameters = method.GetParameters();
			int num = parameters.Count((ParameterInfo x) => x.HasDefaultValue);
			if (rawArgs.Length < argTypes.Length - num || rawArgs.Length > argTypes.Length)
			{
				throw new ArgumentException($"Incorrect number ({rawArgs.Length}) of arguments supplied for {Serializer.SerializeFormatted(method.DeclaringType)}.{method.Name}" + $", expected {argTypes.Length}");
			}
			object[] array = new object[argTypes.Length];
			for (int num2 = 0; num2 < array.Length; num2++)
			{
				if (num2 < rawArgs.Length)
				{
					array[num2] = Parser.Parse(rawArgs[num2], argTypes[num2]);
				}
				else
				{
					array[num2] = parameters[num2].DefaultValue;
				}
			}
			return array;
		}

		private static object InvokeAndUnwrapException(this MethodInfo method, object[] args)
		{
			try
			{
				return method.Invoke(null, args);
			}
			catch (TargetInvocationException ex)
			{
				throw ex.InnerException;
			}
		}

		private static object InvokeAndUnwrapException(this MethodInfo method, IEnumerable<object> targets, object[] args)
		{
			try
			{
				return InvocationTargetFactory.InvokeOnTargets(method, targets, args);
			}
			catch (TargetInvocationException ex)
			{
				throw ex.InnerException;
			}
		}

		[Command("call-static", Platform.AllPlatforms, MonoTargetType.Single)]
		private static object CallStatic(Type classType, string funcName)
		{
			return CallStatic(classType, funcName, Array.Empty<string>());
		}

		[Command("call-static", Platform.AllPlatforms, MonoTargetType.Single)]
		private static object CallStatic(Type classType, string funcName, string[] args)
		{
			MethodInfo idealOverload = GetIdealOverload(ExtractMethods(classType, funcName), isStatic: true, args.Length);
			object[] args2 = CreateArgs(idealOverload, args);
			return idealOverload.InvokeAndUnwrapException(args2);
		}

		[Command("call-static", Platform.AllPlatforms, MonoTargetType.Single)]
		[CommandDescription("Invokes the specified static method or property with the provided arguments. Provide [argTypes] if there are ambiguous overloads")]
		private static object CallStatic([CommandParameterDescription("Namespace qualified typename of the class.")] Type classType, [CommandParameterDescription("Name of the method or property.")] string funcName, [CommandParameterDescription("The arguments for the function call.")] string[] args, [CommandParameterDescription("The types of the arguments to resolve ambiguous overloads.")] Type[] argTypes)
		{
			MethodInfo idealOverload = GetIdealOverload(ExtractMethods(classType, funcName), isStatic: true, argTypes);
			object[] args2 = CreateArgs(idealOverload, argTypes, args);
			return idealOverload.InvokeAndUnwrapException(args2);
		}

		[Command("call-instance", Platform.AllPlatforms, MonoTargetType.Single)]
		private static object CallInstance(Type classType, string funcName, MonoTargetType targetType)
		{
			return CallInstance(classType, funcName, targetType, Array.Empty<string>());
		}

		[Command("call-instance", Platform.AllPlatforms, MonoTargetType.Single)]
		private static object CallInstance(Type classType, string funcName, MonoTargetType targetType, string[] args)
		{
			MethodInfo idealOverload = GetIdealOverload(ExtractMethods(classType, funcName), isStatic: false, args.Length);
			object[] args2 = CreateArgs(idealOverload, args);
			IEnumerable<object> targets = InvocationTargetFactory.FindTargets(classType, targetType);
			return idealOverload.InvokeAndUnwrapException(targets, args2);
		}

		[Command("call-instance", Platform.AllPlatforms, MonoTargetType.Single)]
		[CommandDescription("Invokes the specified non-static method or property with the provided arguments. Provide [argTypes] if there are ambiguous overloads")]
		private static object CallInstance([CommandParameterDescription("Namespace qualified typename of the class.")] Type classType, [CommandParameterDescription("Name of the method or property.")] string funcName, [CommandParameterDescription("The MonoTargetType used to find the target instances.")] MonoTargetType targetType, [CommandParameterDescription("The arguments for the function call.")] string[] args, [CommandParameterDescription("The types of the arguments to resolve ambiguous overloads.")] Type[] argTypes)
		{
			MethodInfo idealOverload = GetIdealOverload(ExtractMethods(classType, funcName), isStatic: false, argTypes);
			object[] args2 = CreateArgs(idealOverload, argTypes, args);
			IEnumerable<object> targets = InvocationTargetFactory.FindTargets(classType, targetType);
			return idealOverload.InvokeAndUnwrapException(targets, args2);
		}
	}
}
