using UnityEngine;

namespace Zorro.Settings
{
	public abstract class SettingInputUICell : MonoBehaviour
	{
		public abstract void Setup(Setting setting, ISettingHandler settingHandler);
	}
}
