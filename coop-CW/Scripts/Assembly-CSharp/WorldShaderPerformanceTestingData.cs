using UnityEngine;
using Zorro.Core;

public class WorldShaderPerformanceTestingData : SingletonAsset<WorldShaderPerformanceTestingData>
{
	public Material[] materials;

	public Shader defaultShader;

	public Shader noLightShader;

	public Shader fullyStrippedShader;
}
