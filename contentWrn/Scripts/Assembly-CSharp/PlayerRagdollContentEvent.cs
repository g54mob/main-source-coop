using System.Collections.Generic;
using UnityEngine;
using Zorro.Core;

public class PlayerRagdollContentEvent : PlayerBaseEvent
{
	public static string[] RAGDOLL_COMMENTS = new string[4] { "Content_PlayerRagdoll_0", "Content_PlayerRagdoll_1", "Content_PlayerRagdoll_2", "Content_PlayerRagdoll_3" };

	public PlayerRagdollContentEvent()
	{
	}

	public PlayerRagdollContentEvent(string playerName, int actorNumber, Vector3 worldPosition)
		: base(playerName, actorNumber, worldPosition)
	{
	}

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.playerRagdollScore;
	}

	public override ushort GetID()
	{
		return 1021;
	}

	public override string GetName()
	{
		return "PlayerRagdoll";
	}

	public override string[] GetAllComments()
	{
		return RAGDOLL_COMMENTS;
	}

	public override Comment GenerateComment()
	{
		List<string> list = new List<string>();
		list.AddRange(RAGDOLL_COMMENTS);
		return new Comment(list.GetRandom(), playerName);
	}
}
