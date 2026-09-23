using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class PresetRowView : MonoBehaviour
	{
		[Tooltip("Preset'in adı.")]
		[SerializeField]
		private TextMeshProUGUI nameLabel;

		[Tooltip("Preset'in görseli.")]
		[SerializeField]
		private Image iconImage;

		[Tooltip("Satıra basılınca seçen buton. Boş bırakılırsa bu objenin üzerinde aranır.")]
		[SerializeField]
		private Button pickButton;

		private int index = -1;

		private Action<int> picked;

		private void Awake()
		{
			if (pickButton == null)
			{
				pickButton = GetComponent<Button>();
			}
			if (pickButton != null)
			{
				pickButton.onClick.AddListener(OnPick);
			}
		}

		public void Bind(int index, PresetPickerData data, Action<int> picked)
		{
			this.index = index;
			this.picked = picked;
			if (nameLabel != null)
			{
				nameLabel.text = ((data != null) ? data.name : "");
			}
			if (iconImage != null)
			{
				iconImage.sprite = data?.icon;
				iconImage.enabled = data != null && data.icon != null;
			}
		}

		private void OnPick()
		{
			picked?.Invoke(index);
		}
	}
}
