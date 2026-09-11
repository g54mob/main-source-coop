using Unity.Mathematics;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zorro.Settings;

public class RenderScaleSetting : FloatSetting
{
	public override void ApplyValue()
	{
		(GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset).renderScale = base.Value;
	}

	protected override float GetDefaultValue()
	{
		return 0.5f;
	}

	protected override float2 GetMinMaxValue()
	{
		return new float2(0.1f, 2f);
	}
}
