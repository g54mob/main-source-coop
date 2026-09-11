using Zorro.Core.Serizalization;

public class MultiMonsterEvent : ContentEvent
{
	public float contentValue;

	public byte monsterCount;

	public override float GetContentValue()
	{
		return contentValue;
	}

	public override ushort GetID()
	{
		return 1029;
	}

	public override string GetName()
	{
		return "MultiMonster";
	}

	public override string[] GetAllComments()
	{
		return new string[1] { "Content_MultiMonster_0" };
	}

	public override Comment GenerateComment()
	{
		return new Comment("Content_MultiMonster_0");
	}

	public override void Serialize(BinarySerializer serializer)
	{
		serializer.WriteFloat(contentValue);
		serializer.WriteByte(monsterCount);
	}

	public override void Deserialize(BinaryDeserializer deserializer)
	{
		contentValue = deserializer.ReadFloat();
		monsterCount = deserializer.ReadByte();
	}

	public override int GetUniqueID()
	{
		return monsterCount;
	}
}
