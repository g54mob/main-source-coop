using System;
using UnityEngine;
using Zorro.Settings.DebugUI;

namespace Zorro.Settings
{
	public abstract class Setting : IDisposable
	{
		public abstract void Load(ISettingsSaveLoad loader);

		public abstract void Save(ISettingsSaveLoad saver);

		public abstract void ApplyValue();

		public abstract SettingUI GetDebugUI(ISettingHandler settingHandler);

		public virtual void Dispose()
		{
		}

		public abstract GameObject GetSettingUICell();

		public virtual void Update()
		{
		}
	}
}
