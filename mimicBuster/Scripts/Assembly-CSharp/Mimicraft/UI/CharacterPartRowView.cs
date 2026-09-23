using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class CharacterPartRowView : MonoBehaviour
	{
		[Tooltip("Parçanın adının yazılacağı yer.")]
		[SerializeField]
		private TextMeshProUGUI nameLabel;

		[Tooltip("Kamerayı bu parçaya getiren buton.")]
		[SerializeField]
		private Button focusButton;

		[Tooltip("Parçayı gizleyip gösteren buton.")]
		[SerializeField]
		private Button visibilityButton;

		[Tooltip("Parça GÖRÜNÜRKEN açık olacak obje - açık göz ikonu gibi. İsteğe bağlı.")]
		[SerializeField]
		private GameObject visibleIndicator;

		[Tooltip("Parça GİZLİYKEN açık olacak obje - kapalı göz ikonu gibi. İsteğe bağlı.")]
		[SerializeField]
		private GameObject hiddenIndicator;

		public void Bind(string partId, string displayName, bool visible, Action<string> onFocus, Action<string> onToggleVisibility)
		{
			if (nameLabel != null)
			{
				nameLabel.text = displayName;
			}
			if (visibleIndicator != null)
			{
				visibleIndicator.SetActive(visible);
			}
			if (hiddenIndicator != null)
			{
				hiddenIndicator.SetActive(!visible);
			}
			if (focusButton != null)
			{
				focusButton.onClick.RemoveAllListeners();
				focusButton.onClick.AddListener(delegate
				{
					onFocus?.Invoke(partId);
				});
			}
			if (visibilityButton != null)
			{
				visibilityButton.onClick.RemoveAllListeners();
				visibilityButton.onClick.AddListener(delegate
				{
					onToggleVisibility?.Invoke(partId);
				});
			}
		}
	}
}
