using System;

internal static class Extension
{
	public static void Shuffle<T>(this T[] array)
	{
		int num = array.Length;
		Random random = new Random();
		while (num > 1)
		{
			int num2 = random.Next(num--);
			T val = array[num];
			array[num] = array[num2];
			array[num2] = val;
		}
	}
}
