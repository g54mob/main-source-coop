public struct ItemDescriptor
{
	public Item item;

	public ItemInstanceData data;

	public static ItemDescriptor Empty => default(ItemDescriptor);

	public ItemDescriptor(Item item, ItemInstanceData data)
	{
		this.item = item;
		this.data = data;
	}
}
