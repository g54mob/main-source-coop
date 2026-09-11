using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Zorro.Core;

public class DebugRecordingCell : VisualElement
{
	private Label m_videoID;

	private Label m_cameraID;

	private VisualTreeAsset clipCell;

	private VisualTreeAsset contentCell;

	private CameraRecording m_recording;

	private Dictionary<ClipID, ClipCell> m_clipCells;

	private VisualElement m_clipParent;

	private VisualElement m_bestContentParent;

	private VisualElement m_commentsParent;

	public DebugRecordingCell(VisualTreeAsset visualTreeAsset, VisualTreeAsset clipCell, VisualTreeAsset contentCell, CameraRecording cameraRecording)
	{
		m_clipCells = new Dictionary<ClipID, ClipCell>();
		this.clipCell = clipCell;
		this.contentCell = contentCell;
		m_recording = cameraRecording;
		visualTreeAsset.CloneTree(this);
		m_videoID = this.Q<Label>("VideoID");
		m_cameraID = this.Q<Label>("CameraID");
		m_clipParent = this.Q("ClipParent");
		m_bestContentParent = this.Q("BestContent");
		m_commentsParent = this.Q("Comments");
		m_videoID.text = cameraRecording.videoHandle.id.ToShortString();
	}

	public void Update(bool isRecording, Guid cameraID, int playerID)
	{
		m_cameraID.text = (isRecording ? $"RECORDING... CameraID: {cameraID.ToShortString()}, Client: {playerID}" : "");
		if (m_clipCells.Count != m_recording.ClipCount)
		{
			for (int i = m_clipCells.Count; i < m_recording.ClipCount; i++)
			{
				AddClip(m_recording.GetClip(i));
			}
		}
		foreach (ClipCell value in m_clipCells.Values)
		{
			value.Update();
		}
	}

	public void AddClip(Clip clip)
	{
		Debug.Log("Adding clip UI: " + clip.clipID.ToShortString());
		ClipCell clipCell = new ClipCell(this.clipCell, clip);
		m_clipParent.Add(clipCell);
		m_clipCells.Add(clip.clipID, clipCell);
	}

	public void ClearContent()
	{
		m_bestContentParent.Clear();
		m_commentsParent.Clear();
	}

	public void SetContent(ContentBuffer contentBuffer)
	{
		ClearContent();
		foreach (ContentBuffer.BufferedContent item in contentBuffer.buffer)
		{
			string label = item.frame.contentEvent.GetName() + " - " + item.score.ToString("F1");
			ContentCell child = new ContentCell(contentCell, label);
			m_bestContentParent.Add(child);
		}
		foreach (Comment item2 in contentBuffer.GenerateComments())
		{
			ContentCell child2 = new ContentCell(contentCell, item2.Text);
			m_commentsParent.Add(child2);
		}
	}
}
