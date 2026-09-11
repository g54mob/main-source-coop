using Zorro.Core;

public class DogContentEvent : MonsterContentEvent
{
	public static string[] NORMAL_COMMENTS = new string[13]
	{
		"Content_Dog_0", "Content_Dog_1", "Content_Dog_2", "Content_Dog_3", "Content_Dog_4", "Content_Dog_5", "Content_Dog_6", "Content_Dog_7", "Content_Dog_8", "Content_Dog_9",
		"Content_Dog_10", "Content_Dog_11", "Content_Dog_12"
	};

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.dogScore;
	}

	public override ushort GetID()
	{
		return 1024;
	}

	public override string GetName()
	{
		return "Dog";
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
