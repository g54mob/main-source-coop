using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QFSW.QC
{
	public static class QuantumMacros
	{
		private class MacroPreprocessor : IQcPreprocessor
		{
			public int Priority => 1000;

			public string Process(string text)
			{
				if (!text.StartsWith("#define", StringComparison.CurrentCulture))
				{
					text = ExpandMacros(text);
				}
				return text;
			}
		}

		private static readonly Dictionary<string, string> _macroTable = new Dictionary<string, string>();

		public static IReadOnlyDictionary<string, string> GetMacros()
		{
			return _macroTable;
		}

		public static string ExpandMacros(string text, int maximumExpansions = 1000)
		{
			if (_macroTable.Count == 0)
			{
				return text;
			}
			KeyValuePair<string, string>[] array = null;
			int num = 0;
			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] != '#')
				{
					continue;
				}
				if (array == null)
				{
					array = _macroTable.OrderByDescending((KeyValuePair<string, string> x) => x.Key.Length).ToArray();
				}
				KeyValuePair<string, string>[] array2 = array;
				for (int num2 = 0; num2 < array2.Length; num2++)
				{
					KeyValuePair<string, string> keyValuePair = array2[num2];
					string key = keyValuePair.Key;
					int length = key.Length;
					if (i + length < text.Length && string.CompareOrdinal(text, i + 1, key, 0, length) == 0)
					{
						if (num >= maximumExpansions)
						{
							throw new ArgumentException($"Maximum macro expansions of {maximumExpansions} was exhausted: infinitely recursive macro is likely.");
						}
						text = string.Concat(text.Substring(0, i), str2: text.Substring(i + 1 + length), str1: keyValuePair.Value);
						num++;
						i--;
					}
				}
			}
			return text;
		}

		[Command("#define", Platform.AllPlatforms, MonoTargetType.Single)]
		[CommandDescription("Adds a macro to the macro table which can then be used in the Quantum Console. If the macro 'name' is added, then all instances of '#name' will be expanded into the full macro expansion. This allows you to define shortcuts for various things such as long type names or commonly used command strings.\n\nMacros may not contain hashtags or whitespace in their name.\n\nNote: macros will not be expanded when using #define, this is so that defining nested macros is possible.")]
		public static void DefineMacro(string macroName, string macroExpansion)
		{
			macroName = macroName.Trim();
			if (macroName.Contains(' '))
			{
				throw new ArgumentException("Macro names cannot contain whitespace.");
			}
			if (macroName.Contains('\n'))
			{
				throw new ArgumentException("Macro names cannot contain newlines.");
			}
			if (macroName.Contains('#'))
			{
				throw new ArgumentException("Macro names cannot contain hashtags.");
			}
			if (macroName == "define")
			{
				throw new ArgumentException("Macros cannot be named define.");
			}
			if (macroExpansion.Contains('\n'))
			{
				throw new ArgumentException("Macro names cannot contain newlines.");
			}
			if (macroExpansion.Contains("#" + macroName))
			{
				throw new ArgumentException("Macros cannot contain themselves within the expansion.");
			}
			if (_macroTable.ContainsKey(macroName))
			{
				_macroTable[macroName] = macroExpansion;
			}
			else
			{
				_macroTable.Add(macroName, macroExpansion);
			}
		}

		[Command("remove-macro", Platform.AllPlatforms, MonoTargetType.Single)]
		[CommandDescription("Removes the specified macro from the macro table")]
		public static void RemoveMacro(string macroName)
		{
			if (_macroTable.ContainsKey(macroName))
			{
				_macroTable.Remove(macroName);
				return;
			}
			throw new Exception("Specified macro #" + macroName + " as it was not defined.");
		}

		[Command("clear-macros", Platform.AllPlatforms, MonoTargetType.Single)]
		[CommandDescription("Clears the macro table")]
		public static void ClearMacros()
		{
			_macroTable.Clear();
		}

		[Command("all-macros", "Displays all of the macros currently stored in the macro table", Platform.AllPlatforms, MonoTargetType.Single)]
		private static string GetAllMacros()
		{
			if (_macroTable.Count == 0)
			{
				return "Macro table is empty";
			}
			return "Macro table:\n" + string.Join("\n", _macroTable.Select((KeyValuePair<string, string> x) => "#" + x.Key + " = " + x.Value));
		}

		[Command("dump-macros", "Creates a file dump of macro table which can the be loaded to repopulate the table using load-macros", Platform.AllPlatforms, MonoTargetType.Single)]
		[CommandPlatform(~Platform.WebGLPlayer)]
		public static void DumpMacrosToFile(string filePath)
		{
			using StreamWriter streamWriter = new StreamWriter(filePath);
			foreach (KeyValuePair<string, string> item in _macroTable)
			{
				streamWriter.WriteLine(item.Key + " " + item.Value);
			}
			streamWriter.Flush();
			streamWriter.Close();
		}

		[Command("load-macros", "Loads macros from an external file into the macro table", Platform.AllPlatforms, MonoTargetType.Single)]
		[CommandPlatform(~Platform.WebGLPlayer)]
		public static string LoadMacrosFromFile(string filePath)
		{
			if (!File.Exists(filePath))
			{
				throw new ArgumentException("file at the specified path '" + filePath + "' did not exist.");
			}
			using StreamReader streamReader = new StreamReader(filePath);
			List<string> list = new List<string>();
			while (!streamReader.EndOfStream)
			{
				string text = streamReader.ReadLine();
				string[] array = text.Split(" ".ToCharArray(), 2);
				if (array.Length != 2)
				{
					list.Add("'" + text + "' is not a valid macro definition");
				}
				try
				{
					DefineMacro(array[0], array[1]);
					list.Add("#" + array[0] + " was successfully defined");
				}
				catch (Exception ex)
				{
					list.Add("#" + array[0] + " could not be defined: " + ex.Message);
				}
			}
			streamReader.Close();
			return string.Join("\n", list);
		}
	}
}
