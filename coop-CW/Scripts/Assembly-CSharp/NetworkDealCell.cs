using System.Collections.Generic;
using System.Globalization;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UIElements;
using Zorro.Core;

public class NetworkDealCell : VisualElement
{
	public Label m_dealName;

	public Label m_dealDesc;

	public ProgressBar m_progressBar;

	public Button m_evaluateButton;

	private NetworkDealBase m_networkDeal;

	public NetworkDealCell(VisualTreeAsset dealCell, NetworkDealBase networkDeal)
	{
		NetworkDealCell networkDealCell = this;
		m_networkDeal = networkDeal;
		dealCell.CloneTree(this);
		m_dealName = this.Q<Label>("DealType");
		m_dealDesc = this.Q<Label>("DealDesc");
		m_progressBar = this.Q<ProgressBar>();
		m_evaluateButton = this.Q<Button>("EvaluateLatestRecording");
		m_dealName.text = networkDeal.DealName_Localized + "    " + networkDeal.GetDifficultyText();
		UpdateDealProgress();
		m_evaluateButton.clicked += delegate
		{
			Dictionary<VideoHandle, CameraRecording> recordings = RecordingsHandler.GetRecordings();
			if (recordings.Count == 0)
			{
				Debug.LogError("No recordings available for " + networkDeal.DealName + " to evaluate!");
			}
			else
			{
				CameraRecording cameraRecording = recordings.Values.MaxBy((CameraRecording recording) => recording.LastModified);
				if (!ContentEvaluator.EvaluateRecording(cameraRecording, out var buffer))
				{
					Debug.LogError("Failed to evaluate latest recording for " + cameraRecording.videoHandle.id.ToShortString());
				}
				else
				{
					int quotaToAdd = Mathf.RoundToInt(buffer.GetScore());
					if (NetworkDealBoss.me != null)
					{
						NetworkDealBoss.me.ReviewUploadContent(buffer);
					}
					UserInterface.ShowMoneyNotification("Debug Upload", quotaToAdd.ToString(CultureInfo.InvariantCulture), MoneyCellUI.MoneyCellType.Revenue);
					if (PhotonNetwork.IsMasterClient)
					{
						SurfaceNetworkHandler.RoomStats.AddQuota(quotaToAdd);
					}
					int num = 100;
					UserInterface.ShowMoneyNotification("Debug Upload Revenue", $"${num}", MoneyCellUI.MoneyCellType.Revenue);
					if (PhotonNetwork.IsMasterClient)
					{
						SurfaceNetworkHandler.RoomStats.AddMoney(num);
					}
					Debug.Log("Evaluating latest recording " + cameraRecording.videoHandle.id.ToShortString() + " for " + networkDeal.DealName);
					networkDealCell.UpdateDealProgress();
				}
			}
		};
	}

	private void UpdateDealProgress()
	{
		m_progressBar.lowValue = 0f;
		m_progressBar.highValue = 1f;
		m_progressBar.value = m_networkDeal.GetProgress();
		m_dealDesc.text = m_networkDeal.DealDescription();
	}
}
