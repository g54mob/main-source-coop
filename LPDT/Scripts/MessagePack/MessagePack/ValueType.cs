namespace MessagePack
{
	internal enum ValueType : byte
	{
		Null = 0,
		True = 1,
		False = 2,
		Double = 3,
		Long = 4,
		ULong = 5,
		Decimal = 6,
		String = 7
	}
}
