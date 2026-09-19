using MessagePack.Formatters;
using MessagePack.Internal;

namespace MessagePack.Resolvers
{
	public sealed class DynamicContractlessObjectResolver : IFormatterResolver
	{
		private static class FormatterCache<T>
		{
			public static readonly IMessagePackFormatter<T>? Formatter = ((typeof(T) == typeof(object)) ? null : DynamicObjectResolver.BuildFormatterHelper<T>(Instance, DynamicAssemblyFactory, forceStringKey: true, contractless: true, allowPrivate: false));
		}

		private const string ModuleName = "MessagePack.Resolvers.DynamicContractlessObjectResolver";

		public static readonly DynamicContractlessObjectResolver Instance;

		private static readonly DynamicAssemblyFactory DynamicAssemblyFactory;

		private DynamicContractlessObjectResolver()
		{
		}

		static DynamicContractlessObjectResolver()
		{
			Instance = new DynamicContractlessObjectResolver();
			DynamicAssemblyFactory = new DynamicAssemblyFactory("MessagePack.Resolvers.DynamicContractlessObjectResolver");
			DynamicAssemblyFactory = new DynamicAssemblyFactory("MessagePack.Resolvers.DynamicContractlessObjectResolver");
		}

		public IMessagePackFormatter<T>? GetFormatter<T>()
		{
			return FormatterCache<T>.Formatter;
		}
	}
}
