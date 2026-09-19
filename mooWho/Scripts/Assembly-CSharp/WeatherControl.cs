using UnityEngine;

public class WeatherControl : MonoBehaviour
{
	[Range(0f, 1f)]
	public float windIntensity;

	[Range(0f, 1f)]
	public float weatherIntensity;

	private void Start()
	{
	}

	private void Update()
	{
		UpdateShaderGlobals();
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawIcon(base.transform.position, "../Synty/PNB_Core/Textures/SyntyLogo.png", allowScaling: true);
		Gizmos.DrawLine(base.transform.position, base.transform.position + base.transform.forward * 4f);
		Gizmos.DrawLine(base.transform.position + base.transform.forward * 4f, base.transform.position + base.transform.forward * 3f + base.transform.right * 0.5f);
		Gizmos.DrawLine(base.transform.position + base.transform.forward * 4f, base.transform.position + base.transform.forward * 3f - base.transform.right * 0.5f);
		UpdateShaderGlobals();
	}

	private void UpdateShaderGlobals()
	{
		Shader.SetGlobalVector("_WindDirection", base.transform.forward);
		Shader.SetGlobalFloat("_GaleStrength", weatherIntensity);
		Shader.SetGlobalFloat("_WindIntensity", windIntensity);
	}
}
