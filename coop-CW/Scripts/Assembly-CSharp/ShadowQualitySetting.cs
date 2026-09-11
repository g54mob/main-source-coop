using System;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zorro.Core;
using Zorro.Settings;

public class ShadowQualitySetting : EnumSetting, IExposedSetting
{
	public bool DisplayShadows { get; private set; }

	public event Action OnSettingsChanged;

	public override void ApplyValue()
	{
		ShadowResolution shadowResolution = ShadowResolution._4096;
		float shadowDistance = 90f;
		int shadowCascadeCount = 1;
		bool displayShadows = true;
		switch (base.Value)
		{
		case 0:
			shadowResolution = ShadowResolution._4096;
			shadowDistance = 90f;
			shadowCascadeCount = 3;
			break;
		case 1:
			shadowResolution = ShadowResolution._4096;
			shadowDistance = 75f;
			break;
		case 2:
			shadowResolution = ShadowResolution._2048;
			shadowDistance = 60f;
			break;
		case 3:
			shadowResolution = ShadowResolution._256;
			shadowDistance = 1f;
			displayShadows = false;
			break;
		}
		SetShadowSettings(shadowResolution, shadowDistance, shadowCascadeCount, displayShadows);
	}

	private void SetShadowSettings(ShadowResolution shadowResolution, float shadowDistance, int shadowCascadeCount, bool displayShadows)
	{
		UniversalRenderPipelineAsset obj = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
		obj.SetInternalProperty("additionalLightsShadowmapResolution", (int)shadowResolution);
		obj.shadowCascadeCount = shadowCascadeCount;
		obj.shadowDistance = shadowDistance;
		DisplayShadows = displayShadows;
		this.OnSettingsChanged?.Invoke();
	}

	public override int GetDefaultValue()
	{
		return 0;
	}

	public override List<string> GetChoices()
	{
		string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.HighSetting);
		string localizedString2 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.MediumSettings);
		string localizedString3 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.LowSetting);
		string localizedString4 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Disabled);
		return new List<string> { localizedString, localizedString2, localizedString3, localizedString4 };
	}

	public override void Dispose()
	{
		base.Dispose();
		SetShadowSettings(ShadowResolution._4096, 90f, 3, displayShadows: true);
	}

	public SettingCategory GetSettingCategory()
	{
		return SettingCategory.Graphics;
	}

	public string GetDisplayName()
	{
		return "Shadow Quality";
	}
}
