using UnityEngine;
using Zorro.Core;

namespace Zorro.Settings
{
	public abstract class KeyCodeSetting : IntSetting
	{
		public override int GetDefaultValue()
		{
			return (int)GetDefaultKey();
		}

		protected abstract KeyCode GetDefaultKey();

		public override GameObject GetSettingUICell()
		{
			return SingletonAsset<InputCellMapper>.Instance.KeyCodeSettingCell;
		}

		public KeyCode Keycode()
		{
			return (KeyCode)base.Value;
		}

		public override void ApplyValue()
		{
		}
	}
}
