using Zorro.Core;

public class FlickerContentEvent : MonsterContentEvent
{
	public static string[] NORMAL_COMMENTS = new string[26]
	{
		"Content_Flicker_0", "Content_Flicker_1", "Content_Flicker_2", "Content_Flicker_3", "Content_Flicker_4", "Content_Flicker_5", "Content_Flicker_6", "Content_Flicker_7", "Content_Flicker_8", "Content_Flicker_9",
		"Content_Flicker_10", "Content_Flicker_11", "Content_Flicker_12", "Content_Flicker_13", "Content_Flicker_14", "Content_Flicker_15", "Content_Flicker_16", "Content_Flicker_17", "Content_Flicker_18", "Content_Flicker_19",
		"Content_Flicker_20", "Content_Flicker_21", "Content_Flicker_22", "Content_Flicker_23", "Content_Flicker_24", "Content_Flicker_25"
	};

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.FlickerScore;
	}

	public override ushort GetID()
	{
		return 1004;
	}

	public override string GetName()
	{
		return "Flicker";
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
