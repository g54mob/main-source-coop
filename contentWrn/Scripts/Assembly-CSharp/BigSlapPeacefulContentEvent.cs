using Zorro.Core;

public class BigSlapPeacefulContentEvent : MonsterContentEvent
{
	public static string[] NORMAL_COMMENTS = new string[22]
	{
		"Content_BigSlapPeaceful_0", "Content_BigSlapPeaceful_1", "Content_BigSlapPeaceful_2", "Content_BigSlapPeaceful_3", "Content_BigSlapPeaceful_4", "Content_BigSlapPeaceful_5", "Content_BigSlapPeaceful_6", "Content_BigSlapPeaceful_7", "Content_BigSlapPeaceful_8", "Content_BigSlapPeaceful_9",
		"Content_BigSlapPeaceful_10", "Content_BigSlapPeaceful_11", "Content_BigSlapPeaceful_12", "Content_BigSlapPeaceful_13", "Content_BigSlapPeaceful_14", "Content_BigSlapPeaceful_15", "Content_BigSlapPeaceful_16", "Content_BigSlapPeaceful_17", "Content_BigSlapPeaceful_18", "Content_BigSlapPeaceful_19",
		"Content_BigSlapPeaceful_20", "Content_BigSlapPeaceful_21"
	};

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.BigSlapPeaceful;
	}

	public override ushort GetID()
	{
		return 1001;
	}

	public override string GetName()
	{
		return "BigSlapPeaceful";
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
