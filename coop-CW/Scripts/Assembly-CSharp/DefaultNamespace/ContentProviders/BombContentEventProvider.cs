using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.ContentProviders
{
	public class BombContentEventProvider : ContentProvider
	{
		private ItemInstanceBehaviour itemInstanceBehaviour_g;

		private void Start()
		{
			itemInstanceBehaviour_g = GetComponent<BombItem>();
			if (itemInstanceBehaviour_g.itemInstance.item == null)
			{
				Debug.LogError("no item on start");
			}
		}

		public override void GetContent(List<ContentEventFrame> contentEvents, float seenAmount, Camera camera, float time)
		{
			Debug.Log("GetContent By BombContentEventProvider");
			contentEvents.Add(new ContentEventFrame(new BombContentEvent
			{
				bombItem = itemInstanceBehaviour_g.itemInstance.item,
				isHeld = itemInstanceBehaviour_g.isHeld
			}, seenAmount, time));
		}
	}
}
