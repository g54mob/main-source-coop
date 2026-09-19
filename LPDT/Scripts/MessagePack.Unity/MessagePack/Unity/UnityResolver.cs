using MessagePack.Formatters;
using MessagePack.Resolvers;

namespace MessagePack.Unity
{
	public class UnityResolver : IFormatterResolver
	{
		private static class FormatterCache<T>
		{
			public static readonly IMessagePackFormatter<T>? Formatter;

			static FormatterCache()
			{
				Formatter = (IMessagePackFormatter<T>)UnityResolveryResolverGetFormatterHelper.GetFormatter(typeof(T));
			}
		}

		private class WithStandardResolver : IFormatterResolver
		{
			private static class FormatterCache<T>
			{
				public static readonly IMessagePackFormatter<T>? Formatter;

				static FormatterCache()
				{
					IMessagePackFormatter<T> formatter = UnityResolver.Instance.GetFormatter<T>();
					if (formatter == null)
					{
						formatter = StandardResolver.Instance.GetFormatter<T>();
					}
					Formatter = formatter;
				}
			}

			public static readonly WithStandardResolver Instance = new WithStandardResolver();

			public IMessagePackFormatter<T>? GetFormatter<T>()
			{
				return FormatterCache<T>.Formatter;
			}
		}

		public static readonly IFormatterResolver Instance = new UnityResolver();

		public static readonly IFormatterResolver InstanceWithStandardResolver = new WithStandardResolver();

		private UnityResolver()
		{
		}

		public IMessagePackFormatter<T>? GetFormatter<T>()
		{
			return FormatterCache<T>.Formatter;
		}
	}
}
