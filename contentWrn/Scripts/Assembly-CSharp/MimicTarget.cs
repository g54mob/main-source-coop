using UnityEngine;

public class MimicTarget
{
	public Vector3 LocalPos { get; private set; }

	public Transform VoiceParent { get; private set; }

	public Transform OverrideParent { get; private set; }

	public GameObject RemoteVoice { get; private set; }

	public MimicTarget(Vector3 localPos, Transform voiceParent, GameObject remoteVoice, Transform overrideParent)
	{
		LocalPos = localPos;
		VoiceParent = voiceParent;
		RemoteVoice = remoteVoice;
		OverrideParent = overrideParent;
	}

	public void Switch()
	{
		Debug.Log("Switching Mimic Targets FROM", VoiceParent);
		Debug.Log("Switching Mimic Targets TO", OverrideParent);
		RemoteVoice.transform.parent = OverrideParent;
		RemoteVoice.transform.localPosition = Vector3.zero;
	}

	public void Reset()
	{
		Debug.Log("Resetting Mimic Targets FROM", RemoteVoice.transform.parent);
		Debug.Log("Resetting Mimic Targets TO", VoiceParent);
		RemoteVoice.transform.parent = VoiceParent;
		RemoteVoice.transform.localPosition = LocalPos;
	}
}
