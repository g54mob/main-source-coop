using Photon.Pun;
using Photon.Voice.PUN;
using UnityEngine;

public class VoiceRemoteMimic
{
	public MimicTarget MimicTarget;

	public void MakeMimicTargets(PhotonView mimicPlayer, Transform mimicParent)
	{
		Debug.Log("Making Mimic!");
		Transform transform = mimicPlayer.GetComponentInChildren<PhotonVoiceView>().transform;
		MimicTarget = new MimicTarget(transform.localPosition, transform.parent, transform.gameObject, mimicParent);
	}

	public PhotonView SearchForTargetToMimic(PhotonView ownerID)
	{
		PhotonVoiceView[] array = Object.FindObjectsOfType<PhotonVoiceView>();
		for (int i = 0; i < array.Length; i++)
		{
			PhotonView component = array[i].GetComponent<PhotonView>();
			if (ownerID.OwnerActorNr != component.OwnerActorNr)
			{
				Debug.Log("Found Target To Mimic Remote Voice: " + component.Owner.ActorNumber);
				return component;
			}
		}
		return null;
	}

	public void SwitchMimics()
	{
		MimicTarget.Switch();
	}

	public void ResetMimicTargets()
	{
		if (MimicTarget != null)
		{
			MimicTarget.Reset();
		}
		MimicTarget = null;
	}
}
