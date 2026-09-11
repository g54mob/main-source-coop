using UnityEngine;
using Zorro.UI;

public class SettingCategoryTab : MonoBehaviour, ITabAction
{
	public SettingsMenu settingsMenu;

	public SettingCategory settingCategory;

	public void Select()
	{
		settingsMenu.SelectCategory(settingCategory);
	}
}
