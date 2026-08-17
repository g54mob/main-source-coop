using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using QFSW.QC.Utilities;
using UnityEngine;

namespace QFSW.QC
{
	public class QuantumParser
	{
		private readonly IQcParser[] _parsers;

		private readonly IQcGrammarConstruct[] _grammarConstructs;

		private readonly ConcurrentDictionary<Type, IQcParser> _parserLookup = new ConcurrentDictionary<Type, IQcParser>();

		private readonly HashSet<Type> _unparseableLookup = new HashSet<Type>();

		private readonly Func<string, Type, object> _recursiveParser;

		private static readonly Dictionary<Type, string> _typeDisplayNames = new Dictionary<Type, string>
		{
			{
				typeof(int),
				"int"
			},
			{
				typeof(float),
				"float"
			},
			{
				typeof(decimal),
				"decimal"
			},
			{
				typeof(double),
				"double"
			},
			{
				typeof(string),
				"string"
			},
			{
				typeof(bool),
				"bool"
			},
			{
				typeof(byte),
				"byte"
			},
			{
				typeof(sbyte),
				"sbyte"
			},
			{
				typeof(uint),
				"uint"
			},
			{
				typeof(short),
				"short"
			},
			{
				typeof(ushort),
				"ushort"
			},
			{
				typeof(long),
				"long"
			},
			{
				typeof(ulong),
				"ulong"
			},
			{
				typeof(char),
				"char"
			},
			{
				typeof(object),
				"object"
			}
		};

		private static readonly Dictionary<string, Type> _reverseTypeDisplayNames = _typeDisplayNames.Invert();

		private static readonly Assembly[] _loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

		private static readonly string[] _defaultNamespaces = new string[6] { "System", "System.Collections", "System.Collections.Generic", "UnityEngine", "UnityEngine.UI", "QFSW.QC" };

		private static readonly List<string> _namespaceTable = new List<string>(_defaultNamespaces);

		private static readonly Regex _arrayTypeRegex = new Regex("^.*\\[,*\\]$");

		private static readonly Regex _genericTypeRegex = new Regex("^.+<.*>$");

		private static readonly Regex _tupleTypeRegex = new Regex("^\\(.*\\)$");

		private static readonly Regex _nullableTypeRegex = new Regex("^.*\\?$");

		private static readonly Type[] _valueTupleTypes = new Type[8]
		{
			typeof(ValueTuple<>),
			typeof(ValueTuple<, >),
			typeof(ValueTuple<, , >),
			typeof(ValueTuple<, , , >),
			typeof(ValueTuple<, , , , >),
			typeof(ValueTuple<, , , , , >),
			typeof(ValueTuple<, , , , , , >),
			typeof(ValueTuple<, , , , , , , >)
		};

		public QuantumParser(IEnumerable<IQcParser> parsers, IEnumerable<IQcGrammarConstruct> grammarConstructs)
		{
			_recursiveParser = Parse;
			_parsers = parsers.OrderByDescending((IQcParser x) => x.Priority).ToArray();
			_grammarConstructs = grammarConstructs.OrderBy((IQcGrammarConstruct x) => x.Precedence).ToArray();
		}

		public QuantumParser()
			: this(new InjectionLoader<IQcParser>().GetInjectedInstances(), new InjectionLoader<IQcGrammarConstruct>().GetInjectedInstances())
		{
		}

		public IQcParser GetParser(Type type)
		{
			if (_parserLookup.ContainsKey(type))
			{
				return _parserLookup[type];
			}
			if (!_unparseableLookup.Contains(type))
			{
				IQcParser[] parsers = _parsers;
				foreach (IQcParser qcParser in parsers)
				{
					try
					{
						if (qcParser.CanParse(type))
						{
							return _parserLookup[type] = qcParser;
						}
					}
					catch (Exception exception)
					{
						Debug.LogError(qcParser.GetType().GetDisplayName() + ".CanParse is malformed and throws");
						Debug.LogException(exception);
					}
				}
				_unparseableLookup.Add(type);
			}
			return null;
		}

		public bool CanParse(Type type)
		{
			return GetParser(type) != null;
		}

		private IQcGrammarConstruct GetMatchingGrammar(string value, Type type)
		{
			IQcGrammarConstruct[] grammarConstructs = _grammarConstructs;
			foreach (IQcGrammarConstruct qcGrammarConstruct in grammarConstructs)
			{
				try
				{
					if (qcGrammarConstruct.Match(value, type))
					{
						return qcGrammarConstruct;
					}
				}
				catch (Exception exception)
				{
					Debug.LogError(qcGrammarConstruct.GetType().GetDisplayName() + ".Match is malformed and throws");
					Debug.LogException(exception);
				}
			}
			return null;
		}

		public T Parse<T>(string value)
		{
			return (T)Parse(value, typeof(T));
		}

		public object Parse(string value, Type type)
		{
			value = value.ReduceScope('(', ')');
			if (type.IsClass && value == "null")
			{
				return null;
			}
			IQcGrammarConstruct matchingGrammar = GetMatchingGrammar(value, type);
			if (matchingGrammar != null)
			{
				try
				{
					return matchingGrammar.Parse(value, type, _recursiveParser);
				}
				catch (ParserException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					throw new Exception($"Parsing of {type.GetDisplayName()} via {matchingGrammar} failed:\n{ex2.Message}", ex2);
				}
			}
			IQcParser parser = GetParser(type);
			if (parser == null)
			{
				throw new ArgumentException("Cannot parse object of type '" + type.GetDisplayName() + "'");
			}
			try
			{
				return parser.Parse(value, type, _recursiveParser);
			}
			catch (ParserException)
			{
				throw;
			}
			catch (Exception ex4)
			{
				throw new Exception($"Parsing of {type.GetDisplayName()} via {parser} failed:\n{ex4.Message}", ex4);
			}
		}

		[Command("reset-namespaces", "Resets the namespace table to its initial state", Platform.AllPlatforms, MonoTargetType.Single)]
		public static void ResetNamespaceTable()
		{
			_namespaceTable.Clear();
			_namespaceTable.AddRange(_defaultNamespaces);
		}

		[Command("use-namespace", "Adds a namespace to the table so that it can be used to type resolution", Platform.AllPlatforms, MonoTargetType.Single)]
		public static void AddNamespace(string namespaceName)
		{
			if (!_namespaceTable.Contains(namespaceName))
			{
				_namespaceTable.Add(namespaceName);
			}
		}

		[Command("remove-namespace", "Removes a namespace from the table", Platform.AllPlatforms, MonoTargetType.Single)]
		public static void RemoveNamespace(string namespaceName)
		{
			if (_namespaceTable.Contains(namespaceName))
			{
				_namespaceTable.Remove(namespaceName);
				return;
			}
			throw new ArgumentException("No namespace named " + namespaceName + " was present in the table");
		}

		[Command("all-namespaces", "Displays all of the namespaces currently in use by the namespace table", Platform.AllPlatforms, MonoTargetType.Single)]
		private static string ShowNamespaces()
		{
			_namespaceTable.Sort();
			if (_namespaceTable.Count == 0)
			{
				return "Namespace table is empty";
			}
			return string.Join("\n", _namespaceTable);
		}

		public static IEnumerable<string> GetAllNamespaces()
		{
			return _namespaceTable;
		}

		public static Type ParseType(string typeName)
		{
			typeName = typeName.Trim();
			if (_reverseTypeDisplayNames.ContainsKey(typeName))
			{
				return _reverseTypeDisplayNames[typeName];
			}
			if (_tupleTypeRegex.IsMatch(typeName))
			{
				return ParseTupleType(typeName);
			}
			if (_arrayTypeRegex.IsMatch(typeName))
			{
				return ParseArrayType(typeName);
			}
			if (_genericTypeRegex.IsMatch(typeName))
			{
				return ParseGenericType(typeName);
			}
			if (_nullableTypeRegex.IsMatch(typeName))
			{
				return ParseNullableType(typeName);
			}
			if (typeName.Contains('`'))
			{
				string key = typeName.Split('`')[0];
				if (_reverseTypeDisplayNames.ContainsKey(key))
				{
					return _reverseTypeDisplayNames[key];
				}
			}
			return ParseTypeBaseCase(typeName);
		}

		private static Type ParseArrayType(string typeName)
		{
			int num = typeName.LastIndexOf('[');
			int num2 = typeName.CountFromIndex(',', num) + 1;
			Type type = ParseType(typeName.Substring(0, num));
			if (num2 <= 1)
			{
				return type.MakeArrayType();
			}
			return type.MakeArrayType(num2);
		}

		private static Type ParseGenericType(string typeName)
		{
			string[] array = typeName.Split(new char[1] { '<' }, 2);
			string[] array2 = ("<" + array[1]).ReduceScope('<', '>').SplitScoped(',');
			Type type = ParseType($"{array[0]}`{Math.Max(1, array2.Length)}");
			if (array2.All(string.IsNullOrWhiteSpace))
			{
				return type;
			}
			Type[] typeArguments = array2.Select(ParseType).ToArray();
			return type.MakeGenericType(typeArguments);
		}

		private static Type ParseNullableType(string typeName)
		{
			Type type = ParseType(typeName.Substring(0, typeName.Length - 1));
			if (!type.IsClass)
			{
				return typeof(Nullable<>).MakeGenericType(type);
			}
			return type;
		}

		private static Type ParseTupleType(string typeName)
		{
			return CreateTupleType(typeName.Substring(1, typeName.Length - 2).SplitScoped(',').Select(ParseType)
				.ToArray());
		}

		private static Type CreateTupleType(Type[] types)
		{
			if (types.Length > 7)
			{
				Type[] types2 = types.Skip(7).ToArray();
				types = types.Take(7).Concat(CreateTupleType(types2).Yield()).ToArray();
			}
			return _valueTupleTypes[types.Length - 1].MakeGenericType(types);
		}

		private static Type ParseTypeBaseCase(string typeName)
		{
			return GetTypeFromAssemblies(typeName, _loadedAssemblies, throwOnError: false, ignoreCase: false) ?? GetTypeFromAssemblies(typeName, _namespaceTable, _loadedAssemblies, throwOnError: false, ignoreCase: false) ?? GetTypeFromAssemblies(typeName, _loadedAssemblies, throwOnError: false, ignoreCase: true) ?? GetTypeFromAssemblies(typeName, _namespaceTable, _loadedAssemblies, throwOnError: true, ignoreCase: true);
		}

		private static Type GetTypeFromAssemblies(string typeName, IEnumerable<string> namespaces, IEnumerable<Assembly> assemblies, bool throwOnError, bool ignoreCase)
		{
			foreach (string @namespace in namespaces)
			{
				Type typeFromAssemblies = GetTypeFromAssemblies(@namespace + "." + typeName, assemblies, throwOnError: false, ignoreCase);
				if (typeFromAssemblies != null)
				{
					return typeFromAssemblies;
				}
			}
			if (throwOnError)
			{
				throw new TypeLoadException("No type of name '" + typeName + "' could be found in the specified assemblies and namespaces.");
			}
			return null;
		}

		private static Type GetTypeFromAssemblies(string typeName, IEnumerable<Assembly> assemblies, bool throwOnError, bool ignoreCase)
		{
			foreach (Assembly assembly in assemblies)
			{
				Type type = Type.GetType(typeName + ", " + assembly.FullName, throwOnError: false, ignoreCase);
				if (type != null)
				{
					return type;
				}
			}
			if (throwOnError)
			{
				throw new TypeLoadException("No type of name '" + typeName + "' could be found in the specified assemblies.");
			}
			return null;
		}
	}
}
