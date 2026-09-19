using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Google.Apis.Util;

namespace Google.Apis.Auth.OAuth2
{
	internal class Pkcs8
	{
		internal class Asn1
		{
			internal enum Tag
			{
				Integer = 2,
				OctetString = 4,
				Null = 5,
				ObjectIdentifier = 6,
				Sequence = 16
			}

			internal class Decoder
			{
				private byte[] _bytes;

				private int _index;

				public Decoder(byte[] bytes)
				{
					_bytes = bytes;
					_index = 0;
				}

				public object Decode()
				{
					Tag tag = ReadTag();
					return tag switch
					{
						Tag.Integer => ReadInteger(), 
						Tag.OctetString => ReadOctetString(), 
						Tag.Null => ReadNull(), 
						Tag.ObjectIdentifier => ReadOid(), 
						Tag.Sequence => ReadSequence(), 
						_ => throw new NotSupportedException($"Tag '{tag}' not supported."), 
					};
				}

				private byte NextByte()
				{
					return _bytes[_index++];
				}

				private byte[] ReadLengthPrefixedBytes()
				{
					int length = ReadLength();
					return ReadBytes(length);
				}

				private byte[] ReadInteger()
				{
					return ReadLengthPrefixedBytes();
				}

				private object ReadOctetString()
				{
					return new Decoder(ReadLengthPrefixedBytes()).Decode();
				}

				private object ReadNull()
				{
					if (ReadLength() != 0)
					{
						throw new InvalidDataException("Invalid data, Null length must be 0.");
					}
					return null;
				}

				private int[] ReadOid()
				{
					byte[] array = ReadLengthPrefixedBytes();
					List<int> list = new List<int>();
					bool flag = true;
					int num = 0;
					while (num < array.Length)
					{
						int num2 = 0;
						byte b;
						do
						{
							b = array[num++];
							if ((num2 & 0xFF000000u) != 0L)
							{
								throw new NotSupportedException("Oid subId > 2^31 not supported.");
							}
							num2 = (num2 << 7) | (b & 0x7F);
						}
						while ((b & 0x80) != 0);
						if (flag)
						{
							flag = false;
							list.Add(num2 / 40);
							list.Add(num2 % 40);
						}
						else
						{
							list.Add(num2);
						}
					}
					return list.ToArray();
				}

				private object[] ReadSequence()
				{
					int num = ReadLength();
					int num2 = _index + num;
					if (num2 < 0 || num2 > _bytes.Length)
					{
						throw new InvalidDataException("Invalid sequence, too long.");
					}
					List<object> list = new List<object>();
					while (_index < num2)
					{
						list.Add(Decode());
					}
					return list.ToArray();
				}

				private byte[] ReadBytes(int length)
				{
					if (length <= 0)
					{
						throw new ArgumentOutOfRangeException("length", "length must be positive.");
					}
					if (_bytes.Length - length < 0)
					{
						throw new ArgumentException("Cannot read past end of buffer.");
					}
					byte[] array = new byte[length];
					Array.Copy(_bytes, _index, array, 0, length);
					_index += length;
					return array;
				}

				private Tag ReadTag()
				{
					int num = NextByte() & 0x1F;
					if (num == 31)
					{
						throw new NotSupportedException("Tags of value > 30 not supported.");
					}
					return (Tag)num;
				}

				private int ReadLength()
				{
					byte b = NextByte();
					if ((b & 0x80) == 0)
					{
						return b;
					}
					if (b == byte.MaxValue)
					{
						throw new InvalidDataException("Invalid length byte: 0xff");
					}
					int num = b & 0x7F;
					if (num == 0)
					{
						throw new NotSupportedException("Lengths in Indefinite Form not supported.");
					}
					int num2 = 0;
					for (int i = 0; i < num; i++)
					{
						if ((num2 & 0xFF800000u) != 0L)
						{
							throw new NotSupportedException("Lengths > 2^31 not supported.");
						}
						num2 = (num2 << 8) | NextByte();
					}
					return num2;
				}
			}

			public static object Decode(byte[] bs)
			{
				return new Decoder(bs).Decode();
			}
		}

		public static RSAParameters DecodeRsaParameters(string pkcs8PrivateKey)
		{
			pkcs8PrivateKey.ThrowIfNullOrEmpty("pkcs8PrivateKey");
			pkcs8PrivateKey = pkcs8PrivateKey.Trim();
			if (!pkcs8PrivateKey.StartsWith("-----BEGIN PRIVATE KEY-----") || !pkcs8PrivateKey.EndsWith("-----END PRIVATE KEY-----"))
			{
				throw new ArgumentException("PKCS8 data must be contained within '-----BEGIN PRIVATE KEY-----' and '-----END PRIVATE KEY-----'.", "pkcs8PrivateKey");
			}
			object[] array = (object[])((object[])Asn1.Decode(Convert.FromBase64String(pkcs8PrivateKey.Substring("-----BEGIN PRIVATE KEY-----".Length, pkcs8PrivateKey.Length - "-----BEGIN PRIVATE KEY-----".Length - "-----END PRIVATE KEY-----".Length))))[2];
			return new RSAParameters
			{
				Modulus = TrimLeadingZeroes((byte[])array[1]),
				Exponent = TrimLeadingZeroes((byte[])array[2], alignTo8Bytes: false),
				D = TrimLeadingZeroes((byte[])array[3]),
				P = TrimLeadingZeroes((byte[])array[4]),
				Q = TrimLeadingZeroes((byte[])array[5]),
				DP = TrimLeadingZeroes((byte[])array[6]),
				DQ = TrimLeadingZeroes((byte[])array[7]),
				InverseQ = TrimLeadingZeroes((byte[])array[8])
			};
		}

		internal static byte[] TrimLeadingZeroes(byte[] bs, bool alignTo8Bytes = true)
		{
			int i;
			for (i = 0; i < bs.Length && bs[i] == 0; i++)
			{
			}
			int num = bs.Length - i;
			if (alignTo8Bytes)
			{
				int num2 = num & 7;
				if (num2 != 0)
				{
					num += 8 - num2;
				}
			}
			if (num == bs.Length)
			{
				return bs;
			}
			byte[] array = new byte[num];
			if (num < bs.Length)
			{
				Buffer.BlockCopy(bs, bs.Length - num, array, 0, num);
			}
			else
			{
				Buffer.BlockCopy(bs, 0, array, num - bs.Length, bs.Length);
			}
			return array;
		}
	}
}
