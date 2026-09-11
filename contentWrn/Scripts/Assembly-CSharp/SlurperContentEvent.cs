using Zorro.Core;

public class SlurperContentEvent : MonsterContentEvent
{
	public static string[] NORMAL_COMMENTS = new string[15]
	{
		"Content_Slurper_0", "Content_Slurper_1", "Content_Slurper_2", "Content_Slurper_3", "Content_Slurper_4", "Content_Slurper_5", "Content_Slurper_6", "Content_Slurper_7", "Content_Slurper_8", "Content_Slurper_9",
		"Content_Slurper_10", "Content_Slurper_11", "Content_Slurper_12", "Content_Slurper_13", "Content_Slurper_14"
	};

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.SlurperScore;
	}

	public override ushort GetID()
	{
		return 1010;
	}

	public override string GetName()
	{
		return "Slurper";
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
