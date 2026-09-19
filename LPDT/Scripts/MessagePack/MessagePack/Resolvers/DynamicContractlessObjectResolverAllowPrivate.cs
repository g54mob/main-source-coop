using MessagePack.Formatters;
using MessagePack.Internal;

namespace MessagePack.Resolvers
{
	public sealed class DynamicContractlessObjectResolverAllowPrivate : IFormatterResolver
	{
		private static class FormatterCache<T>
		{
			public static readonly IMessagePackFormatter<T>? Formatter = ((typeof(T) == typeof(object)) ? null : DynamicObjectResolver.BuildFormatterHelper<T>(Instance, DynamicAssemblyFactory, forceStringKey: true, contractless: true, allowPrivate: true));
		}

		private const string ModuleName = "MessagePack.Resolvers.DynamicContractlessObjectResolverAllowPrivate";

		public static readonly DynamicContractlessObjectResolverAllowPrivate Instance = new DynamicContractlessObjectResolverAllowPrivate();

		private static readonly DynamicAssemblyFactory DynamicAssemblyFactory = new DynamicAssemblyFactory("MessagePack.Resolvers.DynamicContractlessObjectResolverAllowPrivate");

		public IMessagePackFormatter<T>? GetFormatter<T>()
		{
			return FormatterCache<T>.Formatter;
		}
	}
}
