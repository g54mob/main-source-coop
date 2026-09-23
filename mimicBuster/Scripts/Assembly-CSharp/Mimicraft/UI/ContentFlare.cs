using TMPro;
using UnityEngine;

namespace Mimicraft.UI
{
	public static class ContentFlare
	{
		private const string LabelName = "FlareLabel";

		public static string Apply(GameObject card, string displayName, ContentAvailability state)
		{
			if (card == null)
			{
				return displayName;
			}
			Transform transform = card.transform.Find("FlareLabel");
			if (transform == null)
			{
				return displayName + state.FlareSuffix();
			}
			string text = state.Flare();
			TextMeshProUGUI component = transform.GetComponent<TextMeshProUGUI>();
			if (component != null)
			{
				component.text = text;
			}
			transform.gameObject.SetActive(!string.IsNullOrEmpty(text));
			return displayName;
		}
	}
}
