public class PickupDiscObjective : Objective
{
	public override string GetObjectiveDescription()
	{
		TextToShow = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.PickupDiscObjective);
		return TextToShow;
	}
}
