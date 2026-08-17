using System;
using System.Collections.Generic;
using EvilCore.EvilPack.EvilLogger;
using VContainer;
using VContainer.Unity;

namespace EvilCore.DI.Core
{
	public class DIContextManager : MonoSingleton<DIContextManager>
	{
		private Dictionary<string, LifetimeScope> _contexts = new Dictionary<string, LifetimeScope>();

		private LifetimeScope _activeContext;

		public LifetimeScope ActiveContext => _activeContext;

		public IObjectResolver ActiveResolver => _activeContext?.Container;

		public event Action<string> OnContextChanged;

		public void RegisterContext(string contextName, LifetimeScope scope)
		{
			_contexts.ContainsKey(contextName);
			_contexts[contextName] = scope;
		}

		public bool ActivateContext(string contextName)
		{
			if (!_contexts.TryGetValue(contextName, out var value))
			{
				EvilLogger.LogError("Context '" + contextName + "' not found", "ActivateContext", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\DI\\Core\\DIContextManager.cs", 67);
				return false;
			}
			_activeContext = value;
			this.OnContextChanged?.Invoke(contextName);
			return true;
		}

		public T Resolve<T>() where T : class
		{
			if (_activeContext == null)
			{
				EvilLogger.LogError("No active context", "Resolve", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\DI\\Core\\DIContextManager.cs", 86);
				return null;
			}
			return _activeContext.Container.Resolve<T>();
		}

		public T ResolveFromContext<T>(string contextName) where T : class
		{
			if (!_contexts.TryGetValue(contextName, out var value))
			{
				EvilLogger.LogError("Context '" + contextName + "' not found", "ResolveFromContext", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\DI\\Core\\DIContextManager.cs", 103);
				return null;
			}
			return value.Container.Resolve<T>();
		}

		public IEnumerable<string> GetAllContextNames()
		{
			return _contexts.Keys;
		}

		public bool RemoveContext(string contextName)
		{
			if (!_contexts.ContainsKey(contextName))
			{
				return false;
			}
			if (_activeContext == _contexts[contextName])
			{
				_activeContext = null;
			}
			_contexts.Remove(contextName);
			return true;
		}
	}
}
