using Features.ChineseDetectionModule.Scripts.Core;
using Features.ChineseDetectionModule.Scripts.Data;
using Features.ChineseDetectionModule.Scripts.Systems;
using Zenject;

namespace Features.ChineseDetectionModule.Scripts.Installers
{
	public class ChineseDetectionInstaller : Installer<ChineseDetectionInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.Bind<ChineseDetectionModel>().AsSingle();
			base.Container.Bind<IChineseDetectionService>().To<ChineseDetectionService>().AsSingle();
			base.Container.BindInterfacesTo<ChineseDetectionSystem>().AsSingle();
		}
	}
}
