namespace EvilCore.DI
{
	public interface IGameEntryPoint
	{
		void Initialize();

		void OnHostStarted();

		void OnClientStarted();
	}
}
