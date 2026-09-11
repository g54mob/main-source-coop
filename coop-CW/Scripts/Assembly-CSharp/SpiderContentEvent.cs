using Zorro.Core;

public class SpiderContentEvent : MonsterContentEvent
{
	public static string[] NORMAL_COMMENTS = new string[19]
	{
		"Content_Spider_0", "Content_Spider_1", "Content_Spider_2", "Content_Spider_3", "Content_Spider_4", "Content_Spider_5", "Content_Spider_6", "Content_Spider_7", "Content_Spider_8", "Content_Spider_9",
		"Content_Spider_10", "Content_Spider_11", "Content_Spider_12", "Content_Spider_13", "Content_Spider_14", "Content_Spider_15", "Content_Spider_16", "Content_Spider_17", "Content_Spider_18"
	};

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.spiderScore;
	}

	public override ushort GetID()
	{
		return 1019;
	}

	public override string GetName()
	{
		return "Spider";
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
