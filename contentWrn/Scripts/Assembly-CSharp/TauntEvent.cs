using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;
using Zorro.Core;
using Zorro.Core.Serizalization;

public class TauntEvent : ContentEvent
{
	public float contentValue;

	public ushort monsterID;

	public float distance;

	public string playerName;

	public string[] INTERVIEW_COMMENTS = new string[1] { "wow they are so brave, just taunting that monster!" };

	public TauntEvent()
	{
	}

	public TauntEvent(string playerName, ushort monsterID, float distance, float contentValue)
	{
		this.playerName = playerName;
		this.monsterID = monsterID;
		this.distance = distance;
		this.contentValue = contentValue;
	}

	public override float GetContentValue()
	{
		return contentValue;
	}

	public override ushort GetID()
	{
		return 1034;
	}

	public override string GetName()
	{
		return "TauntEvent";
	}

	public override string[] GetAllComments()
	{
		return INTERVIEW_COMMENTS;
	}

	public override Comment GenerateComment()
	{
		List<string> list = new List<string>();
		list.AddRange(INTERVIEW_COMMENTS);
		return new Comment(list.GetRandom());
	}

	public override void Serialize(BinarySerializer serializer)
	{
		serializer.WriteFloat(contentValue);
		serializer.WriteUshort(monsterID);
		serializer.WriteHalf((half)distance);
		serializer.WriteString(playerName, Encoding.UTF8);
	}

	public override void Deserialize(BinaryDeserializer deserializer)
	{
		contentValue = deserializer.ReadFloat();
		monsterID = deserializer.ReadUShort();
		distance = deserializer.ReadHalf();
		playerName = deserializer.ReadString(Encoding.UTF8);
	}

	public override int GetUniqueID()
	{
		return monsterID;
	}
}
