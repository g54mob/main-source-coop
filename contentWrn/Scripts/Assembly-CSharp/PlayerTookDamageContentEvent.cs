using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Zorro.Core;
using Zorro.Core.Serizalization;

public class PlayerTookDamageContentEvent : PlayerBaseEvent
{
	public static string[] TOOK_DMG_COMMENTS = new string[3] { "Content_PlayerTookDamage_0", "Content_PlayerTookDamage_1", "Content_PlayerTookDamage_2" };

	public UniqueDamage recentDamage;

	public PlayerTookDamageContentEvent()
	{
	}

	public PlayerTookDamageContentEvent(string playerName, int actorNumber, UniqueDamage recentDamage, Vector3 worldPosition)
		: base(playerName, actorNumber, worldPosition)
	{
		this.recentDamage = recentDamage;
	}

	public override float GetContentValue()
	{
		return SingletonAsset<BigNumbers>.Instance.playerTookDamageScore * recentDamage.damage;
	}

	public override int GetUniqueID()
	{
		return recentDamage.unqiueHash;
	}

	public override ushort GetID()
	{
		return 1028;
	}

	public override string GetName()
	{
		return "PlayerTookDamage";
	}

	public override string[] GetAllComments()
	{
		return TOOK_DMG_COMMENTS;
	}

	public override Comment GenerateComment()
	{
		List<string> list = new List<string>();
		if (UnityEngine.Random.Range(recentDamage.damage, 100f) < 60f)
		{
			return null;
		}
		list.AddRange(TOOK_DMG_COMMENTS);
		return new Comment(list.GetRandom(), playerName);
	}

	public override void Serialize(BinarySerializer serializer)
	{
		base.Serialize(serializer);
		serializer.WriteHalf((half)recentDamage.damage);
		serializer.WriteInt(recentDamage.unqiueHash);
	}

	public override void Deserialize(BinaryDeserializer deserializer)
	{
		base.Deserialize(deserializer);
		half half5 = deserializer.ReadHalf();
		int hash = deserializer.ReadInt();
		recentDamage = new UniqueDamage(half5, hash);
	}
}
