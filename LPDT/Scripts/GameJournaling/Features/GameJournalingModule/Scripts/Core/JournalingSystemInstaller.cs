using RSG.Muffin.ZenjectMockableRealizationSubmodule.ZenjectMockableRealizationModule;
using Zenject;

namespace Features.GameJournalingModule.Scripts.Core
{
	public class JournalingSystemInstaller : Installer<JournalingSystemInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindAndInstallAll<IConcreteJournalingSystemInstaller>();
			base.Container.BindInterfacesAndSelfTo<CompositeJournalingSystem>().AsSingle();
		}
	}
}
