using MessagePack.Formatters;

namespace MessagePack.Resolvers
{
	public sealed class TypelessObjectResolver : IFormatterResolver
	{
		private static class Cache<T>
		{
			public static readonly IMessagePackFormatter<T?>? Formatter;

			static Cache()
			{
				if (typeof(T).IsAbstract || typeof(T).IsInterface)
				{
					Formatter = new ForceTypelessFormatter<T>();
				}
				if (typeof(T) == typeof(object))
				{
					Formatter = (IMessagePackFormatter<T>)TypelessFormatter.Instance;
					return;
				}
				IFormatterResolver[] resolvers = Resolvers;
				for (int i = 0; i < resolvers.Length; i++)
				{
					IMessagePackFormatter<T> formatter = resolvers[i].GetFormatter<T>();
					if (formatter != null)
					{
						Formatter = formatter;
					}
				}
			}
		}

		public static readonly IFormatterResolver Instance = new TypelessObjectResolver();

		private static readonly IFormatterResolver[] Resolvers = new IFormatterResolver[2]
		{
			ForceSizePrimitiveObjectResolver.Instance,
			ContractlessStandardResolverAllowPrivate.Instance
		};

		private TypelessObjectResolver()
		{
		}

		public IMessagePackFormatter<T>? GetFormatter<T>()
		{
			return Cache<T>.Formatter;
		}
	}
}
