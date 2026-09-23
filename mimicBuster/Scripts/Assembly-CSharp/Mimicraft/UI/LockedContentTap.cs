using Mimicraft.Analytics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mimicraft.UI
{
	public sealed class LockedContentTap : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		private string kind;

		private string id;

		private string state;

		public static void Attach(GameObject card, string kind, string id, ContentAvailability state)
		{
			if (!(card == null))
			{
				LockedContentTap lockedContentTap = card.GetComponent<LockedContentTap>();
				if (lockedContentTap == null)
				{
					lockedContentTap = card.AddComponent<LockedContentTap>();
				}
				lockedContentTap.kind = kind;
				lockedContentTap.id = id ?? "";
				lockedContentTap.state = state.ToString().ToLowerInvariant();
			}
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			Telemetry.Send("locked_content_tapped", ("kind", kind), ("content_id", id), ("state", state), ("session_seconds", Telemetry.SessionSeconds));
		}
	}
}
