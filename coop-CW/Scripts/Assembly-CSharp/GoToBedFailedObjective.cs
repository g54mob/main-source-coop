public class GoToBedFailedObjective : Objective
{
	public override string GetObjectiveDescription()
	{
		TextToShow = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.GoToBedFailedObjective);
		return TextToShow;
	}
}
