using System.Linq;
using UnityEngine.SceneManagement;
using Zenject;

namespace NetworkServices.ObjectsProvider
{
	internal static class NetworkObjectInjectionContainer
	{
		private const string SessionSceneContract = "SessionScene";

		private const string GlobalSceneContract = "GlobalScene";

		public static DiContainer Resolve()
		{
			SceneContextRegistry sceneContextRegistry = ProjectContext.Instance.Container.Resolve<SceneContextRegistry>();
			DiContainer diContainer = TryGetContainerByContract(sceneContextRegistry, "SessionScene");
			if (diContainer != null)
			{
				return diContainer;
			}
			DiContainer diContainer2 = TryGetContainerByContract(sceneContextRegistry, "GlobalScene");
			if (diContainer2 != null)
			{
				return diContainer2;
			}
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				Scene sceneAt = SceneManager.GetSceneAt(i);
				if (sceneAt.isLoaded)
				{
					DiContainer diContainer3 = sceneContextRegistry.TryGetContainerForScene(sceneAt);
					if (diContainer3 != null)
					{
						return diContainer3;
					}
				}
			}
			return ProjectContext.Instance.Container;
		}

		public static bool IsSessionContextMissing()
		{
			return TryGetContainerByContract(ProjectContext.Instance.Container.Resolve<SceneContextRegistry>(), "SessionScene") == null;
		}

		private static DiContainer TryGetContainerByContract(SceneContextRegistry registry, string contractName)
		{
			foreach (SceneContext sceneContext in registry.SceneContexts)
			{
				if (sceneContext.ContractNames.Any((string name) => name == contractName))
				{
					return sceneContext.Container;
				}
			}
			return null;
		}
	}
}
