using Features.AIModule.Data;
using Unity.AI.Navigation;
using UnityEngine;
using Zenject;

namespace Features.AIModule.Scripts.Services
{
	public class NavigationSurfaceRegistrar : MonoBehaviour
	{
		[SerializeField]
		private NavMeshSurface _navMeshSurface;

		private NavigationModel _navigationModel;

		[Inject]
		public void InjectDependencies(NavigationModel navigationModel)
		{
			_navigationModel = navigationModel;
		}

		private void OnEnable()
		{
			_navigationModel.RegisterSurface(_navMeshSurface);
		}

		private void OnDisable()
		{
			_navigationModel.UnRegisterSurface(_navMeshSurface);
		}
	}
}
