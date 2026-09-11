using Zorro.Core;

public class BarnacleBallContentEvent : MonsterContentEvent
{
	public static string[] NORMAL_COMMENTS = new string[30]
	{
		"Content_BarnacleBall_0", "Content_BarnacleBall_1", "Content_BarnacleBall_2", "Content_BarnacleBall_3", "Content_BarnacleBall_4", "Content_BarnacleBall_5", "Content_BarnacleBall_6", "Content_BarnacleBall_7", "Content_BarnacleBall_8", "Content_BarnacleBall_9",
		"Content_BarnacleBall_10", "Content_BarnacleBall_11", "Content_BarnacleBall_12", "Content_BarnacleBall_13", "Content_BarnacleBall_14", "Content_BarnacleBall_15", "Content_BarnacleBall_16", "Content_BarnacleBall_17", "Content_BarnacleBall_18", "Content_BarnacleBall_19",
		"Content_BarnacleBall_20", "Content_BarnacleBall_21", "Content_BarnacleBall_22", "Content_BarnacleBall_23", "Content_BarnacleBall_24", "Content_BarnacleBall_25", "Content_BarnacleBall_26", "Content_BarnacleBall_27", "Content_BarnacleBall_28", "Content_BarnacleBall_29"
	};

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.BarnacleBallScore;
	}

	public override ushort GetID()
	{
		return 1012;
	}

	public override string GetName()
	{
		return "BarnacleBall";
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
