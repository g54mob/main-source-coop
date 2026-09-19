namespace Mirror.Examples.PickupsDropsChilds
{
	internal interface IEquipped
	{
		EquippedItemConfig equippedItemConfig { get; set; }

		void Use();

		void AddUsages(byte usages);

		void ResetUsages();

		void ResetUsages(byte usages);
	}
}
