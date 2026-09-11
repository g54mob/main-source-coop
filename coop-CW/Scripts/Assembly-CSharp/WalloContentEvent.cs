using Zorro.Core;
using pworld.Scripts.Extensions;

public class WalloContentEvent : MonsterContentEvent
{
	public static string[] NORMAL_COMMENTS = new string[4] { "Content_Wallo_0", "Content_Wallo_1", "Content_Wallo_2", "Content_Wallo_3" };

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.walloScore;
	}

	public override ushort GetID()
	{
		return 1036;
	}

	public override string GetName()
	{
		return "Wallo";
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
