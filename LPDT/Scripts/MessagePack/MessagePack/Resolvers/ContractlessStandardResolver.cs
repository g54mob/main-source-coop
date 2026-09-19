using System;
using MessagePack.Formatters;
using MessagePack.Internal;

namespace MessagePack.Resolvers
{
	public sealed class ContractlessStandardResolver : IFormatterResolver
	{
		private static class FormatterCache<T>
		{
			public static readonly IMessagePackFormatter<T>? Formatter;

			static FormatterCache()
			{
				if (typeof(T) == typeof(object))
				{
					Formatter = (DynamicAssembly.AvoidDynamicCode ? PrimitiveObjectResolver.Instance.GetFormatter<T>() : ((IMessagePackFormatter<T>)DynamicObjectTypeFallbackFormatter.Instance));
					return;
				}
				IFormatterResolver[] resolvers = Resolvers;
				for (int i = 0; i < resolvers.Length; i++)
				{
					IMessagePackFormatter<T> formatter = resolvers[i].GetFormatter<T>();
					if (formatter != null)
					{
						Formatter = formatter;
						break;
					}
				}
			}
		}

		public static readonly ContractlessStandardResolver Instance;

		public static readonly MessagePackSerializerOptions Options;

		private static readonly IFormatterResolver[] Resolvers;

		static ContractlessStandardResolver()
		{
			IFormatterResolver[] resolvers;
			if (!DynamicAssembly.AvoidDynamicCode)
			{
				IFormatterResolver[] defaultResolvers = StandardResolverHelper.DefaultResolvers;
				int num = 0;
				IFormatterResolver[] array = new IFormatterResolver[2 + defaultResolvers.Length];
				ReadOnlySpan<IFormatterResolver> readOnlySpan = new ReadOnlySpan<IFormatterResolver>(defaultResolvers);
				readOnlySpan.CopyTo(new Span<IFormatterResolver>(array).Slice(num, readOnlySpan.Length));
				num += readOnlySpan.Length;
				array[num] = DynamicObjectResolver.Instance;
				num++;
				array[num] = DynamicContractlessObjectResolver.Instance;
				num++;
				resolvers = array;
			}
			else
			{
				resolvers = StandardResolverHelper.DefaultResolvers;
			}
			Resolvers = resolvers;
			Instance = new ContractlessStandardResolver();
			Options = new MessagePackSerializerOptions(Instance);
		}

		private ContractlessStandardResolver()
		{
		}

		public IMessagePackFormatter<T>? GetFormatter<T>()
		{
			return FormatterCache<T>.Formatter;
		}
	}
}
