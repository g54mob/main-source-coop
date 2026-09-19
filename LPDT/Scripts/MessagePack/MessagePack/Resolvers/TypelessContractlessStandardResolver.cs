using System;
using System.Collections.Generic;
using MessagePack.Formatters;
using MessagePack.Internal;

namespace MessagePack.Resolvers
{
	public sealed class TypelessContractlessStandardResolver : IFormatterResolver
	{
		private class ResolverCache : CachingFormatterResolver
		{
			private readonly IReadOnlyList<IFormatterResolver> resolvers;

			internal ResolverCache(IReadOnlyList<IFormatterResolver> resolvers)
			{
				this.resolvers = resolvers ?? throw new ArgumentNullException("resolvers");
			}

			protected override IMessagePackFormatter<T>? GetFormatterCore<T>()
			{
				foreach (IFormatterResolver resolver in resolvers)
				{
					IMessagePackFormatter<T> formatter = resolver.GetFormatter<T>();
					if (formatter != null)
					{
						return formatter;
					}
				}
				return null;
			}
		}

		public static readonly TypelessContractlessStandardResolver Instance;

		public static readonly MessagePackSerializerOptions Options;

		private static readonly IReadOnlyList<IFormatterResolver> Resolvers;

		private readonly ResolverCache resolverCache = new ResolverCache(Resolvers);

		static TypelessContractlessStandardResolver()
		{
			IReadOnlyList<IFormatterResolver> resolvers;
			if (!DynamicAssembly.AvoidDynamicCode)
			{
				IReadOnlyList<IFormatterResolver> readOnlyList = new _003C_003Ez__ReadOnlyArray<IFormatterResolver>(new IFormatterResolver[10]
				{
					NativeDateTimeResolver.Instance,
					ForceSizePrimitiveObjectResolver.Instance,
					BuiltinResolver.Instance,
					AttributeFormatterResolver.Instance,
					DynamicEnumResolver.Instance,
					DynamicGenericResolver.Instance,
					DynamicUnionResolver.Instance,
					DynamicObjectResolver.Instance,
					DynamicContractlessObjectResolverAllowPrivate.Instance,
					TypelessObjectResolver.Instance
				});
				resolvers = readOnlyList;
			}
			else
			{
				IReadOnlyList<IFormatterResolver> readOnlyList = new _003C_003Ez__ReadOnlyArray<IFormatterResolver>(new IFormatterResolver[6]
				{
					NativeDateTimeResolver.Instance,
					ForceSizePrimitiveObjectResolver.Instance,
					BuiltinResolver.Instance,
					AttributeFormatterResolver.Instance,
					DynamicContractlessObjectResolverAllowPrivate.Instance,
					TypelessObjectResolver.Instance
				});
				resolvers = readOnlyList;
			}
			Resolvers = resolvers;
			Instance = new TypelessContractlessStandardResolver();
			Options = new MessagePackSerializerOptions(Instance);
		}

		public IMessagePackFormatter<T>? GetFormatter<T>()
		{
			return resolverCache.GetFormatter<T>();
		}
	}
}
