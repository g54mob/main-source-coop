using UnityEngine.UIElements;
using Zorro.Core.CLI;

public class NetworkDealsPage : DebugPage
{
	public NetworkDealsPage(VisualTreeAsset dealCell)
	{
		NetworkDealBase activeDeal = NetworkDealBoss.activeDeal;
		if (activeDeal != null)
		{
			NetworkDealCell child = new NetworkDealCell(dealCell, activeDeal);
			Add(child);
		}
	}
}
