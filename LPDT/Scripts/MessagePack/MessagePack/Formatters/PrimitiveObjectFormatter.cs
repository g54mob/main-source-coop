using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace MessagePack.Formatters
{
	public class PrimitiveObjectFormatter : IMessagePackFormatter<object?>, IMessagePackFormatter
	{
		public static readonly IMessagePackFormatter<object?> Instance = new PrimitiveObjectFormatter();

		private static readonly Dictionary<Type, int> TypeToJumpCode = new Dictionary<Type, int>
		{
			{
				typeof(bool),
				0
			},
			{
				typeof(char),
				1
			},
			{
				typeof(sbyte),
				2
			},
			{
				typeof(byte),
				3
			},
			{
				typeof(short),
				4
			},
			{
				typeof(ushort),
				5
			},
			{
				typeof(int),
				6
			},
			{
				typeof(uint),
				7
			},
			{
				typeof(long),
				8
			},
			{
				typeof(ulong),
				9
			},
			{
				typeof(float),
				10
			},
			{
				typeof(double),
				11
			},
			{
				typeof(DateTime),
				12
			},
			{
				typeof(string),
				13
			},
			{
				typeof(byte[]),
				14
			}
		};

		protected PrimitiveObjectFormatter()
		{
		}

		public static bool IsSupportedType(Type type, TypeInfo typeInfo, object value)
		{
			if (value == null)
			{
				return true;
			}
			if (TypeToJumpCode.ContainsKey(type))
			{
				return true;
			}
			if (typeInfo.IsEnum)
			{
				return true;
			}
			if (value is IDictionary)
			{
				return true;
			}
			if (value is ICollection)
			{
				return true;
			}
			return false;
		}

		public void Serialize(ref MessagePackWriter writer, object? value, MessagePackSerializerOptions options)
		{
			if (value == null)
			{
				writer.WriteNil();
				return;
			}
			Type type = value.GetType();
			if (TypeToJumpCode.TryGetValue(type, out var value2))
			{
				switch (value2)
				{
				case 0:
					writer.Write((bool)value);
					break;
				case 1:
					writer.Write((char)value);
					break;
				case 2:
					writer.WriteInt8((sbyte)value);
					break;
				case 3:
					writer.WriteUInt8((byte)value);
					break;
				case 4:
					writer.WriteInt16((short)value);
					break;
				case 5:
					writer.WriteUInt16((ushort)value);
					break;
				case 6:
					writer.WriteInt32((int)value);
					break;
				case 7:
					writer.WriteUInt32((uint)value);
					break;
				case 8:
					writer.WriteInt64((long)value);
					break;
				case 9:
					writer.WriteUInt64((ulong)value);
					break;
				case 10:
					writer.Write((float)value);
					break;
				case 11:
					writer.Write((double)value);
					break;
				case 12:
					writer.Write((DateTime)value);
					break;
				case 13:
					writer.Write((string)value);
					break;
				case 14:
					writer.Write((byte[])value);
					break;
				default:
					throw new MessagePackSerializationException("Not supported primitive object resolver. type:" + type.Name);
				}
				return;
			}
			if (type.GetTypeInfo().IsEnum)
			{
				Type underlyingType = Enum.GetUnderlyingType(type);
				switch (TypeToJumpCode[underlyingType])
				{
				case 2:
					writer.WriteInt8((sbyte)value);
					return;
				case 3:
					writer.WriteUInt8((byte)value);
					return;
				case 4:
					writer.WriteInt16((short)value);
					return;
				case 5:
					writer.WriteUInt16((ushort)value);
					return;
				case 6:
					writer.WriteInt32((int)value);
					return;
				case 7:
					writer.WriteUInt32((uint)value);
					return;
				case 8:
					writer.WriteInt64((long)value);
					return;
				case 9:
					writer.WriteUInt64((ulong)value);
					return;
				}
			}
			else
			{
				if (value is IDictionary dictionary)
				{
					writer.WriteMapHeader(dictionary.Count);
					{
						foreach (DictionaryEntry item in dictionary.GetEntryEnumerator())
						{
							Serialize(ref writer, item.Key, options);
							Serialize(ref writer, item.Value, options);
						}
						return;
					}
				}
				if (value is ICollection collection)
				{
					writer.WriteArrayHeader(collection.Count);
					{
						foreach (object item2 in collection)
						{
							Serialize(ref writer, item2, options);
						}
						return;
					}
				}
			}
			throw new MessagePackSerializationException("Not supported primitive object resolver. type:" + type.Name);
		}

		public object? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
		{
			checked
			{
				switch (reader.NextMessagePackType)
				{
				case MessagePackType.Integer:
				{
					byte nextCode = reader.NextCode;
					if (MessagePackCode.IsPositiveFixInt(nextCode))
					{
						return reader.ReadByte();
					}
					if (MessagePackCode.IsNegativeFixInt(nextCode))
					{
						return reader.ReadSByte();
					}
					return nextCode switch
					{
						208 => reader.ReadSByte(), 
						209 => reader.ReadInt16(), 
						210 => reader.ReadInt32(), 
						211 => reader.ReadInt64(), 
						204 => reader.ReadByte(), 
						205 => reader.ReadUInt16(), 
						206 => reader.ReadUInt32(), 
						207 => reader.ReadUInt64(), 
						_ => throw new MessagePackSerializationException(string.Format(CultureInfo.CurrentCulture, "Unrecognized primitive code 0x{0:x2} for integer type.", nextCode)), 
					};
				}
				case MessagePackType.Boolean:
					return reader.ReadBoolean();
				case MessagePackType.Float:
					if (reader.NextCode == 202)
					{
						return reader.ReadSingle();
					}
					return reader.ReadDouble();
				case MessagePackType.String:
				{
					IMessagePackFormatter<string> formatter = options.Resolver.GetFormatter<string>();
					if (formatter == null)
					{
						return reader.ReadString();
					}
					return formatter.Deserialize(ref reader, options);
				}
				case MessagePackType.Binary:
				{
					ReadOnlySequence<byte>? readOnlySequence = reader.ReadBytes();
					if (!readOnlySequence.HasValue)
					{
						return null;
					}
					return readOnlySequence.GetValueOrDefault().ToArray<byte>();
				}
				case MessagePackType.Extension:
				{
					ExtensionHeader header = reader.ReadExtensionFormatHeader();
					if (header.TypeCode == -1)
					{
						return reader.ReadDateTime(header);
					}
					throw new MessagePackSerializationException(string.Format(CultureInfo.CurrentCulture, "Extension type code 0x{0:x2} is not supported by the {1}.", header.TypeCode, "PrimitiveObjectFormatter"));
				}
				case MessagePackType.Array:
				{
					int num = reader.ReadArrayHeader();
					if (num == 0)
					{
						return Array.Empty<object>();
					}
					IMessagePackFormatter<object> formatterWithVerify = options.Resolver.GetFormatterWithVerify<object>();
					object[] array = new object[num];
					options.Security.DepthStep(ref reader);
					try
					{
						for (int i = 0; i < num; i++)
						{
							array[i] = formatterWithVerify.Deserialize(ref reader, options);
						}
						return array;
					}
					finally
					{
						reader.Depth--;
					}
				}
				case MessagePackType.Map:
				{
					int length = reader.ReadMapHeader();
					options.Security.DepthStep(ref reader);
					try
					{
						return DeserializeMap(ref reader, length, options);
					}
					finally
					{
						reader.Depth--;
					}
				}
				case MessagePackType.Nil:
					reader.ReadNil();
					return null;
				default:
					throw new MessagePackSerializationException(string.Format(CultureInfo.CurrentCulture, "Unrecognized code: 0x{0:X2}.", reader.NextCode));
				}
			}
		}

		protected virtual object DeserializeMap(ref MessagePackReader reader, int length, MessagePackSerializerOptions options)
		{
			IMessagePackFormatter<object> formatterWithVerify = options.Resolver.GetFormatterWithVerify<object>();
			Dictionary<object, object> dictionary = new Dictionary<object, object>(length, options.Security.GetEqualityComparer<object>());
			for (int i = 0; i < length; i = checked(i + 1))
			{
				object key = formatterWithVerify.Deserialize(ref reader, options);
				object value = formatterWithVerify.Deserialize(ref reader, options);
				dictionary.Add(key, value);
			}
			return dictionary;
		}
	}
}
