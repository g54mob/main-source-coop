using Features.SettingsMenuModule.Scripts.Data;
using Fusion.Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Features.SettingsMenuModule.Scripts.HubSettings
{
	public class RegionInfoButtonView : RegionInfoButtonViewBase
	{
		[SerializeField]
		private Button _settingsButton;

		[SerializeField]
		private TMP_Text _regionInfoText;

		private RegionsPingConfiguration _regionsPingConfiguration;

		[Inject]
		private void InjectDependencies(RegionsPingConfiguration regionsPingConfiguration)
		{
			_regionsPingConfiguration = regionsPingConfiguration;
		}

		protected override void OnEnable()
		{
			_settingsButton.onClick.AddListener(base.InvokeOnClickSettings);
		}

		protected override void OnDisable()
		{
			_settingsButton.onClick.RemoveListener(base.InvokeOnClickSettings);
		}

		public override void SetActualRegionPing(RegionInfo regionInfo)
		{
			_regionInfoText.text = FormatRegionOption(regionInfo.RegionCode, regionInfo.RegionPing);
		}

		public override void SetRegionButtonInteractable(bool interactable)
		{
			_settingsButton.interactable = interactable;
		}

		private string FormatRegionOption(string code, int ping)
		{
			string text = _regionsPingConfiguration.EvaluateHex(ping);
			string text2 = "<color=#" + text + ">•</color>";
			string text3 = $"<color=#{text}>{Mathf.Max(ping, 0)} ms</color>";
			return text2 + " " + code.ToUpper() + " — " + text3;
		}
	}
}
