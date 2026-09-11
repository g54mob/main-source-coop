using UnityEngine;
using Zorro.Core;

namespace Zorro.Settings
{
	[CreateAssetMenu(fileName = "InputCellMapper", menuName = "Zorro/Settings/InputCellMapper")]
	public class InputCellMapper : SingletonAsset<InputCellMapper>
	{
		public GameObject EnumSettingCell;

		public GameObject FloatSettingCell;

		public GameObject IntSettingCell;

		public GameObject BoolSettingCell;

		public GameObject StringSettingCell;

		public GameObject ResolutionSettingCell;

		public GameObject KeyCodeSettingCell;
	}
}
