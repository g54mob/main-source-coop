using Zenject;

namespace Features.EmotesModule.Scripts.Installers
{
	public class EmotesModuleInstaller : Installer<EmotesModuleInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<FaceEmotesTriggerService>().AsSingle();
			base.Container.BindInterfacesTo<HandEmotesTriggerService>().AsSingle();
			base.Container.BindInterfacesTo<BodyEmotesTriggerService>().AsSingle();
			base.Container.BindInterfacesTo<EmoteByGroupTriggerService>().AsSingle();
			base.Container.BindInterfacesTo<CallEmotesGroupByInputSystem>().AsSingle();
			base.Container.BindInterfacesTo<EmoteAvailabilityByPlayerStateSystem>().AsSingle();
			base.Container.Bind<EmotesTriggerModel>().AsSingle();
		}
	}
}
