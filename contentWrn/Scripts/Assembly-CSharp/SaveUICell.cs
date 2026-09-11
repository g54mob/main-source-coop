using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zorro.ControllerSupport;
using Zorro.UI;

public class SaveUICell : TAB_Button, ISelectHandler, IEventSystemHandler, IDeselectHandler
{
	public InputActionReference m_removeAction;

	public InputActionReference m_loadAction;

	public TextMeshProUGUI moneyText;

	public TextMeshProUGUI dayText;

	public TextMeshProUGUI dateText;

	public GameObject m_emtpySave;

	public GameObject m_notEmptySave;

	public GameObject m_deleteButton;

	private string m_MoneyText;

	private string m_DayText;

	public int SaveIndex { get; set; }

	private void OnEnable()
	{
		m_MoneyText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Money) + ": ";
		m_DayText = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Day);
	}

	protected override void UpdateSelection()
	{
		base.UpdateSelection();
		background.enabled = selected;
		text.color = (selected ? Color.black : Color.white);
		if (base.Selected)
		{
			text.SetText("<b>" + text.text);
		}
		else
		{
			text.SetText(text.text.Replace("<b>", ""));
		}
	}

	public void SetSave(Save currentSave, int saveIndex)
	{
		SaveIndex = saveIndex;
		m_emtpySave.SetActive(currentSave == null);
		m_notEmptySave.SetActive(currentSave != null);
		m_deleteButton.SetActive(currentSave != null);
		if (currentSave != null)
		{
			moneyText.text = m_MoneyText + currentSave.SerializedSave.Money;
			dayText.text = m_DayText.Replace("{0}", currentSave.SerializedSave.CurrentDay.ToString());
			dateText.text = currentSave.Date.ToString("d");
		}
	}

	public void OnDeleteClicked()
	{
		string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.DeleteSave);
		string localizedString2 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.DeleteSaveConfirm);
		string localizedString3 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Yes);
		string localizedString4 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.No);
		Modal.Show(localizedString, localizedString2, new ModalOption[2]
		{
			new ModalOption(localizedString3, delegate
			{
				SaveSystem.DeleteSave(SaveIndex);
				SetSave(null, SaveIndex);
			}),
			new ModalOption(localizedString4)
		});
	}

	public void OnSelect(BaseEventData eventData)
	{
		if (InputHandler.GetCurrentUsedInputScheme() == InputScheme.Gamepad)
		{
			base.transform.parent.GetComponent<ITABS>().SelectGeneric(this);
		}
	}

	public override void ButtonClicked()
	{
		base.ButtonClicked();
		if (InputHandler.GetCurrentUsedInputScheme() == InputScheme.Gamepad)
		{
			MainMenuHandler.Instance.Host(SaveIndex);
		}
	}

	public void OnDeselect(BaseEventData eventData)
	{
	}

	private void Update()
	{
		if (base.Selected && m_deleteButton.activeSelf && m_removeAction.action.WasPressedThisFrame())
		{
			OnDeleteClicked();
		}
	}
}
