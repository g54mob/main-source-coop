using Photon.Pun;
using Portningsbolaget.Platforms;

public class UploadCompleteState : UploadVideoStationState
{
	public UploadCompleteUI m_ui;

	private CameraRecording m_recording;

	private string m_SpookTubeViewsText;

	private string m_AdRevenueText;

	public UploadCompleteState(UploadCompleteUI ui)
	{
		m_ui = ui;
		m_SpookTubeViewsText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.SpookTubeViews);
		m_AdRevenueText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.AdRevenue);
	}

	public override void Enter()
	{
		base.Enter();
		m_ui.gameObject.SetActive(value: true);
	}

	public override void Exit()
	{
		base.Exit();
		m_ui.gameObject.SetActive(value: false);
		m_recording?.DisposeClips();
	}

	public void PlayVideo(CameraRecording recording, int score, int views, int money, bool failedExtraction, Comment[] comments)
	{
		m_ui.PlayVideos(recording, views, comments, delegate
		{
			if (!failedExtraction)
			{
				int weeklyViews = BigNumbers.GetScoreToViews(SurfaceNetworkHandler.RoomStats.CurrentQuota, GameAPI.CurrentDay) + views;
				if (PlatformManager.Platform is SteamRuntimeManager steamRuntimeManager)
				{
					steamRuntimeManager.OnVideoUploaded(views, money, weeklyViews);
				}
			}
			UserInterface.ShowMoneyNotification(m_SpookTubeViewsText, views.ToString(), MoneyCellUI.MoneyCellType.Revenue);
			if (PhotonNetwork.IsMasterClient)
			{
				SurfaceNetworkHandler.RoomStats.AddQuota(score);
			}
			MonoFunctions.instance.DelayCall(delegate
			{
				UserInterface.ShowMoneyNotification(m_AdRevenueText, $"${money}", MoneyCellUI.MoneyCellType.Revenue);
				if (PhotonNetwork.IsMasterClient)
				{
					SurfaceNetworkHandler.RoomStats.AddMoney(money);
				}
			}, 1f);
		});
		m_recording = recording;
	}
}
