using TMPro;

public class DivingBellRechargingState : DivingBellState
{
	public override void SetStatusText(TextMeshProUGUI statusText)
	{
		statusText.color = DivingBellState.RED;
		statusText.text = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.DivingBellRechargingState);
	}
}
