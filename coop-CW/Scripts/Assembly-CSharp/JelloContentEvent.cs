using Zorro.Core;

public class JelloContentEvent : MonsterContentEvent
{
	public static string[] NORMAL_COMMENTS = new string[16]
	{
		"Content_Jello_0", "Content_Jello_1", "Content_Jello_2", "Content_Jello_3", "Content_Jello_4", "Content_Jello_5", "Content_Jello_6", "Content_Jello_7", "Content_Jello_8", "Content_Jello_9",
		"Content_Jello_10", "Content_Jello_11", "Content_Jello_12", "Content_Jello_13", "Content_Jello_14", "Content_Jello_15"
	};

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.JelloScore;
	}

	public override ushort GetID()
	{
		return 1005;
	}

	public override string GetName()
	{
		return "Jello";
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
