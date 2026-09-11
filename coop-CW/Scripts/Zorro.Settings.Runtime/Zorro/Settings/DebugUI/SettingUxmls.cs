using UnityEngine;
using UnityEngine.UIElements;
using Zorro.Core;

namespace Zorro.Settings.DebugUI
{
	[CreateAssetMenu(menuName = "Zorro/Settings/SettingUxmls", fileName = "SettingUxmls")]
	public class SettingUxmls : SingletonAsset<SettingUxmls>
	{
		public VisualTreeAsset IntSettingUxml;

		public VisualTreeAsset BoolSettingUxml;

		public VisualTreeAsset StringSettingUxml;

		public VisualTreeAsset FloatSettingUxml;

		public VisualTreeAsset EnumSettingUxml;

		public VisualTreeAsset MultiEnumSettingUxml;
	}
}
