using Zorro.Core;
using pworld.Scripts.Extensions;

public class MimeContentEvent : MonsterContentEvent
{
	public static string[] NORMAL_COMMENTS = new string[1] { "Content_Mime_0" };

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.mimeScore;
	}

	public override ushort GetID()
	{
		return 1045;
	}

	public override string GetName()
	{
		return "Mime";
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
