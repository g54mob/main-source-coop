using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class DummyContentProvider : ContentProvider
{
	public enum PlayerState
	{
		Alive = 0,
		Dead = 1,
		HoldingMic = 2,
		Emoteing = 3
	}

	public PlayerState State;

	public Transform HeadTransform;

	private PhotonView m_photonView;

	public Item EmoteItem;

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
		switch (State)
		{
		case PlayerState.Alive:
			return new PlayerContentEvent("Dummy", m_photonView.ViewID, 1f, 0f, HeadTransform.position);
		case PlayerState.Dead:
			return new PlayerDeadContentEvent("Dummy", m_photonView.ViewID, HeadTransform.position);
		case PlayerState.HoldingMic:
			return new PlayerHoldingMicContentEvent("Dummy", m_photonView.ViewID, HeadTransform.position);
		case PlayerState.Emoteing:
			if (EmoteItem == null)
			{
				Debug.LogError("assign emote item");
			}
			return new PlayerEmoteContentEvent("Dummy", m_photonView.ViewID, EmoteItem, HeadTransform.position);
		default:
			return null;
		}
	}
}
