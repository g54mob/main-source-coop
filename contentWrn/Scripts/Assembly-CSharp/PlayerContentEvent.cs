using System.Collections.Generic;
using UnityEngine;
using Zorro.Core;
using Zorro.Core.Serizalization;

public class PlayerContentEvent : PlayerBaseEvent
{
	public float facingFactor;

	public float microphoneFactor;

	public static string[] NORMAL_COMMENTS = new string[6] { "Content_Player_0", "Content_Player_1", "Content_Player_2", "Content_Player_3", "Content_Player_4", "Content_Player_5" };

	public static string[] SPEAKING_COMMENTS = new string[17]
	{
		"Content_Player_6", "Content_Player_7", "Content_Player_8", "Content_Player_9", "Content_Player_10", "Content_Player_11", "Content_Player_12", "Content_Player_13", "Content_Player_14", "Content_Player_15",
		"Content_Player_16", "Content_Player_17", "Content_Player_18", "Content_Player_19", "Content_Player_20", "Content_Player_21", "Content_Player_22"
	};

	public PlayerContentEvent()
	{
	}

	public PlayerContentEvent(string playerName, int actorNumber, float facingFactor, float microphoneFactor, Vector3 worldPosition)
		: base(playerName, actorNumber, worldPosition)
	{
		this.facingFactor = facingFactor;
		this.microphoneFactor = microphoneFactor;
	}

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.PlayerScore * facingFactor + microphoneFactor * SingletonAsset<BigNumbers>.Instance.PlayerMicrophoneBounus;
	}

	public override ushort GetID()
	{
		return 1000;
	}

	public override string GetName()
	{
		return "Player";
	}

	public override string[] GetAllComments()
	{
		List<string> list = new List<string>(NORMAL_COMMENTS.Length + SPEAKING_COMMENTS.Length);
		list.AddRange(NORMAL_COMMENTS);
		list.AddRange(SPEAKING_COMMENTS);
		return list.ToArray();
	}

	public override Comment GenerateComment()
	{
		List<string> list = new List<string>();
		if (microphoneFactor > 0.4f && Random.value > 0.4f)
		{
			list.AddRange(SPEAKING_COMMENTS);
		}
		list.AddRange(NORMAL_COMMENTS);
		return new Comment(list.GetRandom(), playerName);
	}

	public override void Serialize(BinarySerializer serializer)
	{
		base.Serialize(serializer);
		serializer.WriteFloat(facingFactor);
		serializer.WriteFloat(microphoneFactor);
	}

	public override void Deserialize(BinaryDeserializer deserializer)
	{
		base.Deserialize(deserializer);
		facingFactor = deserializer.ReadFloat();
		microphoneFactor = deserializer.ReadFloat();
	}
}
