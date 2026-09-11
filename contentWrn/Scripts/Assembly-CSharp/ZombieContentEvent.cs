using Zorro.Core;

public class ZombieContentEvent : MonsterContentEvent
{
	public static string[] NORMAL_COMMENTS = new string[22]
	{
		"Content_Zombie_0", "Content_Zombie_1", "Content_Zombie_2", "Content_Zombie_3", "Content_Zombie_4", "Content_Zombie_5", "Content_Zombie_6", "Content_Zombie_7", "Content_Zombie_8", "Content_Zombie_9",
		"Content_Zombie_10", "Content_Zombie_11", "Content_Zombie_12", "Content_Zombie_13", "Content_Zombie_14", "Content_Zombie_15", "Content_Zombie_16", "Content_Zombie_17", "Content_Zombie_18", "Content_Zombie_19",
		"Content_Zombie_20", "Content_Zombie_21"
	};

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.ZombieScore;
	}

	public override ushort GetID()
	{
		return 1003;
	}

	public override string GetName()
	{
		return "Zombie";
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
