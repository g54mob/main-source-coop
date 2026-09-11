public class ExtractVideoObjective : Objective
{
	public override string GetObjectiveDescription()
	{
		TextToShow = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.ExtractVideoObjective);
		return TextToShow;
	}
}
