using System;
using Zorro.Core;
using Zorro.Core.Serizalization;

public class PropContentEvent : ContentEvent
{
	public PropContent content;

	public override float GetContentValue()
	{
		return content.contentValue;
	}

	public override ushort GetID()
	{
		return content.id;
	}

	public override string GetName()
	{
		if (!(content != null))
		{
			return string.Empty;
		}
		return content.name;
	}

	public override string[] GetAllComments()
	{
		return Array.Empty<string>();
	}

	public override Comment GenerateComment()
	{
		return new Comment(content.comments.GetRandom());
	}

	public override void Serialize(BinarySerializer serializer)
	{
	}

	public override void Deserialize(BinaryDeserializer deserializer)
	{
	}

	public override int GetUniqueID()
	{
		return 0;
	}
}
