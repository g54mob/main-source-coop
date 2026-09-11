using Zorro.Core;

public class ToolkitWhiskContentEvent : MonsterContentEvent
{
	public static string[] NORMAL_COMMENTS = new string[17]
	{
		"Content_Toolkit_0", "Content_Toolkit_1", "Content_Toolkit_2", "Content_Toolkit_3", "Content_Toolkit_4", "Content_Toolkit_5", "Content_Toolkit_6", "Content_Toolkit_7", "Content_Toolkit_8", "Content_Toolkit_9",
		"Content_Toolkit_10", "Content_Toolkit_11", "Content_Toolkit_12", "Content_Toolkit_13", "Content_Toolkit_14", "Content_Toolkit_15", "Content_Toolkit_16"
	};

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.ToolkitWhiskScore;
	}

	public override ushort GetID()
	{
		return 1013;
	}

	public override string GetName()
	{
		return "Toolkit";
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
