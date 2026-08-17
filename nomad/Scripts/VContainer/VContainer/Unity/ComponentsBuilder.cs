using UnityEngine;

namespace VContainer.Unity
{
	public readonly struct ComponentsBuilder
	{
		private readonly IContainerBuilder containerBuilder;

		private readonly Transform parentTransform;

		public ComponentsBuilder(IContainerBuilder containerBuilder, Transform parentTransform = null)
		{
			this.containerBuilder = containerBuilder;
			this.parentTransform = parentTransform;
		}

		public RegistrationBuilder AddInstance<TInterface>(TInterface component)
		{
			return containerBuilder.RegisterComponent(component);
		}

		public ComponentRegistrationBuilder AddInHierarchy<T>()
		{
			return containerBuilder.RegisterComponentInHierarchy<T>().UnderTransform(parentTransform);
		}

		public ComponentRegistrationBuilder AddOnNewGameObject<T>(Lifetime lifetime, string newGameObjectName = null) where T : Component
		{
			return containerBuilder.RegisterComponentOnNewGameObject<T>(lifetime, newGameObjectName).UnderTransform(parentTransform);
		}

		public ComponentRegistrationBuilder AddInNewPrefab<T>(T prefab, Lifetime lifetime) where T : Component
		{
			return containerBuilder.RegisterComponentInNewPrefab(prefab, lifetime).UnderTransform(parentTransform);
		}
	}
}
