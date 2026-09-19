using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using MessagePack.Internal;
using Nerdbank.Streams;

namespace MessagePack.Formatters
{
	public sealed class TypelessFormatter : IMessagePackFormatter<object?>, IMessagePackFormatter
	{
		private delegate void SerializeMethod(object dynamicContractlessFormatter, ref MessagePackWriter writer, object value, MessagePackSerializerOptions options);

		private delegate object DeserializeMethod(object dynamicContractlessFormatter, ref MessagePackReader reader, MessagePackSerializerOptions options);

		public static readonly IMessagePackFormatter<object?> Instance;

		private static readonly ThreadsafeTypeKeyHashTable<SerializeMethod> Serializers;

		private static readonly ThreadsafeTypeKeyHashTable<DeserializeMethod> Deserializers;

		private static readonly ThreadsafeTypeKeyHashTable<byte[]?> FullTypeNameCache;

		private static readonly ThreadsafeTypeKeyHashTable<byte[]?> ShortenedTypeNameCache;

		private static readonly AsymmetricKeyHashTable<byte[], ArraySegment<byte>, Type> TypeCache;

		private static readonly HashSet<Type> UseBuiltinTypes;

		private static readonly bool IsMscorlib;

		static TypelessFormatter()
		{
			Instance = new TypelessFormatter();
			Serializers = new ThreadsafeTypeKeyHashTable<SerializeMethod>();
			Deserializers = new ThreadsafeTypeKeyHashTable<DeserializeMethod>();
			FullTypeNameCache = new ThreadsafeTypeKeyHashTable<byte[]>();
			ShortenedTypeNameCache = new ThreadsafeTypeKeyHashTable<byte[]>();
			TypeCache = new AsymmetricKeyHashTable<byte[], ArraySegment<byte>, Type>(new StringArraySegmentByteAscymmetricEqualityComparer());
			UseBuiltinTypes = new HashSet<Type>
			{
				typeof(bool),
				typeof(sbyte),
				typeof(byte),
				typeof(short),
				typeof(ushort),
				typeof(int),
				typeof(uint),
				typeof(long),
				typeof(ulong),
				typeof(float),
				typeof(double),
				typeof(string),
				typeof(byte[]),
				typeof(bool?),
				typeof(sbyte?),
				typeof(byte?),
				typeof(short?),
				typeof(ushort?),
				typeof(int?),
				typeof(uint?),
				typeof(long?),
				typeof(ulong?),
				typeof(float?),
				typeof(double?)
			};
			IsMscorlib = typeof(int).AssemblyQualifiedName.Contains("mscorlib");
			Serializers.TryAdd(typeof(object), (Type _) => delegate
			{
			});
			Deserializers.TryAdd(typeof(object), (Type _) => delegate
			{
				return new object();
			});
		}

		private string BuildTypeName(Type type, MessagePackSerializerOptions options)
		{
			if (options.OmitAssemblyVersion)
			{
				string assemblyQualifiedName = type.AssemblyQualifiedName;
				string text = MessagePackSerializerOptions.AssemblyNameVersionSelectorRegex.Replace(assemblyQualifiedName, string.Empty);
				if (Type.GetType(text, throwOnError: false) == null)
				{
					text = assemblyQualifiedName;
				}
				return text;
			}
			return type.AssemblyQualifiedName;
		}

		public void Serialize(ref MessagePackWriter writer, object? value, MessagePackSerializerOptions options)
		{
			if (value == null)
			{
				writer.WriteNil();
				return;
			}
			Type type = value.GetType();
			ThreadsafeTypeKeyHashTable<byte[]> threadsafeTypeKeyHashTable = (options.OmitAssemblyVersion ? ShortenedTypeNameCache : FullTypeNameCache);
			if (!threadsafeTypeKeyHashTable.TryGetValue(type, out var value2))
			{
				value2 = ((!type.GetTypeInfo().IsAnonymous() && !UseBuiltinTypes.Contains(type)) ? StringEncoding.UTF8.GetBytes(BuildTypeName(type, options)) : null);
				threadsafeTypeKeyHashTable.TryAdd(type, value2);
			}
			if (value2 == null)
			{
				DynamicObjectTypeFallbackFormatter.Instance.Serialize(ref writer, value, options);
				return;
			}
			object formatterDynamicWithVerify = options.Resolver.GetFormatterDynamicWithVerify(type);
			if (!Serializers.TryGetValue(type, out SerializeMethod value3))
			{
				lock (Serializers)
				{
					if (!Serializers.TryGetValue(type, out value3))
					{
						TypeInfo typeInfo = type.GetTypeInfo();
						Type type2 = typeof(IMessagePackFormatter<>).MakeGenericType(type);
						ParameterExpression parameterExpression = Expression.Parameter(typeof(object), "formatter");
						ParameterExpression parameterExpression2 = Expression.Parameter(typeof(MessagePackWriter).MakeByRefType(), "writer");
						ParameterExpression parameterExpression3 = Expression.Parameter(typeof(object), "value");
						ParameterExpression parameterExpression4 = Expression.Parameter(typeof(MessagePackSerializerOptions), "options");
						MethodInfo runtimeMethod = type2.GetRuntimeMethod("Serialize", new Type[3]
						{
							typeof(MessagePackWriter).MakeByRefType(),
							type,
							typeof(MessagePackSerializerOptions)
						});
						value3 = Expression.Lambda<SerializeMethod>(Expression.Call(Expression.Convert(parameterExpression, type2), runtimeMethod, parameterExpression2, typeInfo.IsValueType ? Expression.Unbox(parameterExpression3, type) : Expression.Convert(parameterExpression3, type), parameterExpression4), new ParameterExpression[4] { parameterExpression, parameterExpression2, parameterExpression3, parameterExpression4 }).Compile();
						Serializers.TryAdd(type, value3);
					}
				}
			}
			using SequencePool.Rental rental = options.SequencePool.Rent();
			MessagePackWriter writer2 = writer.Clone(rental.Value);
			writer2.WriteString(value2);
			value3(formatterDynamicWithVerify, ref writer2, value, options);
			writer2.Flush();
			writer.WriteExtensionFormat(new ExtensionResult(100, rental.Value));
		}

		public object? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
		{
			if (reader.TryReadNil())
			{
				return null;
			}
			checked
			{
				if (reader.NextMessagePackType == MessagePackType.Extension)
				{
					MessagePackReader messagePackReader = reader.CreatePeekReader();
					if (messagePackReader.ReadExtensionFormatHeader().TypeCode == 100)
					{
						reader = messagePackReader;
						ReadOnlySequence<byte> source = reader.ReadStringSequence() ?? throw MessagePackSerializationException.ThrowUnexpectedNilWhileDeserializing<object>();
						byte[] array = null;
						if (!source.IsSingleSegment || !MemoryMarshal.TryGetArray(source.First, out var segment))
						{
							array = ArrayPool<byte>.Shared.Rent((int)source.Length);
							BuffersExtensions.CopyTo(in source, array);
							segment = new ArraySegment<byte>(array, 0, (int)source.Length);
						}
						object result = DeserializeByTypeName(segment, ref reader, options);
						if (array != null)
						{
							ArrayPool<byte>.Shared.Return(array);
						}
						return result;
					}
				}
				return DynamicObjectTypeFallbackFormatter.Instance.Deserialize(ref reader, options);
			}
		}

		private object DeserializeByTypeName(ArraySegment<byte> typeName, ref MessagePackReader byteSequence, MessagePackSerializerOptions options)
		{
			Requires.Argument(typeName.Array != null, "typeName", "Array cannot be null.");
			if (!TypeCache.TryGetValue(typeName, out Type value))
			{
				byte[] array = new byte[typeName.Count];
				Buffer.BlockCopy(typeName.Array, typeName.Offset, array, 0, array.Length);
				string text = StringEncoding.UTF8.GetString(array);
				value = options.LoadType(text);
				if (value == null)
				{
					if (IsMscorlib && text.Contains("System.Private.CoreLib"))
					{
						text = text.Replace("System.Private.CoreLib", "mscorlib");
						value = Type.GetType(text, throwOnError: true);
					}
					else if (!IsMscorlib && text.Contains("mscorlib"))
					{
						text = text.Replace("mscorlib", "System.Private.CoreLib");
						value = Type.GetType(text, throwOnError: true);
					}
					else
					{
						value = Type.GetType(text, throwOnError: true);
					}
					if ((object)value == null)
					{
						throw MessagePackSerializationException.ThrowUnexpectedNilWhileDeserializing<Type>();
					}
				}
				TypeCache.TryAdd(array, value);
			}
			options.ThrowIfDeserializingTypeIsDisallowed(value);
			object formatterDynamicWithVerify = options.Resolver.GetFormatterDynamicWithVerify(value);
			if (!Deserializers.TryGetValue(value, out DeserializeMethod value2))
			{
				lock (Deserializers)
				{
					if (!Deserializers.TryGetValue(value, out value2))
					{
						TypeInfo typeInfo = value.GetTypeInfo();
						Type type = typeof(IMessagePackFormatter<>).MakeGenericType(value);
						ParameterExpression parameterExpression = Expression.Parameter(typeof(object), "formatter");
						ParameterExpression parameterExpression2 = Expression.Parameter(typeof(MessagePackReader).MakeByRefType(), "reader");
						ParameterExpression parameterExpression3 = Expression.Parameter(typeof(MessagePackSerializerOptions), "options");
						MethodInfo runtimeMethod = type.GetRuntimeMethod("Deserialize", new Type[2]
						{
							typeof(MessagePackReader).MakeByRefType(),
							typeof(MessagePackSerializerOptions)
						});
						MethodCallExpression methodCallExpression = Expression.Call(Expression.Convert(parameterExpression, type), runtimeMethod, parameterExpression2, parameterExpression3);
						Expression body = methodCallExpression;
						if (typeInfo.IsValueType)
						{
							body = Expression.Convert(methodCallExpression, typeof(object));
						}
						value2 = Expression.Lambda<DeserializeMethod>(body, new ParameterExpression[3] { parameterExpression, parameterExpression2, parameterExpression3 }).Compile();
						Deserializers.TryAdd(value, value2);
					}
				}
			}
			return value2(formatterDynamicWithVerify, ref byteSequence, options);
		}
	}
}
