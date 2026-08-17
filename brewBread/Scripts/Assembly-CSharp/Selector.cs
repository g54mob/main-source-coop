public struct Selector
{
	public int maxValue;

	public int minValue;

	private int Value;

	public int value
	{
		get
		{
			return Value;
		}
		set
		{
			Value = value;
		}
	}

	public Selector(int maxValue = 1, int minValue = -1, int value = 0)
	{
		this.maxValue = maxValue;
		this.minValue = minValue;
		Value = value;
	}
}
