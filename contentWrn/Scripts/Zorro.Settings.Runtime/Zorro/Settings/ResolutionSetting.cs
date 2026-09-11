using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Device;
using Zorro.Core;
using Zorro.Settings.DebugUI;

namespace Zorro.Settings
{
	public class ResolutionSetting : Setting
	{
		public override void Load(ISettingsSaveLoad loader)
		{
		}

		public override void Save(ISettingsSaveLoad saver)
		{
		}

		public override void ApplyValue()
		{
		}

		public override SettingUI GetDebugUI(ISettingHandler settingHandler)
		{
			return new ResolutionSettingUI(this, settingHandler);
		}

		public override GameObject GetSettingUICell()
		{
			return SingletonAsset<InputCellMapper>.Instance.ResolutionSettingCell;
		}

		public void SetValue(Resolution newValue, ISettingHandler settingHandler)
		{
			FullScreenMode fullScreenMode = UnityEngine.Device.Screen.fullScreenMode;
			UnityEngine.Device.Screen.SetResolution(newValue.width, newValue.height, fullScreenMode);
			ApplyValue();
			settingHandler.SaveSetting(this);
		}

		public List<string> GetChoices()
		{
			return (from resolution in GetResolutions()
				select $"{resolution.width}x{resolution.height}").ToList();
		}

		public List<Resolution> GetResolutions()
		{
			HashSet<(float, float)> hashSet = new HashSet<(float, float)>();
			List<Resolution> list = new List<Resolution>();
			foreach (Resolution item in UnityEngine.Device.Screen.resolutions.Reverse())
			{
				if (hashSet.Add((item.width, item.height)))
				{
					list.Add(item);
				}
			}
			return list.ToList();
		}

		public int GetCurrentChoice()
		{
			List<string> choices = GetChoices();
			int2 int5 = new int2(UnityEngine.Device.Screen.width, UnityEngine.Device.Screen.height);
			for (int i = 0; i < choices.Count; i++)
			{
				string[] array = choices[i].Split('x');
				if (int.Parse(array[0]) == int5.x && int.Parse(array[1]) == int5.y)
				{
					return i;
				}
			}
			return 0;
		}
	}
}
