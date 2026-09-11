public class UploadVideoObjective : Objective
{
	public override string GetObjectiveDescription()
	{
		TextToShow = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.UploadVideoObjective);
		return TextToShow;
	}
}
