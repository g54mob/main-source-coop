using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Zorro.Core;
using Zorro.PhotonUtility;
using Zorro.Recorder;

public class RecordingsHandler : RetrievableSingleton<RecordingsHandler>
{
	private Dictionary<VideoHandle, CameraRecording> m_recordings = new Dictionary<VideoHandle, CameraRecording>();

	private Queue<Clip> m_queuedClips = new Queue<Clip>();

	private int m_numCoroutinesEncoding;

	private BidirectionalDictionary<Guid, VideoHandle> m_camerasCurrentRecording = new BidirectionalDictionary<Guid, VideoHandle>(10);

	private BidirectionalDictionary<int, VideoHandle> m_playersRecording = new BidirectionalDictionary<int, VideoHandle>(10);

	private ListenerHandle startRecordingHandle = ListenerHandle.Invalid;

	private ListenerHandle stopRecordingHandle = ListenerHandle.Invalid;

	private ListenerHandle clipEncodedHandle = ListenerHandle.Invalid;

	private ListenerHandle activateNextSharingJobHandle = ListenerHandle.Invalid;

	private ListenerHandle sendClipCompletedHandle = ListenerHandle.Invalid;

	private ListenerHandle reRequestClipHandle = ListenerHandle.Invalid;

	private List<VideoClipShareJob> m_sharingQueue = new List<VideoClipShareJob>();

	private VideoClipShareJob m_currentJob;

	public bool DoneSendingClips;

	public int ClipsLeftToSend;

	private PhotonSendVideoHandler m_sendVideoHandler;

	private Dictionary<ClipID, int> m_clipRequestCount = new Dictionary<ClipID, int>();

	protected override void OnCreated()
	{
		base.OnCreated();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		m_clipRequestCount = new Dictionary<ClipID, int>();
		startRecordingHandle = CustomCommands<CustomCommandType>.RegisterListener<StartRecordingCommandPackage>(OnStartRecording);
		stopRecordingHandle = CustomCommands<CustomCommandType>.RegisterListener<StopRecordingCommandPackage>(OnStopRecording);
		clipEncodedHandle = CustomCommands<CustomCommandType>.RegisterListener<AddClipToShareQueuePackage>(AddClipToShareQueue);
		activateNextSharingJobHandle = CustomCommands<CustomCommandType>.RegisterListener<ActivateNextSharingJobPackage>(OnActivateNextSharingJob);
		sendClipCompletedHandle = CustomCommands<CustomCommandType>.RegisterListener<SendClipCompletedPackage>(SendIsComplete);
		reRequestClipHandle = CustomCommands<CustomCommandType>.RegisterListener<ReRequestClipPackage>(OnReRequestClip);
		m_sendVideoHandler = RetrievableSingleton<PhotonSendVideoHandler>.Instance;
		DirectoryInfo directoryInfo = new DirectoryInfo(GetDirectory());
		try
		{
			if (directoryInfo.Exists)
			{
				directoryInfo.Delete(recursive: true);
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("Failed to delete directory: " + directoryInfo.FullName + " " + ex.Message);
		}
		directoryInfo.Create();
		FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(directoryInfo.FullName);
		fileSystemWatcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size;
		fileSystemWatcher.IncludeSubdirectories = true;
		fileSystemWatcher.EnableRaisingEvents = true;
		fileSystemWatcher.Changed += OnFileSystemChanged;
		fileSystemWatcher.Created += OnFileSystemChanged;
		fileSystemWatcher.Deleted += OnFileSystemChanged;
		fileSystemWatcher.Renamed += OnRenamed;
		fileSystemWatcher.Error += OnFileSystemError;
		fileSystemWatcher.BeginInit();
	}

	private void OnDisable()
	{
		DirectoryInfo directory = new DirectoryInfo(GetDirectory());
		DeleteDirectory(directory);
	}

	private void OnApplicationQuit()
	{
		DirectoryInfo directory = new DirectoryInfo(GetDirectory());
		DeleteDirectory(directory);
	}

	private void DeleteDirectory(DirectoryInfo directory)
	{
		try
		{
			if (directory.Exists)
			{
				directory.Delete(recursive: true);
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("Failed to delete directory: " + directory.FullName + " " + ex.Message);
		}
	}

	private void OnRenamed(object sender, RenamedEventArgs e)
	{
		UnityEngine.Debug.Log($"File Watcher Event: {e.ChangeType}, old name: {e.OldName}, new name: {e.Name}, fullpath: {e.FullPath}");
	}

	private void OnFileSystemError(object sender, ErrorEventArgs e)
	{
		UnityEngine.Debug.LogError("File system error: " + e.GetException().Message);
	}

	private void OnFileSystemChanged(object sender, FileSystemEventArgs e)
	{
		UnityEngine.Debug.Log($"File Watcher Event: {e.ChangeType}, name: {e.Name}, fullpath: {e.FullPath}");
	}

	private void OnDestroy()
	{
		CustomCommands<CustomCommandType>.UnregisterListener(startRecordingHandle);
		CustomCommands<CustomCommandType>.UnregisterListener(stopRecordingHandle);
		CustomCommands<CustomCommandType>.UnregisterListener(clipEncodedHandle);
		CustomCommands<CustomCommandType>.UnregisterListener(activateNextSharingJobHandle);
		CustomCommands<CustomCommandType>.UnregisterListener(sendClipCompletedHandle);
		CustomCommands<CustomCommandType>.UnregisterListener(reRequestClipHandle);
	}

	private void LateUpdate()
	{
		if (RecordingPipe.CheckErrorPopup(out var title, out var body))
		{
			Modal.ShowError(title, body);
		}
		HashSet<int> hashSet = PhotonNetwork.PlayerList.Select((Photon.Realtime.Player player) => player.ActorNumber).ToHashSet();
		foreach (CameraRecording value2 in m_recordings.Values)
		{
			int clipCount = value2.ClipCount;
			for (int num = 0; num < clipCount; num++)
			{
				Clip clip = value2.GetClip(num);
				if (!clip.local && !clip.hasBeenRecieved && clip.Valid && !hashSet.Contains(clip.ownerID))
				{
					UnityEngine.Debug.Log("Player has left, clip " + clip.clipID.ToMiniString() + " is invalid...");
					clip.SetValid(validClip: false);
				}
			}
		}
		if (PhotonNetwork.IsMasterClient && m_sharingQueue.Count > 0 && m_currentJob == null)
		{
			m_currentJob = m_sharingQueue[0];
			m_sharingQueue.RemoveAt(0);
			CustomCommands<CustomCommandType>.SendPackage(new ActivateNextSharingJobPackage
			{
				ClipID = m_currentJob.ClipID
			}, ReceiverGroup.Others);
		}
		VideoClipShareJob currentJob = m_currentJob;
		if (currentJob != null && currentJob.IsLocal && !currentJob.sending && PhotonNetwork.InRoom && m_recordings.TryGetValue(m_currentJob.VideoHandle, out var value))
		{
			m_currentJob.sending = true;
			if (value.TryGetClip(m_currentJob.ClipID, out var clip2))
			{
				StartCoroutine(ShareClipJob(clip2, m_currentJob.isReRequest));
			}
			else
			{
				UnityEngine.Debug.LogError($"Error: Current share/send video clip job references clip that doesn't exist: clip {m_currentJob.ClipID.id} in video {value.videoHandle.id}");
				m_currentJob = null;
			}
		}
		if (m_sharingQueue.Count == 0 && (m_currentJob == null || m_currentJob.sending))
		{
			ClipsLeftToSend = m_sharingQueue.Count;
			DoneSendingClips = true;
		}
		else
		{
			ClipsLeftToSend = m_sharingQueue.Count + 1;
			DoneSendingClips = false;
		}
		if (m_numCoroutinesEncoding == 0)
		{
			foreach (CameraRecording value3 in m_recordings.Values)
			{
				int clipCount2 = value3.ClipCount;
				for (int num2 = 0; num2 < clipCount2; num2++)
				{
					Clip clip3 = value3.GetClip(num2);
					if (clip3.local && !clip3.encoded && !clip3.isRecording && clip3.Valid && !clip3.encodingQueued)
					{
						VerboseDebug.Log("Queue encoding... " + clip3.clipID.ToMiniString());
						clip3.encodingQueued = true;
						m_queuedClips.Enqueue(clip3);
					}
				}
			}
			if (m_queuedClips.Count > 0)
			{
				Clip clip4 = m_queuedClips.Dequeue();
				m_numCoroutinesEncoding++;
				StartCoroutine(Encode(clip4));
			}
		}
		if (m_numCoroutinesEncoding != 0 || !Input.GetKeyDown(KeyCode.F3))
		{
			return;
		}
		UnityEngine.Debug.Log("F3 pressed");
		foreach (CameraRecording recording in m_recordings.Values)
		{
			StartCoroutine(ExtractRecording(recording, delegate(bool lambdaResult)
			{
				if (lambdaResult)
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(recording.GetDirectory());
					FileInfo[] files = directoryInfo.GetFiles();
					if (files.Length != 0)
					{
						ShowExplorer(files.First().FullName);
					}
					else
					{
						ShowExplorer(directoryInfo.FullName);
					}
				}
			}));
		}
		static void ShowExplorer(string itemPath)
		{
			itemPath = itemPath.Replace("/", "\\");
			Process.Start("explorer.exe", "/select," + itemPath);
		}
	}

	public IEnumerator ExtractRecording(CameraRecording recording, Action<bool> callback)
	{
		if (!recording.ReadyToExtract(out var failureReason))
		{
			UnityEngine.Debug.LogWarning("RecordingsHandler.ExtractRecording - ReadyToExtract false: " + failureReason);
			callback(obj: false);
			yield break;
		}
		int clipCount = recording.ClipCount;
		Clip[] source = (from clip in recording.GetAllClips()
			where clip.Valid
			select clip).ToArray();
		UnityEngine.Debug.Log("All clips ready, starting stitching...");
		if (clipCount == 1)
		{
			FileInfo fileInfo = new FileInfo(Path.Combine(recording.GetClip(0).GetClipDirectory(), "output.webm"));
			if (!fileInfo.Exists)
			{
				UnityEngine.Debug.LogError("No file found to stitch: " + fileInfo.FullName);
				callback(obj: false);
				yield break;
			}
			string text = Path.Combine(recording.GetDirectory(), "fullRecording.webm");
			UnityEngine.Debug.Log("Copying file " + fileInfo.FullName + " to: " + text);
			File.Copy(fileInfo.FullName, text, overwrite: true);
			callback(obj: true);
		}
		else
		{
			string[] videos = source.Select((Clip clip) => Path.Combine(clip.GetClipDirectory(), "output.webm")).ToArray();
			string directory = recording.GetDirectory();
			yield return LibVEStitcher.StitchVideosCoroutine(videos, directory, callback);
		}
	}

	private IEnumerator ShareClipJob(Clip clip, bool isReRequest)
	{
		yield return m_sendVideoHandler.SendVideoThroughPhoton(clip, isReRequest);
		UnityEngine.Debug.Log($"Clip: {clip.clipID} has sent!");
		CustomCommands<CustomCommandType>.SendPackage(new SendClipCompletedPackage
		{
			ClipID = clip.clipID,
			VideoHandle = clip.m_recording.videoHandle
		}, ReceiverGroup.All);
	}

	private IEnumerator Encode(Clip clip)
	{
		UnityEngine.Debug.Log("Encode called for clip " + clip.clipID.ToMiniString());
		RecordingSession session = clip.GetRecorder().Session;
		yield return session.FinalizeEncoder();
		var (text3, text4) = session.GetEncoderErrorInfo();
		if (text3 != null || text4 != null)
		{
			Modal.ShowError(text3, text4);
		}
		clip.encoded = true;
		m_numCoroutinesEncoding--;
		UnityEngine.Debug.Log("Encoding done!");
		CustomCommands<CustomCommandType>.SendPackage(new AddClipToShareQueuePackage
		{
			ClipID = clip.clipID,
			VideoHandle = clip.m_recording.videoHandle,
			ClipOwner = PhotonNetwork.LocalPlayer.ActorNumber,
			isReRequest = false
		}, ReceiverGroup.All);
	}

	private void OnStartRecording(StartRecordingCommandPackage package)
	{
		Guid cameraDataGuid = package.CameraDataGuid;
		VideoHandle videoID = package.VideoID;
		int cameraOwner = package.CameraOwner;
		ClipID clipID = package.ClipID;
		if (m_camerasCurrentRecording.ContainsKey(cameraDataGuid))
		{
			UnityEngine.Debug.LogError("Camera is already recording");
			return;
		}
		if (m_camerasCurrentRecording.TryGetValue(videoID, out var value))
		{
			Guid guid = value;
			UnityEngine.Debug.LogWarning("Video ID is already in use by camera, stopping previous camera " + guid.ToString());
			OnStopRecording(value, validClip: true);
		}
		if (videoID.Equals(VideoHandle.Invalid))
		{
			UnityEngine.Debug.LogError($"Video ID is invalid, {videoID}");
			return;
		}
		if (!m_recordings.TryGetValue(videoID, out var value2))
		{
			m_recordings.Add(videoID, value2 = CreateNewRecording(videoID));
		}
		m_camerasCurrentRecording.Add(cameraDataGuid, videoID);
		m_playersRecording.Add(cameraOwner, videoID);
		bool local = cameraOwner == PhotonNetwork.LocalPlayer.ActorNumber;
		Clip clip = new Clip(package.ClipID, local, cameraOwner, value2);
		value2.AddNewClip(clip);
		UnityEngine.Debug.Log($"Start recording, camera instance id:{cameraDataGuid}, video ID: {videoID}, clip ID: {clipID}");
		if (clip.local)
		{
			if (CameraHandler.TryGetCamera(cameraDataGuid, out var videoCamera))
			{
				videoCamera.StartRecording(clip);
			}
			else
			{
				UnityEngine.Debug.LogError($"Failed to find VideoCamera with instance id {cameraDataGuid} to start recording local clip");
			}
		}
	}

	private CameraRecording CreateNewRecording(VideoHandle entryVideoID)
	{
		Guid id = entryVideoID.id;
		UnityEngine.Debug.Log("Creating new recording: " + id.ToString());
		id = entryVideoID.id;
		CameraRecording cameraRecording = new GameObject("Recording: " + id.ToString()).AddComponent<CameraRecording>();
		cameraRecording.transform.SetParent(base.transform);
		cameraRecording.SetInfo(entryVideoID);
		return cameraRecording;
	}

	private void OnStopRecording(StopRecordingCommandPackage package)
	{
		Guid cameraDataGuid = package.CameraDataGuid;
		OnStopRecording(cameraDataGuid, package.IsValidClip);
	}

	private void OnStopRecording(Guid instanceDataID, bool validClip)
	{
		if (!m_camerasCurrentRecording.ContainsKey(instanceDataID))
		{
			UnityEngine.Debug.LogWarning("Camera is not recording any video... ");
			return;
		}
		VideoHandle fromKey = m_camerasCurrentRecording.GetFromKey(instanceDataID);
		m_camerasCurrentRecording.RemoveFromKey(instanceDataID);
		Clip clip = m_recordings[fromKey].EndCurrentClip();
		clip.SetValid(validClip);
		if (m_playersRecording.Contains(fromKey))
		{
			int num = m_playersRecording.Get(fromKey);
			m_playersRecording.RemoveFromValue(fromKey);
			VerboseDebug.Log($"Removed player {num} from recording");
		}
		if (clip.local)
		{
			if (CameraHandler.TryGetCamera(instanceDataID, out var videoCamera))
			{
				videoCamera.StopRecording();
			}
			else
			{
				UnityEngine.Debug.LogError($"Failed to find VideoCamera with instance id {instanceDataID} to stop recording local clip");
			}
		}
		UnityEngine.Debug.Log($"Stop recording, camera instance id:{instanceDataID}, video ID: {fromKey.id}, clip ID: {clip.clipID.id}");
	}

	public void StartRecording(ItemInstanceData data, PhotonView playerView)
	{
		if (!data.TryGetEntry<VideoInfoEntry>(out var t))
		{
			UnityEngine.Debug.LogError("No VideoInfoEntry found in instance data");
			return;
		}
		Guid guid = data.m_guid;
		if (t.videoID.Equals(VideoHandle.Invalid))
		{
			t.videoID = new VideoHandle(Guid.NewGuid());
			UnityEngine.Debug.Log($"Camera has no video ID, creating new video ID, {t.videoID}");
		}
		t.isRecording = true;
		t.SetDirty();
		int num = -1;
		num = ((!(playerView != null)) ? FindFreePlayer() : playerView.OwnerActorNr);
		if (num == -1)
		{
			UnityEngine.Debug.LogError("No free player found to record camera...");
			return;
		}
		if (RetrievableSingleton<RecordingsHandler>.Instance.m_playersRecording.ContainsKey(num))
		{
			VideoHandle fromKey = RetrievableSingleton<RecordingsHandler>.Instance.m_playersRecording.GetFromKey(num);
			if (RetrievableSingleton<RecordingsHandler>.Instance.m_camerasCurrentRecording.Contains(fromKey) && ItemInstanceDataHandler.TryGetInstanceData(RetrievableSingleton<RecordingsHandler>.Instance.m_camerasCurrentRecording.Get(fromKey), out var o))
			{
				StopRecording(o);
				StartRecording(o, null);
			}
		}
		StartRecordingCommandPackage startRecordingCommandPackage = new StartRecordingCommandPackage
		{
			CameraDataGuid = guid,
			VideoID = t.videoID,
			ClipID = new ClipID(Guid.NewGuid()),
			CameraOwner = num
		};
		OnStartRecording(startRecordingCommandPackage);
		CustomCommands<CustomCommandType>.SendPackage(startRecordingCommandPackage, ReceiverGroup.Others);
	}

	private static int FindFreePlayer()
	{
		Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;
		int[] source = RetrievableSingleton<RecordingsHandler>.Instance.m_playersRecording.GetKeys().ToArray();
		int i;
		for (i = 0; i < players.Length; i++)
		{
			if (!source.Any((int playerID) => playerID == players[i].ActorNumber))
			{
				return players[i].ActorNumber;
			}
		}
		UnityEngine.Debug.LogError("No free player found to record camera...");
		return -1;
	}

	public void StopRecording(ItemInstanceData data)
	{
		if (!data.TryGetEntry<VideoInfoEntry>(out var t))
		{
			UnityEngine.Debug.LogError("No VideoInfoEntry found in instance data");
			return;
		}
		if (!TryGetRecording(t.videoID, out var recording))
		{
			Guid id = t.videoID.id;
			UnityEngine.Debug.LogError("No recording found with video ID: " + id.ToString());
			return;
		}
		if (!recording.TryGetLatestClip(out var clip))
		{
			Guid id = t.videoID.id;
			UnityEngine.Debug.LogError("No clip found in recording: " + id.ToString());
			return;
		}
		bool isValidClip = true;
		VideoRecorder recorder = clip.GetRecorder();
		if (recorder == null)
		{
			UnityEngine.Debug.LogError("Failed to find recorder for clip: " + clip.clipID.ToShortString());
			isValidClip = false;
		}
		else if (!recorder.HasRecorded)
		{
			UnityEngine.Debug.LogError("RecordingsHandler: CLIP HAS NO FRAMES!");
			isValidClip = false;
		}
		t.isRecording = false;
		t.SetDirty();
		Guid guid = data.m_guid;
		StopRecordingCommandPackage stopRecordingCommandPackage = new StopRecordingCommandPackage
		{
			CameraDataGuid = guid,
			IsValidClip = isValidClip
		};
		OnStopRecording(stopRecordingCommandPackage);
		CustomCommands<CustomCommandType>.SendPackage(stopRecordingCommandPackage, ReceiverGroup.Others);
	}

	public static Dictionary<VideoHandle, CameraRecording> GetRecordings()
	{
		return RetrievableSingleton<RecordingsHandler>.Instance.m_recordings;
	}

	public static BidirectionalDictionary<Guid, VideoHandle> GetCamerasCurrentRecording()
	{
		return RetrievableSingleton<RecordingsHandler>.Instance.m_camerasCurrentRecording;
	}

	public static BidirectionalDictionary<int, VideoHandle> GetPlayersRecording()
	{
		return RetrievableSingleton<RecordingsHandler>.Instance.m_playersRecording;
	}

	public static string GetDirectory()
	{
		return Path.Combine(Path.GetTempPath(), "Rec");
	}

	private void AddClipToShareQueue(AddClipToShareQueuePackage package)
	{
		VideoClipShareJob item = new VideoClipShareJob(package.VideoHandle, package.ClipID, package.ClipOwner, package.isReRequest);
		m_sharingQueue.Add(item);
		UnityEngine.Debug.Log($"Player {package.ClipOwner} encoded clip {package.ClipID}, added to sharing queue");
	}

	public static List<VideoClipShareJob> GetSharingQueue()
	{
		return RetrievableSingleton<RecordingsHandler>.Instance.m_sharingQueue;
	}

	public static VideoClipShareJob GetCurrentSharingJob()
	{
		return RetrievableSingleton<RecordingsHandler>.Instance.m_currentJob;
	}

	private void OnActivateNextSharingJob(ActivateNextSharingJobPackage package)
	{
		if (m_sharingQueue[0].ClipID.Equals(package.ClipID))
		{
			m_currentJob = m_sharingQueue[0];
			m_sharingQueue.RemoveAt(0);
		}
		else
		{
			UnityEngine.Debug.LogError("FATAL SHARING ERROR. Clip ID's do not match, JOBS ARE NOW WRONG VERY BAD...");
		}
	}

	private void SendIsComplete(SendClipCompletedPackage package)
	{
		VerboseDebug.Log("Received clip is complete package: " + package.ClipID.ToShortString());
		ClipID clipID = package.ClipID;
		if (TryGetRecording(package.VideoHandle, out var recording) && recording.TryGetClip(clipID, out var clip))
		{
			StartCoroutine(CheckIfClipIsComplete(clip));
		}
		if (m_currentJob == null)
		{
			UnityEngine.Debug.LogError("No current job to check");
		}
		else if (m_currentJob.ClipID.Equals(package.ClipID))
		{
			m_currentJob = null;
			VerboseDebug.Log("Clip is complete, removing from current job");
		}
		else
		{
			UnityEngine.Debug.LogError("FATAL SHARING ERROR. Clip ID's do not match, JOBS ARE NOW WRONG VERY BAD...");
		}
	}

	private void OnReRequestClip(ReRequestClipPackage obj)
	{
		UnityEngine.Debug.Log("Received re-request clip package: " + obj.ClipID.ToShortString());
		if (!TryGetRecording(obj.VideoHandle, out var recording) || !recording.TryGetClip(obj.ClipID, out var clip))
		{
			return;
		}
		int valueOrDefault = m_clipRequestCount.GetValueOrDefault(obj.ClipID, 0);
		if (clip.local && valueOrDefault < 4)
		{
			if (!m_clipRequestCount.ContainsKey(obj.ClipID))
			{
				m_clipRequestCount.Add(obj.ClipID, 0);
			}
			m_clipRequestCount[obj.ClipID] = valueOrDefault + 1;
			CustomCommands<CustomCommandType>.SendPackage(new AddClipToShareQueuePackage
			{
				ClipID = clip.clipID,
				VideoHandle = clip.m_recording.videoHandle,
				ClipOwner = PhotonNetwork.LocalPlayer.ActorNumber,
				isReRequest = true
			}, ReceiverGroup.All);
		}
	}

	private IEnumerator CheckIfClipIsComplete(Clip clip)
	{
		yield return new WaitForSeconds(2f);
		if (!clip.local && !clip.hasBeenRecieved)
		{
			UnityEngine.Debug.LogError($"Clip chunk has not been recieved for {clip.clipID}, re-requesting...");
			RetrievableSingleton<PhotonSendVideoHandler>.Instance.ClearChunksForClip(clip.clipID);
			CustomCommands<CustomCommandType>.SendPackage(new ReRequestClipPackage
			{
				ClipID = clip.clipID,
				VideoHandle = clip.m_recording.videoHandle
			}, ReceiverGroup.All);
		}
	}

	public static void RecievedClip(VideoHandle chunkVideoID, ClipID chunkClipID, ContentBuffer contentBuffer)
	{
		if (RetrievableSingleton<RecordingsHandler>.Instance.m_recordings.TryGetValue(chunkVideoID, out var value))
		{
			if (value.TryGetClip(chunkClipID, out var clip))
			{
				clip.SetContentBufffer(contentBuffer);
				clip.hasBeenRecieved = true;
			}
			else
			{
				UnityEngine.Debug.LogError($"Error: Received clip data for clip that we don't have! Ignoring clip data. video={chunkVideoID} clip={chunkClipID}");
			}
		}
		else
		{
			UnityEngine.Debug.LogError($"Error: Received clip data for video that we don't have! Ignoring clip data. video={chunkVideoID} clip={chunkClipID}");
		}
	}

	public static bool TryGetRecording(VideoHandle tVideoID, out CameraRecording recording)
	{
		return RetrievableSingleton<RecordingsHandler>.Instance.m_recordings.TryGetValue(tVideoID, out recording);
	}

	public static bool TryGetRecordingPath(VideoHandle videoHandle, out string path)
	{
		if (TryGetRecording(videoHandle, out var recording))
		{
			path = Path.Combine(recording.GetDirectory(), "fullRecording.webm");
			return true;
		}
		path = null;
		return false;
	}

	public void ClearRecordings()
	{
		m_recordings.Clear();
		m_camerasCurrentRecording.Clear();
		m_playersRecording.Clear();
	}

	protected override void OnRemoved()
	{
		base.OnRemoved();
		foreach (KeyValuePair<VideoHandle, CameraRecording> recording in m_recordings)
		{
			int clipCount = recording.Value.ClipCount;
			for (int i = 0; i < clipCount; i++)
			{
				VideoRecorder recorder = recording.Value.GetClip(i).GetRecorder();
				if (recorder != null)
				{
					UnityEngine.Object.Destroy(recorder.gameObject);
				}
			}
		}
	}
}
