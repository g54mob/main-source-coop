using System;
using System.Reflection;
using MessagePack.Formatters;
using MessagePack.Internal;

namespace MessagePack.Resolvers
{
	public sealed class DynamicEnumAsStringIgnoreCaseResolver : IFormatterResolver
	{
		private static class FormatterCache<T>
		{
			private static readonly object?[] FormatterCtorArgs;

			public static readonly IMessagePackFormatter<T>? Formatter;

			static FormatterCache()
			{
				FormatterCtorArgs = new object[1] { true };
				TypeInfo typeInfo = typeof(T).GetTypeInfo();
				if (typeInfo.IsNullable())
				{
					typeInfo = typeInfo.GenericTypeArguments[0].GetTypeInfo();
					if (typeInfo.IsEnum)
					{
						object formatterDynamic = Instance.GetFormatterDynamic(typeInfo.AsType());
						if (formatterDynamic != null)
						{
							Formatter = (IMessagePackFormatter<T>)Activator.CreateInstance(typeof(StaticNullableFormatter<>).MakeGenericType(typeInfo.AsType()), formatterDynamic);
						}
					}
				}
				else if (typeInfo.IsEnum)
				{
					Formatter = (IMessagePackFormatter<T>)Activator.CreateInstance(typeof(EnumAsStringFormatter<>).MakeGenericType(typeof(T)), FormatterCtorArgs);
				}
			}
		}

		public static readonly DynamicEnumAsStringIgnoreCaseResolver Instance = new DynamicEnumAsStringIgnoreCaseResolver();

		private DynamicEnumAsStringIgnoreCaseResolver()
		{
		}

		public IMessagePackFormatter<T>? GetFormatter<T>()
		{
			return FormatterCache<T>.Formatter;
		}
	}
}
