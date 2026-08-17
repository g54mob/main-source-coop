using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using QFSW.QC.Internal;
using QFSW.QC.Utilities;

namespace QFSW.QC
{
	public class CommandData
	{
		public readonly string CommandName;

		public readonly string CommandDescription;

		public readonly string CommandSignature;

		public readonly string ParameterSignature;

		public readonly string GenericSignature;

		public readonly ParameterInfo[] MethodParamData;

		public readonly Type[] ParamTypes;

		public readonly Type[] GenericParamTypes;

		public readonly MethodInfo MethodData;

		public readonly MonoTargetType MonoTarget;

		public readonly bool HasParamsArgument;

		private readonly object[] _defaultParameters;

		public bool IsGeneric => GenericParamTypes.Length != 0;

		public bool IsStatic => MethodData.IsStatic;

		public bool HasDescription => !string.IsNullOrWhiteSpace(CommandDescription);

		public int ParamCount => ParamTypes.Length - _defaultParameters.Length;

		public Type[] MakeGenericArguments(params Type[] genericTypeArguments)
		{
			if (genericTypeArguments.Length != GenericParamTypes.Length)
			{
				throw new ArgumentException("Incorrect number of generic substitution types were supplied.");
			}
			Dictionary<string, Type> dictionary = new Dictionary<string, Type>();
			for (int i = 0; i < genericTypeArguments.Length; i++)
			{
				dictionary.Add(GenericParamTypes[i].Name, genericTypeArguments[i]);
			}
			Type[] array = new Type[ParamTypes.Length];
			for (int j = 0; j < array.Length; j++)
			{
				if (ParamTypes[j].ContainsGenericParameters)
				{
					Type type = ConstructGenericType(ParamTypes[j], dictionary);
					array[j] = type;
				}
				else
				{
					array[j] = ParamTypes[j];
				}
			}
			return array;
		}

		private Type ConstructGenericType(Type genericType, Dictionary<string, Type> substitutionTable)
		{
			if (!genericType.ContainsGenericParameters)
			{
				return genericType;
			}
			if (substitutionTable.ContainsKey(genericType.Name))
			{
				return substitutionTable[genericType.Name];
			}
			if (genericType.IsArray)
			{
				return ConstructGenericType(genericType.GetElementType(), substitutionTable).MakeArrayType();
			}
			if (genericType.IsGenericType)
			{
				Type genericTypeDefinition = genericType.GetGenericTypeDefinition();
				Type[] genericArguments = genericType.GetGenericArguments();
				for (int i = 0; i < genericArguments.Length; i++)
				{
					genericArguments[i] = ConstructGenericType(genericArguments[i], substitutionTable);
				}
				return genericTypeDefinition.MakeGenericType(genericArguments);
			}
			throw new ArgumentException($"Could not construct the generic type {genericType}");
		}

		public object Invoke(object[] paramData, Type[] genericTypeArguments)
		{
			int num = 0;
			int num2 = paramData.Length;
			if (MonoTarget == MonoTargetType.Argument || MonoTarget == MonoTargetType.ArgumentMulti)
			{
				num++;
				num2--;
			}
			object[] array = new object[num2 + _defaultParameters.Length];
			Array.Copy(paramData, num, array, 0, num2);
			Array.Copy(_defaultParameters, 0, array, num2, _defaultParameters.Length);
			MethodInfo invokingMethod = GetInvokingMethod(genericTypeArguments);
			if (IsStatic)
			{
				return invokingMethod.Invoke(null, array);
			}
			return InvocationTargetFactory.InvokeOnTargets(invokingMethod, MonoTarget switch
			{
				MonoTargetType.Argument => paramData[0].Yield(), 
				MonoTargetType.ArgumentMulti => paramData[0] as IEnumerable<object>, 
				_ => GetInvocationTargets(invokingMethod), 
			}, array);
		}

		protected virtual IEnumerable<object> GetInvocationTargets(MethodInfo invokingMethod)
		{
			return InvocationTargetFactory.FindTargets(invokingMethod.DeclaringType, MonoTarget);
		}

		private MethodInfo GetInvokingMethod(Type[] genericTypeArguments)
		{
			if (!IsGeneric)
			{
				return MethodData;
			}
			Type declaringType = MethodData.DeclaringType;
			MethodInfo method = MethodData;
			if (declaringType.IsGenericTypeDefinition)
			{
				int count = declaringType.GetGenericArguments().Length;
				Type[] genericTypes = genericTypeArguments.Take(count).ToArray();
				genericTypeArguments = genericTypeArguments.Skip(count).ToArray();
				declaringType = WrapConstruction<Type>(() => declaringType.MakeGenericType(genericTypes));
				method = method.RebaseMethod(declaringType);
			}
			if (genericTypeArguments.Length != 0)
			{
				return WrapConstruction<MethodInfo>(() => method.MakeGenericMethod(genericTypeArguments));
			}
			return method;
			T WrapConstruction<T>(Func<T> f)
			{
				try
				{
					return f();
				}
				catch (ArgumentException)
				{
					throw new ArgumentException("Supplied generic parameters did not satisfy the generic constraints imposed by '" + CommandName + "'");
				}
			}
		}

		private string BuildPrefix(Type declaringType)
		{
			List<string> prefixes = new List<string>();
			Assembly assembly = declaringType.Assembly;
			while (declaringType != null)
			{
				AddPrefixes(declaringType.GetCustomAttributes<CommandPrefixAttribute>(), declaringType.Name);
				declaringType = declaringType.DeclaringType;
			}
			AddPrefixes(assembly.GetCustomAttributes<CommandPrefixAttribute>(), assembly.GetName().Name);
			return string.Join("", prefixes.Reversed());
			void AddPrefixes(IEnumerable<CommandPrefixAttribute> prefixAttributes, string defaultName)
			{
				foreach (CommandPrefixAttribute item in prefixAttributes.Reverse())
				{
					if (item.Valid)
					{
						string text = item.Prefix;
						if (string.IsNullOrWhiteSpace(text))
						{
							text = defaultName;
						}
						prefixes.Add(text);
					}
				}
			}
		}

		private string BuildGenericSignature(Type[] genericParamTypes)
		{
			if (genericParamTypes.Length == 0)
			{
				return string.Empty;
			}
			IEnumerable<string> values = genericParamTypes.Select((Type x) => x.Name);
			return "<" + string.Join(", ", values) + ">";
		}

		private string BuildParameterSignature(ParameterInfo[] methodParams, int defaultParameterCount)
		{
			string text = string.Empty;
			for (int i = 0; i < methodParams.Length - defaultParameterCount; i++)
			{
				string text2 = FormatParameterName(methodParams[i]);
				text = text + ((i == 0) ? string.Empty : " ") + text2;
			}
			return text;
		}

		private string FormatParameterName(ParameterInfo param)
		{
			if (!param.HasAttribute<ParamArrayAttribute>())
			{
				return param.Name;
			}
			return param.Name + "...";
		}

		private Type[] BuildGenericParamTypes(MethodInfo method, Type declaringType)
		{
			List<Type> list = new List<Type>();
			if (declaringType.IsGenericTypeDefinition)
			{
				list.AddRange(declaringType.GetGenericArguments());
			}
			if (method.IsGenericMethodDefinition)
			{
				list.AddRange(method.GetGenericArguments());
			}
			return list.ToArray();
		}

		public CommandData(MethodInfo methodData, string commandName, MonoTargetType monoTarget, int defaultParameterCount = 0)
		{
			CommandName = commandName;
			MethodData = methodData;
			MonoTarget = monoTarget;
			if (string.IsNullOrWhiteSpace(commandName))
			{
				CommandName = methodData.Name;
			}
			Type declaringType = methodData.DeclaringType;
			string text = BuildPrefix(declaringType);
			CommandName = text + CommandName;
			List<ParameterInfo> list = methodData.GetParameters().ToList();
			if (MonoTarget == MonoTargetType.Argument)
			{
				list.Insert(0, new DummyParameter(methodData.DeclaringType, "target", 0));
			}
			else if (MonoTarget == MonoTargetType.ArgumentMulti)
			{
				list.Insert(0, new DummyParameter(methodData.DeclaringType.MakeArrayType(), "targets", 0));
			}
			MethodParamData = list.ToArray();
			ParamTypes = MethodParamData.Select((ParameterInfo x) => x.ParameterType).ToArray();
			if (MethodParamData.Length != 0)
			{
				ParameterInfo provider = MethodParamData.Last();
				HasParamsArgument = provider.HasAttribute<ParamArrayAttribute>();
			}
			_defaultParameters = new object[defaultParameterCount];
			for (int num = 0; num < defaultParameterCount; num++)
			{
				int num2 = MethodParamData.Length - defaultParameterCount + num;
				_defaultParameters[num] = MethodParamData[num2].DefaultValue;
			}
			GenericParamTypes = BuildGenericParamTypes(methodData, declaringType);
			ParameterSignature = BuildParameterSignature(MethodParamData, defaultParameterCount);
			GenericSignature = BuildGenericSignature(GenericParamTypes);
			CommandSignature = ((ParamCount > 0) ? (CommandName + GenericSignature + " " + ParameterSignature) : (CommandName + GenericSignature));
		}

		public CommandData(MethodInfo methodData, MonoTargetType monoTarget, int defaultParameterCount = 0)
			: this(methodData, methodData.Name, monoTarget, defaultParameterCount)
		{
		}

		public CommandData(MethodInfo methodData, CommandAttribute commandAttribute, int defaultParameterCount = 0)
			: this(methodData, commandAttribute.Alias, commandAttribute.MonoTarget, defaultParameterCount)
		{
			CommandDescription = commandAttribute.Description;
		}

		public CommandData(MethodInfo methodData, CommandAttribute commandAttribute, CommandDescriptionAttribute descriptionAttribute, int defaultParameterCount = 0)
			: this(methodData, commandAttribute, defaultParameterCount)
		{
			if (descriptionAttribute != null && descriptionAttribute.Valid && string.IsNullOrWhiteSpace(commandAttribute.Description))
			{
				CommandDescription = descriptionAttribute.Description;
			}
		}
	}
}
