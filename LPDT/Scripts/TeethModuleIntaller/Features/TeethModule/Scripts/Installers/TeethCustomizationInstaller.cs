using Features.TeethModule.Scripts.Systems;
using Features.TeethModule.Scripts.Tooth;
using Zenject;

namespace Features.TeethModule.Scripts.Installers
{
	public class TeethCustomizationInstaller : Installer<TeethCustomizationInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<CharacterTeethService>().AsSingle();
			base.Container.BindInterfacesTo<ToothRemoveByDamageSystem>().AsSingle();
		}
	}
}
