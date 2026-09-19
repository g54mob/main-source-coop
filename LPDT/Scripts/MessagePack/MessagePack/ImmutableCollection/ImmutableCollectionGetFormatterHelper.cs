using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using MessagePack.Formatters;

namespace MessagePack.ImmutableCollection
{
	internal static class ImmutableCollectionGetFormatterHelper
	{
		private static readonly Dictionary<Type, Type> FormatterMap = new Dictionary<Type, Type>
		{
			{
				typeof(ImmutableArray<>),
				typeof(ImmutableArrayFormatter<>)
			},
			{
				typeof(ImmutableList<>),
				typeof(ImmutableListFormatter<>)
			},
			{
				typeof(ImmutableDictionary<, >),
				typeof(ImmutableDictionaryFormatter<, >)
			},
			{
				typeof(ImmutableHashSet<>),
				typeof(ImmutableHashSetFormatter<>)
			},
			{
				typeof(ImmutableSortedDictionary<, >),
				typeof(ImmutableSortedDictionaryFormatter<, >)
			},
			{
				typeof(ImmutableSortedSet<>),
				typeof(ImmutableSortedSetFormatter<>)
			},
			{
				typeof(ImmutableQueue<>),
				typeof(ImmutableQueueFormatter<>)
			},
			{
				typeof(ImmutableStack<>),
				typeof(ImmutableStackFormatter<>)
			},
			{
				typeof(IImmutableList<>),
				typeof(InterfaceImmutableListFormatter<>)
			},
			{
				typeof(IImmutableDictionary<, >),
				typeof(InterfaceImmutableDictionaryFormatter<, >)
			},
			{
				typeof(IImmutableQueue<>),
				typeof(InterfaceImmutableQueueFormatter<>)
			},
			{
				typeof(IImmutableSet<>),
				typeof(InterfaceImmutableSetFormatter<>)
			},
			{
				typeof(IImmutableStack<>),
				typeof(InterfaceImmutableStackFormatter<>)
			}
		};

		internal static object? GetFormatter(Type t)
		{
			TypeInfo typeInfo = t.GetTypeInfo();
			if (typeInfo.IsGenericType)
			{
				Type genericTypeDefinition = typeInfo.GetGenericTypeDefinition();
				bool flag = genericTypeDefinition.GetTypeInfo().IsNullable();
				Type type = (flag ? typeInfo.GenericTypeArguments[0] : null);
				if (FormatterMap.TryGetValue(genericTypeDefinition, out Type value))
				{
					return CreateInstance(value, typeInfo.GenericTypeArguments);
				}
				if (flag)
				{
					bool? flag2 = type?.IsConstructedGenericType;
					if (flag2.HasValue && flag2 == true && type.GetGenericTypeDefinition() == typeof(ImmutableArray<>))
					{
						return CreateInstance(typeof(NullableFormatter<>), new Type[1] { type });
					}
				}
			}
			return null;
		}

		private static object? CreateInstance(Type genericType, Type[] genericTypeArguments, params object[] arguments)
		{
			return Activator.CreateInstance(genericType.MakeGenericType(genericTypeArguments), arguments);
		}
	}
}
