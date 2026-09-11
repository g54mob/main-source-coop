using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DealProposalUI : MonoBehaviour
{
	public TextMeshProUGUI title;

	public TextMeshProUGUI m_emailAddress;

	public TextMeshProUGUI m_toAddresses;

	public TextMeshProUGUI m_body;

	public TextMeshProUGUI m_rewards;

	public TextMeshProUGUI m_difficulty;

	public TextMeshProUGUI m_signDeal_Text;

	public Image networkLogo;

	public NetworkDealBase deal;

	public Button acceptButton;

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
		m_signDeal_Text.text = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Deal_SignDeal);
		if (deal != null)
		{
			deal.MakeLocalizedStrings();
			title.text = deal.EmailTitle_Localized;
			m_emailAddress.text = deal.EmailAddress;
			m_difficulty.text = deal.GetDifficultyText();
			m_body.text = deal.DealDescription();
			m_rewards.text = deal.reward.GetRewardDescription();
		}
	}

	public void LoadDeal(NetworkDealBase deal)
	{
		if (!deal.Inited)
		{
			Debug.LogError("Deal not inited");
		}
		this.deal = deal;
		title.text = deal.EmailTitle_Localized;
		m_emailAddress.text = deal.EmailAddress;
		m_difficulty.text = deal.GetDifficultyText();
		string text = "to: " + EmailGenerator.GetEmails();
		m_toAddresses.text = text;
		m_body.text = deal.DealDescription();
		m_rewards.text = deal.reward.GetRewardDescription();
		if ((bool)deal.GetIcon())
		{
			networkLogo.sprite = deal.GetIcon();
		}
		acceptButton.onClick.AddListener(AcceptDeal);
	}

	public void AcceptDeal()
	{
		Debug.Log("Deal accepted");
		NetworkDealBoss.me.AddDeal(deal);
	}
}
