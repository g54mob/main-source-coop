using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class ScreenResolutionSettingsItemVew : ScreenResolutionSettingsItemViewBase
	{
		[SerializeField]
		private SettingsMultipleItems _resolutionSettings;

		private readonly Dictionary<Vector2Int, string> _resolutions = new Dictionary<Vector2Int, string>();

		protected override void OnEnable()
		{
			base.OnEnable();
			_resolutionSettings.OnValueChanged += OnValueChangedHandler;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_resolutionSettings.OnValueChanged -= OnValueChangedHandler;
		}

		private void OnValueChangedHandler(int index)
		{
			if (index >= 0 && index < _resolutions.Count)
			{
				Vector2Int key = _resolutions.ElementAt(index).Key;
				_resolutionSettings.Value.SetText(_resolutions[key]);
				OnValueChanged?.Invoke(key);
			}
		}

		public override void SetResolutionDropdownValues(List<Vector2Int> allPossibleResolutions)
		{
			List<string> list = new List<string>();
			foreach (Vector2Int allPossibleResolution in allPossibleResolutions)
			{
				string text = new string($"{allPossibleResolution.x}x{allPossibleResolution.y}");
				if (_resolutions.TryAdd(allPossibleResolution, text))
				{
					list.Add(text);
				}
			}
			_resolutionSettings.SetValues(list);
		}

		public override void SetDropdownResolutionValue(int x, int y)
		{
			for (int i = 0; i < _resolutions.Count; i++)
			{
				Vector2Int key = _resolutions.ElementAt(i).Key;
				if (key.x == x && key.y == y)
				{
					_resolutionSettings.SetIndex(i);
					_resolutionSettings.Value.SetText(_resolutions[key]);
					break;
				}
			}
		}

		public override void SetDropdownResolutionValueIndex(int index)
		{
			_resolutionSettings.SetIndex(index);
			_resolutionSettings.Value.SetText(_resolutions.ElementAt(index).Value);
		}
	}
}
