using System;
using System.Collections.Generic;
using System.Linq;
using QFSW.QC.Utilities;
using UnityEngine;

namespace QFSW.QC
{
	public static class QuantumRegistry
	{
		private static readonly Dictionary<Type, List<object>> _objectRegistry = new Dictionary<Type, List<object>>();

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetRegistry()
		{
			_objectRegistry.Clear();
		}

		private static bool IsNull(object x)
		{
			if (x is UnityEngine.Object obj)
			{
				return !obj;
			}
			return x == null;
		}

		[Command("register-object", "Adds the object to the registry to be used by commands with MonoTargetType = Registry", Platform.AllPlatforms, MonoTargetType.Single)]
		public static void RegisterObject<T>(T obj) where T : class
		{
			RegisterObject(typeof(T), obj);
		}

		public static void RegisterObject(Type type, object obj)
		{
			if (!type.IsClass)
			{
				throw new Exception("Registry may only contain class types");
			}
			lock (_objectRegistry)
			{
				if (_objectRegistry.ContainsKey(type))
				{
					if (_objectRegistry[type].Contains(obj))
					{
						throw new ArgumentException($"Could not register object '{obj}' of type {type.GetDisplayName()} as it was already registered.");
					}
					_objectRegistry[type].Add(obj);
				}
				else
				{
					_objectRegistry.Add(type, new List<object> { obj });
				}
			}
		}

		[Command("deregister-object", "Removes the object to the registry to be used by commands with MonoTargetType = Registry", Platform.AllPlatforms, MonoTargetType.Single)]
		public static void DeregisterObject<T>(T obj) where T : class
		{
			DeregisterObject(typeof(T), obj);
		}

		public static void DeregisterObject(Type type, object obj)
		{
			if (!type.IsClass)
			{
				throw new Exception("Registry may only contain class types");
			}
			lock (_objectRegistry)
			{
				if (_objectRegistry.ContainsKey(type) && _objectRegistry[type].Contains(obj))
				{
					_objectRegistry[type].Remove(obj);
					return;
				}
				throw new ArgumentException($"Could not deregister object '{obj}' of type {type.GetDisplayName()} as it was not found in the registry.");
			}
		}

		public static int GetRegistrySize<T>() where T : class
		{
			return GetRegistrySize(typeof(T));
		}

		public static int GetRegistrySize(Type type)
		{
			return GetRegistryContents(type).Count();
		}

		public static IEnumerable<T> GetRegistryContents<T>() where T : class
		{
			foreach (object registryContent in GetRegistryContents(typeof(T)))
			{
				yield return (T)registryContent;
			}
		}

		public static IEnumerable<object> GetRegistryContents(Type type)
		{
			if (!type.IsClass)
			{
				throw new Exception("Registry may only contain class types");
			}
			lock (_objectRegistry)
			{
				if (_objectRegistry.ContainsKey(type))
				{
					List<object> list = _objectRegistry[type];
					list.RemoveAll(IsNull);
					return list;
				}
				return Enumerable.Empty<object>();
			}
		}

		[Command("clear-registry", "Clears the contents of the specified registry", Platform.AllPlatforms, MonoTargetType.Single)]
		public static void ClearRegistryContents<T>() where T : class
		{
			ClearRegistryContents(typeof(T));
		}

		public static void ClearRegistryContents(Type type)
		{
			if (!type.IsClass)
			{
				throw new Exception("Registry may only contain class types");
			}
			lock (_objectRegistry)
			{
				if (_objectRegistry.ContainsKey(type))
				{
					_objectRegistry[type].Clear();
				}
			}
		}

		[Command("display-registry", "Displays the contents of the specified registry", Platform.AllPlatforms, MonoTargetType.Single)]
		private static IEnumerable<object> DisplayRegistry<T>() where T : class
		{
			if (GetRegistrySize<T>() <= 0)
			{
				return ("The registry '" + typeof(T).GetDisplayName() + "' is empty").Yield();
			}
			return GetRegistryContents<T>();
		}
	}
}
