using UnityEngine;
using UnityEngine.UIElements;
using Zorro.Core;
using Zorro.Core.CLI;

[CreateAssetMenu(fileName = "RecordingPageInitter", menuName = "Zorro/RecordingPageInitter")]
public class RecordingPageInitter : SingletonAsset<RecordingPageInitter>
{
	[SerializeField]
	private VisualTreeAsset m_videoPage;

	[SerializeField]
	private VisualTreeAsset m_videoCellTemplate;

	[SerializeField]
	private VisualTreeAsset m_clipCell;

	[SerializeField]
	private VisualTreeAsset m_contentCell;

	[SerializeField]
	private VisualTreeAsset m_roomStatsPage;

	[SerializeField]
	private VisualTreeAsset m_roomStatsCell;

	[SerializeField]
	private VisualTreeAsset m_networkDealCell;

	public void Init()
	{
		Singleton<DebugUIHandler>.Instance.RegisterPage("Videos", () => new VideoDebugPage(m_videoPage, m_videoCellTemplate, m_clipCell, m_contentCell));
		Singleton<DebugUIHandler>.Instance.RegisterPage("Game State", () => new GameStatePage(m_roomStatsPage, m_roomStatsCell));
		Singleton<DebugUIHandler>.Instance.RegisterPage("Network Deals", () => new NetworkDealsPage(m_networkDealCell));
		Singleton<DebugUIHandler>.Instance.RegisterPage("Save/Load", () => new SaveLoadPage());
	}
}
