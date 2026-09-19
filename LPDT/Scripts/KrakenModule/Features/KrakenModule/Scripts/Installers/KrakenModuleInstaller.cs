using Features.KrakenModule.Scripts.Core;
using Features.KrakenModule.Scripts.Systems;
using Zenject;

namespace Features.KrakenModule.Scripts.Installers
{
	public class KrakenModuleInstaller : Installer<KrakenModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<IKrakenThrowAssignmentService>().To<KrakenThrowAssignmentService>().AsSingle();
			base.Container.Bind<IKrakenItemThrowService>().To<KrakenItemThrowService>().AsSingle();
			base.Container.Bind<IDeadPartThrowTargetService>().To<DeadPartThrowTargetService>().AsSingle();
			base.Container.BindInterfacesTo<KrakenSessionAppearSystem>().AsSingle();
			base.Container.BindInterfacesTo<KrakenItemDetectionSystem>().AsSingle();
			base.Container.BindInterfacesTo<KrakenEmoteResponseSystem>().AsSingle();
			base.Container.BindInterfacesTo<KrakenAggressiveThrowEvaluationSystem>().AsSingle();
			base.Container.BindInterfacesTo<KrakenHelpThrowEvaluationSystem>().AsSingle();
			base.Container.BindInterfacesTo<KrakenPlayerDisconnectSystem>().AsSingle();
			base.Container.BindInterfacesTo<KrakenStoreAudioMuteSystem>().AsSingle();
		}
	}
}
