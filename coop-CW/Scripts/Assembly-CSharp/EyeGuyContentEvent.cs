using Zorro.Core;

public class EyeGuyContentEvent : MonsterContentEvent
{
	public static string[] NORMAL_COMMENTS = new string[10] { "Content_EyeGuy_0", "Content_EyeGuy_1", "Content_EyeGuy_2", "Content_EyeGuy_3", "Content_EyeGuy_4", "Content_EyeGuy_5", "Content_EyeGuy_6", "Content_EyeGuy_7", "Content_EyeGuy_8", "Content_EyeGuy_9" };

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.eyeGuyScore;
	}

	public override ushort GetID()
	{
		return 1025;
	}

	public override string GetName()
	{
		return "EyeGuy";
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
