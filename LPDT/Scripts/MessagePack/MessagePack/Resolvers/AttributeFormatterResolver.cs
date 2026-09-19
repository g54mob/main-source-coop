using System;
using System.Reflection;
using MessagePack.Formatters;
using MessagePack.Internal;

namespace MessagePack.Resolvers
{
	public sealed class AttributeFormatterResolver : IFormatterResolver
	{
		private static class FormatterCache<T>
		{
			internal static readonly IMessagePackFormatter<T>? Formatter;

			static FormatterCache()
			{
				MessagePackFormatterAttribute customAttribute = typeof(T).GetTypeInfo().GetCustomAttribute<MessagePackFormatterAttribute>();
				if (customAttribute != null)
				{
					Type type = customAttribute.FormatterType;
					if (type.IsGenericType && !type.IsConstructedGenericType)
					{
						type = type.MakeGenericType(typeof(T).GetGenericArguments());
					}
					Formatter = (IMessagePackFormatter<T>)ResolverUtilities.ActivateFormatter(type, customAttribute.Arguments);
				}
			}
		}

		public static readonly AttributeFormatterResolver Instance = new AttributeFormatterResolver();

		private AttributeFormatterResolver()
		{
		}

		public IMessagePackFormatter<T>? GetFormatter<T>()
		{
			return FormatterCache<T>.Formatter;
		}
	}
}
