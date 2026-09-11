using System.Collections.Generic;
using UnityEngine;
using Zorro.Core;
using Zorro.Settings.DebugUI;

namespace Zorro.Settings
{
	public abstract class EnumSetting : IntSetting
	{
		public abstract List<string> GetChoices();

		public override SettingUI GetDebugUI(ISettingHandler settingHandler)
		{
			return new EnumSettingsUI(this, settingHandler);
		}

		public override GameObject GetSettingUICell()
		{
			return SingletonAsset<InputCellMapper>.Instance.EnumSettingCell;
		}

		public virtual bool IsValidValue(int index)
		{
			return true;
		}
	}
}
