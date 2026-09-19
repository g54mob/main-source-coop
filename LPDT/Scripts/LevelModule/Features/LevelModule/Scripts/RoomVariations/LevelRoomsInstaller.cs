using Zenject;

namespace Features.LevelModule.Scripts.RoomVariations
{
	public class LevelRoomsInstaller : Installer<LevelRoomsInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<RoomSpawnService>().AsSingle();
		}
	}
}
