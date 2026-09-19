using System.Collections.Generic;
using System.Linq;
using Features.SettingsMenuModule.Scripts.Data;
using Fusion.Photon.Realtime;
using TMPro;
using UnityEngine;
using Zenject;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class LobbySettingsItemView : LobbySettingsItemViewBase
	{
		[SerializeField]
		private TMP_Dropdown _dropdown;

		private RegionsPingConfiguration _regionsPingConfiguration;

		private List<RegionInfo> _regions = new List<RegionInfo>();

		[Inject]
		private void InjectDependencies(RegionsPingConfiguration regionsPingConfiguration)
		{
			_regionsPingConfiguration = regionsPingConfiguration;
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			_dropdown.onValueChanged.AddListener(SendOnRegionChangedEvent);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_dropdown.onValueChanged.RemoveListener(SendOnRegionChangedEvent);
		}

		private void SendOnRegionChangedEvent(int regionIndex)
		{
			OnRegionChanged(_regions[regionIndex].RegionCode);
		}

		public override void SetRegionsInfo(IReadOnlyList<RegionInfo> regions)
		{
			_regions = (regions?.ToList() ?? new List<RegionInfo>()).OrderBy((RegionInfo info) => info.RegionPing).ToList();
			_dropdown.options.Clear();
			if (_dropdown.captionText != null)
			{
				_dropdown.captionText.richText = true;
			}
			if (_dropdown.itemText != null)
			{
				_dropdown.itemText.richText = true;
			}
			List<TMP_Dropdown.OptionData> list = new List<TMP_Dropdown.OptionData>(_regions.Count);
			foreach (RegionInfo region in _regions)
			{
				string text = FormatRegionOption(region.RegionCode, region.RegionPing);
				list.Add(new TMP_Dropdown.OptionData(text));
			}
			_dropdown.AddOptions(list);
			_dropdown.SetValueWithoutNotify(Mathf.Clamp(_dropdown.value, 0, Mathf.Max(0, _regions.Count - 1)));
			_dropdown.RefreshShownValue();
		}

		public override void SelectCurrentRegion(string appSettingsFixedRegion)
		{
			int num = _regions.FindIndex((RegionInfo info) => info.RegionCode == appSettingsFixedRegion);
			int valueWithoutNotify = ((num >= 0) ? num : Mathf.Clamp(_dropdown.value, 0, Mathf.Max(0, _regions.Count - 1)));
			_dropdown.SetValueWithoutNotify(valueWithoutNotify);
			_dropdown.RefreshShownValue();
		}

		public override void SetRegionSearchInProgress(bool isInProgress, string loadingCaption)
		{
			if (isInProgress)
			{
				if (_dropdown.captionText != null)
				{
					_dropdown.captionText.richText = true;
				}
				if (_dropdown.options.Count == 0)
				{
					_dropdown.AddOptions(new List<TMP_Dropdown.OptionData>
					{
						new TMP_Dropdown.OptionData(loadingCaption)
					});
				}
				else
				{
					_dropdown.options[0].text = loadingCaption;
				}
				_dropdown.SetValueWithoutNotify(0);
				_dropdown.RefreshShownValue();
			}
		}

		private string FormatRegionOption(string code, int ping)
		{
			string text = _regionsPingConfiguration.EvaluateHex(ping);
			string text2 = "<color=#" + text + "><size=35>•</size></color>";
			string text3 = $"<color=#{text}>{Mathf.Max(ping, 0)} ms</color>";
			return text2 + " " + code.ToUpper() + " <color=#212727><size=35>—</size></color> " + text3;
		}
	}
}
