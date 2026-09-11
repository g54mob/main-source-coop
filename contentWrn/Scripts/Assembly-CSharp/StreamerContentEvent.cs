using Zorro.Core;
using pworld.Scripts.Extensions;

public class StreamerContentEvent : MonsterContentEvent
{
	public static string[] NORMAL_COMMENTS = new string[2] { "Content_Streamer_0", "Content_Streamer_2" };

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.streamerScore;
	}

	public override ushort GetID()
	{
		return 1038;
	}

	public override string GetName()
	{
		return "Streamer";
	}

	public override string[] GetAllComments()
	{
		return NORMAL_COMMENTS;
	}

	public override Comment GenerateComment()
	{
		return new Comment(ExtCollections.GetRandom(NORMAL_COMMENTS));
	}
}
