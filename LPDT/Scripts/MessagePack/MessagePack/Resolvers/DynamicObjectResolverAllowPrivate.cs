using MessagePack.Formatters;
using MessagePack.Internal;

namespace MessagePack.Resolvers
{
	public sealed class DynamicObjectResolverAllowPrivate : IFormatterResolver
	{
		private static class FormatterCache<T>
		{
			internal static readonly IMessagePackFormatter<T>? Formatter = DynamicObjectResolver.BuildFormatterHelper<T>(Instance, DynamicAssemblyFactory, forceStringKey: false, contractless: false, allowPrivate: true);
		}

		private const string ModuleName = "MessagePack.Resolvers.DynamicObjectResolverAllowPrivate";

		public static readonly DynamicObjectResolverAllowPrivate Instance = new DynamicObjectResolverAllowPrivate();

		internal static readonly DynamicAssemblyFactory DynamicAssemblyFactory = new DynamicAssemblyFactory("MessagePack.Resolvers.DynamicObjectResolverAllowPrivate");

		private DynamicObjectResolverAllowPrivate()
		{
		}

		public IMessagePackFormatter<T>? GetFormatter<T>()
		{
			return FormatterCache<T>.Formatter;
		}
	}
}
