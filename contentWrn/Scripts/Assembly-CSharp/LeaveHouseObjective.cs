public class LeaveHouseObjective : Objective
{
	public override string GetObjectiveDescription()
	{
		TextToShow = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.LeaveHouseObjective);
		return TextToShow;
	}
}
