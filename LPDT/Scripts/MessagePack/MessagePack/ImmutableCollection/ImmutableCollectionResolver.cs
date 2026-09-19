using MessagePack.Formatters;

namespace MessagePack.ImmutableCollection
{
	public class ImmutableCollectionResolver : IFormatterResolver
	{
		private static class FormatterCache<T>
		{
			internal static readonly IMessagePackFormatter<T>? Formatter;

			static FormatterCache()
			{
				Formatter = (IMessagePackFormatter<T>)ImmutableCollectionGetFormatterHelper.GetFormatter(typeof(T));
			}
		}

		public static readonly ImmutableCollectionResolver Instance = new ImmutableCollectionResolver();

		private ImmutableCollectionResolver()
		{
		}

		public IMessagePackFormatter<T>? GetFormatter<T>()
		{
			return FormatterCache<T>.Formatter;
		}
	}
}
