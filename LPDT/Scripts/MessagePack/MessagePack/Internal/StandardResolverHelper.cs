using MessagePack.Formatters;
using MessagePack.ImmutableCollection;
using MessagePack.Resolvers;

namespace MessagePack.Internal
{
	internal static class StandardResolverHelper
	{
		public static readonly IFormatterResolver[] DefaultResolvers = ((!DynamicAssembly.AvoidDynamicCode) ? new IFormatterResolver[7]
		{
			BuiltinResolver.Instance,
			AttributeFormatterResolver.Instance,
			SourceGeneratedFormatterResolver.Instance,
			ImmutableCollectionResolver.Instance,
			CompositeResolver.Create(ExpandoObjectFormatter.Instance),
			DynamicGenericResolver.Instance,
			DynamicUnionResolver.Instance
		} : new IFormatterResolver[6]
		{
			BuiltinResolver.Instance,
			AttributeFormatterResolver.Instance,
			SourceGeneratedFormatterResolver.Instance,
			ImmutableCollectionResolver.Instance,
			CompositeResolver.Create(ExpandoObjectFormatter.Instance),
			DynamicGenericResolver.Instance
		});
	}
}
