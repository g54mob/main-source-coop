using Zorro.Core;
using pworld.Scripts.Extensions;

public class FireMonsterContentEvent : MonsterContentEvent
{
	public static string[] NORMAL_COMMENTS = new string[5] { "Content_FireMonster_0", "Content_FireMonster_1", "Content_FireMonster_2", "Content_FireMonster_3", "Content_FireMonster_4" };

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.fireMonsterScore;
	}

	public override ushort GetID()
	{
		return 1041;
	}

	public override string GetName()
	{
		return "FireMonster";
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
