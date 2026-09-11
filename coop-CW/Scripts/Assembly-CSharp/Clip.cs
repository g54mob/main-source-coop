using System;
using System.IO;
using UnityEngine;
using Zorro.Recorder;

[Serializable]
public class Clip
{
	public CameraRecording m_recording;

	public ClipID clipID;

	public bool local;

	public int ownerID;

	public bool isRecording;

	public bool encodingQueued;

	public bool encoded;

	public bool hasBeenRecieved;

	public float m_timeStarted;

	public float m_timeEnded;

	private ContentBuffer m_contentBuffer;

	private VideoRecorder m_recorder;

	public bool Valid { get; protected set; }

	public float ClipLength => m_timeEnded - m_timeStarted;

	public Clip(ClipID clipID, bool local, int ownerID, CameraRecording recording)
	{
		this.ownerID = ownerID;
		this.clipID = clipID;
		this.local = local;
		isRecording = true;
		m_recording = recording;
		m_timeStarted = Time.time;
		Valid = true;
	}

	public void EndClip()
	{
		m_timeEnded = Time.time;
		isRecording = false;
		if (m_recorder != null && !m_recorder.HasRecorded)
		{
			Debug.LogError("Clip: CLIP HAS NO FRAMES!");
		}
		VerboseDebug.Log("Ended clip: " + clipID.ToShortString());
		if (m_contentBuffer != null)
		{
			m_contentBuffer.PickBest();
		}
	}

	public void SetRecorder(VideoRecorder recorder)
	{
		m_recorder = recorder;
	}

	public VideoRecorder GetRecorder()
	{
		return m_recorder;
	}

	public string GetClipDirectory()
	{
		return Path.Combine(m_recording.GetDirectory(), clipID.id.ToString());
	}

	public void SetContentBufffer(ContentBuffer contentBuffer)
	{
		m_contentBuffer = contentBuffer;
	}

	public bool TryGetContentBuffer(out ContentBuffer contentBuffer)
	{
		contentBuffer = m_contentBuffer;
		return m_contentBuffer != null;
	}

	public void SetValid(bool validClip)
	{
		Valid = validClip;
	}
}
