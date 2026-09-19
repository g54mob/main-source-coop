using Zenject;

namespace Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer.Installers
{
	public class JsonModelSynchronizationInstaller : Installer<JsonModelSynchronizationInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<JsonModelsSynchronizerSpawnSystem>().AsSingle();
			base.Container.BindInterfacesTo<JsonModelsSynchronizerSystem>().AsSingle();
		}
	}
}
