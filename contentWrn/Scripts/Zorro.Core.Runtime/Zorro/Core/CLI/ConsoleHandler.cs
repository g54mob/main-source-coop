using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Zorro.Core.CLI
{
	public static class ConsoleHandler
	{
		private static ConsoleCommand[] m_consoleCommands;

		private static bool m_loaded = false;

		private static List<string> m_typedHistory = new List<string>();

		public const string TEXT_COLOR_HEX = "#cccaca";

		public const string HIGHLIGHTED_COLOR_HEX = "#ffffff";

		public const string ERROR_COLOR_HEX = "#fc5347";

		public const string TEXT_COLOR = "<color=#cccaca>";

		public const string HIGHLIGHTED_COLOR = "<color=#ffffff>";

		public const string ERROR_COLOR = "<color=#fc5347>";

		private static Dictionary<Type, CLITypeParser> m_typeParsers = new Dictionary<Type, CLITypeParser>();

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		public static void Initialize()
		{
			if (!m_loaded)
			{
				m_loaded = true;
				(MethodInfo, Attribute)[] methodsWithAttribute = ReflectionUtility.GetMethodsWithAttribute<ConsoleCommandAttribute>();
				(Type, TypeParserAttribute)[] classesWithAttribute = ReflectionUtility.GetClassesWithAttribute<TypeParserAttribute>();
				for (int i = 0; i < classesWithAttribute.Length; i++)
				{
					var (type, typeParserAttribute) = classesWithAttribute[i];
					m_typeParsers.Add(typeParserAttribute.ParseType, (CLITypeParser)Activator.CreateInstance(type));
					Debug.Log("Initialized Type Parser for " + typeParserAttribute.ParseType);
				}
				UnityEngine.Object.DontDestroyOnLoad(UnityEngine.Object.Instantiate(SingletonAsset<CoreGlobalDependencies>.Instance.ConsolePrefab, Vector3.zero, Quaternion.identity));
				ConsoleCommand[] array = new ConsoleCommand[methodsWithAttribute.Length];
				for (int j = 0; j < array.Length; j++)
				{
					array[j] = new ConsoleCommand(methodsWithAttribute[j].Item1);
				}
				m_consoleCommands = array;
			}
		}

		public static bool ProcessCommand(string command)
		{
			if (command == "Help")
			{
				Debug.Log("Available Commands:");
				ConsoleCommand[] consoleCommands = m_consoleCommands;
				for (int i = 0; i < consoleCommands.Length; i++)
				{
					ConsoleCommand consoleCommand = consoleCommands[i];
					Debug.Log(consoleCommand.DomainName + "." + consoleCommand.Command);
				}
				return true;
			}
			string[] array = StringUtility.SplitOnFirstOfChar(command, '.');
			if (array != null)
			{
				string text = array[0];
				string text2 = array[1].TrimEnd();
				List<string> list = new List<string>();
				if (text2.Contains(' '))
				{
					string[] array2 = text2.Split(' ');
					text2 = array2.First();
					for (int j = 1; j < array2.Length; j++)
					{
						list.Add(array2[j]);
					}
				}
				ConsoleCommand[] consoleCommands = m_consoleCommands;
				for (int i = 0; i < consoleCommands.Length; i++)
				{
					ConsoleCommand consoleCommand2 = consoleCommands[i];
					if (consoleCommand2.DomainName == text && text2 == consoleCommand2.Command)
					{
						object[] array3 = ((list.Count > 0) ? new object[list.Count] : Array.Empty<object>());
						for (int k = 0; k < list.Count; k++)
						{
							array3[k] = ConvertParameter(list[k], consoleCommand2.ParameterInfo[k].ParameterType);
						}
						if (consoleCommand2.ParameterInfo.Length != array3.Length)
						{
							Debug.LogError($"{consoleCommand2.DomainName}.{consoleCommand2.Command} expected {consoleCommand2.ParameterInfo.Length} parameters, but {array3.Length} were provided.");
							return true;
						}
						try
						{
							consoleCommand2.MethodInfo.Invoke(null, array3);
						}
						catch (Exception ex)
						{
							Debug.LogError("Failed to execute command: " + ex);
						}
						return true;
					}
				}
			}
			return false;
		}

		private static object ConvertParameter(string parameter, Type type)
		{
			if (type == typeof(string))
			{
				return parameter;
			}
			if (m_typeParsers.ContainsKey(type))
			{
				return m_typeParsers[type].Parse(parameter);
			}
			return new Exception($"Invalid Parameter Type: {type}");
		}

		public static List<Suggestion> FindSuggestions(string input)
		{
			List<Suggestion> list = FindCommandSuggestions(input);
			AddParameterSuggestions(list);
			return list;
			void AddParameterSuggestions(List<Suggestion> suggestions)
			{
				if (HasTypedOneFullCommand() && suggestions[0] is CommandSuggestion commandSuggestion && input.Contains(commandSuggestion.FullCommand) && StringUtility.MakeSureNoDoublleChar(input, ' '))
				{
					string[] array = input.Split(' ');
					int num = array.Length - 2;
					if (num >= 0 && commandSuggestion.ParameterInfos.Length > num)
					{
						commandSuggestion.HighlightParameter(num);
						ParameterInfo parameterInfo = commandSuggestion.ParameterInfos[num];
						string parameterText = "";
						if (!input.EndsWith(' '))
						{
							parameterText = array.Last();
						}
						if (m_typeParsers.ContainsKey(parameterInfo.ParameterType))
						{
							List<ParameterAutocomplete> list2 = m_typeParsers[parameterInfo.ParameterType].FindAutocomplete(parameterText);
							string displayTextWithMaxParameter = commandSuggestion.GetDisplayTextWithMaxParameter(num, color: false);
							string text = "";
							for (int i = 0; i < array.Length - 1; i++)
							{
								text = text + array[i] + " ";
							}
							foreach (ParameterAutocomplete item2 in list2)
							{
								suggestions.Add(new ParameterSuggestion(displayTextWithMaxParameter, text, item2.Value));
							}
						}
					}
				}
				bool HasTypedOneFullCommand()
				{
					List<CommandSuggestion> list3 = new List<CommandSuggestion>();
					foreach (Suggestion suggestion in suggestions)
					{
						if (suggestion is CommandSuggestion commandSuggestion2 && input.Contains(commandSuggestion2.FullCommand))
						{
							list3.Add(commandSuggestion2);
						}
					}
					return list3.Count == 1;
				}
			}
			static List<Suggestion> FindCommandSuggestions(string text)
			{
				if (string.IsNullOrEmpty(text))
				{
					return new List<Suggestion>();
				}
				if (text.Contains('.'))
				{
					string[] array = StringUtility.SplitOnFirstOfChar(text, '.');
					List<DomainSuggestion> list2 = FindMatchingDomains(array[0]);
					string text2 = array[1].TrimEnd();
					if (text2.Contains(' '))
					{
						text2 = text2.Split(' ').First();
					}
					if (list2.Count == 1)
					{
						DomainSuggestion selectedDomain = list2.First();
						HashSet<ConsoleCommand> hashSet = new HashSet<ConsoleCommand>();
						ConsoleCommand[] consoleCommands = m_consoleCommands;
						for (int i = 0; i < consoleCommands.Length; i++)
						{
							ConsoleCommand item = consoleCommands[i];
							if (item.DomainName.Equals(selectedDomain.Domain) && item.Command.ToLower().Contains(text2.ToLower()))
							{
								hashSet.Add(item);
							}
						}
						return hashSet.Select((ConsoleCommand s) => new CommandSuggestion(selectedDomain.Domain, s.Command, s.ParameterInfo)).Cast<Suggestion>().ToList();
					}
				}
				return FindMatchingDomains(text).Cast<Suggestion>().ToList();
			}
			static List<DomainSuggestion> FindMatchingDomains(string domain)
			{
				domain = domain.ToLower();
				HashSet<string> hashSet = new HashSet<string>();
				ConsoleCommand[] consoleCommands = m_consoleCommands;
				for (int i = 0; i < consoleCommands.Length; i++)
				{
					ConsoleCommand consoleCommand = consoleCommands[i];
					if (consoleCommand.DomainName.ToLower().Contains(domain))
					{
						hashSet.Add(consoleCommand.DomainName);
					}
				}
				return hashSet.Select((string s) => new DomainSuggestion(s)).ToList();
			}
		}

		public static void AddToHistory(string command)
		{
			m_typedHistory.Add(command);
		}

		public static List<string> GetHistory()
		{
			return m_typedHistory;
		}
	}
}
