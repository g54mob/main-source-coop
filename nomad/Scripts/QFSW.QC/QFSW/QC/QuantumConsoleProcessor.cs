using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using QFSW.QC.Internal;
using QFSW.QC.Suggestors.Tags;
using QFSW.QC.Utilities;
using UnityEngine;

namespace QFSW.QC
{
	public static class QuantumConsoleProcessor
	{
		private const string helpStr = "Welcome to Quantum Console! In order to see specific help about any specific command, please use the 'man' command. Use 'man man' to see more about the man command. To see a full list of all commands, use 'all-commands'.\n\nmono-targets\nVarious commands may show a mono-target in their command signature.\nThis means they are not static commands, and instead requires instance(s) of the class in order to invoke the command.\nEach mono-target works differently as follows:\n - single: uses the first instance of the type found in the scene\n - all: uses all instances of the type found in the scene\n - registry: uses all instances of the type found in the registry\n - singleton: creates and manages a single instance automatically\n\nThe registry is a part of the Quantum Registry that allows you to decide which specific instances of the class should be used when invoking the command. In order to add an object to the registry, either use QFSW.QC.QuantumRegistry.RegisterObject<T> or the runtime command 'register-object<T>'.";

		public static LoggingLevel loggingLevel = LoggingLevel.Full;

		private static readonly QuantumParser _parser = new QuantumParser();

		private static readonly QuantumPreprocessor _preprocessor = new QuantumPreprocessor();

		private static readonly QuantumScanRuleset _scanRuleset = new QuantumScanRuleset();

		private static readonly ConcurrentDictionary<string, CommandData> _commandTable = new ConcurrentDictionary<string, CommandData>();

		private static readonly List<CommandData> _commandCache = new List<CommandData>();

		private static int _loadedCommandCount = 0;

		private static bool _commandCacheDirty = true;

		private static readonly Assembly[] _loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

		public static bool TableGenerated { get; private set; }

		public static bool TableIsGenerating { get; private set; }

		[Command("command-count", "Gets the number of loaded commands", Platform.AllPlatforms, MonoTargetType.Single)]
		public static int LoadedCommandCount => _loadedCommandCount;

		[Command("help", "Shows a basic help guide for Quantum Console", Platform.AllPlatforms, MonoTargetType.Single)]
		private static string GetHelp()
		{
			return "Welcome to Quantum Console! In order to see specific help about any specific command, please use the 'man' command. Use 'man man' to see more about the man command. To see a full list of all commands, use 'all-commands'.\n\nmono-targets\nVarious commands may show a mono-target in their command signature.\nThis means they are not static commands, and instead requires instance(s) of the class in order to invoke the command.\nEach mono-target works differently as follows:\n - single: uses the first instance of the type found in the scene\n - all: uses all instances of the type found in the scene\n - registry: uses all instances of the type found in the registry\n - singleton: creates and manages a single instance automatically\n\nThe registry is a part of the Quantum Registry that allows you to decide which specific instances of the class should be used when invoking the command. In order to add an object to the registry, either use QFSW.QC.QuantumRegistry.RegisterObject<T> or the runtime command 'register-object<T>'.";
		}

		[Command("manual", Platform.AllPlatforms, MonoTargetType.Single)]
		[Command("man", Platform.AllPlatforms, MonoTargetType.Single)]
		private static string ManualHelp()
		{
			return "To use the man command, simply put the desired command name in front of it. For example, 'man my-command' will generate the manual for 'my-command'";
		}

		[CommandDescription("Generates a user manual for any given command, including built in ones. To use the man command, simply put the desired command name infront of it. For example, 'man my-command' will generate the manual for 'my-command'")]
		[Command("help", Platform.AllPlatforms, MonoTargetType.Single)]
		[Command("manual", Platform.AllPlatforms, MonoTargetType.Single)]
		[Command("man", Platform.AllPlatforms, MonoTargetType.Single)]
		private static string GenerateCommandManual([CommandName] string commandName)
		{
			string[] array = (from key in _commandTable.Keys
				where key.Split('(')[0] == commandName
				orderby key
				select key).ToArray();
			if (array.Length == 0)
			{
				throw new ArgumentException("No command with the name " + commandName + " was found.");
			}
			Dictionary<string, ParameterInfo> dictionary = new Dictionary<string, ParameterInfo>();
			Dictionary<string, Type> dictionary2 = new Dictionary<string, Type>();
			Dictionary<string, CommandParameterDescriptionAttribute> dictionary3 = new Dictionary<string, CommandParameterDescriptionAttribute>();
			List<Type> list = new List<Type>(1);
			string text = "Generated user manual for " + commandName + "\nAvailable command signatures:";
			for (int num = 0; num < array.Length; num++)
			{
				CommandData commandData = _commandTable[array[num]];
				list.Add(commandData.MethodData.DeclaringType);
				text = text + "\n   - " + commandData.CommandSignature;
				if (!commandData.IsStatic)
				{
					text = text + " (mono-target = " + commandData.MonoTarget.ToString().ToLower() + ")";
				}
				for (int num2 = 0; num2 < commandData.ParamCount; num2++)
				{
					ParameterInfo parameterInfo = commandData.MethodParamData[num2];
					if (!dictionary.ContainsKey(parameterInfo.Name))
					{
						dictionary.Add(parameterInfo.Name, parameterInfo);
					}
					if (!dictionary3.ContainsKey(parameterInfo.Name))
					{
						CommandParameterDescriptionAttribute customAttribute = parameterInfo.GetCustomAttribute<CommandParameterDescriptionAttribute>();
						if (customAttribute != null && customAttribute.Valid)
						{
							dictionary3.Add(parameterInfo.Name, customAttribute);
						}
					}
				}
				if (!commandData.IsGeneric)
				{
					continue;
				}
				Type[] genericParamTypes = commandData.GenericParamTypes;
				foreach (Type type in genericParamTypes)
				{
					if (!dictionary2.ContainsKey(type.Name))
					{
						dictionary2.Add(type.Name, type);
					}
				}
			}
			if (dictionary.Count > 0)
			{
				text += "\nParameter info:";
				ParameterInfo[] array2 = dictionary.Values.ToArray();
				foreach (ParameterInfo parameterInfo2 in array2)
				{
					string text2 = parameterInfo2.ParameterType.GetDisplayName();
					if (parameterInfo2.HasAttribute<ParamArrayAttribute>())
					{
						text2 = "params " + text2;
					}
					text = text + "\n   - " + parameterInfo2.Name + ": " + text2;
				}
			}
			string text3 = "";
			if (dictionary2.Count > 0)
			{
				Type[] array3 = dictionary2.Values.ToArray();
				for (int num5 = 0; num5 < array3.Length; num5++)
				{
					Type type2 = array3[num5];
					Type[] genericParameterConstraints = type2.GetGenericParameterConstraints();
					GenericParameterAttributes genericParameterAttributes = type2.GenericParameterAttributes;
					List<string> list2 = new List<string>();
					if (genericParameterAttributes.HasFlag(GenericParameterAttributes.NotNullableValueTypeConstraint))
					{
						list2.Add("struct");
					}
					if (genericParameterAttributes.HasFlag(GenericParameterAttributes.ReferenceTypeConstraint))
					{
						list2.Add("class");
					}
					for (int num6 = 0; num6 < genericParameterConstraints.Length; num6++)
					{
						list2.Add(genericParameterConstraints[num5].GetDisplayName());
					}
					if (genericParameterAttributes.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint))
					{
						list2.Add("new()");
					}
					if (list2.Count > 0)
					{
						text3 = text3 + "\n   - " + type2.Name + ": " + string.Join(", ", list2);
					}
				}
			}
			if (!string.IsNullOrWhiteSpace(text3))
			{
				text = text + "\nGeneric constraints:" + text3;
			}
			for (int num7 = 0; num7 < array.Length; num7++)
			{
				CommandData commandData2 = _commandTable[array[num7]];
				if (commandData2.HasDescription)
				{
					text = text + "\n\nCommand description:\n" + commandData2.CommandDescription;
					num7 = array.Length;
				}
			}
			if (dictionary3.Count > 0)
			{
				text += "\n\nParameter descriptions:";
				ParameterInfo[] array4 = dictionary.Values.ToArray();
				foreach (ParameterInfo parameterInfo3 in array4)
				{
					if (dictionary3.ContainsKey(parameterInfo3.Name))
					{
						text = text + "\n - " + parameterInfo3.Name + ": " + dictionary3[parameterInfo3.Name].Description;
					}
				}
			}
			list = list.Distinct().ToList();
			text += "\n\nDeclared in";
			if (list.Count == 1)
			{
				text = text + " " + list[0].GetDisplayName(includeNamespace: true);
			}
			else
			{
				text += ":";
				foreach (Type item in list)
				{
					text = text + "\n   - " + item.GetDisplayName(includeNamespace: true);
				}
			}
			return text;
		}

		public static IEnumerable<CommandData> GetUniqueCommands()
		{
			return from x in GetAllCommands().DistinctBy((CommandData x) => x.CommandName)
				orderby x.CommandName
				select x;
		}

		[CommandDescription("Generates a list of all commands currently loaded by the Quantum Console Processor")]
		[Command("commands", Platform.AllPlatforms, MonoTargetType.Single)]
		[Command("all-commands", Platform.AllPlatforms, MonoTargetType.Single)]
		private static string GenerateCommandList()
		{
			string text = "List of all commands loaded by the Quantum Processor. Use 'man' on any command to see more:";
			foreach (CommandData uniqueCommand in GetUniqueCommands())
			{
				text = text + "\n   - " + uniqueCommand.CommandName;
			}
			return text;
		}

		[Command("user-commands", "Generates a list of all commands added by the user", Platform.AllPlatforms, MonoTargetType.Single)]
		private static IEnumerable<string> GenerateUserCommandList()
		{
			return from x in GetUniqueCommands()
				where !x.MethodData.DeclaringType.Assembly.FullName.StartsWith("QFSW.QC")
				select "   - " + x.CommandName;
		}

		public static IEnumerable<CommandData> GetAllCommands()
		{
			if (_commandCacheDirty)
			{
				lock (_commandCache)
				{
					_commandCache.Clear();
					_commandCache.AddRange(_commandTable.Values);
					_commandCacheDirty = false;
				}
			}
			return _commandCache;
		}

		public static void GenerateCommandTable(bool deployThread = false, bool forceReload = false)
		{
			if (deployThread)
			{
				ThreadPool.QueueUserWorkItem(delegate
				{
					try
					{
						GenerateCommandTable(deployThread: false, forceReload);
					}
					catch (Exception exception)
					{
						Debug.LogException(exception);
					}
				});
				return;
			}
			lock (_commandTable)
			{
				if (!TableGenerated || forceReload)
				{
					TableIsGenerating = true;
					if (forceReload && TableGenerated)
					{
						_commandTable.Clear();
						_loadedCommandCount = 0;
					}
					Parallel.ForEach(_loadedAssemblies, LoadCommandsFromAssembly);
					TableIsGenerating = false;
					TableGenerated = true;
					GC.Collect(3, GCCollectionMode.Forced, blocking: false, compacting: true);
				}
			}
		}

		private static IEnumerable<(MethodInfo method, MemberInfo member)> ExtractCommandMethods(Type type)
		{
			MethodInfo[] methods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			PropertyInfo[] properties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			MethodInfo[] array = methods;
			foreach (MethodInfo methodInfo in array)
			{
				yield return (method: methodInfo, member: methodInfo);
			}
			PropertyInfo[] array2 = properties;
			foreach (PropertyInfo property in array2)
			{
				if (property.CanWrite)
				{
					yield return (method: property.SetMethod, member: property);
				}
				if (property.CanRead)
				{
					yield return (method: property.GetMethod, member: property);
				}
			}
			FieldInfo[] array3 = fields;
			foreach (FieldInfo field in array3)
			{
				if (!field.HasAttribute<CommandAttribute>())
				{
					continue;
				}
				if (field.IsDelegate())
				{
					if (field.IsStrongDelegate())
					{
						FieldDelegateMethod item = new FieldDelegateMethod(field);
						yield return (method: item, member: field);
					}
					else if (loggingLevel >= LoggingLevel.Warnings)
					{
						Debug.LogWarning($"Quantum Processor Warning: Could not add '{field.Name}' from {field.DeclaringType} to the table as it is an invalid delegate type.");
					}
				}
				else
				{
					FieldAutoMethod item2 = new FieldAutoMethod(field, FieldAutoMethod.AccessType.Read);
					yield return (method: item2, member: field);
					if (!field.IsLiteral && !field.IsInitOnly)
					{
						FieldAutoMethod item3 = new FieldAutoMethod(field, FieldAutoMethod.AccessType.Write);
						yield return (method: item3, member: field);
					}
				}
			}
		}

		private static bool GetCommandSupported(CommandData command, out string unsupportedReason)
		{
			for (int i = 0; i < command.ParamCount; i++)
			{
				Type parameterType = command.MethodParamData[i].ParameterType;
				if (!_parser.CanParse(parameterType) && !parameterType.IsGenericParameter)
				{
					unsupportedReason = $"Parameter type {parameterType} is not supported by the Quantum Parser.";
					return false;
				}
			}
			if (command.MonoTarget != MonoTargetType.Registry && !command.MethodData.IsStatic && !command.MethodData.DeclaringType.IsDerivedTypeOf(typeof(MonoBehaviour)))
			{
				unsupportedReason = $"Non static non MonoBehaviour commands are incompatible with MonoTargetType.{command.MonoTarget}.";
				return false;
			}
			unsupportedReason = string.Empty;
			return true;
		}

		private static void LoadCommandsFromAssembly(Assembly assembly)
		{
			if (!_scanRuleset.ShouldScan(assembly))
			{
				return;
			}
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				try
				{
					LoadCommandsFromType(type);
				}
				catch (TypeLoadException)
				{
				}
				catch (BadImageFormatException)
				{
				}
			}
		}

		private static void LoadCommandsFromType(Type type)
		{
			if (!_scanRuleset.ShouldScan(type))
			{
				return;
			}
			foreach (var (method, memberInfo) in ExtractCommandMethods(type))
			{
				if (memberInfo.DeclaringType == type)
				{
					LoadCommandsFromMember(memberInfo, method);
				}
			}
		}

		private static void LoadCommandsFromMember(MemberInfo member, MethodInfo method)
		{
			if (!_scanRuleset.ShouldScan(member))
			{
				return;
			}
			IEnumerable<CommandAttribute> customAttributes = member.GetCustomAttributes<CommandAttribute>();
			CommandDescriptionAttribute customAttribute = member.GetCustomAttribute<CommandDescriptionAttribute>();
			foreach (CommandAttribute item in customAttributes)
			{
				if (!item.Valid)
				{
					if (loggingLevel >= LoggingLevel.Warnings)
					{
						Debug.LogWarning("Quantum Processor Warning: Could not add '" + item.Alias + "' to the table as it is invalid.");
					}
					continue;
				}
				Platform platform = member.GetCustomAttribute<CommandPlatformAttribute>()?.SupportedPlatforms ?? item.SupportedPlatforms;
				if (!platform.HasFlag(Application.platform.ToPlatform()))
				{
					continue;
				}
				foreach (CommandData item2 in CreateCommandOverloads(method, item, customAttribute))
				{
					TryAddCommand(item2);
				}
			}
		}

		private static IEnumerable<CommandData> CreateCommandOverloads(MethodInfo method, CommandAttribute commandAttribute, CommandDescriptionAttribute descriptionAttribute)
		{
			int defaultParameters = method.GetParameters().Count((ParameterInfo x) => x.HasDefaultValue);
			for (int i = 0; i < defaultParameters + 1; i++)
			{
				yield return new CommandData(method, commandAttribute, descriptionAttribute, i);
			}
		}

		private static string GenerateCommandKey(CommandData command)
		{
			string commandName = command.CommandName;
			if (!command.HasParamsArgument)
			{
				return $"{commandName}({command.ParamCount})";
			}
			return commandName + "(#)";
		}

		public static bool TryAddCommand(CommandData command)
		{
			if (!GetCommandSupported(command, out var unsupportedReason))
			{
				if (loggingLevel >= LoggingLevel.Warnings)
				{
					Debug.LogWarning("Quantum Processor Warning: Could not add '" + command.CommandSignature + "' from " + command.MethodData.DeclaringType.GetDisplayName() + " to the table as it is not supported. " + unsupportedReason);
				}
				return false;
			}
			string text = GenerateCommandKey(command);
			if (!_commandTable.TryAdd(text, command))
			{
				if (loggingLevel >= LoggingLevel.Warnings)
				{
					string text2 = command.MethodData.DeclaringType.FullName + "." + command.MethodData.Name;
					Debug.LogWarning("Quantum Processor Warning: Could not add " + text2 + " to the table as another method with the same alias and parameter count, " + text + ", already exists.");
				}
				return false;
			}
			_commandCacheDirty = true;
			Interlocked.Increment(ref _loadedCommandCount);
			return true;
		}

		public static bool TryRemoveCommand(CommandData command)
		{
			string key = GenerateCommandKey(command);
			if (_commandTable.TryRemove(key, out var _))
			{
				_commandCacheDirty = true;
				Interlocked.Decrement(ref _loadedCommandCount);
				return true;
			}
			return false;
		}

		public static object InvokeCommand(string commandString)
		{
			GenerateCommandTable();
			commandString = commandString.Trim();
			commandString = _preprocessor.Process(commandString);
			if (string.IsNullOrWhiteSpace(commandString))
			{
				throw new ArgumentException("Cannot parse an empty string.");
			}
			string[] source = commandString.SplitScoped(' ');
			source = source.Where((string x) => !string.IsNullOrWhiteSpace(x)).ToArray();
			string obj = source[0];
			string[] array = source.SubArray(1, source.Length - 1);
			int paramCount = array.Length;
			string[] array2 = obj.Split(new char[1] { '<' }, 2);
			string genericSignature = ((array2.Length > 1) ? ("<" + array2[1]) : "");
			CommandData commandData = FindCompatibleCommand(array2[0], paramCount);
			Type[] genericTypeArguments = ParseGenericTypes(commandData, genericSignature);
			if (commandData.HasParamsArgument)
			{
				int num = commandData.ParamCount - 1;
				IEnumerable<string> values = array.Skip(num);
				string text = string.Join(",", values);
				string[] array3 = new string[commandData.ParamCount];
				Array.Copy(array, array3, commandData.ParamCount - 1);
				array3[num] = text;
				array = array3;
			}
			object[] paramData = ParseParamData(commandData.MakeGenericArguments(genericTypeArguments), array);
			return commandData.Invoke(paramData, genericTypeArguments);
		}

		private static CommandData FindCompatibleCommand(string commandName, int paramCount)
		{
			string key = $"{commandName}({paramCount})";
			if (_commandTable.TryGetValue(key, out var value))
			{
				return value;
			}
			string key2 = commandName + "(#)";
			if (_commandTable.TryGetValue(key2, out value))
			{
				int num = value.ParamCount - 1;
				if (paramCount >= num)
				{
					return value;
				}
			}
			throw _commandCache.Any((CommandData command) => command.CommandName == commandName) ? new ArgumentException($"No overload of '{commandName}' with {paramCount} parameters could be found.") : new ArgumentException("Command '" + commandName + "' could not be found.");
		}

		private static Type[] ParseGenericTypes(CommandData command, string genericSignature)
		{
			if (!command.IsGeneric)
			{
				if (genericSignature != string.Empty)
				{
					throw new ArgumentException("Command '" + command.CommandName + "' is not a generic command and cannot be invoked as such.");
				}
				return Array.Empty<Type>();
			}
			int num = command.GenericParamTypes.Length;
			string[] array = genericSignature.ReduceScope('<', '>').SplitScoped(',');
			if (array.Length != num)
			{
				throw new ArgumentException(string.Format("Generic command '{0}' requires {1} generic parameter{2} but was supplied with {3}.", command.CommandName, num, (num == 1) ? "" : "s", array.Length));
			}
			Type[] array2 = new Type[array.Length];
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = QuantumParser.ParseType(array[i]);
			}
			return array2;
		}

		private static object[] ParseParamData(Type[] paramTypes, string[] paramData)
		{
			object[] array = new object[paramData.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = _parser.Parse(paramData[i], paramTypes[i]);
			}
			return array;
		}
	}
}
