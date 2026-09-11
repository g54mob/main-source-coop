using System.Collections.Generic;
using DefaultNamespace.ContentEvents;
using UnityEngine;

namespace DefaultNamespace.ContentProviders
{
	public class ArtifactContentProvider : PropContentProvider
	{
		private ItemInstanceBehaviour itemInstanceBehaviour_g;

		private IArtifactContent artifactContent_g;

		private void Start()
		{
			itemInstanceBehaviour_g = GetComponent<ItemInstanceBehaviour>();
			artifactContent_g = GetComponent<IArtifactContent>();
			if (itemInstanceBehaviour_g.itemInstance.item == null)
			{
				Debug.LogError("no item on start");
			}
		}

		protected override bool ShouldShow()
		{
			return false;
		}

		public override void GetContent(List<ContentEventFrame> contentEvents, float seenAmount, Camera camera, float time)
		{
			contentEvents.Add(new ContentEventFrame(new ArtifactContentEvent
			{
				content = itemInstanceBehaviour_g.itemInstance.item.content,
				artifact = itemInstanceBehaviour_g.itemInstance.item,
				isHeld = artifactContent_g.IsHeld,
				isActive = artifactContent_g.IsActive
			}, seenAmount, time));
		}
	}
}
