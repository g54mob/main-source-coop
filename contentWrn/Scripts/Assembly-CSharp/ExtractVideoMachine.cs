using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using Portningsbolaget.Platforms;
using TMPro;
using UnityEngine;
using Zorro.Core;

public class ExtractVideoMachine : MonoBehaviour, ITimeOfDayListener
{
	public Item m_flashCardItem;

	public GameObject m_loadingStateUI;

	public TextMeshProUGUI m_videosLeftUI;

	public GameObject m_extractionStateUI;

	public GameObject m_successStateUI;

	public GameObject m_failedStateUI;

	private byte m_currentCameraID;

	private Dictionary<int, bool> m_arePlayersDone;

	private Coroutine m_extractCoroutine;

	private VideoHandle m_videoHandle;

	private PhotonView m_photonView;

	public ExtractVideoItemDetector Detector;

	public ExtractVideoStationHatch Hatch;

	public CDRom Rom;

	public VideoExtractMachineStateMachine StateMachine;

	public Transform m_discRespawnPoint;

	public Optionable<VideoHandle> m_hasCDInRom = Optionable<VideoHandle>.None;

	private bool m_success;

	private bool m_failedExtraction;

	private int m_failedCount;

	private int m_failedScore;

	private int m_failedMoney;

	public bool AlreadyExtracting => m_extractCoroutine != null;

	public bool FailedExtraction => m_failedExtraction;

	public int FailedScore => m_failedScore;

	public int FailedMoney => m_failedMoney;

	private void Awake()
	{
		m_photonView = GetComponent<PhotonView>();
		StateMachine = new VideoExtractMachineStateMachine();
		StateMachine.RegisterState(new ExtractMachineIdleState(this));
		StateMachine.RegisterState(new ExtractMachineCheckItemState(this));
		StateMachine.RegisterState(new ExtractMachineEjectState(this));
		StateMachine.RegisterState(new ExtractMachineExtractingState(this));
		StateMachine.RegisterState(new ExtractMachineClosedState(this));
		TimeOfDayToggler.AddListener(this);
		DayTimeChanged(TimeOfDayHandler.TimeOfDay);
	}

	private void Update()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			StateMachine.Update();
			if (m_hasCDInRom.IsSome)
			{
				if (!Rom.m_opened)
				{
					Rom.Open();
				}
			}
			else if (Rom.m_opened)
			{
				Rom.Close();
			}
		}
		Rom.SetRom(m_hasCDInRom);
	}

	private void FixedUpdate()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			StateMachine.FixedUpdate();
		}
	}

	public List<Item> CheckForItems()
	{
		return Detector.CheckForItems().Select(((Item item, Pickup pickup) tuple, int _) => tuple.item).ToList();
	}

	public void SyncHatchState()
	{
		m_photonView.RPC("RPC_SyncHatch", RpcTarget.Others, Hatch.m_opened);
	}

	[PunRPC]
	public void RPC_SyncHatch(bool state)
	{
		if (state)
		{
			Hatch.Open();
		}
		else
		{
			Hatch.Close();
		}
	}

	public void StartExtract(VideoHandle tVideoID, bool isBrokenCamera)
	{
		m_photonView.RPC("RPC_StartExtract", RpcTarget.All, tVideoID.id.ToByteArray(), isBrokenCamera);
	}

	[PunRPC]
	public void RPC_StartExtract(byte[] buffer, bool isBrokenCamera)
	{
		if (PhotonNetwork.IsMasterClient)
		{
			m_arePlayersDone = new Dictionary<int, bool>();
			Photon.Realtime.Player[] playerList = PhotonNetwork.PlayerList;
			foreach (Photon.Realtime.Player player in playerList)
			{
				m_arePlayersDone.Add(player.ActorNumber, value: false);
			}
		}
		VideoHandle handle = (m_videoHandle = new VideoHandle(new Guid(buffer)));
		Guid id = handle.id;
		Debug.Log("EXTRACT! video=" + id.ToString());
		m_extractCoroutine = StartCoroutine(ExtractVideo(handle, isBrokenCamera));
		if (isBrokenCamera)
		{
			PlatformManager.UnlockAchievement(Achievements.ACH_RECOVER_CAMERA);
		}
	}

	[PunRPC]
	public void RPC_Failed(int score, int money)
	{
		m_failedExtraction = true;
		m_failedScore = score;
		m_failedMoney = money;
		int scoreToViews = BigNumbers.GetScoreToViews(score, GameAPI.CurrentDay);
		UserInterface.ShowMoneyNotification(LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.SpookTubeViews), $"${scoreToViews}", MoneyCellUI.MoneyCellType.Revenue);
		UserInterface.ShowMoneyNotification(LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.AdRevenue), $"${money}", MoneyCellUI.MoneyCellType.Revenue);
	}

	[PunRPC]
	public void RPC_Success()
	{
		Debug.Log("EVERYONE DONE!");
		if (m_success)
		{
			m_loadingStateUI.SetActive(value: false);
			m_extractionStateUI.SetActive(value: false);
			m_successStateUI.SetActive(value: true);
		}
	}

	private IEnumerator ExtractVideo(VideoHandle handle, bool isBrokenCamera)
	{
		m_successStateUI.SetActive(value: false);
		m_failedStateUI.SetActive(value: false);
		m_failedExtraction = false;
		m_success = false;
		m_failedCount = 0;
		float t = 60f;
		if (!isBrokenCamera && !RetrievableSingleton<RecordingsHandler>.Instance.DoneSendingClips)
		{
			m_videosLeftUI.text = RetrievableSingleton<RecordingsHandler>.Instance.ClipsLeftToSend.ToString();
			m_loadingStateUI.SetActive(value: true);
			while (!RetrievableSingleton<RecordingsHandler>.Instance.DoneSendingClips && t > 0f)
			{
				m_videosLeftUI.text = RetrievableSingleton<RecordingsHandler>.Instance.ClipsLeftToSend.ToString();
				t -= 0.5f;
				yield return new WaitForSecondsRealtime(0.5f);
			}
			m_loadingStateUI.SetActive(value: false);
			t = 60f;
		}
		m_extractionStateUI.SetActive(value: true);
		yield return new WaitForSecondsRealtime(2f);
		if (RecordingsHandler.TryGetRecording(handle, out var recording))
		{
			string oldmsg = null;
			while (t > 0f && !m_success)
			{
				t -= Time.deltaTime;
				yield return null;
				if (recording.ReadyToExtract(out var failureReason))
				{
					Debug.Log("REAAADDYYY STARTING EXTRACT!");
					yield return RetrievableSingleton<RecordingsHandler>.Instance.ExtractRecording(recording, delegate(bool lambdaResult)
					{
						m_success = lambdaResult;
					});
					Debug.Log("Done extracting");
					if (!m_success)
					{
						break;
					}
				}
				else if (failureReason != oldmsg)
				{
					oldmsg = failureReason;
					Debug.Log($"Not ready to extract, t={t}! {failureReason}");
				}
			}
			if (!m_success)
			{
				Debug.LogError($"Timed out, t={t}, failed to extract");
			}
		}
		else
		{
			Debug.LogError($"Failed to get recording!: {handle}");
		}
		Debug.Log("Invoking Photon RPC, Extract is done.");
		m_photonView.RPC("RPC_ExtractDone", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.ActorNumber, m_success);
		m_extractCoroutine = null;
		if (!m_success)
		{
			m_loadingStateUI.SetActive(value: false);
			m_extractionStateUI.SetActive(value: false);
			m_failedStateUI.SetActive(value: true);
		}
	}

	[PunRPC]
	public void RPC_ExtractDone(int playerID, bool success)
	{
		m_arePlayersDone[playerID] = true;
		if (!success)
		{
			Debug.Log($"Player: {playerID} FAILED!");
			m_failedCount++;
			if (playerID == PhotonNetwork.MasterClient.ActorNumber)
			{
				int num = Mathf.CeilToInt((float)SurfaceNetworkHandler.RoomStats.QuotaToReach / (float)SurfaceNetworkHandler.RoomStats.DaysPerQutoa);
				int num2 = 100;
				SurfaceNetworkHandler.RoomStats.AddMoney(num2);
				SurfaceNetworkHandler.RoomStats.AddQuota(num);
				m_photonView.RPC("RPC_Failed", RpcTarget.All, num, num2);
			}
		}
		if (AllDone())
		{
			if (m_failedCount < m_arePlayersDone.Count)
			{
				ItemInstanceData itemInstanceData = new ItemInstanceData(Guid.NewGuid());
				FlashcardEntry entry = new FlashcardEntry
				{
					videoID = m_videoHandle
				};
				itemInstanceData.AddDataEntry(entry);
				m_photonView.RPC("RPC_SetRom", RpcTarget.All, m_videoHandle.id.ToByteArray());
				m_photonView.RPC("RPC_Success", RpcTarget.All);
				PhotonGameLobbyHandler.Instance.SetCurrentObjective(new PickupDiscObjective());
			}
			MonoFunctions.instance.DelayCall(delegate
			{
				StateMachine.SwitchState<ExtractMachineEjectState>();
			}, 1f);
		}
		bool AllDone()
		{
			foreach (bool value in m_arePlayersDone.Values)
			{
				if (!value)
				{
					return false;
				}
			}
			return true;
		}
	}

	[PunRPC]
	public void RPC_SetRom(byte[] buffer)
	{
		m_hasCDInRom = Optionable<VideoHandle>.Some(new VideoHandle(new Guid(buffer)));
	}

	public void SyncCDRomState()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			m_photonView.RPC("RPC_ComState", RpcTarget.Others, Rom.m_opened);
		}
	}

	[PunRPC]
	public void RPC_ComState(bool state)
	{
		if (state)
		{
			Rom.Open();
		}
		else
		{
			Rom.Close();
		}
	}

	public void TryPickupCD(Player player)
	{
		if (m_hasCDInRom.IsSome)
		{
			m_photonView.RPC("RPC_PickupCD", RpcTarget.MasterClient, player.refs.view.ViewID);
		}
	}

	[PunRPC]
	public void RPC_PickupCD(int playerID)
	{
		if (m_hasCDInRom.IsNone)
		{
			return;
		}
		Player component = PhotonView.Find(playerID).GetComponent<Player>();
		if (component.TryGetInventory(out var o))
		{
			ItemInstanceData itemInstanceData = new ItemInstanceData(Guid.NewGuid());
			FlashcardEntry entry = new FlashcardEntry
			{
				videoID = m_hasCDInRom.Value
			};
			itemInstanceData.AddDataEntry(entry);
			if (o.TryAddItem(new ItemDescriptor(m_flashCardItem, itemInstanceData), out var slot))
			{
				m_hasCDInRom = Optionable<VideoHandle>.None;
				m_photonView.RPC("RPC_RemoveCD", RpcTarget.Others);
				component.refs.view.RPC("RPC_SelectSlot", component.refs.view.Owner, slot.SlotID);
				PhotonGameLobbyHandler.Instance.SetCurrentObjective(new UploadVideoObjective());
			}
		}
	}

	[PunRPC]
	public void RPC_RemoveCD()
	{
		m_hasCDInRom = Optionable<VideoHandle>.None;
	}

	public void DayTimeChanged(TimeOfDay timeOfDay)
	{
		Debug.Log("Time of day changed to " + timeOfDay);
		if (timeOfDay == TimeOfDay.Evening)
		{
			StateMachine.SwitchState<ExtractMachineIdleState>();
		}
		else
		{
			StateMachine.SwitchState<ExtractMachineClosedState>();
		}
		SyncHatchState();
	}
}
