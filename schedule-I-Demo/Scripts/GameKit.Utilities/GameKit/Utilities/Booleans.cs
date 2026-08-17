namespace GameKit.Utilities
{
	public static class Booleans
	{
		public static int ToInt(this bool b)
		{
			if (!b)
			{
				return 0;
			}
			return 1;
		}
	}
}
