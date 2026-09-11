using Zorro.Core;
using pworld.Scripts.Extensions;

public class SnailSpawnerContentEvent : MonsterContentEvent
{
	public static string[] NORMAL_COMMENTS = new string[4] { "Content_SnailSpawner_0", "Content_SnailSpawner_1", "Content_SnailSpawner_2", "Content_SnailSpawner_3" };

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.snailSpawnerScore;
	}

	public override ushort GetID()
	{
		return 1040;
	}

	public override string GetName()
	{
		return "SnailSpawner";
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
