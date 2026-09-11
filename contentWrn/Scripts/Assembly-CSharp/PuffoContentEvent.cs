using Zorro.Core;
using pworld.Scripts.Extensions;

public class PuffoContentEvent : MonsterContentEvent
{
	public static string[] NORMAL_COMMENTS = new string[5] { "Content_Puffo_0", "Content_Puffo_1", "Content_Puffo_2", "Content_Puffo_3", "Content_Puffo_4" };

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.puffoScore;
	}

	public override ushort GetID()
	{
		return 1042;
	}

	public override string GetName()
	{
		return "Puffo";
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
