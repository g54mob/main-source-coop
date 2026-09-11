using Zorro.Core;

public class SnatchoContentEvent : MonsterContentEvent
{
	public static string[] NORMAL_COMMENTS = new string[27]
	{
		"Content_Snatcho_0", "Content_Snatcho_1", "Content_Snatcho_2", "Content_Snatcho_3", "Content_Snatcho_4", "Content_Snatcho_5", "Content_Snatcho_6", "Content_Snatcho_7", "Content_Snatcho_8", "Content_Snatcho_9",
		"Content_Snatcho_10", "Content_Snatcho_11", "Content_Snatcho_12", "Content_Snatcho_13", "Content_Snatcho_14", "Content_Snatcho_15", "Content_Snatcho_16", "Content_Snatcho_17", "Content_Snatcho_18", "Content_Snatcho_19",
		"Content_Snatcho_20", "Content_Snatcho_21", "Content_Snatcho_22", "Content_Snatcho_23", "Content_Snatcho_24", "Content_Snatcho_25", "Content_Snatcho_26"
	};

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.SnatchoScore;
	}

	public override ushort GetID()
	{
		return 1011;
	}

	public override string GetName()
	{
		return "Snatcho";
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
