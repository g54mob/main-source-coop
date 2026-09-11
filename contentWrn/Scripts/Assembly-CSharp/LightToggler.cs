using System;
using UnityEngine;

public class LightToggler : MonoBehaviour
{
	private Light light;

	private LightShadows shadowMode;

	private void Start()
	{
		light = GetComponent<Light>();
		shadowMode = light.shadows;
		if ((bool)Level.currentLevel)
		{
			Level currentLevel = Level.currentLevel;
			currentLevel.toggleLights = (Action<bool, Vector3, float>)Delegate.Combine(currentLevel.toggleLights, new Action<bool, Vector3, float>(ToggleLights));
			Level.currentLevel.lights.Add(light);
			Level.currentLevel.lightTogglers.Add(this);
		}
	}

	private void ToggleLights(bool setEnabled, Vector3 position, float range)
	{
		if (setEnabled || !(Vector3.Distance(base.transform.position, position) > range))
		{
			light.enabled = setEnabled;
		}
	}

	public void Check(bool disableShadows, bool enableLights)
	{
		float num = Vector3.Distance(base.transform.position, MainCamera.instance.transform.position);
		bool flag = true;
		if (num > 30f + light.range)
		{
			light.shadows = LightShadows.None;
			if (num > light.range * 4f + 50f)
			{
				flag = false;
			}
		}
		else if (!disableShadows)
		{
			light.shadows = shadowMode;
		}
		flag = flag && enableLights;
		if (light.enabled != flag)
		{
			light.enabled = flag;
		}
	}

	private void OnDestroy()
	{
		if ((bool)Level.currentLevel)
		{
			Level currentLevel = Level.currentLevel;
			currentLevel.toggleLights = (Action<bool, Vector3, float>)Delegate.Remove(currentLevel.toggleLights, new Action<bool, Vector3, float>(ToggleLights));
			if (Level.currentLevel.lights.Contains(light))
			{
				Level.currentLevel.lights.Remove(light);
			}
		}
	}
}
