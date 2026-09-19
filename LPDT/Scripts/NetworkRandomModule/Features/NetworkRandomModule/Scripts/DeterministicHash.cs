using System;
using System.Security.Cryptography;
using System.Text;

namespace Features.NetworkRandomModule.Scripts
{
	public class DeterministicHash
	{
		private readonly int _raw;

		public DeterministicHash(string input)
		{
			using MD5 mD = MD5.Create();
			byte[] value = mD.ComputeHash(Encoding.UTF8.GetBytes(input));
			_raw = BitConverter.ToInt32(value, 0);
		}

		public int GetRaw()
		{
			return _raw;
		}
	}
}
