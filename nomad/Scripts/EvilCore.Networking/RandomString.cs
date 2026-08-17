using System;
using System.Text;

public class RandomString
{
	public static string Generate(int size)
	{
		StringBuilder stringBuilder = new StringBuilder(size);
		Random random = new Random();
		char c = 'a';
		char c2 = 'A';
		for (int i = 0; i < size; i++)
		{
			char c3 = ((random.Next(0, 2) != 0) ? c2 : c);
			char value = (char)random.Next(c3, c3 + 26);
			stringBuilder.Append(value);
		}
		return stringBuilder.ToString();
	}
}
