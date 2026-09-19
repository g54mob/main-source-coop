using System;
using System.Collections.Generic;
using Unity.AI.Navigation;

namespace Features.AIModule.Data
{
	public class NavigationModel
	{
		private readonly List<NavMeshSurface> _activeSurfaces = new List<NavMeshSurface>();

		public IReadOnlyList<NavMeshSurface> ActiveSurfaces => _activeSurfaces;

		public event Action<NavMeshSurface> OnSurfaceRegistered;

		public event Action<NavMeshSurface> OnSurfaceUnRegistered;

		public void RegisterSurface(NavMeshSurface surface)
		{
			_activeSurfaces.Add(surface);
			this.OnSurfaceRegistered?.Invoke(surface);
		}

		public void UnRegisterSurface(NavMeshSurface surface)
		{
			_activeSurfaces.Remove(surface);
			this.OnSurfaceUnRegistered?.Invoke(surface);
		}
	}
}
