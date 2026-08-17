using System;
using UnityEngine;
using VContainer.Internal;

namespace VContainer.Unity
{
	public static class ContainerBuilderUnityExtensions
	{
		public static void UseEntryPoints(this IContainerBuilder builder, Action<EntryPointsBuilder> configuration)
		{
			builder.UseEntryPoints(Lifetime.Singleton, configuration);
		}

		public static void UseEntryPoints(this IContainerBuilder builder, Lifetime lifetime, Action<EntryPointsBuilder> configuration)
		{
			EntryPointsBuilder.EnsureDispatcherRegistered(builder);
			configuration(new EntryPointsBuilder(builder, lifetime));
		}

		public static void UseComponents(this IContainerBuilder builder, Action<ComponentsBuilder> configuration)
		{
			configuration(new ComponentsBuilder(builder));
		}

		public static void UseComponents(this IContainerBuilder builder, Transform root, Action<ComponentsBuilder> configuration)
		{
			configuration(new ComponentsBuilder(builder, root));
		}

		public static RegistrationBuilder RegisterEntryPoint<T>(this IContainerBuilder builder, Lifetime lifetime = Lifetime.Singleton)
		{
			EntryPointsBuilder.EnsureDispatcherRegistered(builder);
			return builder.Register<T>(lifetime).AsImplementedInterfaces();
		}

		public static RegistrationBuilder RegisterEntryPoint<TInterface>(this IContainerBuilder builder, Func<IObjectResolver, TInterface> implementationConfiguration, Lifetime lifetime)
		{
			EntryPointsBuilder.EnsureDispatcherRegistered(builder);
			return builder.Register(new FuncRegistrationBuilder((IObjectResolver container) => implementationConfiguration(container), typeof(TInterface), lifetime)).AsImplementedInterfaces();
		}

		public static void RegisterEntryPointExceptionHandler(this IContainerBuilder builder, Action<Exception> exceptionHandler)
		{
			builder.Register((IObjectResolver c) => new EntryPointExceptionHandler(exceptionHandler), Lifetime.Scoped);
		}

		public static RegistrationBuilder RegisterComponent<TInterface>(this IContainerBuilder builder, TInterface component)
		{
			RegistrationBuilder registrationBuilder = new ComponentRegistrationBuilder(component).As(typeof(TInterface));
			builder.RegisterBuildCallback(delegate(IObjectResolver container)
			{
				container.Resolve<TInterface>();
			});
			return builder.Register(registrationBuilder);
		}

		public static ComponentRegistrationBuilder RegisterComponentInHierarchy(this IContainerBuilder builder, Type type)
		{
			ComponentRegistrationBuilder registrationBuilder = new ComponentRegistrationBuilder(((LifetimeScope)builder.ApplicationOrigin).gameObject.scene, type);
			builder.RegisterBuildCallback(delegate(IObjectResolver container)
			{
				container.Resolve((registrationBuilder.InterfaceTypes != null) ? registrationBuilder.InterfaceTypes[0] : registrationBuilder.ImplementationType);
			});
			return builder.Register(registrationBuilder);
		}

		public static ComponentRegistrationBuilder RegisterComponentInHierarchy<T>(this IContainerBuilder builder)
		{
			return builder.RegisterComponentInHierarchy(typeof(T));
		}

		public static ComponentRegistrationBuilder RegisterComponentOnNewGameObject(this IContainerBuilder builder, Type type, Lifetime lifetime, string newGameObjectName = null)
		{
			return builder.Register(new ComponentRegistrationBuilder(newGameObjectName, type, lifetime));
		}

		public static ComponentRegistrationBuilder RegisterComponentOnNewGameObject<T>(this IContainerBuilder builder, Lifetime lifetime, string newGameObjectName = null) where T : Component
		{
			return builder.RegisterComponentOnNewGameObject(typeof(T), lifetime, newGameObjectName);
		}

		public static ComponentRegistrationBuilder RegisterComponentInNewPrefab(this IContainerBuilder builder, Type interfaceType, Component prefab, Lifetime lifetime)
		{
			ComponentRegistrationBuilder componentRegistrationBuilder = builder.Register(new ComponentRegistrationBuilder((IObjectResolver _) => prefab, prefab.GetType(), lifetime));
			componentRegistrationBuilder.As(interfaceType);
			return componentRegistrationBuilder;
		}

		public static ComponentRegistrationBuilder RegisterComponentInNewPrefab<T>(this IContainerBuilder builder, T prefab, Lifetime lifetime) where T : Component
		{
			return builder.RegisterComponentInNewPrefab(typeof(T), prefab, lifetime);
		}

		public static ComponentRegistrationBuilder RegisterComponentInNewPrefab<T>(this IContainerBuilder builder, Func<IObjectResolver, T> prefab, Lifetime lifetime) where T : Component
		{
			return builder.Register(new ComponentRegistrationBuilder(prefab, typeof(T), lifetime));
		}

		public static ComponentRegistrationBuilder RegisterComponentInNewPrefab<TInterface, TImplement>(this IContainerBuilder builder, Func<IObjectResolver, TImplement> prefab, Lifetime lifetime) where TImplement : Component, TInterface
		{
			ComponentRegistrationBuilder componentRegistrationBuilder = builder.Register(new ComponentRegistrationBuilder(prefab, typeof(TImplement), lifetime));
			componentRegistrationBuilder.As<TInterface>();
			return componentRegistrationBuilder;
		}
	}
}
