using EvilCore.EvilPack.EvilLogger;
using UnityEngine;
using VContainer;

namespace EvilCore.DI.Core
{
	public abstract class DIContextAwareComponent : MonoBehaviour
	{
		private IObjectResolver _cachedResolver;

		protected IObjectResolver Resolver
		{
			get
			{
				if (_cachedResolver == null)
				{
					_cachedResolver = MonoSingleton<DIContextManager>.Instance.ActiveResolver;
					if (_cachedResolver == null)
					{
						EvilLogger.LogError("No active DI context found. Make sure a context is activated before using Resolver.", "Resolver", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\DI\\Core\\DIContextManager.cs", 185);
					}
				}
				return _cachedResolver;
			}
		}

		protected T Resolve<T>() where T : class
		{
			IObjectResolver resolver = Resolver;
			if (resolver == null)
			{
				return null;
			}
			return resolver.Resolve<T>();
		}

		protected void ResetResolverCache()
		{
			_cachedResolver = null;
		}

		protected virtual void OnEnable()
		{
			MonoSingleton<DIContextManager>.Instance.OnContextChanged += OnContextChanged;
		}

		protected virtual void OnDisable()
		{
			MonoSingleton<DIContextManager>.Instance.OnContextChanged -= OnContextChanged;
		}

		protected virtual void OnContextChanged(string contextName)
		{
			ResetResolverCache();
		}
	}
}
