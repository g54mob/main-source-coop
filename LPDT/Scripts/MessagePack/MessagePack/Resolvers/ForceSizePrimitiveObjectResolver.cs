using System;
using System.Collections.Generic;
using MessagePack.Formatters;

namespace MessagePack.Resolvers
{
	internal sealed class ForceSizePrimitiveObjectResolver : IFormatterResolver
	{
		private static class FormatterCache<T>
		{
			public static readonly IMessagePackFormatter<T>? Formatter;

			static FormatterCache()
			{
				Formatter = (IMessagePackFormatter<T>)Helper.GetFormatter(typeof(T));
			}
		}

		private static class Helper
		{
			private static readonly Dictionary<Type, object> FormatterMap = new Dictionary<Type, object>
			{
				{
					typeof(short),
					ForceInt16BlockFormatter.Instance
				},
				{
					typeof(int),
					ForceInt32BlockFormatter.Instance
				},
				{
					typeof(long),
					ForceInt64BlockFormatter.Instance
				},
				{
					typeof(ushort),
					ForceUInt16BlockFormatter.Instance
				},
				{
					typeof(uint),
					ForceUInt32BlockFormatter.Instance
				},
				{
					typeof(ulong),
					ForceUInt64BlockFormatter.Instance
				},
				{
					typeof(byte),
					ForceByteBlockFormatter.Instance
				},
				{
					typeof(sbyte),
					ForceSByteBlockFormatter.Instance
				},
				{
					typeof(short?),
					NullableForceInt16BlockFormatter.Instance
				},
				{
					typeof(int?),
					NullableForceInt32BlockFormatter.Instance
				},
				{
					typeof(long?),
					NullableForceInt64BlockFormatter.Instance
				},
				{
					typeof(ushort?),
					NullableForceUInt16BlockFormatter.Instance
				},
				{
					typeof(uint?),
					NullableForceUInt32BlockFormatter.Instance
				},
				{
					typeof(ulong?),
					NullableForceUInt64BlockFormatter.Instance
				},
				{
					typeof(byte?),
					NullableForceByteBlockFormatter.Instance
				},
				{
					typeof(sbyte?),
					NullableForceSByteBlockFormatter.Instance
				},
				{
					typeof(short[]),
					ForceInt16BlockArrayFormatter.Instance
				},
				{
					typeof(int[]),
					ForceInt32BlockArrayFormatter.Instance
				},
				{
					typeof(long[]),
					ForceInt64BlockArrayFormatter.Instance
				},
				{
					typeof(ushort[]),
					ForceUInt16BlockArrayFormatter.Instance
				},
				{
					typeof(uint[]),
					ForceUInt32BlockArrayFormatter.Instance
				},
				{
					typeof(ulong[]),
					ForceUInt64BlockArrayFormatter.Instance
				},
				{
					typeof(sbyte[]),
					ForceSByteBlockArrayFormatter.Instance
				}
			};

			public static object? GetFormatter(Type type)
			{
				if (!FormatterMap.TryGetValue(type, out object value))
				{
					return null;
				}
				return value;
			}
		}

		public static readonly ForceSizePrimitiveObjectResolver Instance;

		public static readonly MessagePackSerializerOptions Options;

		static ForceSizePrimitiveObjectResolver()
		{
			Instance = new ForceSizePrimitiveObjectResolver();
			Options = new MessagePackSerializerOptions(Instance);
		}

		private ForceSizePrimitiveObjectResolver()
		{
		}

		public IMessagePackFormatter<T>? GetFormatter<T>()
		{
			return FormatterCache<T>.Formatter;
		}
	}
}
