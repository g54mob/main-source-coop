using UnityEngine;
using UnityEngine.Rendering.Universal;

[DefaultExecutionOrder(1)]
public class CameraShadowController : MonoBehaviour
{
	private UniversalAdditionalCameraData m_cameraData;

	private ShadowQualitySetting m_shadowSettings;

	private void Awake()
	{
		if (!TryGetComponent<UniversalAdditionalCameraData>(out m_cameraData))
		{
			Debug.LogError("Missing UniversalAdditionalCameraData");
			return;
		}
		m_shadowSettings = GameHandler.Instance.SettingsHandler.GetSetting<ShadowQualitySetting>();
		if (m_shadowSettings == null)
		{
			Debug.LogError("Missing ShadowQualitySetting");
			return;
		}
		m_shadowSettings.OnSettingsChanged += OnCameraSettingsChanged;
		OnCameraSettingsChanged();
	}

	private void OnDestroy()
	{
		if (m_shadowSettings != null)
		{
			m_shadowSettings.OnSettingsChanged -= OnCameraSettingsChanged;
		}
	}

	private void OnCameraSettingsChanged()
	{
		if (m_cameraData == null && !TryGetComponent<UniversalAdditionalCameraData>(out m_cameraData))
		{
			Debug.LogError("Missing UniversalAdditionalCameraData");
		}
		else
		{
			m_cameraData.renderShadows = m_shadowSettings.DisplayShadows;
		}
	}
}
