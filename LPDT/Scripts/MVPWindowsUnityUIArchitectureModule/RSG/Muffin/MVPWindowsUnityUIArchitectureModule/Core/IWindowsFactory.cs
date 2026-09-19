using System;
using System.Threading.Tasks;
using RSG.Muffin.AssetLoaderModule.Core;

namespace RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core
{
	public interface IWindowsFactory
	{
		IWindowInstance GetWindowInstanceForWindowType(Type windowType, AssetLoadSource assetLoadSource, string assetGroupName = "Default");

		Task<IWindowInstance> GetWindowInstanceForWindowTypeAsync(Type windowType, AssetLoadSource assetLoadSource, string assetGroupName = "Default");

		IWindowInstance GetPreloadWindowInstanceForWindowType(Type windowType);
	}
}
