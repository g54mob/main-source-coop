using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using QFSW.QC.Comparators;
using QFSW.QC.Utilities;
using UnityEngine;

namespace QFSW.QC
{
	public static class InvocationTargetFactory
	{
		private static readonly Dictionary<(MonoTargetType, Type), object> TargetCache = new Dictionary<(MonoTargetType, Type), object>();

		public static IEnumerable<T> FindTargets<T>(MonoTargetType method) where T : MonoBehaviour
		{
			foreach (object item in FindTargets(typeof(T), method))
			{
				yield return item as T;
			}
		}

		public static IEnumerable<object> FindTargets(Type classType, MonoTargetType method)
		{
			switch (method)
			{
			case MonoTargetType.Single:
			{
				UnityEngine.Object obj = UnityEngine.Object.FindFirstObjectByType(classType);
				if (!(obj == null))
				{
					return obj.Yield();
				}
				return Enumerable.Empty<object>();
			}
			case MonoTargetType.SingleInactive:
				return WrapSingleCached(classType, method, (Type type) => Resources.FindObjectsOfTypeAll(type).FirstOrDefault((UnityEngine.Object x) => !x.hideFlags.HasFlag(HideFlags.HideInHierarchy)));
			case MonoTargetType.All:
				return UnityEngine.Object.FindObjectsByType(classType, FindObjectsSortMode.None).OrderBy((UnityEngine.Object x) => x.name, new AlphanumComparator());
			case MonoTargetType.AllInactive:
				return (from x in Resources.FindObjectsOfTypeAll(classType)
					where !x.hideFlags.HasFlag(HideFlags.HideInHierarchy)
					select x).OrderBy((UnityEngine.Object x) => x.name, new AlphanumComparator());
			case MonoTargetType.Registry:
				return QuantumRegistry.GetRegistryContents(classType);
			case MonoTargetType.Singleton:
				return GetSingletonInstance(classType).Yield();
			default:
				throw new ArgumentException($"Unsupported MonoTargetType {method}");
			}
		}

		private static IEnumerable<object> WrapSingleCached(Type classType, MonoTargetType method, Func<Type, object> targetFinder)
		{
			if (!TargetCache.TryGetValue((method, classType), out var value) || value as UnityEngine.Object == null)
			{
				value = targetFinder(classType);
				TargetCache[(method, classType)] = value;
			}
			if (value != null)
			{
				return value.Yield();
			}
			return Enumerable.Empty<object>();
		}

		public static object InvokeOnTargets(MethodInfo invokingMethod, IEnumerable<object> targets, object[] arguments)
		{
			int num = 0;
			int num2 = 0;
			Dictionary<object, object> dictionary = new Dictionary<object, object>();
			foreach (object target in targets)
			{
				num2++;
				object obj = invokingMethod.Invoke(target, arguments);
				if (obj != null)
				{
					dictionary.Add(target, obj);
					num++;
				}
			}
			if (num > 1)
			{
				return dictionary;
			}
			if (num == 1)
			{
				return dictionary.Values.First();
			}
			if (num2 == 0)
			{
				string displayName = invokingMethod.DeclaringType.GetDisplayName();
				throw new Exception("Could not invoke the command because no objects of type " + displayName + " could be found.");
			}
			return null;
		}

		private static string FormatInvocationMessage(int invocationCount, object lastTarget = null)
		{
			switch (invocationCount)
			{
			case 0:
				throw new Exception("No targets could be found");
			case 1:
			{
				string text = ((!(lastTarget is UnityEngine.Object obj)) ? lastTarget?.ToString() : obj.name);
				return "> Invoked on " + text;
			}
			default:
				return $"> Invoked on {invocationCount} targets";
			}
		}

		private static object GetSingletonInstance(Type classType)
		{
			if (QuantumRegistry.GetRegistrySize(classType) > 0)
			{
				return QuantumRegistry.GetRegistryContents(classType).First();
			}
			object obj = CreateCommandSingletonInstance(classType);
			QuantumRegistry.RegisterObject(classType, obj);
			return obj;
		}

		private static Component CreateCommandSingletonInstance(Type classType)
		{
			GameObject gameObject = new GameObject($"{classType}Singleton");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			return gameObject.AddComponent(classType);
		}
	}
}
