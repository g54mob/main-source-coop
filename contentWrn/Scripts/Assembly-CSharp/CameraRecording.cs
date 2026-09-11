using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zorro.Core;
using Zorro.Recorder;

public class CameraRecording : MonoBehaviour, IPlayableVideo
{
	public VideoHandle videoHandle;

	public List<Clip> m_clips;

	private DateTime m_lastModified;

	public int ClipCount => m_clips.Count;

	public DateTime LastModified => m_lastModified;

	public bool ReadyToExtract(out string failureReason)
	{
		int clipCount = ClipCount;
		failureReason = $"video={videoHandle.id}:";
		bool result = true;
		for (int i = 0; i < clipCount; i++)
		{
			Clip clip = GetClip(i);
			if (!clip.Valid)
			{
				continue;
			}
			if (clip.isRecording)
			{
				failureReason += $" clip {i} of {clipCount} ({clip.clipID.id}) still recording!";
				result = false;
				continue;
			}
			if (clip.local && !clip.encoded)
			{
				failureReason += $" clip {i} of {clipCount} ({clip.clipID.id}) is local but not encoded!";
				result = false;
			}
			if (!clip.local && !clip.hasBeenRecieved)
			{
				failureReason += $" clip {i} of {clipCount} ({clip.clipID.id}) is from another player but not downloaded!";
				result = false;
			}
		}
		return result;
	}

	public void SetInfo(VideoHandle videoID)
	{
		videoHandle = videoID;
		m_clips = new List<Clip>();
	}

	public void AddNewClip(Clip clip)
	{
		m_lastModified = DateTime.Now;
		m_clips.Add(clip);
		Debug.Log($"video {videoHandle.id.ToShortString()} added new clip: {clip.clipID.ToShortString()}, total clips: {m_clips.Count}");
	}

	public Clip EndCurrentClip()
	{
		if (m_clips.Count == 0)
		{
			Debug.LogWarning("No clips to end");
			return null;
		}
		List<Clip> clips = m_clips;
		Clip clip = clips[clips.Count - 1];
		clip.EndClip();
		return clip;
	}

	public Clip GetClip(int i)
	{
		return m_clips[i];
	}

	public string GetDirectory()
	{
		return Path.Combine(RecordingsHandler.GetDirectory(), videoHandle.id.ToString());
	}

	public List<Clip> GetAllClips()
	{
		return m_clips;
	}

	public bool TryGetLatestClip(out Clip clip)
	{
		if (m_clips.Count > 0)
		{
			List<Clip> clips = m_clips;
			clip = clips[clips.Count - 1];
			return true;
		}
		clip = null;
		return false;
	}

	public bool SaveToDesktop(out string videoFileName)
	{
		if (!RecordingsHandler.TryGetRecordingPath(videoHandle, out var path))
		{
			videoFileName = string.Empty;
			return false;
		}
		if (!File.Exists(path))
		{
			Debug.LogError("Video: " + path + " Does Not Exist!");
			videoFileName = string.Empty;
			return false;
		}
		videoFileName = "content_warning_" + videoHandle.id.ToShortString() + ".webm";
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), videoFileName);
		if (string.IsNullOrEmpty(text))
		{
			Debug.Log("Video: " + path + " has Invalid Target Path!");
			return false;
		}
		File.Copy(path, text);
		return true;
	}

	public bool TryGetClip(ClipID clipID, out Clip clip)
	{
		foreach (Clip clip2 in m_clips)
		{
			if (clipID.Equals(clip2.clipID))
			{
				clip = clip2;
				return true;
			}
		}
		clip = null;
		return false;
	}

	public bool TryGetVideoPath(out string path)
	{
		return RecordingsHandler.TryGetRecordingPath(videoHandle, out path);
	}

	public void DisposeClips()
	{
		for (int i = 0; i < m_clips.Count; i++)
		{
			VideoRecorder recorder = m_clips[i].GetRecorder();
			if (recorder != null)
			{
				UnityEngine.Object.Destroy(recorder.gameObject);
			}
		}
		m_clips.Clear();
	}
}
