using System;
using UnityEngine;

public class DebugVideo : MonoBehaviour
{
	private void Go()
	{
		if (!ItemInstanceDataHandler.TryGetInstanceData(Player.localPlayer.GetComponentInChildren<VideoCamera>().itemInstance.m_guid.Value, out var o))
		{
			Debug.LogError("No data found for camera");
			return;
		}
		if (!o.TryGetEntry<VideoInfoEntry>(out var t))
		{
			Debug.LogError("No VideoInfoEntry found for camera");
			return;
		}
		if (t.videoID.Equals(VideoHandle.Invalid))
		{
			Debug.LogError("VideoID is invalid");
			return;
		}
		byte[] b = t.videoID.id.ToByteArray();
		if (!RecordingsHandler.TryGetRecording(new VideoHandle(new Guid(b)), out var recording))
		{
			Debug.Log("Cant Get Recording");
			return;
		}
		foreach (Clip allClip in recording.GetAllClips())
		{
			allClip.SetValid(validClip: true);
		}
		foreach (Clip allClip2 in recording.GetAllClips())
		{
			allClip2.TryGetContentBuffer(out var contentBuffer);
			foreach (ContentBuffer.BufferedContent item in contentBuffer.buffer)
			{
				Debug.Log("Content " + item.frame.contentEvent.GetName());
			}
		}
		StartCoroutine(RetrievableSingleton<RecordingsHandler>.Instance.ExtractRecording(recording, delegate(bool callbackResult)
		{
			if (!callbackResult)
			{
				Debug.Log("AllClips Not Ready");
			}
		}));
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
