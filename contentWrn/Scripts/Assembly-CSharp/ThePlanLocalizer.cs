using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class ThePlanLocalizer : MonoBehaviour
{
	[SerializeField]
	private MeshRenderer m_PlanRenderer;

	private void Awake()
	{
		LocalizationKeys.OnLanguageChanged += LocalizeMe;
	}

	private void OnDestroy()
	{
		LocalizationKeys.OnLanguageChanged -= LocalizeMe;
	}

	private void Start()
	{
		LocalizeMe();
	}

	private void LocalizeMe()
	{
		Locale selectedLocale = LocalizationSettings.SelectedLocale;
		Texture2D localizedAsset = LocalizationSettings.AssetDatabase.GetLocalizedAsset<Texture2D>("ThePlan", selectedLocale);
		m_PlanRenderer.material.SetTexture("_Variation", localizedAsset);
	}
}
