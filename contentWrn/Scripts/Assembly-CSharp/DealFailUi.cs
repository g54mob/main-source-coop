using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DealFailUi : MonoBehaviour
{
	public Image networkLogo;

	public TextMeshProUGUI toAddresses;

	public TextMeshProUGUI m_emailAddress;

	public TextMeshProUGUI m_body;

	public Button acceptFailButton;

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
			m_body.text = deal.GetFailedEmailBody();
		}
	}

	public void LoadDeal(NetworkDealBase deal)
	{
		this.deal = deal;
		networkLogo.sprite = deal.GetIcon();
		m_emailAddress.text = deal.EmailAddress;
		acceptFailButton.onClick.AddListener(AcceptFail);
		toAddresses.text = "to: " + EmailGenerator.GetEmails();
		m_body.text = deal.GetFailedEmailBody();
	}

	private void AcceptFail()
	{
		Debug.Log("Accept fail");
		NetworkDealBoss.me.RemoveDeal(deal);
		click.Play(base.transform.position);
	}
}
