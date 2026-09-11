using TMPro;
using UnityEngine;

public class UI_Views : MonoBehaviour
{
	private TextMeshProUGUI text;

	private string m_ViewsText;

	private int m_currentViews;

	private int m_requiredViews;

	private void Awake()
	{
		LocalizationKeys.OnLanguageChanged += OnLanguageChanged;
	}

	private void OnDestroy()
	{
		LocalizationKeys.OnLanguageChanged -= OnLanguageChanged;
	}

	private void Start()
	{
		text = GetComponent<TextMeshProUGUI>();
		m_ViewsText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Views);
	}

	private void Update()
	{
		if (SurfaceNetworkHandler.RoomStats != null)
		{
			int scoreToViews = BigNumbers.GetScoreToViews(SurfaceNetworkHandler.RoomStats.CurrentQuota, GameAPI.CurrentDay);
			int scoreToViews2 = BigNumbers.GetScoreToViews(SurfaceNetworkHandler.RoomStats.QuotaToReach, GameAPI.CurrentDay);
			if (scoreToViews != m_currentViews || scoreToViews2 != m_requiredViews)
			{
				m_currentViews = scoreToViews;
				m_requiredViews = scoreToViews2;
				text.text = BigNumbers.ViewsToString(scoreToViews) + "/" + BigNumbers.ViewsToString(scoreToViews2) + " " + m_ViewsText;
			}
		}
	}

	private void OnLanguageChanged()
	{
		m_ViewsText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Views);
		text.text = BigNumbers.ViewsToString(m_currentViews) + "/" + BigNumbers.ViewsToString(m_requiredViews) + " " + m_ViewsText;
	}
}
