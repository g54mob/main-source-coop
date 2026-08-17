using System;
using System.Collections.Generic;
using EvilCore.EvilPack.EvilLogger;
using UnityEngine;
using VContainer;

namespace EvilCore.DI.Core
{
	public class ServiceFactory<TProduct> where TProduct : class
	{
		private readonly IObjectResolver _container;

		private readonly Dictionary<string, Func<TProduct>> _factoryMethods = new Dictionary<string, Func<TProduct>>();

		[Inject]
		public ServiceFactory(IObjectResolver container)
		{
			_container = container;
		}

		public void RegisterFactoryMethod(string key, Func<TProduct> factoryMethod)
		{
			_factoryMethods.ContainsKey(key);
			_factoryMethods[key] = factoryMethod;
		}

		public void RegisterType<TConcreteProduct>() where TConcreteProduct : TProduct
		{
			string name = typeof(TConcreteProduct).Name;
			RegisterFactoryMethod(name, () => (TProduct)(object)_container.Resolve<TConcreteProduct>());
		}

		public void RegisterPrefab(string key, GameObject prefab)
		{
			if (prefab == null)
			{
				throw new ArgumentNullException("prefab");
			}
			RegisterFactoryMethod(key, delegate
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(prefab);
				TProduct component = gameObject.GetComponent<TProduct>();
				if (component == null)
				{
					EvilLogger.LogError("Prefab does not have component of type " + typeof(TProduct).Name, "RegisterPrefab", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\DI\\Core\\ServiceFactory.cs", 68);
					UnityEngine.Object.Destroy(gameObject);
					return (TProduct)null;
				}
				return component;
			});
		}

		public TProduct Create(string key)
		{
			if (!_factoryMethods.TryGetValue(key, out var value))
			{
				throw new KeyNotFoundException("No factory method registered for key '" + key + "'");
			}
			return value();
		}

		public TProduct Create<TConcreteProduct>() where TConcreteProduct : TProduct
		{
			string name = typeof(TConcreteProduct).Name;
			return Create(name);
		}
	}
}
