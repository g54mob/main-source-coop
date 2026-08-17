using System;
using Cysharp.Threading.Tasks;

namespace EvilCore.Networking
{
	public interface IRegionService
	{
		RegionCode LocalRegion { get; }

		bool IsResolved { get; }

		event Action<RegionCode> OnRegionResolved;

		UniTask<RegionCode> ResolveLocalRegionAsync(bool forceRefresh = false);

		int EstimatePingMs(RegionCode hostRegion);

		string ToAttribute(RegionCode region);

		RegionCode FromAttribute(string attribute);
	}
}
