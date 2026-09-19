using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MessagePack.Formatters;
using MessagePack.Internal;
using MessagePack.LZ4;
using MessagePack.Resolvers;
using Nerdbank.Streams;

namespace MessagePack
{
	public static class MessagePackSerializer
	{
		public static class Typeless
		{
			public static MessagePackSerializerOptions DefaultOptions { get; set; } = TypelessContractlessStandardResolver.Options;

			public static void Serialize(ref MessagePackWriter writer, object? obj, MessagePackSerializerOptions? options = null)
			{
				MessagePackSerializer.Serialize(ref writer, obj, options ?? DefaultOptions);
			}

			public static void Serialize(IBufferWriter<byte> writer, object? obj, MessagePackSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
			{
				MessagePackSerializer.Serialize(writer, obj, options ?? DefaultOptions, cancellationToken);
			}

			public static byte[] Serialize(object? obj, MessagePackSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
			{
				return MessagePackSerializer.Serialize(obj, options ?? DefaultOptions, cancellationToken);
			}

			public static void Serialize(Stream stream, object? obj, MessagePackSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
			{
				MessagePackSerializer.Serialize(stream, obj, options ?? DefaultOptions, cancellationToken);
			}

			public static Task SerializeAsync(Stream stream, object? obj, MessagePackSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
			{
				return MessagePackSerializer.SerializeAsync(stream, obj, options ?? DefaultOptions, cancellationToken);
			}

			public static object? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions? options = null)
			{
				return Deserialize<object>(ref reader, options ?? DefaultOptions);
			}

			public static object? Deserialize(in ReadOnlySequence<byte> byteSequence, MessagePackSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
			{
				return Deserialize<object>(in byteSequence, options ?? DefaultOptions, cancellationToken);
			}

			public static object? Deserialize(Stream stream, MessagePackSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
			{
				return Deserialize<object>(stream, options ?? DefaultOptions, cancellationToken);
			}

			public static object? Deserialize(Memory<byte> bytes, MessagePackSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
			{
				return Deserialize<object>(bytes, options ?? DefaultOptions, cancellationToken);
			}

			public static object? Deserialize(ReadOnlyMemory<byte> bytes, MessagePackSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
			{
				return Deserialize<object>(bytes, options ?? DefaultOptions, cancellationToken);
			}

			public static ValueTask<object?> DeserializeAsync(Stream stream, MessagePackSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
			{
				return DeserializeAsync<object>(stream, options ?? DefaultOptions, cancellationToken);
			}
		}

		private delegate int LZ4Transform(ReadOnlySpan<byte> input, Span<byte> output);

		private static class PrimitiveChecker<T>
		{
			public static readonly bool IsMessagePackFixedSizePrimitive;

			static PrimitiveChecker()
			{
				IsMessagePackFixedSizePrimitive = IsMessagePackFixedSizePrimitiveTypeHelper(typeof(T));
			}
		}

		private class CompiledMethods
		{
			internal delegate void MessagePackWriterSerialize(ref MessagePackWriter writer, object? value, MessagePackSerializerOptions? options);

			internal delegate object? MessagePackReaderDeserialize(ref MessagePackReader reader, MessagePackSerializerOptions? options);

			internal readonly Func<object?, MessagePackSerializerOptions?, CancellationToken, byte[]> Serialize_T_Options;

			internal readonly Action<Stream, object?, MessagePackSerializerOptions?, CancellationToken> Serialize_Stream_T_Options_CancellationToken;

			internal readonly Func<Stream, object?, MessagePackSerializerOptions?, CancellationToken, Task> SerializeAsync_Stream_T_Options_CancellationToken;

			internal readonly MessagePackWriterSerialize Serialize_MessagePackWriter_T_Options;

			internal readonly Action<IBufferWriter<byte>, object?, MessagePackSerializerOptions?, CancellationToken> Serialize_IBufferWriter_T_Options_CancellationToken;

			internal readonly MessagePackReaderDeserialize Deserialize_MessagePackReader_Options;

			internal readonly Func<Stream, MessagePackSerializerOptions?, CancellationToken, object?> Deserialize_Stream_Options_CancellationToken;

			internal readonly Func<Stream, MessagePackSerializerOptions?, CancellationToken, ValueTask<object?>> DeserializeAsync_Stream_Options_CancellationToken;

			internal readonly Func<ReadOnlyMemory<byte>, MessagePackSerializerOptions?, CancellationToken, object?> Deserialize_ReadOnlyMemory_Options;

			internal readonly Func<ReadOnlySequence<byte>, MessagePackSerializerOptions?, CancellationToken, object?> Deserialize_ReadOnlySequence_Options_CancellationToken;

			private bool PreferInterpretation => DynamicAssembly.AvoidDynamicCode;

			internal CompiledMethods(Type type)
			{
				TypeInfo typeInfo = type.GetTypeInfo();
				MethodInfo serialize = GetMethod("Serialize", type, new Type[3]
				{
					null,
					typeof(MessagePackSerializerOptions),
					typeof(CancellationToken)
				});
				if (DynamicAssembly.AvoidDynamicCode)
				{
					Serialize_T_Options = (object? x, MessagePackSerializerOptions? y, CancellationToken z) => (byte[])serialize.Invoke(null, new object[3] { x, y, z });
				}
				else
				{
					ParameterExpression parameterExpression = Expression.Parameter(typeof(object), "obj");
					ParameterExpression parameterExpression2 = Expression.Parameter(typeof(MessagePackSerializerOptions), "options");
					ParameterExpression parameterExpression3 = Expression.Parameter(typeof(CancellationToken), "cancellationToken");
					Serialize_T_Options = Expression.Lambda<Func<object, MessagePackSerializerOptions, CancellationToken, byte[]>>(Expression.Call(null, serialize, typeInfo.IsValueType ? Expression.Unbox(parameterExpression, type) : Expression.Convert(parameterExpression, type), parameterExpression2, parameterExpression3), new ParameterExpression[3] { parameterExpression, parameterExpression2, parameterExpression3 }).Compile(PreferInterpretation);
				}
				MethodInfo serialize2 = GetMethod("Serialize", type, new Type[4]
				{
					typeof(Stream),
					null,
					typeof(MessagePackSerializerOptions),
					typeof(CancellationToken)
				});
				if (DynamicAssembly.AvoidDynamicCode)
				{
					Serialize_Stream_T_Options_CancellationToken = delegate(Stream x, object? y, MessagePackSerializerOptions? z, CancellationToken a)
					{
						serialize2.Invoke(null, new object[4] { x, y, z, a });
					};
				}
				else
				{
					ParameterExpression parameterExpression4 = Expression.Parameter(typeof(Stream), "stream");
					ParameterExpression parameterExpression5 = Expression.Parameter(typeof(object), "obj");
					ParameterExpression parameterExpression6 = Expression.Parameter(typeof(MessagePackSerializerOptions), "options");
					ParameterExpression parameterExpression7 = Expression.Parameter(typeof(CancellationToken), "cancellationToken");
					Action<Stream, object, MessagePackSerializerOptions, CancellationToken> serialize_Stream_T_Options_CancellationToken = Expression.Lambda<Action<Stream, object, MessagePackSerializerOptions, CancellationToken>>(Expression.Call(null, serialize2, parameterExpression4, typeInfo.IsValueType ? Expression.Unbox(parameterExpression5, type) : Expression.Convert(parameterExpression5, type), parameterExpression6, parameterExpression7), new ParameterExpression[4] { parameterExpression4, parameterExpression5, parameterExpression6, parameterExpression7 }).Compile(PreferInterpretation);
					Serialize_Stream_T_Options_CancellationToken = serialize_Stream_T_Options_CancellationToken;
				}
				MethodInfo serialize3 = GetMethod("SerializeAsync", type, new Type[4]
				{
					typeof(Stream),
					null,
					typeof(MessagePackSerializerOptions),
					typeof(CancellationToken)
				});
				if (DynamicAssembly.AvoidDynamicCode)
				{
					SerializeAsync_Stream_T_Options_CancellationToken = (Stream x, object? y, MessagePackSerializerOptions? z, CancellationToken a) => (Task)serialize3.Invoke(null, new object[4] { x, y, z, a });
				}
				else
				{
					ParameterExpression parameterExpression8 = Expression.Parameter(typeof(Stream), "stream");
					ParameterExpression parameterExpression9 = Expression.Parameter(typeof(object), "obj");
					ParameterExpression parameterExpression10 = Expression.Parameter(typeof(MessagePackSerializerOptions), "options");
					ParameterExpression parameterExpression11 = Expression.Parameter(typeof(CancellationToken), "cancellationToken");
					Func<Stream, object, MessagePackSerializerOptions, CancellationToken, Task> serializeAsync_Stream_T_Options_CancellationToken = Expression.Lambda<Func<Stream, object, MessagePackSerializerOptions, CancellationToken, Task>>(Expression.Call(null, serialize3, parameterExpression8, typeInfo.IsValueType ? Expression.Unbox(parameterExpression9, type) : Expression.Convert(parameterExpression9, type), parameterExpression10, parameterExpression11), new ParameterExpression[4] { parameterExpression8, parameterExpression9, parameterExpression10, parameterExpression11 }).Compile(PreferInterpretation);
					SerializeAsync_Stream_T_Options_CancellationToken = serializeAsync_Stream_T_Options_CancellationToken;
				}
				MethodInfo serialize4 = GetMethod("Serialize", type, new Type[4]
				{
					typeof(IBufferWriter<byte>),
					null,
					typeof(MessagePackSerializerOptions),
					typeof(CancellationToken)
				});
				if (DynamicAssembly.AvoidDynamicCode)
				{
					Serialize_IBufferWriter_T_Options_CancellationToken = delegate(IBufferWriter<byte> x, object? y, MessagePackSerializerOptions? z, CancellationToken a)
					{
						serialize4.Invoke(null, new object[4] { x, y, z, a });
					};
				}
				else
				{
					ParameterExpression parameterExpression12 = Expression.Parameter(typeof(IBufferWriter<byte>), "writer");
					ParameterExpression parameterExpression13 = Expression.Parameter(typeof(object), "obj");
					ParameterExpression parameterExpression14 = Expression.Parameter(typeof(MessagePackSerializerOptions), "options");
					ParameterExpression parameterExpression15 = Expression.Parameter(typeof(CancellationToken), "cancellationToken");
					Action<IBufferWriter<byte>, object, MessagePackSerializerOptions, CancellationToken> serialize_IBufferWriter_T_Options_CancellationToken = Expression.Lambda<Action<IBufferWriter<byte>, object, MessagePackSerializerOptions, CancellationToken>>(Expression.Call(null, serialize4, parameterExpression12, typeInfo.IsValueType ? Expression.Unbox(parameterExpression13, type) : Expression.Convert(parameterExpression13, type), parameterExpression14, parameterExpression15), new ParameterExpression[4] { parameterExpression12, parameterExpression13, parameterExpression14, parameterExpression15 }).Compile(PreferInterpretation);
					Serialize_IBufferWriter_T_Options_CancellationToken = serialize_IBufferWriter_T_Options_CancellationToken;
				}
				MethodInfo method = GetMethod("SerializeSemiGeneric", type, new Type[3]
				{
					typeof(MessagePackWriter).MakeByRefType(),
					typeof(object),
					typeof(MessagePackSerializerOptions)
				});
				if (DynamicAssembly.AvoidDynamicCode)
				{
					Serialize_MessagePackWriter_T_Options = delegate
					{
						ThrowRefStructNotSupported();
					};
				}
				else
				{
					Serialize_MessagePackWriter_T_Options = (MessagePackWriterSerialize)method.CreateDelegate(typeof(MessagePackWriterSerialize));
				}
				MethodInfo method2 = GetMethod("DeserializeSemiGeneric", type, new Type[2]
				{
					typeof(MessagePackReader).MakeByRefType(),
					typeof(MessagePackSerializerOptions)
				});
				if (DynamicAssembly.AvoidDynamicCode)
				{
					Deserialize_MessagePackReader_Options = delegate
					{
						ThrowRefStructNotSupported();
						return (object?)null;
					};
				}
				else
				{
					Deserialize_MessagePackReader_Options = (MessagePackReaderDeserialize)method2.CreateDelegate(typeof(MessagePackReaderDeserialize));
				}
				MethodInfo deserialize = GetMethod("Deserialize", type, new Type[3]
				{
					typeof(Stream),
					typeof(MessagePackSerializerOptions),
					typeof(CancellationToken)
				});
				if (DynamicAssembly.AvoidDynamicCode)
				{
					Deserialize_Stream_Options_CancellationToken = (Stream x, MessagePackSerializerOptions? y, CancellationToken z) => deserialize.Invoke(null, new object[3] { x, y, z });
				}
				else
				{
					ParameterExpression parameterExpression16 = Expression.Parameter(typeof(Stream), "stream");
					ParameterExpression parameterExpression17 = Expression.Parameter(typeof(MessagePackSerializerOptions), "options");
					ParameterExpression parameterExpression18 = Expression.Parameter(typeof(CancellationToken), "cancellationToken");
					Func<Stream, MessagePackSerializerOptions, CancellationToken, object> deserialize_Stream_Options_CancellationToken = Expression.Lambda<Func<Stream, MessagePackSerializerOptions, CancellationToken, object>>(Expression.Convert(Expression.Call(null, deserialize, parameterExpression16, parameterExpression17, parameterExpression18), typeof(object)), new ParameterExpression[3] { parameterExpression16, parameterExpression17, parameterExpression18 }).Compile(PreferInterpretation);
					Deserialize_Stream_Options_CancellationToken = deserialize_Stream_Options_CancellationToken;
				}
				MethodInfo deserialize2 = GetMethod("DeserializeObjectAsync", type, new Type[3]
				{
					typeof(Stream),
					typeof(MessagePackSerializerOptions),
					typeof(CancellationToken)
				});
				if (DynamicAssembly.AvoidDynamicCode)
				{
					DeserializeAsync_Stream_Options_CancellationToken = (Stream x, MessagePackSerializerOptions? y, CancellationToken z) => (ValueTask<object>)deserialize2.Invoke(null, new object[3] { x, y, z });
				}
				else
				{
					ParameterExpression parameterExpression19 = Expression.Parameter(typeof(Stream), "stream");
					ParameterExpression parameterExpression20 = Expression.Parameter(typeof(MessagePackSerializerOptions), "options");
					ParameterExpression parameterExpression21 = Expression.Parameter(typeof(CancellationToken), "cancellationToken");
					Func<Stream, MessagePackSerializerOptions, CancellationToken, ValueTask<object>> deserializeAsync_Stream_Options_CancellationToken = Expression.Lambda<Func<Stream, MessagePackSerializerOptions, CancellationToken, ValueTask<object>>>(Expression.Convert(Expression.Call(null, deserialize2, parameterExpression19, parameterExpression20, parameterExpression21), typeof(ValueTask<object>)), new ParameterExpression[3] { parameterExpression19, parameterExpression20, parameterExpression21 }).Compile(PreferInterpretation);
					DeserializeAsync_Stream_Options_CancellationToken = deserializeAsync_Stream_Options_CancellationToken;
				}
				MethodInfo deserialize3 = GetMethod("Deserialize", type, new Type[3]
				{
					typeof(ReadOnlyMemory<byte>),
					typeof(MessagePackSerializerOptions),
					typeof(CancellationToken)
				});
				if (DynamicAssembly.AvoidDynamicCode)
				{
					Deserialize_ReadOnlyMemory_Options = (ReadOnlyMemory<byte> x, MessagePackSerializerOptions? y, CancellationToken z) => deserialize3.Invoke(null, new object[3] { x, y, z });
				}
				else
				{
					ParameterExpression parameterExpression22 = Expression.Parameter(typeof(ReadOnlyMemory<byte>), "bytes");
					ParameterExpression parameterExpression23 = Expression.Parameter(typeof(MessagePackSerializerOptions), "options");
					ParameterExpression parameterExpression24 = Expression.Parameter(typeof(CancellationToken), "cancellationToken");
					Func<ReadOnlyMemory<byte>, MessagePackSerializerOptions, CancellationToken, object> deserialize_ReadOnlyMemory_Options = Expression.Lambda<Func<ReadOnlyMemory<byte>, MessagePackSerializerOptions, CancellationToken, object>>(Expression.Convert(Expression.Call(null, deserialize3, parameterExpression22, parameterExpression23, parameterExpression24), typeof(object)), new ParameterExpression[3] { parameterExpression22, parameterExpression23, parameterExpression24 }).Compile(PreferInterpretation);
					Deserialize_ReadOnlyMemory_Options = deserialize_ReadOnlyMemory_Options;
				}
				MethodInfo deserialize4 = GetMethod("Deserialize", type, new Type[3]
				{
					typeof(ReadOnlySequence<byte>).MakeByRefType(),
					typeof(MessagePackSerializerOptions),
					typeof(CancellationToken)
				});
				if (DynamicAssembly.AvoidDynamicCode)
				{
					Deserialize_ReadOnlySequence_Options_CancellationToken = (ReadOnlySequence<byte> x, MessagePackSerializerOptions? y, CancellationToken z) => deserialize4.Invoke(null, new object[3] { x, y, z });
				}
				else
				{
					ParameterExpression parameterExpression25 = Expression.Parameter(typeof(ReadOnlySequence<byte>), "bytes");
					ParameterExpression parameterExpression26 = Expression.Parameter(typeof(MessagePackSerializerOptions), "options");
					ParameterExpression parameterExpression27 = Expression.Parameter(typeof(CancellationToken), "cancellationToken");
					Func<ReadOnlySequence<byte>, MessagePackSerializerOptions, CancellationToken, object> deserialize_ReadOnlySequence_Options_CancellationToken = Expression.Lambda<Func<ReadOnlySequence<byte>, MessagePackSerializerOptions, CancellationToken, object>>(Expression.Convert(Expression.Call(null, deserialize4, parameterExpression25, parameterExpression26, parameterExpression27), typeof(object)), new ParameterExpression[3] { parameterExpression25, parameterExpression26, parameterExpression27 }).Compile(PreferInterpretation);
					Deserialize_ReadOnlySequence_Options_CancellationToken = deserialize_ReadOnlySequence_Options_CancellationToken;
				}
			}

			private static void ThrowRefStructNotSupported()
			{
				throw new NotSupportedException("MessagePackWriter/Reader overload is not supported in MessagePackSerializer.NonGenerics.");
			}

			private static MethodInfo GetMethod(string methodName, Type type, Type?[] parameters)
			{
				return typeof(MessagePackSerializer).GetRuntimeMethods().Single(delegate(MethodInfo x)
				{
					if (methodName != x.Name)
					{
						return false;
					}
					ParameterInfo[] parameters2 = x.GetParameters();
					if (parameters2.Length != parameters.Length)
					{
						return false;
					}
					for (int i = 0; i < parameters2.Length; i = checked(i + 1))
					{
						if ((!(parameters[i] == null) || !parameters2[i].ParameterType.IsGenericParameter) && parameters2[i].ParameterType != parameters[i])
						{
							return false;
						}
					}
					return true;
				}).MakeGenericMethod(type);
			}
		}

		private static MessagePackSerializerOptions? defaultOptions;

		[ThreadStatic]
		private static byte[]? scratchArray;

		private static readonly LZ4Transform LZ4CodecEncode;

		private static readonly LZ4Transform LZ4CodecDecode;

		private static readonly Func<Type, CompiledMethods> CreateCompiledMethods;

		private static readonly ThreadsafeTypeKeyHashTable<CompiledMethods> Serializes;

		public static MessagePackSerializerOptions DefaultOptions
		{
			get
			{
				return defaultOptions ?? (defaultOptions = MessagePackSerializerOptions.Standard);
			}
			set
			{
				defaultOptions = value;
			}
		}

		public static void Serialize<T>(IBufferWriter<byte> writer, T value, MessagePackSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			MessagePackWriter messagePackWriter = new MessagePackWriter(writer);
			messagePackWriter.CancellationToken = cancellationToken;
			MessagePackWriter writer2 = messagePackWriter;
			Serialize(ref writer2, value, options);
			writer2.Flush();
		}

		public static void Serialize<T>(ref MessagePackWriter writer, T value, MessagePackSerializerOptions? options = null)
		{
			options = options ?? DefaultOptions;
			bool oldSpec = writer.OldSpec;
			if (options.OldSpec.HasValue)
			{
				writer.OldSpec = options.OldSpec.Value;
			}
			try
			{
				if (options.Compression.IsCompression() && !PrimitiveChecker<T>.IsMessagePackFixedSizePrimitive)
				{
					using (SequencePool.Rental rental = options.SequencePool.Rent())
					{
						Sequence<byte> value2 = rental.Value;
						MessagePackWriter writer2 = writer.Clone(value2);
						options.Resolver.GetFormatterWithVerify<T>().Serialize(ref writer2, value, options);
						writer2.Flush();
						ToLZ4BinaryCore((ReadOnlySequence<byte>)value2, ref writer, options.Compression, options.CompressionMinLength);
						return;
					}
				}
				options.Resolver.GetFormatterWithVerify<T>().Serialize(ref writer, value, options);
			}
			catch (Exception inner)
			{
				throw new MessagePackSerializationException("Failed to serialize " + typeof(T).FullName + " value.", inner);
			}
			finally
			{
				writer.OldSpec = oldSpec;
			}
		}

		public static byte[] Serialize<T>(T value, MessagePackSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			byte[] array = scratchArray;
			if (array == null)
			{
				array = (scratchArray = new byte[65536]);
			}
			options = options ?? DefaultOptions;
			MessagePackWriter messagePackWriter = new MessagePackWriter(options.SequencePool, array);
			messagePackWriter.CancellationToken = cancellationToken;
			MessagePackWriter writer = messagePackWriter;
			Serialize(ref writer, value, options);
			return writer.FlushAndGetArray();
		}

		public static void Serialize<T>(Stream stream, T value, MessagePackSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			options = options ?? DefaultOptions;
			cancellationToken.ThrowIfCancellationRequested();
			using SequencePool.Rental rental = options.SequencePool.Rent();
			Serialize(rental.Value, value, options, cancellationToken);
			try
			{
				ReadOnlySequence<byte>.Enumerator enumerator = rental.Value.AsReadOnlySequence.GetEnumerator();
				while (enumerator.MoveNext())
				{
					ReadOnlyMemory<byte> current = enumerator.Current;
					cancellationToken.ThrowIfCancellationRequested();
					stream.Write(current.Span);
				}
			}
			catch (Exception inner)
			{
				throw new MessagePackSerializationException("Error occurred while writing the serialized data to the stream.", inner);
			}
		}

		public static async Task SerializeAsync<T>(Stream stream, T value, MessagePackSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			options = options ?? DefaultOptions;
			cancellationToken.ThrowIfCancellationRequested();
			using SequencePool.Rental sequenceRental = options.SequencePool.Rent();
			Serialize(sequenceRental.Value, value, options, cancellationToken);
			try
			{
				ReadOnlySequence<byte>.Enumerator enumerator = sequenceRental.Value.AsReadOnlySequence.GetEnumerator();
				while (enumerator.MoveNext())
				{
					ReadOnlyMemory<byte> current = enumerator.Current;
					cancellationToken.ThrowIfCancellationRequested();
					await stream.WriteAsync(current, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			catch (Exception inner)
			{
				throw new MessagePackSerializationException("Error occurred while writing the serialized data to the stream.", inner);
			}
		}

		public static T Deserialize<T>(in ReadOnlySequence<byte> byteSequence, MessagePackSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			MessagePackReader messagePackReader = new MessagePackReader(in byteSequence);
			messagePackReader.CancellationToken = cancellationToken;
			MessagePackReader reader = messagePackReader;
			return Deserialize<T>(ref reader, options);
		}

		public static T Deserialize<T>(ref MessagePackReader reader, MessagePackSerializerOptions? options = null)
		{
			options = options ?? DefaultOptions;
			try
			{
				if (options.Compression.IsCompression())
				{
					using (SequencePool.Rental rental = options.SequencePool.Rent())
					{
						Sequence<byte> value = rental.Value;
						if (TryDecompress(ref reader, value))
						{
							MessagePackReader reader2 = reader.Clone(value.AsReadOnlySequence);
							return options.Resolver.GetFormatterWithVerify<T>().Deserialize(ref reader2, options);
						}
						return options.Resolver.GetFormatterWithVerify<T>().Deserialize(ref reader, options);
					}
				}
				return options.Resolver.GetFormatterWithVerify<T>().Deserialize(ref reader, options);
			}
			catch (Exception inner)
			{
				throw new MessagePackSerializationException("Failed to deserialize " + typeof(T).FullName + " value.", inner);
			}
		}

		public static T Deserialize<T>(ReadOnlyMemory<byte> buffer, MessagePackSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			MessagePackReader messagePackReader = new MessagePackReader(buffer);
			messagePackReader.CancellationToken = cancellationToken;
			MessagePackReader reader = messagePackReader;
			return Deserialize<T>(ref reader, options);
		}

		public static T Deserialize<T>(ReadOnlyMemory<byte> buffer, out int bytesRead, CancellationToken cancellationToken = default(CancellationToken))
		{
			return Deserialize<T>(buffer, null, out bytesRead, cancellationToken);
		}

		public static T Deserialize<T>(ReadOnlyMemory<byte> buffer, MessagePackSerializerOptions? options, out int bytesRead, CancellationToken cancellationToken = default(CancellationToken))
		{
			MessagePackReader messagePackReader = new MessagePackReader(buffer);
			messagePackReader.CancellationToken = cancellationToken;
			MessagePackReader reader = messagePackReader;
			T result = Deserialize<T>(ref reader, options);
			bytesRead = buffer.Slice(0, checked((int)reader.Consumed)).Length;
			return result;
		}

		public static T Deserialize<T>(Stream stream, MessagePackSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			options = options ?? DefaultOptions;
			if (TryDeserializeFromMemoryStream<T>(stream, options, cancellationToken, out var result))
			{
				return result;
			}
			using SequencePool.Rental rental = options.SequencePool.Rent();
			Sequence<byte> value = rental.Value;
			try
			{
				int num;
				do
				{
					cancellationToken.ThrowIfCancellationRequested();
					Span<byte> span = value.GetSpan(stream.CanSeek ? checked((int)Math.Min(options.SuggestedContiguousMemorySize, stream.Length - stream.Position)) : 0);
					num = stream.Read(span);
					value.Advance(num);
				}
				while (num > 0);
			}
			catch (Exception inner)
			{
				throw new MessagePackSerializationException("Error occurred while reading from the stream.", inner);
			}
			return DeserializeFromSequenceAndRewindStreamIfPossible<T>(stream, options, value, cancellationToken);
		}

		public static async ValueTask<T> DeserializeAsync<T>(Stream stream, MessagePackSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (options == null)
			{
				options = DefaultOptions;
			}
			if (TryDeserializeFromMemoryStream<T>(stream, options, cancellationToken, out var result))
			{
				return result;
			}
			using SequencePool.Rental sequenceRental = options.SequencePool.Rent();
			Sequence<byte> sequence = sequenceRental.Value;
			try
			{
				int num;
				do
				{
					Memory<byte> memory = sequence.GetMemory(stream.CanSeek ? checked((int)Math.Min(options.SuggestedContiguousMemorySize, stream.Length - stream.Position)) : 0);
					num = await stream.ReadAsync(memory, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
					sequence.Advance(num);
				}
				while (num > 0);
			}
			catch (Exception inner)
			{
				throw new MessagePackSerializationException("Error occurred while reading from the stream.", inner);
			}
			return DeserializeFromSequenceAndRewindStreamIfPossible<T>(stream, options, sequence, cancellationToken);
		}

		private static bool TryDeserializeFromMemoryStream<T>(Stream stream, MessagePackSerializerOptions options, CancellationToken cancellationToken, [MaybeNullWhen(false)] out T result)
		{
			cancellationToken.ThrowIfCancellationRequested();
			if (stream is MemoryStream memoryStream && memoryStream.TryGetBuffer(out var buffer))
			{
				result = Deserialize<T>(buffer.AsMemory(checked((int)memoryStream.Position)), options, out var bytesRead, cancellationToken);
				memoryStream.Seek(bytesRead, SeekOrigin.Current);
				return true;
			}
			result = default(T);
			return false;
		}

		private static T DeserializeFromSequenceAndRewindStreamIfPossible<T>(Stream streamToRewind, MessagePackSerializerOptions? options, ReadOnlySequence<byte> sequence, CancellationToken cancellationToken)
		{
			if (streamToRewind == null)
			{
				throw new ArgumentNullException("streamToRewind");
			}
			MessagePackReader messagePackReader = new MessagePackReader(in sequence);
			messagePackReader.CancellationToken = cancellationToken;
			MessagePackReader reader = messagePackReader;
			T result = Deserialize<T>(ref reader, options);
			if (streamToRewind.CanSeek && !reader.End)
			{
				long length = reader.Sequence.Slice(reader.Position).Length;
				streamToRewind.Seek(checked(-length), SeekOrigin.Current);
			}
			return result;
		}

		private static int LZ4Operation(in ReadOnlySequence<byte> input, Span<byte> output, LZ4Transform lz4Operation)
		{
			byte[] array = null;
			checked
			{
				ReadOnlySpan<byte> input2;
				if (input.IsSingleSegment)
				{
					input2 = input.First.Span;
				}
				else
				{
					array = ArrayPool<byte>.Shared.Rent((int)input.Length);
					BuffersExtensions.CopyTo(in input, array);
					input2 = array.AsSpan(0, (int)input.Length);
				}
				try
				{
					return lz4Operation(input2, output);
				}
				finally
				{
					if (array != null)
					{
						ArrayPool<byte>.Shared.Return(array);
					}
				}
			}
		}

		private static bool TryDecompress(ref MessagePackReader reader, IBufferWriter<byte> writer)
		{
			checked
			{
				if (!reader.End)
				{
					Span<byte> span;
					if (reader.NextMessagePackType == MessagePackType.Extension && reader.CreatePeekReader().ReadExtensionFormatHeader().TypeCode == 99)
					{
						ReadOnlySequence<byte> readOnlySequence = reader.ReadExtensionFormat().Data;
						MessagePackReader messagePackReader = new MessagePackReader(in readOnlySequence);
						int num = messagePackReader.ReadInt32();
						readOnlySequence = messagePackReader.Sequence;
						ReadOnlySequence<byte> input = readOnlySequence.Slice(messagePackReader.Position);
						span = writer.GetSpan(num);
						Span<byte> output = span.Slice(0, num);
						int count = LZ4Operation(in input, output, LZ4CodecDecode);
						writer.Advance(count);
						return true;
					}
					if (reader.NextMessagePackType == MessagePackType.Array)
					{
						MessagePackReader messagePackReader2 = reader.CreatePeekReader();
						int num2 = messagePackReader2.ReadArrayHeader();
						if (num2 != 0 && messagePackReader2.NextMessagePackType == MessagePackType.Extension && messagePackReader2.ReadExtensionFormatHeader().TypeCode == 98)
						{
							reader = messagePackReader2;
							int num3 = num2 - 1;
							int[] array = ArrayPool<int>.Shared.Rent(num3);
							try
							{
								for (int i = 0; i < num3; i++)
								{
									array[i] = reader.ReadInt32();
								}
								for (int j = 0; j < num3; j++)
								{
									int num4 = array[j];
									ReadOnlySequence<byte> input2 = reader.ReadBytes() ?? throw MessagePackSerializationException.ThrowUnexpectedNilWhileDeserializing<ReadOnlySequence<byte>>();
									span = writer.GetSpan(num4);
									Span<byte> output2 = span.Slice(0, num4);
									int count2 = LZ4Operation(in input2, output2, LZ4CodecDecode);
									writer.Advance(count2);
								}
								return true;
							}
							finally
							{
								ArrayPool<int>.Shared.Return(array);
							}
						}
					}
				}
				return false;
			}
		}

		private static void ToLZ4BinaryCore(in ReadOnlySequence<byte> msgpackUncompressedData, ref MessagePackWriter writer, MessagePackCompression compression, int minCompressionSize)
		{
			if (msgpackUncompressedData.Length < minCompressionSize)
			{
				writer.WriteRaw(in msgpackUncompressedData);
				return;
			}
			checked
			{
				switch (compression)
				{
				case MessagePackCompression.Lz4Block:
				{
					int minimumLength = LZ4Codec.MaximumOutputLength((int)msgpackUncompressedData.Length);
					byte[] array = ArrayPool<byte>.Shared.Rent(minimumLength);
					try
					{
						int num5 = LZ4Operation(in msgpackUncompressedData, array, LZ4CodecEncode);
						writer.WriteExtensionFormatHeader(new ExtensionHeader(99, 5 + (uint)num5));
						writer.WriteInt32((int)msgpackUncompressedData.Length);
						writer.WriteRaw(array.AsSpan(0, num5));
						break;
					}
					finally
					{
						ArrayPool<byte>.Shared.Return(array);
					}
				}
				case MessagePackCompression.Lz4BlockArray:
				{
					int num = 0;
					int num2 = 0;
					ReadOnlySequence<byte>.Enumerator enumerator = msgpackUncompressedData.GetEnumerator();
					while (enumerator.MoveNext())
					{
						ReadOnlyMemory<byte> current = enumerator.Current;
						num++;
						num2 += GetUInt32WriteSize((uint)current.Length);
					}
					writer.WriteArrayHeader(num + 1);
					writer.WriteExtensionFormatHeader(new ExtensionHeader(98, num2));
					enumerator = msgpackUncompressedData.GetEnumerator();
					while (enumerator.MoveNext())
					{
						writer.Write(enumerator.Current.Length);
					}
					enumerator = msgpackUncompressedData.GetEnumerator();
					while (enumerator.MoveNext())
					{
						ReadOnlyMemory<byte> current2 = enumerator.Current;
						int num3 = LZ4Codec.MaximumOutputLength(current2.Length);
						Span<byte> span = writer.GetSpan(num3 + 5);
						int num4 = LZ4Codec.Encode(current2.Span, span.Slice(5, span.Length - 5));
						WriteBin32Header((uint)num4, span);
						writer.Advance(num4 + 5);
					}
					break;
				}
				default:
					throw new ArgumentException("Invalid MessagePackCompression Code. Code:" + compression);
				}
			}
		}

		private static int GetUInt32WriteSize(uint value)
		{
			if (value <= 127)
			{
				return 1;
			}
			if (value <= 255)
			{
				return 2;
			}
			if (value <= 65535)
			{
				return 3;
			}
			return 5;
		}

		private static void WriteBin32Header(uint value, Span<byte> span)
		{
			span[0] = 198;
			span[4] = (byte)value;
			span[3] = (byte)(value >> 8);
			span[2] = (byte)(value >> 16);
			span[1] = (byte)(value >> 24);
		}

		private static bool IsMessagePackFixedSizePrimitiveTypeHelper(Type type)
		{
			if (!(type == typeof(short)) && !(type == typeof(int)) && !(type == typeof(long)) && !(type == typeof(ushort)) && !(type == typeof(uint)) && !(type == typeof(ulong)) && !(type == typeof(float)) && !(type == typeof(double)) && !(type == typeof(bool)) && !(type == typeof(byte)) && !(type == typeof(sbyte)))
			{
				return type == typeof(char);
			}
			return true;
		}

		public static void SerializeToJson<T>(TextWriter textWriter, T obj, MessagePackSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			options = options ?? DefaultOptions;
			using SequencePool.Rental rental = options.SequencePool.Rent();
			MessagePackWriter messagePackWriter = new MessagePackWriter(rental.Value);
			messagePackWriter.CancellationToken = cancellationToken;
			MessagePackWriter writer = messagePackWriter;
			Serialize(ref writer, obj, options);
			writer.Flush();
			MessagePackReader messagePackReader = new MessagePackReader((ReadOnlySequence<byte>)rental.Value);
			messagePackReader.CancellationToken = cancellationToken;
			MessagePackReader reader = messagePackReader;
			ConvertToJson(ref reader, textWriter, options);
		}

		public static string SerializeToJson<T>(T obj, MessagePackSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			StringWriter stringWriter = new StringWriter();
			SerializeToJson(stringWriter, obj, options, cancellationToken);
			return stringWriter.ToString();
		}

		public static string ConvertToJson(ReadOnlyMemory<byte> bytes, MessagePackSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return ConvertToJson(new ReadOnlySequence<byte>(bytes), options, cancellationToken);
		}

		public static string ConvertToJson(in ReadOnlySequence<byte> bytes, MessagePackSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			StringWriter stringWriter = new StringWriter();
			MessagePackReader messagePackReader = new MessagePackReader(in bytes);
			messagePackReader.CancellationToken = cancellationToken;
			MessagePackReader reader = messagePackReader;
			ConvertToJson(ref reader, stringWriter, options);
			return stringWriter.ToString();
		}

		public static void ConvertToJson(ref MessagePackReader reader, TextWriter jsonWriter, MessagePackSerializerOptions? options = null)
		{
			if (reader.End)
			{
				return;
			}
			options = options ?? DefaultOptions;
			try
			{
				if (options.Compression.IsCompression())
				{
					using (SequencePool.Rental rental = options.SequencePool.Rent())
					{
						if (TryDecompress(ref reader, rental.Value))
						{
							MessagePackReader messagePackReader = new MessagePackReader((ReadOnlySequence<byte>)rental.Value);
							messagePackReader.CancellationToken = reader.CancellationToken;
							MessagePackReader reader2 = messagePackReader;
							if (!reader2.End)
							{
								ToJsonCore(ref reader2, jsonWriter, options);
							}
						}
						else
						{
							ToJsonCore(ref reader, jsonWriter, options);
						}
						return;
					}
				}
				ToJsonCore(ref reader, jsonWriter, options);
			}
			catch (Exception inner)
			{
				throw new MessagePackSerializationException("Error occurred while translating msgpack to JSON.", inner);
			}
		}

		public static void ConvertFromJson(string str, ref MessagePackWriter writer, MessagePackSerializerOptions? options = null)
		{
			using StringReader reader = new StringReader(str);
			ConvertFromJson(reader, ref writer, options);
		}

		public static byte[] ConvertFromJson(string str, MessagePackSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			options = options ?? DefaultOptions;
			using SequencePool.Rental rental = options.SequencePool.Rent();
			MessagePackWriter messagePackWriter = new MessagePackWriter(rental.Value);
			messagePackWriter.CancellationToken = cancellationToken;
			MessagePackWriter writer = messagePackWriter;
			using (StringReader reader = new StringReader(str))
			{
				ConvertFromJson(reader, ref writer, options);
			}
			writer.Flush();
			return rental.Value.AsReadOnlySequence.ToArray<byte>();
		}

		public static void ConvertFromJson(TextReader reader, ref MessagePackWriter writer, MessagePackSerializerOptions? options = null)
		{
			options = options ?? DefaultOptions;
			if (options.Compression.IsCompression())
			{
				using (SequencePool.Rental rental = options.SequencePool.Rent())
				{
					MessagePackWriter writer2 = writer.Clone(rental.Value);
					using (TinyJsonReader jr = new TinyJsonReader(reader, disposeInnerReader: false))
					{
						FromJsonCore(jr, ref writer2, options);
					}
					writer2.Flush();
					ToLZ4BinaryCore((ReadOnlySequence<byte>)rental.Value, ref writer, options.Compression, options.CompressionMinLength);
					return;
				}
			}
			using TinyJsonReader jr2 = new TinyJsonReader(reader, disposeInnerReader: false);
			FromJsonCore(jr2, ref writer, options);
		}

		private static uint FromJsonCore(TinyJsonReader jr, ref MessagePackWriter writer, MessagePackSerializerOptions options)
		{
			uint num = 0u;
			checked
			{
				while (jr.Read())
				{
					switch (jr.TokenType)
					{
					case TinyJsonToken.StartObject:
					{
						using (SequencePool.Rental rental2 = options.SequencePool.Rent())
						{
							MessagePackWriter writer3 = writer.Clone(rental2.Value);
							uint num2 = FromJsonCore(jr, ref writer3, options);
							writer3.Flush();
							num2 = unchecked(num2 / 2);
							writer.WriteMapHeader(num2);
							writer.WriteRaw((ReadOnlySequence<byte>)rental2.Value);
						}
						num++;
						break;
					}
					case TinyJsonToken.EndObject:
						return num;
					case TinyJsonToken.StartArray:
					{
						using (SequencePool.Rental rental = options.SequencePool.Rent())
						{
							MessagePackWriter writer2 = writer.Clone(rental.Value);
							uint count = FromJsonCore(jr, ref writer2, options);
							writer2.Flush();
							writer.WriteArrayHeader(count);
							writer.WriteRaw((ReadOnlySequence<byte>)rental.Value);
						}
						num++;
						break;
					}
					case TinyJsonToken.EndArray:
						return num;
					case TinyJsonToken.Number:
						switch (jr.ValueType)
						{
						case ValueType.Double:
							writer.Write(jr.DoubleValue);
							break;
						case ValueType.Long:
							writer.Write(jr.LongValue);
							break;
						case ValueType.ULong:
							writer.Write(jr.ULongValue);
							break;
						case ValueType.Decimal:
							DecimalFormatter.Instance.Serialize(ref writer, jr.DecimalValue, options);
							break;
						}
						num++;
						break;
					case TinyJsonToken.String:
						writer.Write(jr.StringValue);
						num++;
						break;
					case TinyJsonToken.True:
						writer.Write(value: true);
						num++;
						break;
					case TinyJsonToken.False:
						writer.Write(value: false);
						num++;
						break;
					case TinyJsonToken.Null:
						writer.WriteNil();
						num++;
						break;
					}
				}
				return num;
			}
		}

		private static void ToJsonCore(ref MessagePackReader reader, TextWriter writer, MessagePackSerializerOptions options)
		{
			checked
			{
				switch (reader.NextMessagePackType)
				{
				case MessagePackType.Integer:
					if (MessagePackCode.IsSignedInteger(reader.NextCode))
					{
						writer.Write(reader.ReadInt64().ToString(CultureInfo.InvariantCulture));
					}
					else
					{
						writer.Write(reader.ReadUInt64().ToString(CultureInfo.InvariantCulture));
					}
					break;
				case MessagePackType.Boolean:
					writer.Write(reader.ReadBoolean() ? "true" : "false");
					break;
				case MessagePackType.Float:
					if (reader.NextCode == 202)
					{
						writer.Write(reader.ReadSingle().ToString("R", CultureInfo.InvariantCulture));
					}
					else
					{
						writer.Write(reader.ReadDouble().ToString("R", CultureInfo.InvariantCulture));
					}
					break;
				case MessagePackType.String:
					WriteJsonString(reader.ReadString(), writer);
					break;
				case MessagePackType.Binary:
				{
					ArraySegment<byte> arraySegment = ByteArraySegmentFormatter.Instance.Deserialize(ref reader, options);
					writer.Write("\"" + Convert.ToBase64String(arraySegment.Array ?? Array.Empty<byte>(), arraySegment.Offset, arraySegment.Count) + "\"");
					break;
				}
				case MessagePackType.Array:
				{
					int num2 = reader.ReadArrayHeader();
					options.Security.DepthStep(ref reader);
					try
					{
						writer.Write("[");
						for (int i = 0; i < num2; i++)
						{
							ToJsonCore(ref reader, writer, options);
							if (i != num2 - 1)
							{
								writer.Write(",");
							}
						}
						writer.Write("]");
						break;
					}
					finally
					{
						reader.Depth--;
					}
				}
				case MessagePackType.Map:
				{
					int num3 = reader.ReadMapHeader();
					options.Security.DepthStep(ref reader);
					try
					{
						writer.Write("{");
						for (int j = 0; j < num3; j++)
						{
							MessagePackType nextMessagePackType2 = reader.NextMessagePackType;
							if (nextMessagePackType2 == MessagePackType.String || nextMessagePackType2 == MessagePackType.Binary)
							{
								ToJsonCore(ref reader, writer, options);
							}
							else
							{
								writer.Write("\"");
								ToJsonCore(ref reader, writer, options);
								writer.Write("\"");
							}
							writer.Write(":");
							ToJsonCore(ref reader, writer, options);
							if (j != num3 - 1)
							{
								writer.Write(",");
							}
						}
						writer.Write("}");
						break;
					}
					finally
					{
						reader.Depth--;
					}
				}
				case MessagePackType.Extension:
				{
					ExtensionHeader header = reader.ReadExtensionFormatHeader();
					if (header.TypeCode == -1)
					{
						DateTime dateTime = reader.ReadDateTime(header);
						writer.Write("\"");
						writer.Write(dateTime.ToString("o", CultureInfo.InvariantCulture));
						writer.Write("\"");
					}
					else if (header.TypeCode == 100)
					{
						StringBuilder stringBuilder = new StringBuilder();
						StringBuilder stringBuilder2 = new StringBuilder();
						SequencePosition position = reader.Position;
						ToJsonCore(ref reader, new StringWriter(stringBuilder2), options);
						int num = (int)reader.Sequence.Slice(position, reader.Position).Length;
						if (header.Length > num)
						{
							MessagePackType nextMessagePackType = reader.NextMessagePackType;
							if (nextMessagePackType != MessagePackType.Array && nextMessagePackType != MessagePackType.Map)
							{
								stringBuilder.Append("{");
							}
							ToJsonCore(ref reader, new StringWriter(stringBuilder), options);
							if (nextMessagePackType != MessagePackType.Array)
							{
								stringBuilder2.Insert(0, "\"$type\":");
							}
							if (nextMessagePackType != MessagePackType.Array && nextMessagePackType != MessagePackType.Map)
							{
								stringBuilder.Append("}");
							}
							if (stringBuilder.Length > 2)
							{
								stringBuilder2.Append(",");
							}
							stringBuilder.Insert(1, stringBuilder2.ToString());
							writer.Write(stringBuilder.ToString());
						}
						else
						{
							writer.Write("{\"$type\":" + stringBuilder2.ToString() + "}");
						}
					}
					else
					{
						ReadOnlySequence<byte> sequence = reader.ReadRaw(header.Length);
						writer.Write("[");
						writer.Write(header.TypeCode);
						writer.Write(",");
						writer.Write("\"");
						writer.Write(Convert.ToBase64String(BuffersExtensions.ToArray(in sequence)));
						writer.Write("\"");
						writer.Write("]");
					}
					break;
				}
				case MessagePackType.Nil:
					reader.Skip();
					writer.Write("null");
					break;
				default:
					throw new MessagePackSerializationException($"code is invalid. code: {reader.NextCode} format: {MessagePackCode.ToFormatName(reader.NextCode)}");
				}
			}
		}

		private static void WriteJsonString(string value, TextWriter builder)
		{
			builder.Write('"');
			int length = value.Length;
			for (int i = 0; i < length; i = checked(i + 1))
			{
				char c = value[i];
				switch (c)
				{
				case '"':
					builder.Write("\\\"");
					break;
				case '\\':
					builder.Write("\\\\");
					break;
				case '\b':
					builder.Write("\\b");
					break;
				case '\f':
					builder.Write("\\f");
					break;
				case '\n':
					builder.Write("\\n");
					break;
				case '\r':
					builder.Write("\\r");
					break;
				case '\t':
					builder.Write("\\t");
					break;
				default:
					builder.Write(c);
					break;
				}
			}
			builder.Write('"');
		}

		static MessagePackSerializer()
		{
			LZ4CodecEncode = LZ4Codec.Encode;
			LZ4CodecDecode = LZ4Codec.Decode;
			Serializes = new ThreadsafeTypeKeyHashTable<CompiledMethods>(64);
			CreateCompiledMethods = (Type t) => new CompiledMethods(t);
		}

		public static void Serialize(Type type, ref MessagePackWriter writer, object? obj, MessagePackSerializerOptions? options = null)
		{
			GetOrAdd(type).Serialize_MessagePackWriter_T_Options(ref writer, obj, options);
		}

		public static void Serialize(Type type, IBufferWriter<byte> writer, object? obj, MessagePackSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			GetOrAdd(type).Serialize_IBufferWriter_T_Options_CancellationToken(writer, obj, options, cancellationToken);
		}

		public static byte[] Serialize(Type type, object? obj, MessagePackSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return GetOrAdd(type).Serialize_T_Options(obj, options, cancellationToken);
		}

		public static void Serialize(Type type, Stream stream, object? obj, MessagePackSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			GetOrAdd(type).Serialize_Stream_T_Options_CancellationToken(stream, obj, options, cancellationToken);
		}

		public static Task SerializeAsync(Type type, Stream stream, object? obj, MessagePackSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return GetOrAdd(type).SerializeAsync_Stream_T_Options_CancellationToken(stream, obj, options, cancellationToken);
		}

		public static object? Deserialize(Type type, ref MessagePackReader reader, MessagePackSerializerOptions? options = null)
		{
			return GetOrAdd(type).Deserialize_MessagePackReader_Options(ref reader, options);
		}

		public static object? Deserialize(Type type, Stream stream, MessagePackSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return GetOrAdd(type).Deserialize_Stream_Options_CancellationToken(stream, options, cancellationToken);
		}

		public static ValueTask<object?> DeserializeAsync(Type type, Stream stream, MessagePackSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return GetOrAdd(type).DeserializeAsync_Stream_Options_CancellationToken(stream, options, cancellationToken);
		}

		public static object? Deserialize(Type type, ReadOnlyMemory<byte> bytes, MessagePackSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return GetOrAdd(type).Deserialize_ReadOnlyMemory_Options(bytes, options, cancellationToken);
		}

		public static object? Deserialize(Type type, ReadOnlySequence<byte> bytes, MessagePackSerializerOptions? options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return GetOrAdd(type).Deserialize_ReadOnlySequence_Options_CancellationToken(bytes, options, cancellationToken);
		}

		private static void SerializeSemiGeneric<T>(ref MessagePackWriter writer, object valueObject, MessagePackSerializerOptions? options = null)
		{
			Serialize(ref writer, (T)valueObject, options);
		}

		private static object? DeserializeSemiGeneric<T>(ref MessagePackReader reader, MessagePackSerializerOptions? options = null)
		{
			return Deserialize<T>(ref reader, options);
		}

		private static async ValueTask<object?> DeserializeObjectAsync<T>(Stream stream, MessagePackSerializerOptions options, CancellationToken cancellationToken)
		{
			return await DeserializeAsync<T>(stream, options, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		private static CompiledMethods GetOrAdd(Type type)
		{
			return Serializes.GetOrAdd(type, CreateCompiledMethods);
		}
	}
}
