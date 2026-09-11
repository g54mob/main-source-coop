public class PickupTheCameraObjective : Objective
{
	public override string GetObjectiveDescription()
	{
		TextToShow = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.PickupTheCameraObjective);
		return TextToShow;
	}
}
