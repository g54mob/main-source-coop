namespace EvilCore.CustomPass
{
	public interface ICustomPassManager
	{
		bool IsInitialized { get; }

		void EnablePass(string passId);

		void DisablePass(string passId);

		void SetPassEnabled(string passId, bool enabled);

		bool IsPassEnabled(string passId);

		bool HasPass(string passId);

		CustomPassPropertyAccessor GetProperties(string passId);

		void DisableAll();

		void EnableAll();
	}
}
