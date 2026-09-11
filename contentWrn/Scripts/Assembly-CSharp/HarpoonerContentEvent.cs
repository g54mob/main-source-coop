using Zorro.Core;
using pworld.Scripts.Extensions;

public class HarpoonerContentEvent : MonsterContentEvent
{
	public static string[] NORMAL_COMMENTS = new string[11]
	{
		"Content_Harpooner_0", "Content_Harpooner_1", "Content_Harpooner_2", "Content_Harpooner_3", "Content_Harpooner_4", "Content_Harpooner_5", "Content_Harpooner_6", "Content_Harpooner_7", "Content_Harpooner_8", "Content_Harpooner_9",
		"Content_Harpooner_10"
	};

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.harpoonerScore;
	}

	public override ushort GetID()
	{
		return 1037;
	}

	public override string GetName()
	{
		return "Harpooner";
	}

	public override string[] GetAllComments()
	{
		return NORMAL_COMMENTS;
	}

	public override Comment GenerateComment()
	{
		return new Comment(ExtCollections.GetRandom(NORMAL_COMMENTS));
	}
}
