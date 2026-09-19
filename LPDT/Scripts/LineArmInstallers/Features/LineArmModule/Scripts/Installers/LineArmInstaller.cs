using Features.LineArmModule.Scripts.Systems;
using Zenject;

namespace Features.LineArmModule.Scripts.Installers
{
	public class LineArmInstaller : Installer<LineArmInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<LineArmAnalyticsSystem>().AsSingle();
		}
	}
}
