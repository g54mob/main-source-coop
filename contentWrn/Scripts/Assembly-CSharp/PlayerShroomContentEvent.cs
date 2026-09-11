using System.Collections.Generic;
using UnityEngine;
using Zorro.Core;

public class PlayerShroomContentEvent : PlayerBaseEvent
{
	public static List<string> comments = new List<string> { "Content_Shroom_0", "Content_Shroom_1", "Content_Shroom_2", "Content_Shroom_3", "Content_Shroom_4", "Content_Shroom_5", "Content_Shroom_6" };

	public PlayerShroomContentEvent()
	{
	}

	public PlayerShroomContentEvent(string playerName, int actorNumber, Vector3 worldPosition)
		: base(playerName, actorNumber, worldPosition)
	{
	}

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.playerRagdollScore;
	}

	public override ushort GetID()
	{
		return 1031;
	}

	public override string GetName()
	{
		return "Shroom";
	}

	public override string[] GetAllComments()
	{
		return comments.ToArray();
	}

	public override Comment GenerateComment()
	{
		return new Comment(comments.GetRandom(), playerName);
	}
}
