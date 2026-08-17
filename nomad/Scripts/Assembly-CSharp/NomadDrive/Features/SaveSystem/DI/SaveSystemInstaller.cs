using EvilCore;
using EvilCore.DI.Core;
using VContainer;

namespace NomadDrive.Features.SaveSystem.DI
{
	public class SaveSystemInstaller : MonoInstaller
	{
		public override void Install(IContainerBuilder builder)
		{
			RegisterSingletonAs<IGameSaveService, GameSaveService>(builder);
		}
	}
}
