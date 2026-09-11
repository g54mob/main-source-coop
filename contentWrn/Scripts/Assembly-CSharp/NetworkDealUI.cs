using System.Collections.Generic;
using System.Linq;
using Portningsbolaget.Platforms;
using UnityEngine;

public class NetworkDealUI : MonoBehaviour
{
	public enum UI_STATE
	{
		nothing = 0,
		selection = 1,
		progress = 2,
		success = 3,
		failed = 4
	}

	public List<GameObject> desktops;

	public List<DealSelectionUi> dealSelectionUis;

	public GameObject dealProgress;

	public GameObject dealComplete;

	public GameObject dealFailed;

	private List<GameObject> allScreens;

	public UI_STATE currentState;

	private NetworkDealBase dealLastFrame;

	public NetworkDealBase ActiveDeal => NetworkDealBoss.activeDeal;

	public void Awake()
	{
		allScreens = new List<GameObject>();
		allScreens.Add(dealProgress);
		allScreens.Add(dealComplete);
		allScreens.Add(dealFailed);
		dealSelectionUis = GetComponentsInChildren<DealSelectionUi>(includeInactive: true).ToList();
		currentState = UI_STATE.nothing;
	}

	private void Start()
	{
		HideAllScreens();
	}

	private void Update()
	{
		CheckState();
	}

	public void CheckState()
	{
		if (dealLastFrame != ActiveDeal)
		{
			currentState = UI_STATE.nothing;
		}
		dealLastFrame = ActiveDeal;
		if (ActiveDeal == null)
		{
			if (currentState == UI_STATE.selection || SurfaceNetworkHandler.RoomStats == null)
			{
				return;
			}
			NetworkDealBase[] networkDealsToSelect = SurfaceNetworkHandler.RoomStats.NetworkDealsToSelect;
			HideAllScreens();
			if (networkDealsToSelect.Length != 0)
			{
				HideAllDesktops();
				currentState = UI_STATE.selection;
				for (int i = 0; i < dealSelectionUis.Count; i++)
				{
					DealSelectionUi dealSelectionUi = dealSelectionUis[i];
					dealSelectionUi.gameObject.SetActive(value: true);
					dealSelectionUi.LoadNewDeal(networkDealsToSelect[i]);
				}
			}
		}
		else if (!ActiveDeal.IsDone)
		{
			if (currentState != UI_STATE.progress)
			{
				currentState = UI_STATE.progress;
				HideAllScreens();
				dealProgress.SetActive(value: true);
				desktops.First().SetActive(value: false);
				dealProgress.GetComponent<DealProgressUi>().LoadDeal(ActiveDeal);
			}
		}
		else if (ActiveDeal.IsSuccess)
		{
			if (currentState != UI_STATE.success)
			{
				currentState = UI_STATE.success;
				HideAllScreens();
				dealComplete.SetActive(value: true);
				desktops.First().SetActive(value: false);
				dealComplete.GetComponent<DealCompleteUi>().LoadDeal(ActiveDeal);
				OnNetworkDealSucess(ActiveDeal.GetIndex());
			}
		}
		else if (ActiveDeal.IsFailed && currentState != UI_STATE.failed)
		{
			currentState = UI_STATE.failed;
			HideAllScreens();
			dealFailed.SetActive(value: true);
			desktops.First().SetActive(value: false);
			dealFailed.GetComponent<DealFailUi>().LoadDeal(ActiveDeal);
		}
	}

	private void OnNetworkDealSucess(byte networkDealIndex)
	{
		Debug.Log("OnNetworkDealSucess Index: " + networkDealIndex);
		switch (networkDealIndex)
		{
		case 0:
			PlatformManager.UnlockAchievement(Achievements.ACH_DEAL_NORF);
			break;
		case 1:
			PlatformManager.UnlockAchievement(Achievements.ACH_DEAL_MONEY);
			break;
		case 2:
			PlatformManager.UnlockAchievement(Achievements.ACH_DEAL_REPORTER);
			break;
		case 3:
			PlatformManager.UnlockAchievement(Achievements.ACH_DEAL_DANCE);
			break;
		case 4:
			PlatformManager.UnlockAchievement(Achievements.ACH_DEAL_DONKEY);
			break;
		case 5:
			PlatformManager.UnlockAchievement(Achievements.ACH_DEAL_MULTIMONSTER);
			break;
		case 9:
			PlatformManager.UnlockAchievement(Achievements.ACH_DEAL_BOMB);
			break;
		case 6:
		case 7:
		case 8:
			break;
		}
	}

	private void HideAllDesktops()
	{
		foreach (GameObject desktop in desktops)
		{
			desktop.SetActive(value: false);
		}
	}

	private void HideAllScreens()
	{
		foreach (GameObject allScreen in allScreens)
		{
			allScreen.SetActive(value: false);
		}
		foreach (DealSelectionUi dealSelectionUi in dealSelectionUis)
		{
			dealSelectionUi.gameObject.SetActive(value: false);
		}
		foreach (GameObject desktop in desktops)
		{
			desktop.SetActive(value: true);
		}
	}
}
