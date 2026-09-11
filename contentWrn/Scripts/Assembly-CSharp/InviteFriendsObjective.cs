public class InviteFriendsObjective : Objective
{
	public override string GetObjectiveDescription()
	{
		TextToShow = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.InviteFriendsObjective);
		return TextToShow;
	}
}
