using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class DummyMonsterContentProvider : ContentProvider
{
	public enum Monster
	{
		Zombe = 0,
		Mouthe = 1,
		Eye = 2,
		Ear = 3,
		Weeping = 4,
		BigSlap = 5
	}

	public Monster State;

	public Transform HeadTransform;

	private PhotonView m_photonView;

	private void Awake()
	{
		m_photonView = GetComponent<PhotonView>();
	}

	public override void GetContent(List<ContentEventFrame> contentEvents, float seenAmount, Camera camera, float time)
	{
		ContentEvent contentEvent = GetEvent();
		if (contentEvent != null)
		{
			contentEvents.Add(new ContentEventFrame(contentEvent, seenAmount, time));
		}
	}

	private ContentEvent GetEvent()
	{
		return State switch
		{
			Monster.Zombe => GetContentEvent<ZombieContentEvent>(), 
			Monster.Ear => GetContentEvent<EarContentEvent>(), 
			Monster.Eye => GetContentEvent<EyeGuyContentEvent>(), 
			Monster.Mouthe => GetContentEvent<MouthContentEvent>(), 
			Monster.Weeping => GetContentEvent<WeepingContentEvent>(), 
			Monster.BigSlap => GetContentEvent<BigSlapPeacefulContentEvent>(), 
			_ => null, 
		};
	}

	public MonsterContentEvent GetContentEvent<T>() where T : MonsterContentEvent, new()
	{
		T val = new T();
		int viewID = 0;
		if (m_photonView != null)
		{
			viewID = m_photonView.ViewID;
		}
		val.viewID = viewID;
		val.worldPosition = HeadTransform.position;
		return val;
	}
}
