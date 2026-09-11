using System.Collections.Generic;
using UnityEngine;
using Zorro.Core;
using Zorro.Core.Serizalization;

public class PlayerFallingContentEvent : PlayerBaseEvent
{
	public float sinceGrounded;

	public static string[] SMALL_FALL_COMMENTS = new string[4] { "Content_PlayerFalling_0", "Content_PlayerFalling_1", "Content_PlayerFalling_2", "Content_PlayerFalling_3" };

	public static string[] BIG_FALL_COMMENTS = new string[4] { "Content_PlayerFalling_4", "Content_PlayerFalling_5", "Content_PlayerFalling_6", "Content_PlayerFalling_7" };

	private bool IsBigFall => sinceGrounded > PlayerRagdoll.RagdollIfFellForLongerThan;

	public PlayerFallingContentEvent()
	{
	}

	public PlayerFallingContentEvent(string playerName, int actorNumber, float sinceGrounded, Vector3 worldPosition)
		: base(playerName, actorNumber, worldPosition)
	{
		this.sinceGrounded = sinceGrounded;
	}

	public override float GetContentValue()
	{
		if (!IsBigFall)
		{
			return SingletonAsset<BigNumbers>.Instance.playerSmallFallScore;
		}
		return SingletonAsset<BigNumbers>.Instance.playerBigFallScore;
	}

	public override ushort GetID()
	{
		return 1022;
	}

	public override string GetName()
	{
		return "PlayerFalling";
	}

	public override string[] GetAllComments()
	{
		List<string> list = new List<string>(SMALL_FALL_COMMENTS.Length + BIG_FALL_COMMENTS.Length);
		list.AddRange(SMALL_FALL_COMMENTS);
		list.AddRange(BIG_FALL_COMMENTS);
		return list.ToArray();
	}

	public override Comment GenerateComment()
	{
		int num = (IsBigFall ? Random.Range(0, BIG_FALL_COMMENTS.Length) : Random.Range(0, SMALL_FALL_COMMENTS.Length));
		return new Comment(IsBigFall ? BIG_FALL_COMMENTS[num] : SMALL_FALL_COMMENTS[num], playerName);
	}

	public override void Serialize(BinarySerializer serializer)
	{
		base.Serialize(serializer);
		serializer.WriteFloat(sinceGrounded);
	}

	public override void Deserialize(BinaryDeserializer deserializer)
	{
		base.Deserialize(deserializer);
		sinceGrounded = deserializer.ReadFloat();
	}
}
