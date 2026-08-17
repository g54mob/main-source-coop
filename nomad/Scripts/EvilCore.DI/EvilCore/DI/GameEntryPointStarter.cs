using VContainer.Unity;

namespace EvilCore.DI
{
	public class GameEntryPointStarter : IStartable
	{
		private readonly IGameEntryPoint _gameEntryPoint;

		protected GameEntryPointStarter(IGameEntryPoint gameEntryPoint)
		{
			_gameEntryPoint = gameEntryPoint;
		}

		void IStartable.Start()
		{
			_gameEntryPoint.Initialize();
		}
	}
}
