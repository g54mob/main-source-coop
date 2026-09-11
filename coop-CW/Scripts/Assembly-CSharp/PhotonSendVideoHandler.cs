using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Realtime;
using Unity.Collections;
using UnityEngine;
using Zorro.Core.Serizalization;
using Zorro.PhotonUtility;

public class PhotonSendVideoHandler : RetrievableSingleton<PhotonSendVideoHandler>
{
	private readonly string VIDEO_EXTENSION = ".webm";

	private const float TIMER_PER_PACKAGE = 0.2f;

	private const int BYTES_PER_CHUNK = 30000;

	private string PATH_TO_VIDEO;

	private ListenerHandle m_sendChunkListenHandle;

	private Dictionary<ClipID, VideoChunk> m_VideoChunkDic = new Dictionary<ClipID, VideoChunk>();

	private bool m_UseSteamNetwork;

	protected override void OnCreated()
	{
		base.OnCreated();
		PATH_TO_VIDEO = RecordingsHandler.GetDirectory();
		InitSendVideoHandler();
		m_sendChunkListenHandle = CustomCommands<CustomCommandType>.RegisterListener<SendVideoChunkPackage>(RecieveClipChunk);
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnDestroy()
	{
		CustomCommands<CustomCommandType>.UnregisterListener(m_sendChunkListenHandle);
	}

	private void InitSendVideoHandler()
	{
		bool flag = Directory.Exists(PATH_TO_VIDEO);
		Debug.Log("INIT PhotonSendVideoHandler with path: " + PATH_TO_VIDEO + " Exist? " + flag);
		if (!flag)
		{
			Directory.CreateDirectory(PATH_TO_VIDEO);
			if (!Directory.Exists(PATH_TO_VIDEO))
			{
				string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Error_RecordingsFolder);
				Debug.LogError("Failed To Create Recordingsfolder! " + PATH_TO_VIDEO);
				Modal.ShowError(localizedString, PATH_TO_VIDEO);
			}
		}
		SteamLobbyHandler steamLobbyHandler = MainMenuHandler.SteamLobbyHandler;
		if (steamLobbyHandler != null)
		{
			m_UseSteamNetwork = steamLobbyHandler.UseSteamNetwork;
		}
		else
		{
			m_UseSteamNetwork = false;
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F6))
		{
			Application.OpenURL(PATH_TO_VIDEO);
		}
	}

	public IEnumerator SendVideoThroughPhoton(Clip clipToSend, bool isReRequest)
	{
		Guid id;
		if (!clipToSend.TryGetContentBuffer(out var contentBuffer))
		{
			id = clipToSend.clipID.id;
			Debug.LogError("FATAL! Clip: " + id.ToString() + " Does not have a content buffer! Cannot send video!");
			yield break;
		}
		VideoHandle videoHandle = clipToSend.m_recording.videoHandle;
		id = clipToSend.clipID.id;
		string text = id.ToString();
		VideoHandle videoHandle2 = videoHandle;
		VerboseDebug.Log("Sending Video Through Photon: CLIP: " + text + " VIDEO: " + videoHandle2.ToString());
		string text2 = Path.Combine(clipToSend.GetClipDirectory(), "output.webm");
		if (!File.Exists(text2))
		{
			string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Error_File_Not_Found);
			string localizedString2 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Ok);
			Modal.Show(localizedString, "Failed to send video: File " + text2 + " Not found", new ModalOption[1]
			{
				new ModalOption(localizedString2)
			});
			Debug.LogError("Failed to send video: File " + text2 + " Not found");
			yield break;
		}
		byte[] array = File.ReadAllBytes(text2);
		VerboseDebug.Log("Found video: " + text2 + " Bytes: " + array.Length);
		int num = 30000;
		int num2 = 0;
		int num3 = 0;
		List<byte[]> list = new List<byte[]>();
		for (int i = 0; i < 10000; i++)
		{
			byte[] array2;
			if (num2 + num > array.Length)
			{
				array2 = new byte[array.Length - num2];
				Array.Copy(array, num2, array2, 0, array2.Length);
				list.Add(array2);
				num3 += array2.Length;
				break;
			}
			array2 = new byte[num];
			Array.Copy(array, num2, array2, 0, array2.Length);
			list.Add(array2);
			num2 += array2.Length;
			VerboseDebug.Log("Adding Chunk: New Pointer " + num2 + " Wrote: " + array2.Length + " Bytes!");
			num3 += array2.Length;
		}
		VerboseDebug.Log("Chunks made: " + list.Count + " Bytes written: " + num3);
		yield return SendVideoChunks(list, clipToSend.clipID, videoHandle, contentBuffer, isReRequest);
	}

	private IEnumerator SendVideoChunks(List<byte[]> videoChunks, ClipID clipID, VideoHandle videoID, ContentBuffer contentBuffer, bool isReRequest)
	{
		Debug.Log($"Begin To Send VideoChunks: {videoChunks.Count} Clip: {clipID.id} Video: {videoID}");
		ushort chunkIndex = 0;
		BinarySerializer serializer = new BinarySerializer(512, Allocator.Persistent);
		foreach (byte[] videoChunk in videoChunks)
		{
			SendVideoChunkPackage sendVideoChunkPackage = new SendVideoChunkPackage
			{
				ChunkCount = (ushort)videoChunks.Count,
				VideoChunkData = videoChunk,
				ChunkIndex = chunkIndex,
				VideoHandle = videoID,
				ClipID = clipID
			};
			if (chunkIndex == 0)
			{
				contentBuffer.Serialize(serializer);
				sendVideoChunkPackage.ContentEventData = serializer.buffer;
			}
			if (m_UseSteamNetwork && !isReRequest)
			{
				using BinarySerializer binarySerializer = sendVideoChunkPackage.Serialize();
				MainMenuHandler.SteamLobbyHandler.SendPackageToAll(binarySerializer.buffer);
			}
			else if (!CustomCommands<CustomCommandType>.SendPackage(sendVideoChunkPackage, ReceiverGroup.Others))
			{
				Debug.LogError("Failed To Send chunk!");
			}
			chunkIndex++;
			yield return new WaitForSeconds(0.2f);
		}
		serializer.Dispose();
	}

	public void RecieveClipChunk(SendVideoChunkPackage package)
	{
		ClipID clipID = package.ClipID;
		if (!m_VideoChunkDic.TryGetValue(package.ClipID, out var value))
		{
			m_VideoChunkDic.Add(package.ClipID, value = new VideoChunk(package.ChunkCount, package.ClipID, package.VideoHandle));
		}
		value.AddChunk(package.VideoChunkData, package.ChunkIndex, package.ContentEventData);
		if (m_VideoChunkDic[clipID].Completed)
		{
			Debug.Log($"Received Last Chunk For Clip: {clipID.id}, Saving Video: " + m_VideoChunkDic[clipID].VideoID.ToString());
			TempSaveReceivedVideo(m_VideoChunkDic[clipID]);
		}
	}

	private void TempSaveReceivedVideo(VideoChunk chunk)
	{
		string text = PATH_TO_VIDEO + "/" + chunk.VideoID.ToString() + "/" + chunk.ClipID.id.ToString();
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		string text2 = text + "/output" + VIDEO_EXTENSION;
		try
		{
			File.WriteAllBytes(text2, chunk.ChunkData);
		}
		catch (IOException ex) when (ex.HResult == -2147024784)
		{
			Debug.LogException(ex);
			Debug.LogError("Failed to write to disk due to the above exception");
			Modal.ShowError("Disk full", "Downloading other player's video clip failed due to full disk. Please clear out your disk and restart the game.\n" + ex.Message);
		}
		catch (Exception ex2)
		{
			Debug.LogException(ex2);
			Debug.LogError("Failed to write to disk due to the above exception");
			Modal.ShowError("Failed to write other player's video clip to disk", ex2.ToString());
		}
		if (File.Exists(text2))
		{
			Debug.Log("Successfully Saved Remote Clip: " + text2);
		}
		else
		{
			Debug.LogError("Failed To Save Remote Clip: " + text2);
		}
		m_VideoChunkDic.Remove(chunk.ClipID);
		RecordingsHandler.RecievedClip(chunk.VideoID, chunk.ClipID, chunk.contentBuffer);
	}

	public void ClearChunksForClip(ClipID clipID)
	{
		if (m_VideoChunkDic.ContainsKey(clipID))
		{
			m_VideoChunkDic.Remove(clipID);
		}
	}
}
