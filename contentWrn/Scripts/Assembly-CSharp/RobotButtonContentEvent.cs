using Zorro.Core;

public class RobotButtonContentEvent : MonsterContentEvent
{
	public static string[] NORMAL_COMMENTS = new string[8] { "Content_RobotButton_0", "Content_RobotButton_1", "Content_RobotButton_2", "Content_RobotButton_3", "Content_RobotButton_4", "Content_RobotButton_5", "Content_RobotButton_6", "Content_RobotButton_7" };

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.robotButton;
	}

	public override ushort GetID()
	{
		return 1035;
	}

	public override string GetName()
	{
		return "RobotButton";
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
