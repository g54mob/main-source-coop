using System;
using Microsoft.NET.StringTools;

namespace MessagePack.Formatters
{
	public sealed class StringInterningFormatter : IMessagePackFormatter<string?>, IMessagePackFormatter
	{
		public string? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
		{
			if (reader.TryReadNil())
			{
				return null;
			}
			MessagePackReader messagePackReader = reader;
			if (reader.TryReadStringSpan(out var span))
			{
				if (span.Length < 4096)
				{
					if (span.Length == 0)
					{
						return string.Empty;
					}
					Span<char> chars = stackalloc char[span.Length];
					return Strings.WeakIntern(chars[..StringEncoding.UTF8.GetChars(span, chars)]);
				}
				reader = messagePackReader;
			}
			string text = reader.ReadString();
			if (text == null)
			{
				return null;
			}
			return Strings.WeakIntern(text);
		}

		public void Serialize(ref MessagePackWriter writer, string? value, MessagePackSerializerOptions options)
		{
			writer.Write(value);
		}
	}
}
