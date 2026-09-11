using TMPro;
using UnityEngine;

public class DivingBellSuitCellUI : MonoBehaviour
{
	public TextMeshProUGUI m_nameText;

	public TextMeshProUGUI m_oxygenText;

	public TextMeshProUGUI m_distanceText;

	private string m_OxygenText;

	private string m_DistanceText;

	private int m_OxygenValue;

	private int m_DistanceValue;

	private void Awake()
	{
		LocalizationKeys.OnLanguageChanged += OnLanguageChanged;
		OnLanguageChanged();
	}

	private void OnDestroy()
	{
		LocalizationKeys.OnLanguageChanged -= OnLanguageChanged;
	}

	private void OnLanguageChanged()
	{
		m_OxygenText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Oxygen).ToUpper();
		UpdateOxygenText();
		m_DistanceText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Distance).ToUpper();
		UpdateDistanceText();
	}

	public void Set(Player player, float dst)
	{
		m_nameText.text = player.refs.view.Owner.NickName;
		int num = Mathf.CeilToInt(player.data.OxygenDisplayPercentage() * 100f);
		int num2 = Mathf.RoundToInt(dst);
		if (m_OxygenValue != num)
		{
			m_OxygenValue = num;
			UpdateOxygenText();
		}
		if (m_DistanceValue != num2)
		{
			m_DistanceValue = num2;
			UpdateDistanceText();
		}
	}

	private void UpdateOxygenText()
	{
		m_oxygenText.text = $"{m_OxygenText}: {m_OxygenValue}%";
	}

	private void UpdateDistanceText()
	{
		m_distanceText.text = $"{m_DistanceText}: {m_DistanceValue}m";
	}
}
