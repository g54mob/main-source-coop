public class FilmSomethingScaryObjective : Objective
{
	public override string GetObjectiveDescription()
	{
		TextToShow = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.FilmSomethingScaryObjective);
		return TextToShow;
	}
}
