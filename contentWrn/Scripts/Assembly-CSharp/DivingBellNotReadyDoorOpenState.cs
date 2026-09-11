using TMPro;

public class DivingBellNotReadyDoorOpenState : DivingBellState
{
	public override void SetStatusText(TextMeshProUGUI statusText)
	{
		statusText.color = DivingBellState.RED;
		string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.DivingBellNotReadyDoorOpenState);
		statusText.text = localizedString;
	}
}
