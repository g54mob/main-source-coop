using Zenject;

namespace Features.SynchronizedModelsModule.Scripts.DataStreamingSynchronizer.Installers
{
	public class DataStreamingModelSynchronizationInstaller : Installer<DataStreamingModelSynchronizationInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<DataStreamingModelsSynchronizer>().AsSingle();
			base.Container.BindInterfacesTo<DataStreamingModelsSynchronizerSystem>().AsSingle();
		}
	}
}
