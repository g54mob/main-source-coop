using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class DealProgressUi : MonoBehaviour
{
	public TextMeshProUGUI title;

	public TextMeshProUGUI m_body;

	public TextMeshProUGUI m_rewards;

	public TextMeshProUGUI m_progressText;

	public Image networkIcon;

	public ProceduralImage progress;

	public NetworkDealBase deal;

	public SFX_Instance click;

	private void Awake()
	{
		LocalizationKeys.OnLanguageChanged += OnLanguageChanged;
	}

	private void OnDestroy()
	{
		LocalizationKeys.OnLanguageChanged -= OnLanguageChanged;
	}

	private void OnLanguageChanged()
	{
		if (deal != null)
		{
			deal.MakeLocalizedStrings();
			m_body.text = deal.DealDescription();
			m_rewards.text = deal.reward.GetRewardDescription();
		}
	}

	public void LoadDeal(NetworkDealBase deal)
	{
		this.deal = deal;
		m_body.text = deal.DealDescription();
		m_rewards.text = deal.reward.GetRewardDescription();
		if ((bool)deal.GetIcon())
		{
			networkIcon.sprite = deal.GetIcon();
		}
		progress.fillAmount = deal.GetProgress();
		m_progressText.text = deal.ProgressInt + "/" + deal.RequiredAmount();
	}

	public void Update()
	{
		progress.fillAmount = deal.GetProgress();
		m_progressText.text = deal.GetProgressText();
	}
}
