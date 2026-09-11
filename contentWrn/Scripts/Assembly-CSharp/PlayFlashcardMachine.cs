using UnityEngine;

public class PlayFlashcardMachine : Interactable
{
	public VideoPlaybackScreen playbackScreen;

	public Item m_flashCardItem;

	public override void Interact(Player player)
	{
		if (player.TryGetInventory(out var o))
		{
			FlashcardEntry t;
			if (!o.TryGetSlot(player.data.selectedItemSlot, out var slot) && !o.TryGetSlotWithItem(m_flashCardItem, out slot))
			{
				Debug.LogError("Player has no flashcard");
			}
			else if (slot.ItemInSlot.data.TryGetEntry<FlashcardEntry>(out t))
			{
				if (!t.videoID.Equals(VideoHandle.Invalid))
				{
					slot.Clear();
				}
				else
				{
					Debug.LogError("Flashcard has invalid video ID");
				}
			}
			else
			{
				Debug.LogError("Flashcard has no data entry");
			}
		}
		else
		{
			Debug.LogError("Player has no inventory");
		}
	}
}
