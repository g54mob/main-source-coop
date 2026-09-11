using Zorro.Core;

public class WeepingContentEventFail : MonsterContentEvent
{
	public static string[] FAIL_COMMENTS = new string[4] { "Content_Weeping_Fail_0", "Content_Weeping_Fail_1", "Content_Weeping_Fail_2", "Content_Weeping_Fail_3" };

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.WeepingScoreFail;
	}

	public override ushort GetID()
	{
		return 1015;
	}

	public override string GetName()
	{
		return "Weeping_Fail";
	}

	public override string[] GetAllComments()
	{
		return FAIL_COMMENTS;
	}

	public override Comment GenerateComment()
	{
		return new Comment(FAIL_COMMENTS.GetRandom());
	}
}
