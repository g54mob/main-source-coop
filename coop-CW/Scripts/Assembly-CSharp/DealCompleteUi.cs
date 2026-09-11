using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DealCompleteUi : MonoBehaviour
{
	public Image networkLogo;

	public TextMeshProUGUI emailHeader;

	public TextMeshProUGUI emailAddress;

	public TextMeshProUGUI toAddresses;

	public TextMeshProUGUI emailBody;

	public TextMeshProUGUI rewards;

	public Button claimButton;

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
			emailAddress.text = deal.EmailAddress;
			emailBody.text = deal.GetSuccessEmailBody();
			rewards.text = deal.reward.GetRewardClaimDescription();
		}
	}

	public void LoadDeal(NetworkDealBase deal)
	{
		this.deal = deal;
		claimButton.onClick.AddListener(ClaimReward);
		emailHeader.text = "Great Job!";
		emailAddress.text = deal.EmailAddress;
		emailBody.text = deal.GetSuccessEmailBody();
		rewards.text = deal.reward.GetRewardClaimDescription();
		string text = "to: " + EmailGenerator.GetEmails();
		toAddresses.text = text;
		if ((bool)deal.GetIcon())
		{
			networkLogo.sprite = deal.GetIcon();
		}
	}

	private void ClaimReward()
	{
		Debug.Log("ClaimReward");
		NetworkDealBoss.me.RequestClaimReward();
		click.Play(base.transform.position);
	}
}
