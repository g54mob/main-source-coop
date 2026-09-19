using System;
using Features.GrabModule.Scripts;
using Zenject;

namespace Features.ObjectDespawnModule.Scripts
{
	public class PointGrabbableCleanupSystem : IInitializable, IDisposable
	{
		private readonly ObjectDespawnModel _despawnModel;

		public PointGrabbableCleanupSystem(ObjectDespawnModel despawnModel)
		{
			_despawnModel = despawnModel;
		}

		public void Initialize()
		{
			_despawnModel.OnObjectAddedToDespawn += TryCleanupPointGrabbable;
		}

		public void Dispose()
		{
			_despawnModel.OnObjectAddedToDespawn -= TryCleanupPointGrabbable;
		}

		private void TryCleanupPointGrabbable(DespawnObjectData despawnObjectData)
		{
			despawnObjectData.TargetObject.GetComponentInChildren<IPointGrabable>()?.Cleanup();
		}
	}
}
