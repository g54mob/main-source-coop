using Zorro.Core;

public class KnifoContentEvent : MonsterContentEvent
{
	public static string[] NORMAL_COMMENTS = new string[29]
	{
		"Content_Knifo_0", "Content_Knifo_1", "Content_Knifo_2", "Content_Knifo_3", "Content_Knifo_4", "Content_Knifo_5", "Content_Knifo_6", "Content_Knifo_7", "Content_Knifo_8", "Content_Knifo_9",
		"Content_Knifo_10", "Content_Knifo_11", "Content_Knifo_12", "Content_Knifo_13", "Content_Knifo_14", "Content_Knifo_15", "Content_Knifo_16", "Content_Knifo_17", "Content_Knifo_18", "Content_Knifo_19",
		"Content_Knifo_20", "Content_Knifo_21", "Content_Knifo_22", "Content_Knifo_23", "Content_Knifo_24", "Content_Knifo_25", "Content_Knifo_26", "Content_Knifo_27", "Content_Knifo_28"
	};

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.KnifoScore;
	}

	public override ushort GetID()
	{
		return 1006;
	}

	public override string GetName()
	{
		return "Knifo";
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
