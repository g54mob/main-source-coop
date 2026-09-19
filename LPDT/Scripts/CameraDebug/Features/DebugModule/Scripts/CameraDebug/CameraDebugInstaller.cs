using Zenject;

namespace Features.DebugModule.Scripts.CameraDebug
{
	public class CameraDebugInstaller : Installer<CameraDebugInstaller>
	{
		public override void InstallBindings()
		{
			base.Container.BindInterfacesTo<PlayerCameraDebugSystem>().AsSingle();
		}
	}
}
