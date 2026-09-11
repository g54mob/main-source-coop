using System.Collections.Generic;
using UnityEngine;
using Zorro.Core;
using Zorro.Settings;

public class WorldShaderSetting : EnumSetting
{
	public override void ApplyValue()
	{
		Debug.Log("WorldShaderSetting ApplyValue() called. Value: " + base.Value);
		switch (base.Value)
		{
		case 0:
			SetShader(SingletonAsset<WorldShaderPerformanceTestingData>.Instance.defaultShader);
			break;
		case 1:
			SetShader(SingletonAsset<WorldShaderPerformanceTestingData>.Instance.noLightShader);
			break;
		case 2:
			SetShader(SingletonAsset<WorldShaderPerformanceTestingData>.Instance.fullyStrippedShader);
			break;
		}
	}

	private void SetShader(Shader shader)
	{
		Material[] materials = SingletonAsset<WorldShaderPerformanceTestingData>.Instance.materials;
		for (int i = 0; i < materials.Length; i++)
		{
			materials[i].shader = shader;
		}
	}

	public override int GetDefaultValue()
	{
		return 0;
	}

	public override List<string> GetChoices()
	{
		return new List<string> { "Default", "No Lighting", "Fully Stripped" };
	}

	public override void Dispose()
	{
		base.Dispose();
		SetShader(SingletonAsset<WorldShaderPerformanceTestingData>.Instance.defaultShader);
	}
}
