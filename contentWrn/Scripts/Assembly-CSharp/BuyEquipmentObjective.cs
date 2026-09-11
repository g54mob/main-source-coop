public class BuyEquipmentObjective : Objective
{
	public override string GetObjectiveDescription()
	{
		TextToShow = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.BuyEquipmentObjective);
		return TextToShow;
	}
}
