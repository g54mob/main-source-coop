using Mimicraft.Localization;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mimicraft.UI
{
	public class UITooltipTrigger : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		[Tooltip("Çeviri anahtarı. Doluysa metin çeviri tablosundan gelir ve dil değişince kendiliğinden güncellenir. Boş bırakılırsa aşağıdaki Text olduğu gibi gösterilir.")]
		[SerializeField]
		private string key;

		[TextArea]
		[SerializeField]
		private string text;

		private string suffix;

		public string Text
		{
			get
			{
				string text2;
				string text = ((!string.IsNullOrEmpty(key) && Loc.TryGet(key, out text2)) ? text2 : this.text);
				if (!string.IsNullOrEmpty(suffix))
				{
					return text + suffix;
				}
				return text;
			}
			set
			{
				text = value;
			}
		}

		public string Suffix
		{
			get
			{
				return suffix;
			}
			set
			{
				suffix = value;
			}
		}

		public string Key
		{
			get
			{
				return key;
			}
			set
			{
				key = value;
			}
		}

		public static UITooltipTrigger AttachKey(Component target, string tooltipKey)
		{
			if (target == null || string.IsNullOrEmpty(tooltipKey))
			{
				return null;
			}
			UITooltipTrigger component = target.GetComponent<UITooltipTrigger>();
			if (component == null)
			{
				component = target.gameObject.AddComponent<UITooltipTrigger>();
				component.key = tooltipKey;
				return component;
			}
			if (string.IsNullOrEmpty(component.key) && string.IsNullOrEmpty(component.text))
			{
				component.key = tooltipKey;
			}
			return component;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			TooltipView.Instance?.Show(Text, (RectTransform)base.transform);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			TooltipView.Instance?.Hide();
		}

		private void OnDisable()
		{
			TooltipView.Instance?.HideOwnedBy((RectTransform)base.transform);
		}
	}
}
