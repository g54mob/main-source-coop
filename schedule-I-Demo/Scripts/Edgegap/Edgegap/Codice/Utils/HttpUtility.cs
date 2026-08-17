using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace Edgegap.Codice.Utils
{
	public sealed class HttpUtility
	{
		private sealed class HttpQSCollection : NameValueCollection
		{
			public override string ToString()
			{
				int count = Count;
				if (count == 0)
				{
					return "";
				}
				StringBuilder stringBuilder = new StringBuilder();
				string[] allKeys = AllKeys;
				for (int i = 0; i < count; i++)
				{
					stringBuilder.AppendFormat("{0}={1}&", allKeys[i], UrlEncode(base[allKeys[i]]));
				}
				if (stringBuilder.Length > 0)
				{
					int length = stringBuilder.Length - 1;
					stringBuilder.Length = length;
				}
				return stringBuilder.ToString();
			}
		}

		private static void WriteCharBytes(IList buf, char ch, Encoding e)
		{
			if (ch > 'ÿ')
			{
				char[] chars = new char[1] { ch };
				byte[] bytes = e.GetBytes(chars);
				foreach (byte b in bytes)
				{
					buf.Add(b);
				}
			}
			else
			{
				buf.Add((byte)ch);
			}
		}

		public static string UrlDecode(string s, Encoding e)
		{
			if (s == null)
			{
				return null;
			}
			if (s.IndexOf('%') == -1 && s.IndexOf('+') == -1)
			{
				return s;
			}
			if (e == null)
			{
				e = Encoding.UTF8;
			}
			long num = s.Length;
			List<byte> list = new List<byte>();
			for (int i = 0; i < num; i++)
			{
				char c = s[i];
				if (c == '%' && i + 2 < num && s[i + 1] != '%')
				{
					int num3;
					if (s[i + 1] == 'u' && i + 5 < num)
					{
						int num2 = GetChar(s, i + 2, 4);
						if (num2 != -1)
						{
							WriteCharBytes(list, (char)num2, e);
							i += 5;
						}
						else
						{
							WriteCharBytes(list, '%', e);
						}
					}
					else if ((num3 = GetChar(s, i + 1, 2)) != -1)
					{
						WriteCharBytes(list, (char)num3, e);
						i += 2;
					}
					else
					{
						WriteCharBytes(list, '%', e);
					}
				}
				else if (c == '+')
				{
					WriteCharBytes(list, ' ', e);
				}
				else
				{
					WriteCharBytes(list, c, e);
				}
			}
			byte[] bytes = list.ToArray();
			return e.GetString(bytes);
		}

		private static int GetInt(byte b)
		{
			char c = (char)b;
			if (c >= '0' && c <= '9')
			{
				return c - 48;
			}
			if (c >= 'a' && c <= 'f')
			{
				return c - 97 + 10;
			}
			if (c < 'A' || c > 'F')
			{
				return -1;
			}
			return c - 65 + 10;
		}

		private static int GetChar(string str, int offset, int length)
		{
			int num = 0;
			int num2 = length + offset;
			for (int i = offset; i < num2; i++)
			{
				char c = str[i];
				if (c > '\u007f')
				{
					return -1;
				}
				int num3 = GetInt((byte)c);
				if (num3 == -1)
				{
					return -1;
				}
				num = (num << 4) + num3;
			}
			return num;
		}

		public static string UrlEncode(string str)
		{
			return UrlEncode(str, Encoding.UTF8);
		}

		public static string UrlEncode(string s, Encoding Enc)
		{
			if (s == null)
			{
				return null;
			}
			if (s == string.Empty)
			{
				return string.Empty;
			}
			bool flag = false;
			int length = s.Length;
			for (int i = 0; i < length; i++)
			{
				char c = s[i];
				if ((c < '0' || (c < 'A' && c > '9') || (c > 'Z' && c < 'a') || c > 'z') && !HttpEncoder.NotEncoded(c))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return s;
			}
			byte[] bytes = new byte[Enc.GetMaxByteCount(s.Length)];
			int bytes2 = Enc.GetBytes(s, 0, s.Length, bytes, 0);
			return Encoding.ASCII.GetString(UrlEncodeToBytes(bytes, 0, bytes2));
		}

		public static byte[] UrlEncodeToBytes(byte[] bytes, int offset, int count)
		{
			if (bytes != null)
			{
				return HttpEncoder.Current.UrlEncode(bytes, offset, count);
			}
			return null;
		}

		public static string HtmlDecode(string s)
		{
			if (s == null)
			{
				return null;
			}
			using StringWriter stringWriter = new StringWriter();
			HttpEncoder.Current.HtmlDecode(s, stringWriter);
			return stringWriter.ToString();
		}

		public static NameValueCollection ParseQueryString(string query)
		{
			return ParseQueryString(query, Encoding.UTF8);
		}

		public static NameValueCollection ParseQueryString(string query, Encoding encoding)
		{
			if (query == null)
			{
				throw new ArgumentNullException("query");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (query.Length == 0 || (query.Length == 1 && query[0] == '?'))
			{
				return new HttpQSCollection();
			}
			if (query[0] == '?')
			{
				query = query.Substring(1);
			}
			NameValueCollection result = new HttpQSCollection();
			ParseQueryString(query, encoding, result);
			return result;
		}

		internal static void ParseQueryString(string query, Encoding encoding, NameValueCollection result)
		{
			if (query.Length == 0)
			{
				return;
			}
			string text = HtmlDecode(query);
			int length = text.Length;
			int num = 0;
			bool flag = true;
			while (num <= length)
			{
				int num2 = -1;
				int num3 = -1;
				for (int i = num; i < length; i++)
				{
					if (num2 == -1 && text[i] == '=')
					{
						num2 = i + 1;
					}
					else if (text[i] == '&')
					{
						num3 = i;
						break;
					}
				}
				if (flag)
				{
					flag = false;
					if (text[num] == '?')
					{
						num++;
					}
				}
				string name;
				if (num2 == -1)
				{
					name = null;
					num2 = num;
				}
				else
				{
					name = UrlDecode(text.Substring(num, num2 - num - 1), encoding);
				}
				if (num3 < 0)
				{
					num = -1;
					num3 = text.Length;
				}
				else
				{
					num = num3 + 1;
				}
				string value = UrlDecode(text.Substring(num2, num3 - num2), encoding);
				result.Add(name, value);
				if (num == -1)
				{
					break;
				}
			}
		}
	}
}
