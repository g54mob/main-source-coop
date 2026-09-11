using Zorro.Core;
using pworld.Scripts.Extensions;

public class WormContentEvent : MonsterContentEvent
{
	public static string[] NORMAL_COMMENTS = new string[9] { "Content_Worm_0", "Content_Worm_1", "Content_Worm_2", "Content_Worm_3", "Content_Worm_4", "Content_Worm_5", "Content_Worm_6", "Content_Worm_7", "Content_Worm_8" };

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.wormScore;
	}

	public override ushort GetID()
	{
		return 1039;
	}

	public override string GetName()
	{
		return "Worm";
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
