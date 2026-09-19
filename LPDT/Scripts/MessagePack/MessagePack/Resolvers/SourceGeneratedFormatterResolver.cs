using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using MessagePack.Formatters;
using MessagePack.Internal;

namespace MessagePack.Resolvers
{
	public sealed class SourceGeneratedFormatterResolver : IFormatterResolver
	{
		private static class FormatterCache<T>
		{
			internal static readonly IMessagePackFormatter<T>? Formatter = FindPrecompiledFormatter();

			private static IMessagePackFormatter<T>? FindPrecompiledFormatter()
			{
				return AssemblyResolverCache.GetOrAdd(typeof(T).Assembly, delegate
				{
					GeneratedAssemblyMessagePackResolverAttribute generatedAssemblyMessagePackResolverAttribute = typeof(T).Assembly.GetCustomAttributes<GeneratedAssemblyMessagePackResolverAttribute>().FirstOrDefault();
					return (generatedAssemblyMessagePackResolverAttribute != null) ? ((IFormatterResolver)(generatedAssemblyMessagePackResolverAttribute.ResolverType.GetField("Instance", BindingFlags.Static | BindingFlags.Public)?.GetValue(null))) : null;
				})?.GetFormatter<T>();
			}
		}

		public static readonly SourceGeneratedFormatterResolver Instance = new SourceGeneratedFormatterResolver();

		private static readonly ConcurrentDictionary<Assembly, IFormatterResolver?> AssemblyResolverCache = new ConcurrentDictionary<Assembly, IFormatterResolver>();

		private SourceGeneratedFormatterResolver()
		{
		}

		public IMessagePackFormatter<T>? GetFormatter<T>()
		{
			return FormatterCache<T>.Formatter;
		}
	}
}
