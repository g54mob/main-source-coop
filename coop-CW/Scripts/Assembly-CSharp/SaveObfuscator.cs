using System;

public static class SaveObfuscator
{
	public static string OfbuscateSaveData(string data)
	{
		return Caesar(data, 42);
	}

	public static string DeobfuscateSaveData(string data)
	{
		return Caesar(data, -42);
	}

	public static string Caesar(string source, short shift)
	{
		int num = Convert.ToInt32('\uffff');
		int num2 = Convert.ToInt32('\0');
		char[] array = source.ToCharArray();
		for (int i = 0; i < array.Length; i++)
		{
			int num3 = Convert.ToInt32(array[i]) + shift;
			if (num3 > num)
			{
				num3 -= num;
			}
			else if (num3 < num2)
			{
				num3 += num;
			}
			array[i] = Convert.ToChar(num3);
		}
		return new string(array);
	}
}
