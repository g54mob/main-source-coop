using System;
using System.Collections.Generic;
using System.Text;
using DefaultNamespace;
using Photon.Pun;
using Portningsbolaget.Platforms;
using Unity.Collections;
using UnityEngine;
using Zorro.Core;
using Zorro.Core.Serizalization;

public class UploadVideoStation : Interactable, ITimeOfDayListener
{
	public Item m_flashCardItem;

	public Item m_lostDisk;

	private PhotonView photonView;

	public GameObject m_uploadStateUI;

	public GameObject m_uploadingStateUI;

	public GameObject m_ClosedStateUI;

	public UploadCompleteUI m_uploadCompleteUI;

	private UploadVideoStationStateMachine m_stateMachine;

	private ExtractVideoMachine m_extractVideoMachine;

	protected override void Awake()
	{
		base.Awake();
		photonView = GetComponent<PhotonView>();
		m_extractVideoMachine = UnityEngine.Object.FindFirstObjectByType<ExtractVideoMachine>();
		if (m_extractVideoMachine == null)
		{
			Debug.LogError("#! MISSING EXTRACT VIDEO STATION MACHINE");
		}
		m_stateMachine = new UploadVideoStationStateMachine();
		m_stateMachine.RegisterState(new UploadVideoState(m_uploadStateUI));
		m_stateMachine.RegisterState(new UploadingVideoState(m_uploadingStateUI));
		m_stateMachine.RegisterState(new UploadCompleteState(m_uploadCompleteUI));
		m_stateMachine.RegisterState(new UploadLostDiscCompleteState(m_uploadCompleteUI));
		m_stateMachine.RegisterState(new UploadVideoClosedState(m_ClosedStateUI));
		TimeOfDayToggler.AddListener(this);
	}

	private void Start()
	{
		hoverText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.UploadToSpookTube);
		DayTimeChanged(TimeOfDayHandler.TimeOfDay);
	}

	public void DayTimeChanged(TimeOfDay timeOfDay)
	{
		if (timeOfDay == TimeOfDay.Evening)
		{
			m_stateMachine.SwitchState<UploadVideoState>();
		}
		else
		{
			m_stateMachine.SwitchState<UploadVideoClosedState>();
		}
	}

	public void Unlock()
	{
		m_stateMachine.SwitchState<UploadVideoState>();
	}

	public override bool IsValid(Player player)
	{
		return m_stateMachine.CurrentState is UploadVideoState;
	}

	public override void Interact(Player player)
	{
		if (player.TryGetInventory(out var o) && o.TryGetSlot(player.data.selectedItemSlot, out var slot) && slot.ItemInSlot.item != null)
		{
			IntEntry t2;
			if (slot.ItemInSlot.data.TryGetEntry<FlashcardEntry>(out var t))
			{
				slot.Clear();
				UploadFlashcard(t);
			}
			else if (slot.ItemInSlot.item == m_lostDisk && slot.ItemInSlot.data.TryGetEntry<IntEntry>(out t2))
			{
				slot.Clear();
				UploadLostDisk(t2.i);
			}
		}
	}

	private void UploadLostDisk(int lostDiscIndex)
	{
		photonView.RPC("RPC_UploadLostDisc", RpcTarget.MasterClient, lostDiscIndex);
	}

	private void UploadFlashcard(FlashcardEntry flashcardEntry)
	{
		photonView.RPC("RPC_UploadFlashcard", RpcTarget.MasterClient, flashcardEntry.videoID.id.ToByteArray());
	}

	[PunRPC]
	public void RPC_UploadLostDisc(int lostFootageIndex)
	{
		LostFootageHandle lostFootageHandle = new LostFootageHandle(lostFootageIndex);
		LostFootageHandle lostFootageHandle2 = lostFootageHandle;
		Debug.Log("Uploading Lost Disk: " + lostFootageHandle2.ToString());
		if (!LostFootageDatabase.TryGetLostFootage(lostFootageHandle, out var _))
		{
			lostFootageHandle2 = lostFootageHandle;
			Debug.LogError("Failed to get lost footage by index: " + lostFootageHandle2.ToString());
		}
		else
		{
			ContentEvaluator.GenerateLostDiscComments();
			photonView.RPC("RPC_UploadLostDiscComplete", RpcTarget.All, lostFootageHandle.index);
		}
	}

	[PunRPC]
	public void RPC_UploadLostDiscComplete(int handle)
	{
		LostFootageHandle footage = new LostFootageHandle(handle);
		m_stateMachine.SwitchState<UploadLostDiscCompleteState>().PlayLostFootage(footage);
	}

	[PunRPC]
	public void RPC_UploadFlashcard(byte[] videoHandleData)
	{
		VideoHandle tVideoID = new VideoHandle(new Guid(videoHandleData));
		Debug.Log("Uploading Videohandle... Evaluating views...");
		if (RecordingsHandler.TryGetRecording(tVideoID, out var recording))
		{
			if (ContentEvaluator.EvaluateRecording(recording, out var buffer))
			{
				if (NetworkDealBoss.me != null)
				{
					NetworkDealBoss.me.ReviewUploadContent(buffer);
				}
				ExtractVideoMachine extractVideoMachine = m_extractVideoMachine;
				float num = (((object)extractVideoMachine != null && extractVideoMachine.FailedExtraction) ? ((float)m_extractVideoMachine.FailedScore) : buffer.GetScore());
				Debug.Log("Recording score: " + num);
				PlatformManager.UnlockAchievement(Achievements.ACH_FIRST_VIDEO);
				List<Comment> list = buffer.GenerateComments();
				BinarySerializer binarySerializer = new BinarySerializer(256, Allocator.Persistent);
				binarySerializer.WriteInt(list.Count);
				for (int i = 0; i < list.Count; i++)
				{
					Comment comment = list[i];
					binarySerializer.WriteString(comment.Text, Encoding.ASCII);
					binarySerializer.WriteBool(comment.Detailed);
					if (comment.Detailed)
					{
						binarySerializer.WriteString(comment.Player, Encoding.ASCII);
					}
					binarySerializer.WriteInt(comment.Likes);
					binarySerializer.WriteFloat(comment.Time);
					binarySerializer.WriteByte(comment.Face);
					binarySerializer.WriteByte(comment.FaceColor);
					binarySerializer.WriteUshort(comment.EventID);
				}
				byte[] dst = new byte[binarySerializer.buffer.Length];
				NativeArray<byte> src = binarySerializer.buffer;
				ByteArrayConvertion.MoveToByteArray(ref src, ref dst);
				binarySerializer.Dispose();
				photonView.RPC("RPC_OnEvaluationComplete", RpcTarget.All, num, videoHandleData, dst);
			}
			else
			{
				Debug.LogError("Failed to evaluate recording. No content buffer found.");
			}
		}
		else
		{
			Debug.LogError("Failed to find recording. VideoHandle: " + tVideoID.id.ToShortString() + " not found");
		}
	}

	[PunRPC]
	public void RPC_OnEvaluationComplete(float score, byte[] videoHandleData, byte[] commentsData)
	{
		VideoHandle tVideoID = new VideoHandle(new Guid(videoHandleData));
		UploadCompleteState uploadCompleteState = m_stateMachine.SwitchState<UploadCompleteState>();
		Debug.Log("Evaluation complete. Score: " + score);
		if (!RecordingsHandler.TryGetRecording(tVideoID, out var recording))
		{
			return;
		}
		NativeArray<byte> dst = new NativeArray<byte>(commentsData.Length, Allocator.Persistent);
		ByteArrayConvertion.MoveFromByteArray(ref commentsData, ref dst);
		BinaryDeserializer binaryDeserializer = new BinaryDeserializer(dst);
		int num = binaryDeserializer.ReadInt();
		Debug.Log("Comment count: " + num);
		Comment[] array = new Comment[num];
		for (int i = 0; i < num; i++)
		{
			string text = binaryDeserializer.ReadString(Encoding.ASCII);
			Debug.Log("Comment: " + text);
			array[i] = new Comment(text);
			array[i].Detailed = binaryDeserializer.ReadBool();
			if (array[i].Detailed)
			{
				array[i].Player = binaryDeserializer.ReadString(Encoding.ASCII);
			}
			array[i].Likes = binaryDeserializer.ReadInt();
			array[i].Time = binaryDeserializer.ReadFloat();
			array[i].Face = binaryDeserializer.ReadByte();
			array[i].FaceColor = binaryDeserializer.ReadByte();
			array[i].EventID = binaryDeserializer.ReadUShort();
		}
		binaryDeserializer.Dispose();
		bool flag = m_extractVideoMachine?.FailedExtraction ?? false;
		int scoreToViews = BigNumbers.GetScoreToViews(score, GameAPI.CurrentDay);
		int money = (flag ? m_extractVideoMachine.FailedMoney : BigNumbers.GetMoneyFromScore(score, GameAPI.CurrentDay));
		if (PhotonNetwork.IsMasterClient)
		{
			PhotonGameLobbyHandler.Instance.SetCurrentObjective(new CelebrateObjective());
		}
		uploadCompleteState.PlayVideo(recording, Mathf.RoundToInt(score), scoreToViews, money, flag, array);
	}

	public void CloseVideo()
	{
		m_stateMachine.SwitchState<UploadVideoState>();
	}
}
