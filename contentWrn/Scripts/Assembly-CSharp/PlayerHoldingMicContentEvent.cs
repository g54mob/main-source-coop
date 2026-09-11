using System.Collections.Generic;
using UnityEngine;
using Zorro.Core;

public class PlayerHoldingMicContentEvent : PlayerBaseEvent
{
	public static string[] HOLDING_MIC_COMMENTS = new string[1] { "Content_PlayerHoldingMic_0" };

	public PlayerHoldingMicContentEvent()
	{
	}

	public PlayerHoldingMicContentEvent(string playerName, int actorNumber, Vector3 worldPosition)
		: base(playerName, actorNumber, worldPosition)
	{
	}

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.playerHoldingMicScore;
	}

	public override ushort GetID()
	{
		return 1032;
	}

	public override string GetName()
	{
		return "PlayerHoldingMic";
	}

	public override string[] GetAllComments()
	{
		return HOLDING_MIC_COMMENTS;
	}

	public override Comment GenerateComment()
	{
		List<string> list = new List<string>();
		list.AddRange(HOLDING_MIC_COMMENTS);
		return new Comment(list.GetRandom(), playerName);
	}
}
