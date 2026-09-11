public class WakeUpObjective : Objective
{
	public override string GetObjectiveDescription()
	{
		TextToShow = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.WakeUpObjective);
		return TextToShow;
	}
}
