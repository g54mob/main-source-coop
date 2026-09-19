using UnityEngine;
using Zenject;

namespace Features.ContextHierarchy.Scripts.GlobalContextScene
{
	public sealed class CustomInjectedPrefabFactory : IFactory<GameObject, Transform, GameObject>, IFactory
	{
		private readonly DiContainer _diContainer;

		public CustomInjectedPrefabFactory(DiContainer diContainer)
		{
			_diContainer = diContainer;
		}

		public GameObject Create(GameObject gameObject, Transform parent)
		{
			return _diContainer.InstantiatePrefab(gameObject, parent);
		}
	}
}
