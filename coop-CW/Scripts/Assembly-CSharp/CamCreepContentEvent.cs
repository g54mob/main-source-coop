using Zorro.Core;

public class CamCreepContentEvent : MonsterContentEvent
{
	public static string[] NORMAL_COMMENTS = new string[1] { "Content_CamCreep_0" };

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.camCreepScore;
	}

	public override ushort GetID()
	{
		return 1026;
	}

	public override string GetName()
	{
		return "CamCreep";
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
