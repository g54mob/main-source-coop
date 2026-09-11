using TMPro;
using UnityEngine;

public class UI_DaysLeft : MonoBehaviour
{
	private TextMeshProUGUI text;

	private string m_DaysLeftText;

	private string m_LastDayText;

	private int m_daysLeft = -1;

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
		m_DaysLeftText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.DaysLeft);
		m_LastDayText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.LastDay);
	}

	private void Update()
	{
		if (SurfaceNetworkHandler.RoomStats != null)
		{
			int num = SurfaceNetworkHandler.RoomStats.DaysPerQutoa - SurfaceNetworkHandler.RoomStats.CurrentQuotaDay;
			num++;
			if (num != m_daysLeft)
			{
				m_daysLeft = num;
				text.text = ((num == 1) ? m_LastDayText : m_DaysLeftText.Replace("{0}", num.ToString()));
			}
		}
	}

	private void OnLanguageChanged()
	{
		m_DaysLeftText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.DaysLeft);
		m_LastDayText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.LastDay);
		text.text = ((m_daysLeft == 1) ? m_LastDayText : m_DaysLeftText.Replace("{0}", m_daysLeft.ToString()));
	}
}
