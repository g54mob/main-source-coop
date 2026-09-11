using System.Collections.Generic;
using UnityEngine;
using Zorro.Core;

public class GoodCatchContentEvent : PlayerBaseEvent
{
	public string[] GOOD_CATCH_COMMENTS = new string[5] { "Content_GoodCatch_0", "Content_GoodCatch_1", "Content_GoodCatch_2", "Content_GoodCatch_3", "Content_GoodCatch_4" };

	public GoodCatchContentEvent()
	{
	}

	public GoodCatchContentEvent(string playerName, int actorNumber, Vector3 worldPosition)
		: base(playerName, actorNumber, worldPosition)
	{
	}

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.playerGoodCatchScore;
	}

	public override ushort GetID()
	{
		return 1027;
	}

	public override string GetName()
	{
		return "GoodCatch";
	}

	public override string[] GetAllComments()
	{
		return GOOD_CATCH_COMMENTS;
	}

	public override Comment GenerateComment()
	{
		List<string> list = new List<string>();
		list.AddRange(GOOD_CATCH_COMMENTS);
		return new Comment(list.GetRandom(), playerName);
	}
}
