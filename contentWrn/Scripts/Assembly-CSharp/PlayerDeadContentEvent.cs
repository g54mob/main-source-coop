using System.Collections.Generic;
using UnityEngine;
using Zorro.Core;

public class PlayerDeadContentEvent : PlayerBaseEvent
{
	public static string[] DEAD_COMMENTS = new string[5] { "Content_PlayerDead_0", "Content_PlayerDead_1", "Content_PlayerDead_2", "Content_PlayerDead_3", "Content_PlayerDead_4" };

	public PlayerDeadContentEvent()
	{
	}

	public PlayerDeadContentEvent(string playerName, int actorNumber, Vector3 worldPosition)
		: base(playerName, actorNumber, worldPosition)
	{
	}

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.playerDeadScore;
	}

	public override ushort GetID()
	{
		return 1020;
	}

	public override string GetName()
	{
		return "PlayerDead";
	}

	public override string[] GetAllComments()
	{
		return DEAD_COMMENTS;
	}

	public override Comment GenerateComment()
	{
		List<string> list = new List<string>();
		list.AddRange(DEAD_COMMENTS);
		return new Comment(list.GetRandom(), playerName);
	}
}
