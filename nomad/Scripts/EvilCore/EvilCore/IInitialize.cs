namespace EvilCore
{
	public interface IInitialize
	{
		bool IsInitialized { get; set; }

		void Init();
	}
}
