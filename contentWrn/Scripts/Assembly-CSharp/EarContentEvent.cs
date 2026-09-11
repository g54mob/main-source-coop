using Zorro.Core;

public class EarContentEvent : MonsterContentEvent
{
	public static string[] NORMAL_COMMENTS = new string[15]
	{
		"Content_Ear_0", "Content_Ear_1", "Content_Ear_2", "Content_Ear_3", "Content_Ear_4", "Content_Ear_5", "Content_Ear_6", "Content_Ear_7", "Content_Ear_8", "Content_Ear_9",
		"Content_Ear_10", "Content_Ear_11", "Content_Ear_12", "Content_Ear_13", "Content_Ear_14"
	};

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.EarScore;
	}

	public override ushort GetID()
	{
		return 1008;
	}

	public override string GetName()
	{
		return "Ear";
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
