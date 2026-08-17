using System;

namespace EpicTransport
{
	public struct Packet
	{
		public const int headerSize = 9;

		public int id;

		public int fragment;

		public bool moreFragments;

		public byte[] data;

		public int size => 9 + data.Length;

		public byte[] ToBytes()
		{
			byte[] array = new byte[size];
			array[0] = (byte)id;
			array[1] = (byte)(id >> 8);
			array[2] = (byte)(id >> 16);
			array[3] = (byte)(id >> 24);
			array[4] = (byte)fragment;
			array[5] = (byte)(fragment >> 8);
			array[6] = (byte)(fragment >> 16);
			array[7] = (byte)(fragment >> 24);
			array[8] = (byte)(moreFragments ? 1 : 0);
			Array.Copy(data, 0, array, 9, data.Length);
			return array;
		}

		public void FromBytes(byte[] array)
		{
			id = BitConverter.ToInt32(array, 0);
			fragment = BitConverter.ToInt32(array, 4);
			moreFragments = array[8] == 1;
			data = new byte[array.Length - 9];
			Array.Copy(array, 9, data, 0, data.Length);
		}
	}
}
