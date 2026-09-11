using Zorro.Core;

public class WeepingContentEventSuccess : MonsterContentEvent
{
	public static string[] SUCCESS_COMMENTS = new string[6] { "Content_Weeping_Success_0", "Content_Weeping_Success_1", "Content_Weeping_Success_2", "Content_Weeping_Success_3", "Content_Weeping_Success_4", "Content_Weeping_Success_5" };

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.WeepingScoreSuccuess;
	}

	public override ushort GetID()
	{
		return 1016;
	}

	public override string GetName()
	{
		return "Weeping_Success";
	}

	public override string[] GetAllComments()
	{
		return SUCCESS_COMMENTS;
	}

	public override Comment GenerateComment()
	{
		return new Comment(SUCCESS_COMMENTS.GetRandom());
	}
}
