using Zorro.Core;

public class WeepingContentEventCaptured : MonsterContentEvent
{
	public static string[] CAPTURED_COMMENTS = new string[3] { "Content_Weeping_Captured_0", "Content_Weeping_Captured_1", "Content_Weeping_Captured_2" };

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.WeepingScoreCaptured;
	}

	public override ushort GetID()
	{
		return 1014;
	}

	public override string GetName()
	{
		return "Weeping_Captured";
	}

	public override string[] GetAllComments()
	{
		return CAPTURED_COMMENTS;
	}

	public override Comment GenerateComment()
	{
		return new Comment(CAPTURED_COMMENTS.GetRandom());
	}
}
