using System;
using System.Buffers;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using MessagePack.Formatters;

namespace MessagePack.Internal
{
	internal static class DynamicGenericResolverGetFormatterHelper
	{
		private static readonly Dictionary<Type, Type> FormatterMap = new Dictionary<Type, Type>
		{
			{
				typeof(List<>),
				typeof(ListFormatter<>)
			},
			{
				typeof(LinkedList<>),
				typeof(LinkedListFormatter<>)
			},
			{
				typeof(Queue<>),
				typeof(QueueFormatter<>)
			},
			{
				typeof(Stack<>),
				typeof(StackFormatter<>)
			},
			{
				typeof(HashSet<>),
				typeof(HashSetFormatter<>)
			},
			{
				typeof(ReadOnlyCollection<>),
				typeof(ReadOnlyCollectionFormatter<>)
			},
			{
				typeof(IList<>),
				typeof(InterfaceListFormatter2<>)
			},
			{
				typeof(ICollection<>),
				typeof(InterfaceCollectionFormatter2<>)
			},
			{
				typeof(IEnumerable<>),
				typeof(InterfaceEnumerableFormatter<>)
			},
			{
				typeof(Dictionary<, >),
				typeof(DictionaryFormatter<, >)
			},
			{
				typeof(IDictionary<, >),
				typeof(InterfaceDictionaryFormatter<, >)
			},
			{
				typeof(SortedDictionary<, >),
				typeof(SortedDictionaryFormatter<, >)
			},
			{
				typeof(SortedList<, >),
				typeof(SortedListFormatter<, >)
			},
			{
				typeof(ILookup<, >),
				typeof(InterfaceLookupFormatter<, >)
			},
			{
				typeof(IGrouping<, >),
				typeof(InterfaceGroupingFormatter<, >)
			},
			{
				typeof(ObservableCollection<>),
				typeof(ObservableCollectionFormatter<>)
			},
			{
				typeof(ReadOnlyObservableCollection<>),
				typeof(ReadOnlyObservableCollectionFormatter<>)
			},
			{
				typeof(IReadOnlyList<>),
				typeof(InterfaceReadOnlyListFormatter<>)
			},
			{
				typeof(IReadOnlyCollection<>),
				typeof(InterfaceReadOnlyCollectionFormatter<>)
			},
			{
				typeof(ISet<>),
				typeof(InterfaceSetFormatter<>)
			},
			{
				typeof(ConcurrentBag<>),
				typeof(ConcurrentBagFormatter<>)
			},
			{
				typeof(ConcurrentQueue<>),
				typeof(ConcurrentQueueFormatter<>)
			},
			{
				typeof(ConcurrentStack<>),
				typeof(ConcurrentStackFormatter<>)
			},
			{
				typeof(ReadOnlyDictionary<, >),
				typeof(ReadOnlyDictionaryFormatter<, >)
			},
			{
				typeof(IReadOnlyDictionary<, >),
				typeof(InterfaceReadOnlyDictionaryFormatter<, >)
			},
			{
				typeof(ConcurrentDictionary<, >),
				typeof(ConcurrentDictionaryFormatter<, >)
			},
			{
				typeof(Lazy<>),
				typeof(LazyFormatter<>)
			}
		};

		internal static object? GetFormatter(Type t)
		{
			TypeInfo typeInfo = t.GetTypeInfo();
			if (t.IsArray)
			{
				switch (t.GetArrayRank())
				{
				case 1:
					if (t.GetElementType() == typeof(byte))
					{
						return ByteArrayFormatter.Instance;
					}
					return Activator.CreateInstance(typeof(ArrayFormatter<>).MakeGenericType(t.GetElementType()));
				case 2:
					return Activator.CreateInstance(typeof(TwoDimensionalArrayFormatter<>).MakeGenericType(t.GetElementType()));
				case 3:
					return Activator.CreateInstance(typeof(ThreeDimensionalArrayFormatter<>).MakeGenericType(t.GetElementType()));
				case 4:
					return Activator.CreateInstance(typeof(FourDimensionalArrayFormatter<>).MakeGenericType(t.GetElementType()));
				default:
					return null;
				}
			}
			if (typeInfo.IsGenericType)
			{
				Type genericTypeDefinition = typeInfo.GetGenericTypeDefinition();
				bool flag = genericTypeDefinition.GetTypeInfo().IsNullable();
				Type type = (flag ? typeInfo.GenericTypeArguments[0] : null);
				if (genericTypeDefinition == typeof(KeyValuePair<, >))
				{
					return CreateInstance(typeof(KeyValuePairFormatter<, >), typeInfo.GenericTypeArguments);
				}
				if (typeInfo.FullName?.StartsWith("System.Tuple") ?? false)
				{
					Type type2 = null;
					return CreateInstance(typeInfo.GenericTypeArguments.Length switch
					{
						1 => typeof(TupleFormatter<>), 
						2 => typeof(TupleFormatter<, >), 
						3 => typeof(TupleFormatter<, , >), 
						4 => typeof(TupleFormatter<, , , >), 
						5 => typeof(TupleFormatter<, , , , >), 
						6 => typeof(TupleFormatter<, , , , , >), 
						7 => typeof(TupleFormatter<, , , , , , >), 
						8 => typeof(TupleFormatter<, , , , , , , >), 
						_ => throw new MessagePackSerializationException("Unsupported arity for Tuple generic type: " + typeInfo.Name), 
					}, typeInfo.GenericTypeArguments);
				}
				if (typeInfo.FullName?.StartsWith("System.ValueTuple") ?? false)
				{
					Type type3 = null;
					return CreateInstance(typeInfo.GenericTypeArguments.Length switch
					{
						1 => typeof(ValueTupleFormatter<>), 
						2 => typeof(ValueTupleFormatter<, >), 
						3 => typeof(ValueTupleFormatter<, , >), 
						4 => typeof(ValueTupleFormatter<, , , >), 
						5 => typeof(ValueTupleFormatter<, , , , >), 
						6 => typeof(ValueTupleFormatter<, , , , , >), 
						7 => typeof(ValueTupleFormatter<, , , , , , >), 
						8 => typeof(ValueTupleFormatter<, , , , , , , >), 
						_ => throw new MessagePackSerializationException("Unsupported arity for ValueTuple generic type: " + typeInfo.Name), 
					}, typeInfo.GenericTypeArguments);
				}
				if (genericTypeDefinition == typeof(ArraySegment<>))
				{
					if (typeInfo.GenericTypeArguments[0] == typeof(byte))
					{
						return ByteArraySegmentFormatter.Instance;
					}
					return CreateInstance(typeof(ArraySegmentFormatter<>), typeInfo.GenericTypeArguments);
				}
				if (genericTypeDefinition == typeof(Memory<>))
				{
					if (typeInfo.GenericTypeArguments[0] == typeof(byte))
					{
						return ByteMemoryFormatter.Instance;
					}
					return CreateInstance(typeof(MemoryFormatter<>), typeInfo.GenericTypeArguments);
				}
				if (genericTypeDefinition == typeof(ReadOnlyMemory<>))
				{
					if (typeInfo.GenericTypeArguments[0] == typeof(byte))
					{
						return ByteReadOnlyMemoryFormatter.Instance;
					}
					return CreateInstance(typeof(ReadOnlyMemoryFormatter<>), typeInfo.GenericTypeArguments);
				}
				if (genericTypeDefinition == typeof(ReadOnlySequence<>))
				{
					if (typeInfo.GenericTypeArguments[0] == typeof(byte))
					{
						return ByteReadOnlySequenceFormatter.Instance;
					}
					return CreateInstance(typeof(ReadOnlySequenceFormatter<>), typeInfo.GenericTypeArguments);
				}
				if (flag)
				{
					return CreateInstance(typeof(NullableFormatter<>), new Type[1] { type });
				}
				if (FormatterMap.TryGetValue(genericTypeDefinition, out Type value))
				{
					return CreateInstance(value, typeInfo.GenericTypeArguments);
				}
			}
			else
			{
				if (typeInfo.IsEnum)
				{
					return CreateInstance(typeof(GenericEnumFormatter<>), new Type[1] { t });
				}
				if (t == typeof(IEnumerable))
				{
					return NonGenericInterfaceEnumerableFormatter.Instance;
				}
				if (t == typeof(ICollection))
				{
					return NonGenericInterfaceCollectionFormatter.Instance;
				}
				if (t == typeof(IList))
				{
					return NonGenericInterfaceListFormatter.Instance;
				}
				if (t == typeof(IDictionary))
				{
					return NonGenericInterfaceDictionaryFormatter.Instance;
				}
				if (typeof(IList).GetTypeInfo().IsAssignableFrom(typeInfo) && typeInfo.DeclaredConstructors.Any((ConstructorInfo x) => x.GetParameters().Length == 0))
				{
					return Activator.CreateInstance(typeof(NonGenericListFormatter<>).MakeGenericType(t));
				}
				if (typeof(IDictionary).GetTypeInfo().IsAssignableFrom(typeInfo) && typeInfo.DeclaredConstructors.Any((ConstructorInfo x) => x.GetParameters().Length == 0))
				{
					return Activator.CreateInstance(typeof(NonGenericDictionaryFormatter<>).MakeGenericType(t));
				}
			}
			Type type4 = typeInfo.ImplementedInterfaces.FirstOrDefault((Type x) => x.GetTypeInfo().IsConstructedGenericType() && x.GetGenericTypeDefinition() == typeof(IDictionary<, >));
			if (type4 != null && typeInfo.DeclaredConstructors.Any((ConstructorInfo x) => !x.IsStatic && x.GetParameters().Length == 0))
			{
				Type type5 = type4.GenericTypeArguments[0];
				Type type6 = type4.GenericTypeArguments[1];
				return CreateInstance(typeof(GenericDictionaryFormatter<, , >), new Type[3] { type5, type6, t });
			}
			Type type7 = typeInfo.ImplementedInterfaces.FirstOrDefault((Type x) => x.GetTypeInfo().IsConstructedGenericType() && x.GetGenericTypeDefinition() == typeof(IReadOnlyDictionary<, >));
			if (type7 != null)
			{
				Type type8 = type7.GenericTypeArguments[0];
				Type type9 = type7.GenericTypeArguments[1];
				Type[] source = new Type[3]
				{
					typeof(IDictionary<, >).MakeGenericType(type8, type9),
					typeof(IReadOnlyDictionary<, >).MakeGenericType(type8, type9),
					typeof(IEnumerable<>).MakeGenericType(typeof(KeyValuePair<, >).MakeGenericType(type8, type9))
				};
				foreach (ConstructorInfo declaredConstructor in typeInfo.DeclaredConstructors)
				{
					ParameterInfo[] parameters = declaredConstructor.GetParameters();
					if (parameters.Length == 1 && source.Any((Type allowedType) => parameters[0].ParameterType.IsAssignableFrom(allowedType)))
					{
						return CreateInstance(typeof(GenericReadOnlyDictionaryFormatter<, , >), new Type[3] { type8, type9, t });
					}
				}
			}
			Type type10 = typeInfo.ImplementedInterfaces.FirstOrDefault((Type x) => x.GetTypeInfo().IsConstructedGenericType() && x.GetGenericTypeDefinition() == typeof(ICollection<>));
			if (type10 != null && typeInfo.DeclaredConstructors.Any((ConstructorInfo x) => !x.IsStatic && x.GetParameters().Length == 0))
			{
				Type type11 = type10.GenericTypeArguments[0];
				return CreateInstance(typeof(GenericCollectionFormatter<, >), new Type[2] { type11, t });
			}
			foreach (Type item in typeInfo.ImplementedInterfaces.Where((Type x) => x.GetTypeInfo().IsConstructedGenericType() && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
			{
				Type type12 = item.GenericTypeArguments[0];
				Type c = typeof(IEnumerable<>).MakeGenericType(type12);
				foreach (ConstructorInfo declaredConstructor2 in typeInfo.DeclaredConstructors)
				{
					ParameterInfo[] parameters2 = declaredConstructor2.GetParameters();
					if (parameters2.Length == 1 && parameters2[0].ParameterType.IsAssignableFrom(c))
					{
						return CreateInstance(typeof(GenericEnumerableFormatter<, >), new Type[2] { type12, t });
					}
				}
			}
			return null;
		}

		private static object? CreateInstance(Type genericType, Type[] genericTypeArguments, params object?[] arguments)
		{
			return Activator.CreateInstance(genericType.MakeGenericType(genericTypeArguments), arguments);
		}
	}
}
