using TMPro;
using UnityEngine;

public class QuotaScreenViewer : MonoBehaviour
{
	private TextMeshPro m_QuotaText;

	private SurfaceNetworkHandler m_Surface;

	private void Awake()
	{
		m_QuotaText = GetComponentInChildren<TextMeshPro>();
	}

	private void Start()
	{
		InitReferences();
	}

	private void InitReferences()
	{
		m_Surface = SurfaceNetworkHandler.Instance;
		if (SurfaceNetworkHandler.RoomStats != null)
		{
			OnQuotaUpdate(SurfaceNetworkHandler.RoomStats);
		}
		m_Surface.AddOnStatsUpdateCallBack(OnQuotaUpdate);
	}

	private void OnQuotaUpdate(RoomStatsHolder stats)
	{
		int currentQuotaDay = stats.CurrentQuotaDay;
		int money = stats.Money;
		int daysPerQutoa = stats.DaysPerQutoa;
		int currentQuota = stats.CurrentQuota;
		int quotaToReach = stats.QuotaToReach;
		string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Day);
		string localizedString2 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Quota);
		string localizedString3 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Money);
		m_QuotaText.text = $"{localizedString} {currentQuotaDay}/{daysPerQutoa}\n {localizedString2} {currentQuota}/{quotaToReach} \n {localizedString3} {money}";
	}
}
