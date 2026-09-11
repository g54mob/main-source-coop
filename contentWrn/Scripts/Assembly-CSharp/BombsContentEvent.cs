using Zorro.Core;

public class BombsContentEvent : MonsterContentEvent
{
	public static string[] NORMAL_COMMENTS = new string[25]
	{
		"Content_Bombs_0", "Content_Bombs_1", "Content_Bombs_2", "Content_Bombs_3", "Content_Bombs_4", "Content_Bombs_5", "Content_Bombs_6", "Content_Bombs_7", "Content_Bombs_8", "Content_Bombs_9",
		"Content_Bombs_10", "Content_Bombs_11", "Content_Bombs_12", "Content_Bombs_13", "Content_Bombs_14", "Content_Bombs_15", "Content_Bombs_16", "Content_Bombs_17", "Content_Bombs_18", "Content_Bombs_19",
		"Content_Bombs_20", "Content_Bombs_21", "Content_Bombs_22", "Content_Bombs_23", "Content_Bombs_24"
	};

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.bombsScore;
	}

	public override ushort GetID()
	{
		return 1017;
	}

	public override string GetName()
	{
		return "Bombs";
	}

	public override string[] GetAllComments()
	{
		return NORMAL_COMMENTS;
	}

	public override Comment GenerateComment()
	{
		return new Comment(NORMAL_COMMENTS.GetRandom());
	}
}
