using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.Settings
{
	[RequireComponent(typeof(Button))]
	public class SettingsMenuOpener : MonoBehaviour
	{
		private void Awake()
		{
			Button component = GetComponent<Button>();
			component.onClick.RemoveListener(OpenSettings);
			component.onClick.AddListener(OpenSettings);
		}

		private void OpenSettings()
		{
			if (SettingsMenuView.Instance != null)
			{
				SettingsMenuView.Instance.Open();
			}
			else
			{
				Debug.LogWarning("[Settings] Ayarlar paneli yok - Resources/SettingsRoot prefab'i bulunamamis olmali.", this);
			}
		}
	}
}
