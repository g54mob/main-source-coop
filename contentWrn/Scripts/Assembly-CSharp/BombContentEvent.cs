using Zorro.Core;
using Zorro.Core.Serizalization;

public class BombContentEvent : ContentEvent
{
	public Item bombItem;

	public bool isHeld;

	public override float GetContentValue()
	{
		return bombItem.content.contentValue * (isHeld ? 1f : 0.1f);
	}

	public override ushort GetID()
	{
		return 1030;
	}

	public override string GetName()
	{
		if (bombItem == null)
		{
			return CONTENT_ID.Bomb.ToString();
		}
		return bombItem.name;
	}

	public override string[] GetAllComments()
	{
		return new string[1] { "Content_Bomb_0" };
	}

	public override Comment GenerateComment()
	{
		if (isHeld)
		{
			return new Comment("Content_Bomb_0");
		}
		return new Comment(bombItem.content.comments.GetRandom());
	}

	public override void Serialize(BinarySerializer serializer)
	{
		serializer.WriteByte(bombItem.id);
		serializer.WriteBool(isHeld);
	}

	public override void Deserialize(BinaryDeserializer deserializer)
	{
		ItemDatabase.TryGetItemFromID(deserializer.ReadByte(), out bombItem);
		isHeld = deserializer.ReadBool();
	}
}
