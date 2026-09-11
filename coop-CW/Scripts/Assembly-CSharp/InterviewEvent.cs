using System.Collections.Generic;
using Unity.Mathematics;
using Zorro.Core;
using Zorro.Core.Serizalization;

public class InterviewEvent : ContentEvent
{
	public float contentValue;

	public ushort monsterID;

	public float distance;

	public string[] INTERVIEW_COMMENTS = new string[1] { "they really interview that monster!" };

	public override float GetContentValue()
	{
		return contentValue;
	}

	public override ushort GetID()
	{
		return 1033;
	}

	public override string GetName()
	{
		return "InterviewEvent";
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
	}

	public override void Deserialize(BinaryDeserializer deserializer)
	{
		contentValue = deserializer.ReadFloat();
		monsterID = deserializer.ReadUShort();
		distance = deserializer.ReadHalf();
	}

	public override int GetUniqueID()
	{
		return monsterID;
	}
}
