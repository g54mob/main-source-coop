using System;

namespace NetworkServices.RoomCodeGenerators
{
	public class DefaultRoomCodeGenerator : IRoomCodeGenerator
	{
		private const string CODE_CHARACTERS = "ABCDEFGHJKLMNPQRSTUVWYZ23456789";

		private readonly Random _random = new Random();

		public string Generate(int length)
		{
			char[] array = "ABCDEFGHJKLMNPQRSTUVWYZ23456789".ToCharArray();
			string text = "";
			for (int i = 0; i < length; i++)
			{
				text += array[_random.Next(0, array.Length)];
			}
			return text;
		}
	}
}
