using Zorro.Core;

public class MouthContentEvent : MonsterContentEvent
{
	public static string[] NORMAL_COMMENTS = new string[17]
	{
		"Content_Mouth_0", "Content_Mouth_1", "Content_Mouth_2", "Content_Mouth_3", "Content_Mouth_4", "Content_Mouth_5", "Content_Mouth_6", "Content_Mouth_7", "Content_Mouth_8", "Content_Mouth_9",
		"Content_Mouth_10", "Content_Mouth_11", "Content_Mouth_12", "Content_Mouth_13", "Content_Mouth_14", "Content_Mouth_15", "Content_Mouth_16"
	};

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.MouthScore;
	}

	public override ushort GetID()
	{
		return 1009;
	}

	public override string GetName()
	{
		return "Mouth";
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
