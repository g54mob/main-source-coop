using System.Collections;
using HorizonBasedAmbientOcclusion.Universal;
using SCPE;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostVolumeHandler : MonoBehaviour
{
	public bool isSurface;

	private AmbientOcclusionSetting m_ambientOcclusionSetting;

	private ChromaticAberrationSetting m_chromaticAberrationSetting;

	private EdgeDetectionSetting m_edgeDetectionSetting;

	private BrightnessSetting m_brightnessSetting;

	private HBAO m_hbao;

	private ChromaticAberration m_chromaticAberration;

	private EdgeDetection m_edgeDetection;

	private LiftGammaGain m_lightGammaGain;

	private Volume m_volume;

	private IEnumerator Start()
	{
		yield return null;
		m_volume = GetComponent<Volume>();
		m_ambientOcclusionSetting = GameHandler.Instance.SettingsHandler.GetSetting<AmbientOcclusionSetting>();
		m_chromaticAberrationSetting = GameHandler.Instance.SettingsHandler.GetSetting<ChromaticAberrationSetting>();
		m_edgeDetectionSetting = GameHandler.Instance.SettingsHandler.GetSetting<EdgeDetectionSetting>();
		m_brightnessSetting = GameHandler.Instance.SettingsHandler.GetSetting<BrightnessSetting>();
		if (m_volume.profile.TryGet<HBAO>(out var component))
		{
			m_hbao = component;
		}
		else
		{
			Debug.LogError("HBAO not found in volume");
		}
		if (m_volume.profile.TryGet<ChromaticAberration>(out var component2))
		{
			m_chromaticAberration = component2;
		}
		else
		{
			Debug.LogError("ChromaticAberration not found in volume");
		}
		if (m_volume.profile.TryGet<EdgeDetection>(out var component3))
		{
			m_edgeDetection = component3;
		}
		else
		{
			Debug.LogError("EdgeDetection not found in volume");
		}
		if (m_volume.profile.TryGet<LiftGammaGain>(out var component4))
		{
			m_lightGammaGain = component4;
		}
		else
		{
			Debug.LogError("ColorAdjustments not found in volume");
		}
	}

	private void LateUpdate()
	{
		if (!(m_volume == null))
		{
			m_hbao.active = m_ambientOcclusionSetting.Value == 1;
			m_chromaticAberration.active = m_chromaticAberrationSetting.Value == 1;
			m_edgeDetection.active = m_edgeDetectionSetting.Value == 1;
			Vector4 value = m_lightGammaGain.gamma.value;
			value.w = m_brightnessSetting.GetGamma();
			m_lightGammaGain.gamma.value = value;
		}
	}
}
