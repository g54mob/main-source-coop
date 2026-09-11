using TMPro;

public class DivingBellReadyState : DivingBellState
{
	public bool OnSurface { get; private set; }

	public DivingBellReadyState(bool onSurface)
	{
		OnSurface = onSurface;
	}

	public override void SetStatusText(TextMeshProUGUI statusText)
	{
		statusText.color = DivingBellState.GREEN;
		statusText.text = (OnSurface ? LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.DivingBellReady) : LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.DivingBellReadySurface));
	}
}
