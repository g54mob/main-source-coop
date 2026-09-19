using System;
using MessagePack.Formatters;

namespace MessagePack
{
	internal class GeneratedMessagePackResolver : IFormatterResolver
	{
		private static class FormatterCache<T>
		{
			internal static readonly IMessagePackFormatter<T> Formatter;

			static FormatterCache()
			{
				object formatter = GeneratedMessagePackResolverGetFormatterHelper.GetFormatter(typeof(T));
				if (formatter != null)
				{
					Formatter = (IMessagePackFormatter<T>)formatter;
				}
			}
		}

		private static class GeneratedMessagePackResolverGetFormatterHelper
		{
			internal static object GetFormatter(Type t)
			{
				return null;
			}
		}

		public static readonly IFormatterResolver Instance = new GeneratedMessagePackResolver();

		private GeneratedMessagePackResolver()
		{
		}

		public IMessagePackFormatter<T> GetFormatter<T>()
		{
			return FormatterCache<T>.Formatter;
		}
	}
}
