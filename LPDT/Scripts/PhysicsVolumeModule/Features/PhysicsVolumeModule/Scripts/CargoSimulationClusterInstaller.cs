using System;
using Zenject;

namespace Features.PhysicsVolumeModule.Scripts
{
	public class CargoSimulationClusterInstaller : IInitializable, IDisposable
	{
		private readonly CargoSimulationClusterService _cargoSimulationClusterService;

		public CargoSimulationClusterInstaller(CargoSimulationClusterService cargoSimulationClusterService)
		{
			_cargoSimulationClusterService = cargoSimulationClusterService;
		}

		public void Initialize()
		{
			PhysicsInfluenceVolume.ClusterService = _cargoSimulationClusterService;
		}

		public void Dispose()
		{
			_cargoSimulationClusterService.Dispose();
			PhysicsInfluenceVolume.ClusterService = null;
		}
	}
}
