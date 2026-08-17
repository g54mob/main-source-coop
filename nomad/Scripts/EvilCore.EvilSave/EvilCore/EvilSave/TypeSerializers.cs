using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEngine;

namespace EvilCore.EvilSave
{
	internal static class TypeSerializers
	{
		private static readonly Dictionary<Type, EvilTypeCode> TypeToCode = new Dictionary<Type, EvilTypeCode>
		{
			{
				typeof(bool),
				EvilTypeCode.Bool
			},
			{
				typeof(byte),
				EvilTypeCode.Byte
			},
			{
				typeof(sbyte),
				EvilTypeCode.SByte
			},
			{
				typeof(short),
				EvilTypeCode.Short
			},
			{
				typeof(ushort),
				EvilTypeCode.UShort
			},
			{
				typeof(int),
				EvilTypeCode.Int
			},
			{
				typeof(uint),
				EvilTypeCode.UInt
			},
			{
				typeof(long),
				EvilTypeCode.Long
			},
			{
				typeof(ulong),
				EvilTypeCode.ULong
			},
			{
				typeof(float),
				EvilTypeCode.Float
			},
			{
				typeof(double),
				EvilTypeCode.Double
			},
			{
				typeof(decimal),
				EvilTypeCode.Decimal
			},
			{
				typeof(char),
				EvilTypeCode.Char
			},
			{
				typeof(string),
				EvilTypeCode.String
			},
			{
				typeof(DateTime),
				EvilTypeCode.DateTime
			},
			{
				typeof(byte[]),
				EvilTypeCode.ByteArray
			},
			{
				typeof(Vector2),
				EvilTypeCode.Vector2
			},
			{
				typeof(Vector3),
				EvilTypeCode.Vector3
			},
			{
				typeof(Vector4),
				EvilTypeCode.Vector4
			},
			{
				typeof(Vector2Int),
				EvilTypeCode.Vector2Int
			},
			{
				typeof(Vector3Int),
				EvilTypeCode.Vector3Int
			},
			{
				typeof(Quaternion),
				EvilTypeCode.Quaternion
			},
			{
				typeof(Color),
				EvilTypeCode.Color
			},
			{
				typeof(Color32),
				EvilTypeCode.Color32
			},
			{
				typeof(Rect),
				EvilTypeCode.Rect
			},
			{
				typeof(RectInt),
				EvilTypeCode.RectInt
			},
			{
				typeof(Bounds),
				EvilTypeCode.Bounds
			},
			{
				typeof(BoundsInt),
				EvilTypeCode.BoundsInt
			},
			{
				typeof(Matrix4x4),
				EvilTypeCode.Matrix4x4
			},
			{
				typeof(AnimationCurve),
				EvilTypeCode.AnimationCurve
			},
			{
				typeof(Gradient),
				EvilTypeCode.Gradient
			},
			{
				typeof(LayerMask),
				EvilTypeCode.LayerMask
			}
		};

		private static readonly Dictionary<Type, FieldInfo[]> FieldCache = new Dictionary<Type, FieldInfo[]>();

		public static EvilTypeCode GetEvilTypeCode(Type type)
		{
			if (type == null)
			{
				return EvilTypeCode.Null;
			}
			if (TypeToCode.TryGetValue(type, out var value))
			{
				return value;
			}
			if (type.IsEnum)
			{
				return EvilTypeCode.Enum;
			}
			if (type.IsArray)
			{
				return EvilTypeCode.Array;
			}
			if (type.IsGenericType)
			{
				Type genericTypeDefinition = type.GetGenericTypeDefinition();
				if (genericTypeDefinition == typeof(List<>))
				{
					return EvilTypeCode.List;
				}
				if (genericTypeDefinition == typeof(Dictionary<, >))
				{
					return EvilTypeCode.Dictionary;
				}
				if (genericTypeDefinition == typeof(HashSet<>))
				{
					return EvilTypeCode.HashSet;
				}
			}
			return EvilTypeCode.Reflected;
		}

		public static void WriteValue(BinaryWriter bw, object value, Type type)
		{
			if (value == null)
			{
				bw.Write((byte)0);
				return;
			}
			EvilTypeCode evilTypeCode = GetEvilTypeCode(type);
			bw.Write((byte)evilTypeCode);
			switch (evilTypeCode)
			{
			case EvilTypeCode.Bool:
				bw.Write((bool)value);
				break;
			case EvilTypeCode.Byte:
				bw.Write((byte)value);
				break;
			case EvilTypeCode.SByte:
				bw.Write((sbyte)value);
				break;
			case EvilTypeCode.Short:
				bw.Write((short)value);
				break;
			case EvilTypeCode.UShort:
				bw.Write((ushort)value);
				break;
			case EvilTypeCode.Int:
				bw.Write((int)value);
				break;
			case EvilTypeCode.UInt:
				bw.Write((uint)value);
				break;
			case EvilTypeCode.Long:
				bw.Write((long)value);
				break;
			case EvilTypeCode.ULong:
				bw.Write((ulong)value);
				break;
			case EvilTypeCode.Float:
				bw.Write((float)value);
				break;
			case EvilTypeCode.Double:
				bw.Write((double)value);
				break;
			case EvilTypeCode.Decimal:
				bw.Write((decimal)value);
				break;
			case EvilTypeCode.Char:
				bw.Write((char)value);
				break;
			case EvilTypeCode.String:
				bw.Write((string)value);
				break;
			case EvilTypeCode.DateTime:
				bw.Write(((DateTime)value).ToBinary());
				break;
			case EvilTypeCode.ByteArray:
			{
				byte[] array = (byte[])value;
				bw.Write(array.Length);
				bw.Write(array);
				break;
			}
			case EvilTypeCode.Enum:
				WriteEnum(bw, value, type);
				break;
			case EvilTypeCode.Vector2:
			{
				Vector2 vector3 = (Vector2)value;
				bw.Write(vector3.x);
				bw.Write(vector3.y);
				break;
			}
			case EvilTypeCode.Vector3:
			{
				Vector3 vector2 = (Vector3)value;
				bw.Write(vector2.x);
				bw.Write(vector2.y);
				bw.Write(vector2.z);
				break;
			}
			case EvilTypeCode.Vector4:
			{
				Vector4 vector = (Vector4)value;
				bw.Write(vector.x);
				bw.Write(vector.y);
				bw.Write(vector.z);
				bw.Write(vector.w);
				break;
			}
			case EvilTypeCode.Vector2Int:
			{
				Vector2Int vector2Int = (Vector2Int)value;
				bw.Write(vector2Int.x);
				bw.Write(vector2Int.y);
				break;
			}
			case EvilTypeCode.Vector3Int:
			{
				Vector3Int vector3Int = (Vector3Int)value;
				bw.Write(vector3Int.x);
				bw.Write(vector3Int.y);
				bw.Write(vector3Int.z);
				break;
			}
			case EvilTypeCode.Quaternion:
			{
				Quaternion quaternion = (Quaternion)value;
				bw.Write(quaternion.x);
				bw.Write(quaternion.y);
				bw.Write(quaternion.z);
				bw.Write(quaternion.w);
				break;
			}
			case EvilTypeCode.Color:
			{
				Color color2 = (Color)value;
				bw.Write(color2.r);
				bw.Write(color2.g);
				bw.Write(color2.b);
				bw.Write(color2.a);
				break;
			}
			case EvilTypeCode.Color32:
			{
				Color32 color = (Color32)value;
				bw.Write(color.r);
				bw.Write(color.g);
				bw.Write(color.b);
				bw.Write(color.a);
				break;
			}
			case EvilTypeCode.Rect:
			{
				Rect rect = (Rect)value;
				bw.Write(rect.x);
				bw.Write(rect.y);
				bw.Write(rect.width);
				bw.Write(rect.height);
				break;
			}
			case EvilTypeCode.RectInt:
			{
				RectInt rectInt = (RectInt)value;
				bw.Write(rectInt.x);
				bw.Write(rectInt.y);
				bw.Write(rectInt.width);
				bw.Write(rectInt.height);
				break;
			}
			case EvilTypeCode.Bounds:
			{
				Bounds bounds = (Bounds)value;
				WriteVector3(bw, bounds.center);
				WriteVector3(bw, bounds.size);
				break;
			}
			case EvilTypeCode.BoundsInt:
			{
				BoundsInt boundsInt = (BoundsInt)value;
				bw.Write(boundsInt.position.x);
				bw.Write(boundsInt.position.y);
				bw.Write(boundsInt.position.z);
				bw.Write(boundsInt.size.x);
				bw.Write(boundsInt.size.y);
				bw.Write(boundsInt.size.z);
				break;
			}
			case EvilTypeCode.Matrix4x4:
			{
				Matrix4x4 matrix4x = (Matrix4x4)value;
				for (int i = 0; i < 16; i++)
				{
					bw.Write(matrix4x[i]);
				}
				break;
			}
			case EvilTypeCode.AnimationCurve:
				WriteAnimationCurve(bw, (AnimationCurve)value);
				break;
			case EvilTypeCode.Gradient:
				WriteGradient(bw, (Gradient)value);
				break;
			case EvilTypeCode.LayerMask:
				bw.Write(((LayerMask)value).value);
				break;
			case EvilTypeCode.Array:
				WriteArray(bw, value, type);
				break;
			case EvilTypeCode.List:
				WriteList(bw, value, type);
				break;
			case EvilTypeCode.Dictionary:
				WriteDictionary(bw, value, type);
				break;
			case EvilTypeCode.HashSet:
				WriteHashSet(bw, value, type);
				break;
			case EvilTypeCode.Reflected:
				WriteReflected(bw, value, type);
				break;
			}
		}

		public static object ReadValue(BinaryReader br, Type expectedType)
		{
			EvilTypeCode evilTypeCode = (EvilTypeCode)br.ReadByte();
			return evilTypeCode switch
			{
				EvilTypeCode.Null => null, 
				EvilTypeCode.Bool => br.ReadBoolean(), 
				EvilTypeCode.Byte => br.ReadByte(), 
				EvilTypeCode.SByte => br.ReadSByte(), 
				EvilTypeCode.Short => br.ReadInt16(), 
				EvilTypeCode.UShort => br.ReadUInt16(), 
				EvilTypeCode.Int => br.ReadInt32(), 
				EvilTypeCode.UInt => br.ReadUInt32(), 
				EvilTypeCode.Long => br.ReadInt64(), 
				EvilTypeCode.ULong => br.ReadUInt64(), 
				EvilTypeCode.Float => br.ReadSingle(), 
				EvilTypeCode.Double => br.ReadDouble(), 
				EvilTypeCode.Decimal => br.ReadDecimal(), 
				EvilTypeCode.Char => br.ReadChar(), 
				EvilTypeCode.String => br.ReadString(), 
				EvilTypeCode.DateTime => DateTime.FromBinary(br.ReadInt64()), 
				EvilTypeCode.ByteArray => ReadByteArray(br), 
				EvilTypeCode.Enum => ReadEnum(br, expectedType), 
				EvilTypeCode.Vector2 => new Vector2(br.ReadSingle(), br.ReadSingle()), 
				EvilTypeCode.Vector3 => new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()), 
				EvilTypeCode.Vector4 => new Vector4(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle()), 
				EvilTypeCode.Vector2Int => new Vector2Int(br.ReadInt32(), br.ReadInt32()), 
				EvilTypeCode.Vector3Int => new Vector3Int(br.ReadInt32(), br.ReadInt32(), br.ReadInt32()), 
				EvilTypeCode.Quaternion => new Quaternion(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle()), 
				EvilTypeCode.Color => new Color(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle()), 
				EvilTypeCode.Color32 => new Color32(br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte()), 
				EvilTypeCode.Rect => new Rect(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle()), 
				EvilTypeCode.RectInt => new RectInt(br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), br.ReadInt32()), 
				EvilTypeCode.Bounds => new Bounds(ReadVector3(br), ReadVector3(br)), 
				EvilTypeCode.BoundsInt => new BoundsInt(new Vector3Int(br.ReadInt32(), br.ReadInt32(), br.ReadInt32()), new Vector3Int(br.ReadInt32(), br.ReadInt32(), br.ReadInt32())), 
				EvilTypeCode.Matrix4x4 => ReadMatrix4x4(br), 
				EvilTypeCode.AnimationCurve => ReadAnimationCurve(br), 
				EvilTypeCode.Gradient => ReadGradient(br), 
				EvilTypeCode.LayerMask => (LayerMask)br.ReadInt32(), 
				EvilTypeCode.Array => ReadArray(br, expectedType), 
				EvilTypeCode.List => ReadList(br, expectedType), 
				EvilTypeCode.Dictionary => ReadDictionary(br, expectedType), 
				EvilTypeCode.HashSet => ReadHashSet(br, expectedType), 
				EvilTypeCode.Reflected => ReadReflected(br, expectedType), 
				_ => throw new InvalidOperationException($"Unknown type code: {evilTypeCode}"), 
			};
		}

		private static void WriteVector3(BinaryWriter bw, Vector3 v)
		{
			bw.Write(v.x);
			bw.Write(v.y);
			bw.Write(v.z);
		}

		private static Vector3 ReadVector3(BinaryReader br)
		{
			return new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
		}

		private static void WriteEnum(BinaryWriter bw, object value, Type type)
		{
			Type underlyingType = Enum.GetUnderlyingType(type);
			bw.Write(underlyingType.FullName ?? underlyingType.Name);
			if (underlyingType == typeof(byte))
			{
				bw.Write((byte)Convert.ChangeType(value, typeof(byte)));
			}
			else if (underlyingType == typeof(int))
			{
				bw.Write((int)Convert.ChangeType(value, typeof(int)));
			}
			else if (underlyingType == typeof(short))
			{
				bw.Write((short)Convert.ChangeType(value, typeof(short)));
			}
			else if (underlyingType == typeof(long))
			{
				bw.Write((long)Convert.ChangeType(value, typeof(long)));
			}
			else
			{
				bw.Write((int)Convert.ChangeType(value, typeof(int)));
			}
		}

		private static object ReadEnum(BinaryReader br, Type expectedType)
		{
			string text = br.ReadString();
			object obj = ((text == typeof(byte).FullName) ? ((object)br.ReadByte()) : ((text == typeof(short).FullName) ? ((object)br.ReadInt16()) : ((!(text == typeof(long).FullName)) ? ((object)br.ReadInt32()) : ((object)br.ReadInt64()))));
			if (expectedType != null && expectedType.IsEnum)
			{
				return Enum.ToObject(expectedType, obj);
			}
			return obj;
		}

		private static byte[] ReadByteArray(BinaryReader br)
		{
			int count = br.ReadInt32();
			return br.ReadBytes(count);
		}

		private static void WriteAnimationCurve(BinaryWriter bw, AnimationCurve curve)
		{
			bw.Write((byte)curve.preWrapMode);
			bw.Write((byte)curve.postWrapMode);
			bw.Write(curve.keys.Length);
			Keyframe[] keys = curve.keys;
			for (int i = 0; i < keys.Length; i++)
			{
				Keyframe keyframe = keys[i];
				bw.Write(keyframe.time);
				bw.Write(keyframe.value);
				bw.Write(keyframe.inTangent);
				bw.Write(keyframe.outTangent);
				bw.Write(keyframe.inWeight);
				bw.Write(keyframe.outWeight);
				bw.Write((byte)keyframe.weightedMode);
			}
		}

		private static AnimationCurve ReadAnimationCurve(BinaryReader br)
		{
			WrapMode preWrapMode = (WrapMode)br.ReadByte();
			WrapMode postWrapMode = (WrapMode)br.ReadByte();
			int num = br.ReadInt32();
			Keyframe[] array = new Keyframe[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = new Keyframe
				{
					time = br.ReadSingle(),
					value = br.ReadSingle(),
					inTangent = br.ReadSingle(),
					outTangent = br.ReadSingle(),
					inWeight = br.ReadSingle(),
					outWeight = br.ReadSingle(),
					weightedMode = (WeightedMode)br.ReadByte()
				};
			}
			return new AnimationCurve(array)
			{
				preWrapMode = preWrapMode,
				postWrapMode = postWrapMode
			};
		}

		private static void WriteGradient(BinaryWriter bw, Gradient gradient)
		{
			bw.Write((byte)gradient.mode);
			GradientColorKey[] colorKeys = gradient.colorKeys;
			bw.Write(colorKeys.Length);
			GradientColorKey[] array = colorKeys;
			for (int i = 0; i < array.Length; i++)
			{
				GradientColorKey gradientColorKey = array[i];
				bw.Write(gradientColorKey.color.r);
				bw.Write(gradientColorKey.color.g);
				bw.Write(gradientColorKey.color.b);
				bw.Write(gradientColorKey.time);
			}
			GradientAlphaKey[] alphaKeys = gradient.alphaKeys;
			bw.Write(alphaKeys.Length);
			GradientAlphaKey[] array2 = alphaKeys;
			for (int i = 0; i < array2.Length; i++)
			{
				GradientAlphaKey gradientAlphaKey = array2[i];
				bw.Write(gradientAlphaKey.alpha);
				bw.Write(gradientAlphaKey.time);
			}
		}

		private static Gradient ReadGradient(BinaryReader br)
		{
			GradientMode mode = (GradientMode)br.ReadByte();
			int num = br.ReadInt32();
			GradientColorKey[] array = new GradientColorKey[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = new GradientColorKey(new Color(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()), br.ReadSingle());
			}
			int num2 = br.ReadInt32();
			GradientAlphaKey[] array2 = new GradientAlphaKey[num2];
			for (int j = 0; j < num2; j++)
			{
				array2[j] = new GradientAlphaKey(br.ReadSingle(), br.ReadSingle());
			}
			Gradient gradient = new Gradient();
			gradient.mode = mode;
			gradient.SetKeys(array, array2);
			return gradient;
		}

		private static Matrix4x4 ReadMatrix4x4(BinaryReader br)
		{
			Matrix4x4 result = default(Matrix4x4);
			for (int i = 0; i < 16; i++)
			{
				result[i] = br.ReadSingle();
			}
			return result;
		}

		private static void WriteArray(BinaryWriter bw, object value, Type type)
		{
			Type elementType = type.GetElementType();
			bw.Write(elementType.AssemblyQualifiedName ?? elementType.FullName ?? elementType.Name);
			Array array = (Array)value;
			bw.Write(array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				WriteValue(bw, array.GetValue(i), elementType);
			}
		}

		private static object ReadArray(BinaryReader br, Type expectedType)
		{
			string text = br.ReadString();
			int num = br.ReadInt32();
			Type type = expectedType?.GetElementType() ?? Type.GetType(text);
			if (type == null)
			{
				throw new InvalidOperationException("Cannot resolve array element type: " + text);
			}
			Array array = Array.CreateInstance(type, num);
			for (int i = 0; i < num; i++)
			{
				array.SetValue(ReadValue(br, type), i);
			}
			return array;
		}

		private static void WriteList(BinaryWriter bw, object value, Type type)
		{
			Type type2 = type.GetGenericArguments()[0];
			bw.Write(type2.AssemblyQualifiedName ?? type2.FullName ?? type2.Name);
			IList list = (IList)value;
			bw.Write(list.Count);
			foreach (object item in list)
			{
				WriteValue(bw, item, type2);
			}
		}

		private static object ReadList(BinaryReader br, Type expectedType)
		{
			string text = br.ReadString();
			int num = br.ReadInt32();
			Type type = (((object)expectedType != null) ? expectedType.GetGenericArguments()[0] : null) ?? Type.GetType(text);
			if (type == null)
			{
				throw new InvalidOperationException("Cannot resolve list element type: " + text);
			}
			IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type), num);
			for (int i = 0; i < num; i++)
			{
				list.Add(ReadValue(br, type));
			}
			return list;
		}

		private static void WriteDictionary(BinaryWriter bw, object value, Type type)
		{
			Type[] genericArguments = type.GetGenericArguments();
			bw.Write(genericArguments[0].AssemblyQualifiedName ?? genericArguments[0].FullName ?? genericArguments[0].Name);
			bw.Write(genericArguments[1].AssemblyQualifiedName ?? genericArguments[1].FullName ?? genericArguments[1].Name);
			IDictionary dictionary = (IDictionary)value;
			bw.Write(dictionary.Count);
			foreach (DictionaryEntry item in dictionary)
			{
				WriteValue(bw, item.Key, genericArguments[0]);
				WriteValue(bw, item.Value, genericArguments[1]);
			}
		}

		private static object ReadDictionary(BinaryReader br, Type expectedType)
		{
			string text = br.ReadString();
			string text2 = br.ReadString();
			int num = br.ReadInt32();
			Type type;
			Type type2;
			if (expectedType != null && expectedType.IsGenericType)
			{
				Type[] genericArguments = expectedType.GetGenericArguments();
				type = genericArguments[0];
				type2 = genericArguments[1];
			}
			else
			{
				type = Type.GetType(text);
				type2 = Type.GetType(text2);
			}
			if (type == null || type2 == null)
			{
				throw new InvalidOperationException("Cannot resolve dictionary types: " + text + ", " + text2);
			}
			IDictionary dictionary = (IDictionary)Activator.CreateInstance(typeof(Dictionary<, >).MakeGenericType(type, type2), num);
			for (int i = 0; i < num; i++)
			{
				object key = ReadValue(br, type);
				object value = ReadValue(br, type2);
				dictionary[key] = value;
			}
			return dictionary;
		}

		private static void WriteHashSet(BinaryWriter bw, object value, Type type)
		{
			Type type2 = type.GetGenericArguments()[0];
			bw.Write(type2.AssemblyQualifiedName ?? type2.FullName ?? type2.Name);
			IEnumerable enumerable = (IEnumerable)value;
			int num = 0;
			foreach (object item in enumerable)
			{
				_ = item;
				num++;
			}
			bw.Write(num);
			foreach (object item2 in enumerable)
			{
				WriteValue(bw, item2, type2);
			}
		}

		private static object ReadHashSet(BinaryReader br, Type expectedType)
		{
			string text = br.ReadString();
			int num = br.ReadInt32();
			Type type = (((object)expectedType != null) ? expectedType.GetGenericArguments()[0] : null) ?? Type.GetType(text);
			if (type == null)
			{
				throw new InvalidOperationException("Cannot resolve hashset element type: " + text);
			}
			Type type2 = typeof(HashSet<>).MakeGenericType(type);
			object obj = Activator.CreateInstance(type2);
			MethodInfo method = type2.GetMethod("Add");
			for (int i = 0; i < num; i++)
			{
				object obj2 = ReadValue(br, type);
				method.Invoke(obj, new object[1] { obj2 });
			}
			return obj;
		}

		private static void WriteReflected(BinaryWriter bw, object value, Type type)
		{
			bw.Write(type.AssemblyQualifiedName ?? type.FullName ?? type.Name);
			FieldInfo[] cachedFields = GetCachedFields(type);
			bw.Write(cachedFields.Length);
			FieldInfo[] array = cachedFields;
			foreach (FieldInfo fieldInfo in array)
			{
				bw.Write(fieldInfo.Name);
				WriteValue(bw, fieldInfo.GetValue(value), fieldInfo.FieldType);
			}
		}

		private static object ReadReflected(BinaryReader br, Type expectedType)
		{
			string text = br.ReadString();
			Type obj = expectedType ?? Type.GetType(text);
			if (obj == null)
			{
				throw new InvalidOperationException("Cannot resolve reflected type: " + text);
			}
			object obj2 = Activator.CreateInstance(obj);
			int num = br.ReadInt32();
			Dictionary<string, FieldInfo> dictionary = new Dictionary<string, FieldInfo>();
			FieldInfo[] cachedFields = GetCachedFields(obj);
			foreach (FieldInfo fieldInfo in cachedFields)
			{
				dictionary[fieldInfo.Name] = fieldInfo;
			}
			for (int j = 0; j < num; j++)
			{
				string key = br.ReadString();
				if (dictionary.TryGetValue(key, out var value))
				{
					object value2 = ReadValue(br, value.FieldType);
					value.SetValue(obj2, value2);
				}
				else
				{
					ReadValue(br, null);
				}
			}
			return obj2;
		}

		private static FieldInfo[] GetCachedFields(Type type)
		{
			if (FieldCache.TryGetValue(type, out var value))
			{
				return value;
			}
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			List<FieldInfo> list = new List<FieldInfo>();
			FieldInfo[] array = fields;
			foreach (FieldInfo fieldInfo in array)
			{
				if (!fieldInfo.IsNotSerialized && (fieldInfo.IsPublic || fieldInfo.GetCustomAttribute<SerializeField>() != null || fieldInfo.GetCustomAttribute<DataMemberAttribute>() != null))
				{
					list.Add(fieldInfo);
				}
			}
			FieldInfo[] array2 = list.ToArray();
			FieldCache[type] = array2;
			return array2;
		}
	}
}
