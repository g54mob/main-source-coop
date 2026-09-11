public class CelebrateObjective : Objective
{
	public override string GetObjectiveDescription()
	{
		TextToShow = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.CelebrateObjective);
		return TextToShow;
	}
}
