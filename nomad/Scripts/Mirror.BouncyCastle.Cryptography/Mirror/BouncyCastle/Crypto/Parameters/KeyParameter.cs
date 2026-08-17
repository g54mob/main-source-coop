using System;
using Mirror.BouncyCastle.Utilities;

namespace Mirror.BouncyCastle.Crypto.Parameters
{
	public class KeyParameter : ICipherParameters
	{
		private readonly byte[] m_key;

		public int KeyLength => m_key.Length;

		public KeyParameter(byte[] key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			m_key = (byte[])key.Clone();
		}

		public void CopyTo(byte[] buf, int off, int len)
		{
			if (m_key.Length != len)
			{
				throw new ArgumentOutOfRangeException("len");
			}
			Array.Copy(m_key, 0, buf, off, len);
		}

		public byte[] GetKey()
		{
			return (byte[])m_key.Clone();
		}

		internal bool FixedTimeEquals(byte[] data)
		{
			return Arrays.FixedTimeEquals(m_key, data);
		}
	}
}
