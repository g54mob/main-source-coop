using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Zorro.Core;
using Zorro.Core.CLI;

public class VideoDebugPage : DebugPage
{
	private VisualElement m_contentParent;

	private VisualTreeAsset m_videoCellTemplate;

	private VisualTreeAsset m_clipCell;

	private VisualTreeAsset m_contentCell;

	private Label m_currentlySharingLabel;

	private Dictionary<VideoHandle, DebugRecordingCell> m_recordingCells;

	private VisualElement m_queueParent;

	public VideoDebugPage(VisualTreeAsset visualTreeAsset, VisualTreeAsset videoCellTemplate, VisualTreeAsset clipCell, VisualTreeAsset contentCell)
	{
		visualTreeAsset.CloneTree(this);
		m_contentCell = contentCell;
		m_recordingCells = new Dictionary<VideoHandle, DebugRecordingCell>();
		m_videoCellTemplate = videoCellTemplate;
		m_clipCell = clipCell;
		m_contentParent = this.Q("CONTENT");
		m_queueParent = this.Q("SharingQueueParent");
		m_currentlySharingLabel = this.Q<Label>("CurrentlySharingLabel");
		foreach (CameraRecording value in RecordingsHandler.GetRecordings().Values)
		{
			AddRecording(value);
		}
	}

	public void AddRecording(CameraRecording cameraRecorder)
	{
		DebugRecordingCell debugRecordingCell = new DebugRecordingCell(m_videoCellTemplate, m_clipCell, m_contentCell, cameraRecorder);
		m_contentParent.Add(debugRecordingCell);
		m_recordingCells.Add(cameraRecorder.videoHandle, debugRecordingCell);
	}

	public override void Update()
	{
		base.Update();
		BidirectionalDictionary<Guid, VideoHandle> camerasCurrentRecording = RecordingsHandler.GetCamerasCurrentRecording();
		BidirectionalDictionary<int, VideoHandle> playersRecording = RecordingsHandler.GetPlayersRecording();
		Dictionary<VideoHandle, CameraRecording>.ValueCollection values = RecordingsHandler.GetRecordings().Values;
		foreach (CameraRecording item in values)
		{
			if (!m_recordingCells.ContainsKey(item.videoHandle))
			{
				AddRecording(item);
			}
			Guid value;
			bool isRecording = camerasCurrentRecording.TryGetValue(item.videoHandle, out value);
			if (!playersRecording.TryGetValue(item.videoHandle, out var value2))
			{
				value2 = -1;
			}
			m_recordingCells[item.videoHandle].Update(isRecording, value, value2);
		}
		List<VideoClipShareJob> sharingQueue = RecordingsHandler.GetSharingQueue();
		VideoClipShareJob currentSharingJob = RecordingsHandler.GetCurrentSharingJob();
		m_queueParent.Clear();
		foreach (VideoClipShareJob item2 in sharingQueue)
		{
			SharingClipCell child = new SharingClipCell(m_clipCell, item2.ClipID, item2.IsLocal);
			m_queueParent.Add(child);
		}
		m_currentlySharingLabel.text = ((currentSharingJob == null) ? "Currently not sharing any clips..." : ($"Player: {currentSharingJob.ClipOwner} is currently sharing: " + currentSharingJob.ClipID.ToMiniString()));
		if (!Input.GetKeyDown(KeyCode.F4))
		{
			return;
		}
		foreach (CameraRecording item3 in values)
		{
			DebugRecordingCell debugRecordingCell = m_recordingCells[item3.videoHandle];
			if (ContentEvaluator.EvaluateRecording(item3, out var buffer))
			{
				Debug.Log("Recording score: " + buffer.GetScore());
				debugRecordingCell.SetContent(buffer);
			}
			else
			{
				debugRecordingCell.ClearContent();
			}
		}
	}
}
