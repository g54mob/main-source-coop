using Zorro.Core;

public class WeepingContentEvent : MonsterContentEvent
{
	public static string[] NORMAL_COMMENTS = new string[7] { "Content_Weeping_Idle_0", "Content_Weeping_Idle_1", "Content_Weeping_Idle_2", "Content_Weeping_Idle_3", "Content_Weeping_Idle_4", "Content_Weeping_Idle_5", "Content_Weeping_Idle_6" };

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.WeepingScoreIdle;
	}

	public override ushort GetID()
	{
		return 1007;
	}

	public override string GetName()
	{
		return "Weeping_Idle";
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
