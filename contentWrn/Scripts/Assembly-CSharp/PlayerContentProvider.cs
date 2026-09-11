using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Zorro.Core;

public class PlayerContentProvider : ContentProvider
{
	public Hat hardHat;

	public Item reporterMic;

	[FormerlySerializedAs("Player")]
	public Player player;

	public override void GetContent(List<ContentEventFrame> contentEvents, float seenAmount, Camera cam, float time)
	{
		string nickName = player.refs.view.Owner.NickName;
		int actorNumber = player.refs.view.Owner.ActorNumber;
		Vector3 worldPosition = player.HeadPosition();
		if (player.data.currentHat != null && player.data.currentHat.giveContentEvent)
		{
			contentEvents.Add(new ContentEventFrame(new PlayerWearingHatContentEvent(nickName, actorNumber, worldPosition, player.data.currentHat.runtimeHatIndex), seenAmount, time));
		}
		if (player.data.looksLikeShroomMonster)
		{
			contentEvents.Add(new ContentEventFrame(new PlayerShroomContentEvent(nickName, actorNumber, worldPosition), seenAmount, time));
			return;
		}
		if (player.data.recentDamage.Any())
		{
			UniqueDamage recentDamage = player.data.recentDamage.MaxBy((UniqueDamage d) => d.damage);
			contentEvents.Add(new ContentEventFrame(new PlayerTookDamageContentEvent(nickName, actorNumber, recentDamage, worldPosition), seenAmount, time));
		}
		PlayerInventory o;
		bool flag = player.TryGetInventory(out o);
		if (flag && o.TryGetSlot(player.data.selectedItemSlot, out var slot) && slot.ItemInSlot.item != null)
		{
			if (slot.ItemInSlot.data.timeSinceGrounded > 3f && slot.ItemInSlot.data.timeInHand < 1f)
			{
				contentEvents.Add(new ContentEventFrame(new GoodCatchContentEvent(nickName, actorNumber, worldPosition), seenAmount, time));
				Debug.LogError("Good Catch Event Added!");
			}
			if (slot.ItemInSlot.item.id == reporterMic.id)
			{
				contentEvents.Add(new ContentEventFrame(new PlayerHoldingMicContentEvent(nickName, actorNumber, worldPosition), seenAmount, time));
			}
		}
		if (player.data.emoteTime > 0f && flag && o.emote != null)
		{
			contentEvents.Add(new ContentEventFrame(new PlayerEmoteContentEvent(nickName, actorNumber, o.emote, worldPosition), seenAmount, time));
			return;
		}
		if (player.data.dead)
		{
			contentEvents.Add(new ContentEventFrame(new PlayerDeadContentEvent(nickName, actorNumber, worldPosition), seenAmount, time));
			return;
		}
		if (player.data.fallTime > 0f)
		{
			contentEvents.Add(new ContentEventFrame(new PlayerRagdollContentEvent(nickName, actorNumber, worldPosition), seenAmount, time));
			return;
		}
		if (player.data.sinceGrounded > 0.75f)
		{
			contentEvents.Add(new ContentEventFrame(new PlayerFallingContentEvent(nickName, actorNumber, player.data.sinceGrounded, worldPosition), seenAmount, time));
			return;
		}
		Vector3 forward = player.refs.ragdoll.GetBodypart(BodypartType.Torso).transform.forward;
		Vector3 rhs = -cam.transform.forward;
		float t = math.saturate(Vector3.Dot(forward, rhs));
		float facingFactor = Mathf.Lerp(SingletonAsset<BigNumbers>.Instance.PlayerFacingAwayFactor, SingletonAsset<BigNumbers>.Instance.PlayerFacingTowardsFactor, t);
		contentEvents.Add(new ContentEventFrame(new PlayerContentEvent(nickName, actorNumber, facingFactor, player.data.microphoneValue, worldPosition), seenAmount, time));
	}
}
