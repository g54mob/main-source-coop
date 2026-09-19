using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Zenject;

namespace RSG.Muffin.ApplicationFilesModule.Core.Scripts
{
	public class ApplicationFilesHolder : IApplicationFilesHolder, IInitializable
	{
		private readonly List<Type> _allEnumTypes = new List<Type>();

		private List<Type> _allTypes = new List<Type>();

		public void Initialize()
		{
			InitializeAllEnums();
		}

		private void InitializeAllEnums()
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				_allTypes.AddRange(assembly.GetTypes());
			}
			_allEnumTypes.AddRange(_allTypes.Where((Type t) => t.IsEnum));
		}

		public bool TryGetEnumByName(string enumName, out Type type)
		{
			type = null;
			foreach (Type allEnumType in _allEnumTypes)
			{
				if (allEnumType.Name == enumName)
				{
					type = allEnumType;
					return true;
				}
			}
			return false;
		}

		public Type GetEnumByName(string enumName)
		{
			foreach (Type allEnumType in _allEnumTypes)
			{
				if (allEnumType.Name == enumName)
				{
					return allEnumType;
				}
			}
			throw new MissingEnumException(enumName);
		}

		public bool TryGetTypeByName(string typeName, out Type type)
		{
			type = null;
			foreach (Type allType in _allTypes)
			{
				if (allType.Name == typeName)
				{
					type = allType;
					return true;
				}
			}
			return false;
		}

		public Type GetTypeByName(string typeName)
		{
			foreach (Type allType in _allTypes)
			{
				if (allType.Name == typeName)
				{
					return allType;
				}
			}
			throw new MissingTypeException(typeName);
		}

		public Type GetFirstOrDefaultType(Func<Type, bool> predicate)
		{
			return _allTypes.FirstOrDefault(predicate);
		}

		public List<Type> GetAssignableFrom(Type type)
		{
			return _allTypes.Where((Type t) => type.IsAssignableFrom(t) && type != t).ToList();
		}
	}
}
