using TMPro;

public class DivingBellNotReadyMissingPlayersState : DivingBellState
{
	public override void SetStatusText(TextMeshProUGUI statusText)
	{
		statusText.color = DivingBellState.RED;
		string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.DivingBellNotReadyMissingPlayersState);
		statusText.text = localizedString;
	}
}
