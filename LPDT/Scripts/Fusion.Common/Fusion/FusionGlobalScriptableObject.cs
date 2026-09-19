#define DEBUG
#define TRACE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Fusion
{
	public abstract class FusionGlobalScriptableObject : FusionScriptableObject
	{
		private class TypeFullNameComparer : IEqualityComparer<Type>
		{
			public bool Equals(Type x, Type y)
			{
				if (x == y)
				{
					return true;
				}
				if (x == null)
				{
					return false;
				}
				return string.Equals(x.FullName, y.FullName, StringComparison.Ordinal);
			}

			public int GetHashCode(Type obj)
			{
				return (obj?.FullName?.GetHashCode()).GetValueOrDefault();
			}
		}

		private static readonly Lazy<FusionGlobalScriptableObjectSourceAttribute[]> s_sourceAttributes = new Lazy<FusionGlobalScriptableObjectSourceAttribute[]>(() => (from x in FusionPlatform.GetLoadedAssemblyAttributes<FusionGlobalScriptableObjectSourceAttribute>()
			orderby x.Order
			select x).ToArray());

		internal static readonly HashSet<Type> _typesFailedToLoad = new HashSet<Type>(new TypeFullNameComparer());

		internal static FusionGlobalScriptableObjectSourceAttribute[] SourceAttributes => s_sourceAttributes.Value;

		public static void ClearTypesFailedToLoad()
		{
			_typesFailedToLoad.Clear();
		}
	}
	public abstract class FusionGlobalScriptableObject<T> : FusionGlobalScriptableObject where T : FusionGlobalScriptableObject<T>
	{
		private static T s_instance;

		private static FusionGlobalScriptableObjectUnloadDelegate s_unloadHandler;

		public bool IsGlobal { get; private set; }

		private static string LogPrefix => "[Global " + typeof(T).Name + "]: ";

		protected static T GlobalInternal
		{
			get
			{
				T orLoadGlobalInstance = GetOrLoadGlobalInstance();
				if ((object)orLoadGlobalInstance == null)
				{
					throw new InvalidOperationException("Failed to load " + typeof(T).Name + ". If this happens in edit mode, make sure Fusion is properly installed in the Fusion HUB. Otherwise, if the default path does not exist or does not point to a Resource, you need to use FusionGlobalScriptableObjectAttribute attribute to point to a method that will perform the loading.");
				}
				return orLoadGlobalInstance;
			}
			set
			{
				if (!(value == s_instance))
				{
					SetGlobalInternal(value, null);
				}
			}
		}

		protected static bool IsGlobalLoadedInternal => s_instance != null;

		protected virtual void OnLoadedAsGlobal()
		{
		}

		protected virtual void OnUnloadedAsGlobal(bool destroyed)
		{
		}

		private static string AsId(FusionGlobalScriptableObject<T> obj)
		{
			return obj ? $"[HashCode:{obj.GetHashCode()}]" : "null";
		}

		protected virtual void OnDisable()
		{
			if (!IsGlobal)
			{
				InternalLogStreams.LogTrace?.Log(LogPrefix + "OnDisable called for " + AsId(this) + ", but is not global");
				return;
			}
			if (s_unloadHandler != null)
			{
				InternalLogStreams.LogTrace?.Log(LogPrefix + "OnDisable called for " + AsId(this) + ", setting global instance to null. The unload handler is still set, not going to be used.");
			}
			else
			{
				InternalLogStreams.LogTrace?.Log(LogPrefix + "OnDisable called for " + AsId(this) + ", setting global instance to null.");
			}
			Assert.Check((object)this == s_instance, "Expected this to be the global instance");
			s_instance = null;
			s_unloadHandler = null;
			IsGlobal = false;
			OnUnloadedAsGlobal(destroyed: true);
		}

		protected static bool TryGetGlobalInternal(out T global)
		{
			T orLoadGlobalInstance = GetOrLoadGlobalInstance();
			if ((object)orLoadGlobalInstance == null)
			{
				global = null;
				return false;
			}
			global = orLoadGlobalInstance;
			return true;
		}

		protected static async Task<T> GetGlobalAsyncInternal()
		{
			T instance = await GetOrLoadGlobalInstanceAsync();
			if ((object)instance == null)
			{
				throw new InvalidOperationException("Failed to load " + typeof(T).Name + ". If this happens in edit mode, make sure Fusion is properly installed in the Fusion HUB. Otherwise, if the default path does not exist or does not point to a Resource, you need to use FusionGlobalScriptableObjectAttribute attribute to point to a method that will perform the loading.");
			}
			return instance;
		}

		protected static bool UnloadGlobalInternal()
		{
			T val = s_instance;
			if (!val)
			{
				return false;
			}
			Assert.Check(val.IsGlobal, "instance.IsGlobal");
			try
			{
				if (s_unloadHandler != null)
				{
					InternalLogStreams.LogTrace?.Log(LogPrefix + " Unloading global instance " + AsId(val) + " with unloader");
					FusionGlobalScriptableObjectUnloadDelegate fusionGlobalScriptableObjectUnloadDelegate = s_unloadHandler;
					s_unloadHandler = null;
					fusionGlobalScriptableObjectUnloadDelegate(val);
				}
				else
				{
					InternalLogStreams.LogTrace?.Log(LogPrefix + " Instance " + AsId(val) + " has no unloader, simply nulling it out");
				}
			}
			finally
			{
				s_instance = null;
				if (val.IsGlobal)
				{
					val.IsGlobal = false;
					val.OnUnloadedAsGlobal(destroyed: false);
				}
			}
			return true;
		}

		private static T GetOrLoadGlobalInstance()
		{
			if ((bool)s_instance)
			{
				return s_instance;
			}
			if (FusionGlobalScriptableObject._typesFailedToLoad.Contains(typeof(T)))
			{
				InternalLogStreams.LogTrace?.Log("Type " + typeof(T).FullName + " is in the failed list, early out with null");
				return null;
			}
			FusionGlobalScriptableObjectSourceAttribute[] sourceAttributes = FusionGlobalScriptableObject.SourceAttributes;
			foreach (FusionGlobalScriptableObjectSourceAttribute fusionGlobalScriptableObjectSourceAttribute in sourceAttributes)
			{
				if (Application.isEditor && !Application.isPlaying && !fusionGlobalScriptableObjectSourceAttribute.AllowEditMode)
				{
					InternalLogStreams.LogTrace?.Log($"{LogPrefix} Loader {fusionGlobalScriptableObjectSourceAttribute} failed to load {typeof(T).FullName}: edit mode");
					continue;
				}
				if (fusionGlobalScriptableObjectSourceAttribute.ObjectType != typeof(T) && !typeof(T).IsSubclassOf(fusionGlobalScriptableObjectSourceAttribute.ObjectType))
				{
					InternalLogStreams.LogTrace?.Log($"{LogPrefix} Loader {fusionGlobalScriptableObjectSourceAttribute} not compatible with {typeof(T).FullName}");
					continue;
				}
				FusionGlobalScriptableObjectLoadResult fusionGlobalScriptableObjectLoadResult = fusionGlobalScriptableObjectSourceAttribute.Load(typeof(T));
				if ((bool)s_instance)
				{
					InternalLogStreams.LogTrace?.Log("Something has loaded the instance of " + typeof(T).FullName + " while sync load was happening. Unloading the newly loaded instance and returning the actual global.");
					if ((bool)fusionGlobalScriptableObjectLoadResult.Object)
					{
						fusionGlobalScriptableObjectLoadResult.Unloader?.Invoke(fusionGlobalScriptableObjectLoadResult.Object);
					}
					return s_instance;
				}
				if ((bool)fusionGlobalScriptableObjectLoadResult.Object)
				{
					T val = (T)fusionGlobalScriptableObjectLoadResult.Object;
					InternalLogStreams.LogTrace?.Log($"{LogPrefix} Loader {fusionGlobalScriptableObjectSourceAttribute} was used to load {AsId(val)}, has unloader: {fusionGlobalScriptableObjectLoadResult.Unloader != null}");
					SetGlobalInternal(val, fusionGlobalScriptableObjectLoadResult.Unloader);
					return val;
				}
				InternalLogStreams.LogTrace?.Log($"{LogPrefix} Loader {fusionGlobalScriptableObjectSourceAttribute} failed to load {typeof(T).FullName}");
				if (!fusionGlobalScriptableObjectSourceAttribute.AllowFallback)
				{
					InternalLogStreams.LogTrace?.Log($"{LogPrefix} Loader {fusionGlobalScriptableObjectSourceAttribute} failed to load {typeof(T).FullName} and disallows fallback");
					break;
				}
			}
			if (Application.isEditor)
			{
				InternalLogStreams.LogTrace?.Log(LogPrefix + " No source attribute was able to load the global instance, adding to the failed list: " + typeof(T).FullName);
				FusionGlobalScriptableObject._typesFailedToLoad.Add(typeof(T));
			}
			else
			{
				InternalLogStreams.LogTrace?.Log(LogPrefix + " No source attribute was able to load the global instance: " + typeof(T).FullName);
			}
			return null;
		}

		private static async Task<T> GetOrLoadGlobalInstanceAsync()
		{
			if ((bool)s_instance)
			{
				return s_instance;
			}
			FusionGlobalScriptableObjectSourceAttribute[] sourceAttributes = FusionGlobalScriptableObject.SourceAttributes;
			foreach (FusionGlobalScriptableObjectSourceAttribute sourceAttribute in sourceAttributes)
			{
				if ((Application.isEditor && !Application.isPlaying && !sourceAttribute.AllowEditMode) || (sourceAttribute.ObjectType != typeof(T) && !typeof(T).IsSubclassOf(sourceAttribute.ObjectType)))
				{
					continue;
				}
				Task<FusionGlobalScriptableObjectLoadResult> task = sourceAttribute.LoadAsync(typeof(T));
				if (task == null)
				{
					if (!sourceAttribute.AllowFallback)
					{
						break;
					}
					continue;
				}
				FusionGlobalScriptableObjectLoadResult result = await task;
				if ((bool)s_instance)
				{
					InternalLogStreams.LogWarn?.Log("Something has loaded the instance of " + typeof(T).FullName + " while async load was happening. Unloading the newly loaded instance and returning the actual global.");
					if ((bool)result.Object)
					{
						result.Unloader?.Invoke(result.Object);
					}
					return s_instance;
				}
				if ((bool)result.Object)
				{
					T instance = (T)result.Object;
					InternalLogStreams.LogTrace?.Log($"{LogPrefix} Loader {sourceAttribute} was used to load async {AsId(instance)}, has unloader: {result.Unloader != null}");
					SetGlobalInternal(instance, result.Unloader);
					return instance;
				}
				if (!sourceAttribute.AllowFallback)
				{
					break;
				}
			}
			InternalLogStreams.LogTrace?.Log(LogPrefix + " No source attribute was able to load the global instance");
			return null;
		}

		private static void SetGlobalInternal(T value, FusionGlobalScriptableObjectUnloadDelegate unloadHandler)
		{
			if ((bool)s_instance)
			{
				throw new InvalidOperationException("Failed to set " + typeof(T).Name + " as global. A global instance is already loaded - it needs to be unloaded first");
			}
			Assert.Check(value, "Expected value to be non-null");
			if ((object)s_instance == null)
			{
				Assert.Check(s_unloadHandler == null, "Expected unload handler to be null");
			}
			if ((bool)value)
			{
				s_instance = value;
				s_unloadHandler = unloadHandler;
				s_instance.IsGlobal = true;
				s_instance.OnLoadedAsGlobal();
			}
		}
	}
}
