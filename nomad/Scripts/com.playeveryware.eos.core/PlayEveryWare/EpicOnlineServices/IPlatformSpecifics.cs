using System.Collections.Generic;

namespace PlayEveryWare.EpicOnlineServices
{
	public interface IPlatformSpecifics
	{
		string GetTempDir();

		void AddPluginSearchPaths(ref List<string> pluginPaths);

		string GetDynamicLibraryExtension();

		void LoadDelegatesWithEOSBindingAPI();

		void ConfigureSystemInitOptions(ref EOSInitializeOptions initializeOptions);

		void InitializeOverlay(IEOSCoroutineOwner owner);

		void InitializeNetworkChecks(IEOSCoroutineOwner owner);

		void ConfigureSystemPlatformCreateOptions(ref EOSCreateOptions createOptions);

		void RegisterForPlatformNotifications();

		bool IsApplicationConstrainedWhenOutOfFocus();

		int IsReadyForNetworkActivity();

		void SetDefaultAudioSession();

		void UpdateNetworkStatus();

		bool CanShowExitButton();
	}
}
