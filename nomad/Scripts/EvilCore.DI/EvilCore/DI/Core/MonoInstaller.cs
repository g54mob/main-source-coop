using System;
using System.Reflection;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace EvilCore.DI.Core
{
	public abstract class MonoInstaller : MonoBehaviour
	{
		public abstract void Install(IContainerBuilder builder);

		protected void RegisterIfNotNull<T>(IContainerBuilder builder, T component, Action<RegistrationBuilder> configuration = null, string logMessage = null) where T : Component
		{
			if (component != null)
			{
				RegistrationBuilder obj = builder.RegisterComponent(component);
				configuration?.Invoke(obj);
				string.IsNullOrEmpty(logMessage);
			}
		}

		protected void RegisterAsInterface<TInterface, TComponent>(IContainerBuilder builder, TComponent component, string logMessage = null) where TComponent : Component, TInterface
		{
			RegisterIfNotNull(builder, component, delegate(RegistrationBuilder registration)
			{
				registration.As<TInterface>();
			}, logMessage);
		}

		protected void RegisterSingleton<T>(IContainerBuilder builder, string logMessage = null) where T : class
		{
			builder.Register<T>(Lifetime.Singleton);
			string.IsNullOrEmpty(logMessage);
		}

		protected void RegisterSingletonAs<TInterface, TImplementation>(IContainerBuilder builder, string logMessage = null) where TInterface : class where TImplementation : class, TInterface
		{
			builder.Register<TImplementation>(Lifetime.Singleton).As<TInterface>();
			string.IsNullOrEmpty(logMessage);
		}

		protected T FindComponentInScene<T>(string fieldName = null) where T : Component
		{
			T val = UnityEngine.Object.FindObjectOfType<T>();
			if (val != null)
			{
				if (!string.IsNullOrEmpty(fieldName))
				{
					FieldInfo field = GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (field != null && field.FieldType.IsAssignableFrom(typeof(T)))
					{
						field.SetValue(this, val);
					}
				}
				return val;
			}
			return null;
		}
	}
}
