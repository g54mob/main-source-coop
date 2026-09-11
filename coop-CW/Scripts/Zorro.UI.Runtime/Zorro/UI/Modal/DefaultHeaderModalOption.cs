using TMPro;
using UnityEngine;

namespace Zorro.UI.Modal
{
	public class DefaultHeaderModalOption : HeaderModalOption
	{
		public string Title { get; set; }

		public string Subheader { get; set; }

		public DefaultHeaderModalOption(string title, string subheader)
		{
			Title = title;
			Subheader = subheader;
		}

		public override void Setup(Transform parent)
		{
			GameObject original = Resources.Load<GameObject>("HeaderModalField");
			GameObject original2 = Resources.Load<GameObject>("SubheaderModalField");
			GameObject gameObject = Object.Instantiate(original, parent);
			GameObject gameObject2 = Object.Instantiate(original2, parent);
			gameObject.GetComponentInChildren<TextMeshProUGUI>().text = Title;
			gameObject2.GetComponentInChildren<TextMeshProUGUI>().text = Subheader;
		}
	}
}
