using Zorro.Core;

public class LarvaContentEvent : MonsterContentEvent
{
	public static string[] NORMAL_COMMENTS = new string[14]
	{
		"Content_Larva_0", "Content_Larva_1", "Content_Larva_2", "Content_Larva_3", "Content_Larva_4", "Content_Larva_5", "Content_Larva_6", "Content_Larva_7", "Content_Larva_8", "Content_Larva_9",
		"Content_Larva_10", "Content_Larva_11", "Content_Larva_12", "Content_Larva_13"
	};

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.larvaScore;
	}

	public override ushort GetID()
	{
		return 1018;
	}

	public override string GetName()
	{
		return "Larva";
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
