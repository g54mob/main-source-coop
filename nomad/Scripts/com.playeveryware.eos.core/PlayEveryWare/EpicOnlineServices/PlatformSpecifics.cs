using System.Collections.Generic;
using Epic.OnlineServices.Platform;
using UnityEngine;

namespace PlayEveryWare.EpicOnlineServices
{
	public abstract class PlatformSpecifics<T> : IPlatformSpecifics where T : PlatformConfig
	{
		protected PlatformManager.Platform Platform;

		protected PlatformSpecifics(PlatformManager.Platform platform)
		{
			Platform = platform;
		}

		public string GetDynamicLibraryExtension()
		{
			return PlatformManager.GetDynamicLibraryExtension(Platform);
		}

		public virtual string GetTempDir()
		{
			return Application.temporaryCachePath;
		}

		public virtual void InitializeOverlay(IEOSCoroutineOwner owner)
		{
		}

		public virtual void InitializeNetworkChecks(IEOSCoroutineOwner owner)
		{
		}

		public virtual void AddPluginSearchPaths(ref List<string> pluginPaths)
		{
		}

		public virtual void LoadDelegatesWithEOSBindingAPI()
		{
		}

		public virtual void RegisterForPlatformNotifications()
		{
		}

		public virtual void SetDefaultAudioSession()
		{
		}

		public virtual bool IsApplicationConstrainedWhenOutOfFocus()
		{
			return false;
		}

		public virtual void ConfigureSystemPlatformCreateOptions(ref EOSCreateOptions createOptions)
		{
			createOptions.options.RTCOptions = default(WindowsRTCOptions);
		}

		public virtual void ConfigureSystemInitOptions(ref EOSInitializeOptions initializeOptionsRef)
		{
		}

		public virtual int IsReadyForNetworkActivity()
		{
			return 1;
		}

		public virtual void UpdateNetworkStatus()
		{
		}

		public virtual bool CanShowExitButton()
		{
			return true;
		}
	}
}
