using Zorro.Core;

public class BigSlapAgroContentEvent : MonsterContentEvent
{
	public static string[] NORMAL_COMMENTS = new string[27]
	{
		"Content_BigSlapAgro_0", "Content_BigSlapAgro_1", "Content_BigSlapAgro_2", "Content_BigSlapAgro_3", "Content_BigSlapAgro_4", "Content_BigSlapAgro_5", "Content_BigSlapAgro_6", "Content_BigSlapAgro_7", "Content_BigSlapAgro_8", "Content_BigSlapAgro_9",
		"Content_BigSlapAgro_10", "Content_BigSlapAgro_11", "Content_BigSlapAgro_12", "Content_BigSlapAgro_13", "Content_BigSlapAgro_14", "Content_BigSlapAgro_15", "Content_BigSlapAgro_16", "Content_BigSlapAgro_17", "Content_BigSlapAgro_18", "Content_BigSlapAgro_19",
		"Content_BigSlapAgro_20", "Content_BigSlapAgro_21", "Content_BigSlapAgro_22", "Content_BigSlapAgro_23", "Content_BigSlapAgro_24", "Content_BigSlapAgro_25", "Content_BigSlapAgro_26"
	};

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.BigSlapAgro;
	}

	public override ushort GetID()
	{
		return 1002;
	}

	public override string GetName()
	{
		return "BigSlapAgro";
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
