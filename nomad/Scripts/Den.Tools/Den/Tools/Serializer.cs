using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Den.Tools
{
	[Serializable]
	public static class Serializer
	{
		public interface IAlternativeType
		{
			Type AlternativeSerializationType { get; }
		}

		public interface ICustomSerialization
		{
			void PostprocessAfterSerialize(Object serObj, Dictionary<object, Object> allSerialized);

			void PreprocessBeforeDeserialize(Object serObj, Object[] allSerialized, object[] allDeserialized);
		}

		public sealed class NoCopyAttribute : Attribute
		{
		}

		[Serializable]
		[StructLayout(LayoutKind.Explicit, Size = 16)]
		public struct Value
		{
			[FieldOffset(0)]
			[SerializeField]
			public byte t;

			[FieldOffset(8)]
			[SerializeField]
			private long v;

			[FieldOffset(8)]
			[NonSerialized]
			private bool boolVal;

			[FieldOffset(8)]
			[NonSerialized]
			private byte byteVal;

			[FieldOffset(8)]
			[NonSerialized]
			private sbyte sbyteVal;

			[FieldOffset(8)]
			[NonSerialized]
			private char charVal;

			[FieldOffset(8)]
			[NonSerialized]
			private decimal decimalVal;

			[FieldOffset(8)]
			[NonSerialized]
			private double doubleVal;

			[FieldOffset(8)]
			[NonSerialized]
			private float floatVal;

			[FieldOffset(8)]
			[NonSerialized]
			private int intVal;

			[FieldOffset(8)]
			[NonSerialized]
			private uint uintVal;

			[FieldOffset(8)]
			[NonSerialized]
			private long longVal;

			[FieldOffset(8)]
			[NonSerialized]
			private ulong ulongVal;

			[FieldOffset(8)]
			[NonSerialized]
			private short shortVal;

			[FieldOffset(8)]
			[NonSerialized]
			private ushort ushortVal;

			[FieldOffset(8)]
			[NonSerialized]
			private int reference;

			public static implicit operator Value(int i)
			{
				return new Value
				{
					t = 255,
					intVal = i
				};
			}

			public static implicit operator int(Value v)
			{
				if (v.t != 255)
				{
					throw new Exception("Value t:" + v.t + " v:" + v.v + " is used like a reference");
				}
				return v.intVal;
			}

			public void Set(object obj, Type type)
			{
				if (type == typeof(int))
				{
					intVal = (int)obj;
					t = 1;
					return;
				}
				if (type == typeof(float))
				{
					floatVal = (float)obj;
					t = 2;
					return;
				}
				if (type == typeof(bool))
				{
					boolVal = (bool)obj;
					t = 3;
					return;
				}
				if (type == typeof(byte))
				{
					byteVal = (byte)obj;
					t = 4;
					return;
				}
				if (type == typeof(char))
				{
					charVal = (char)obj;
					t = 5;
					return;
				}
				if (type == typeof(uint))
				{
					uintVal = (uint)obj;
					t = 6;
					return;
				}
				if (type == typeof(sbyte))
				{
					sbyteVal = (sbyte)obj;
					t = 7;
					return;
				}
				if (type == typeof(decimal))
				{
					decimalVal = (decimal)obj;
					t = 8;
					return;
				}
				if (type == typeof(double))
				{
					doubleVal = (double)obj;
					t = 9;
					return;
				}
				if (type == typeof(long))
				{
					longVal = (long)obj;
					t = 10;
					return;
				}
				if (type == typeof(ulong))
				{
					ulongVal = (ulong)obj;
					t = 11;
					return;
				}
				if (type == typeof(short))
				{
					shortVal = (short)obj;
					t = 12;
					return;
				}
				if (type == typeof(ushort))
				{
					ushortVal = (ushort)obj;
					t = 13;
					return;
				}
				throw new Exception("Could not set value for " + type);
			}

			public object Get(Type type)
			{
				if (type == typeof(bool))
				{
					return boolVal;
				}
				if (type == typeof(byte))
				{
					return byteVal;
				}
				if (type == typeof(sbyte))
				{
					return sbyteVal;
				}
				if (type == typeof(char))
				{
					return charVal;
				}
				if (type == typeof(decimal))
				{
					return decimalVal;
				}
				if (type == typeof(double))
				{
					return doubleVal;
				}
				if (type == typeof(float))
				{
					return floatVal;
				}
				if (type == typeof(int))
				{
					return intVal;
				}
				if (type == typeof(uint))
				{
					return uintVal;
				}
				if (type == typeof(long))
				{
					return longVal;
				}
				if (type == typeof(ulong))
				{
					return ulongVal;
				}
				if (type == typeof(short))
				{
					return shortVal;
				}
				if (type == typeof(ushort))
				{
					return ushortVal;
				}
				throw new Exception("Could not get value for " + type);
			}

			public object Get()
			{
				return t switch
				{
					1 => intVal, 
					2 => floatVal, 
					3 => boolVal, 
					4 => byteVal, 
					5 => charVal, 
					6 => uintVal, 
					7 => sbyteVal, 
					8 => decimalVal, 
					9 => doubleVal, 
					10 => longVal, 
					11 => ulongVal, 
					12 => shortVal, 
					13 => ushortVal, 
					_ => throw new Exception("Could not get value for " + t), 
				};
			}

			public bool CheckType(Type type)
			{
				if (t == 0)
				{
					return false;
				}
				return t switch
				{
					1 => type == typeof(int), 
					2 => type == typeof(float), 
					3 => type == typeof(bool), 
					4 => type == typeof(byte), 
					5 => type == typeof(char), 
					6 => type == typeof(uint), 
					7 => type == typeof(sbyte), 
					8 => type == typeof(decimal), 
					9 => type == typeof(double), 
					10 => type == typeof(long), 
					11 => type == typeof(ulong), 
					12 => type == typeof(short), 
					13 => type == typeof(ushort), 
					255 => !type.IsPrimitive, 
					_ => throw new Exception("Could not get value for " + t), 
				};
			}

			public override bool Equals(object obj)
			{
				if (!(obj is Value))
				{
					return false;
				}
				if (t == ((Value)obj).t)
				{
					return v == ((Value)obj).v;
				}
				return false;
			}

			public override int GetHashCode()
			{
				return base.GetHashCode();
			}
		}

		[Serializable]
		public class Object
		{
			public int refId = -1;

			public string type;

			public string altType;

			public string[] fields;

			public Value[] values;

			public string special;

			public UnityEngine.Object uniObj;

			public static Object Null => new Object
			{
				refId = -1,
				type = null
			};
		}

		public enum ClassMatch
		{
			None = 0,
			ShouldMatch = 1,
			ShouldDiffer = 2
		}

		public static Object[] Serialize(object obj, Action<object, Object> onAfterSerialize = null)
		{
			Dictionary<object, Object> dictionary = new Dictionary<object, Object>();
			SerializeObject(obj, dictionary, skipNoCopyAttribute: false, onAfterSerialize);
			Object[] serialized = null;
			CopyObjectsToArray(dictionary, ref serialized);
			return serialized;
		}

		public static object Deserialize(Object[] serialized, Action<Object> onBeforeDeserialize = null)
		{
			if (serialized.Length == 0)
			{
				return null;
			}
			object[] deserialized = new object[serialized.Length];
			return DeserializeObject(0, serialized, deserialized, null, onBeforeDeserialize);
		}

		public static object DeepCopy(object obj)
		{
			Dictionary<object, Object> dictionary = new Dictionary<object, Object>();
			SerializeObject(obj, dictionary);
			Object[] serialized = null;
			CopyObjectsToArray(dictionary, ref serialized);
			object[] deserialized = new object[serialized.Length];
			return DeserializeObject(0, serialized, deserialized);
		}

		public static object DeepCopyPreservingRefs(object obj, Dictionary<object, object> srcDstRefs)
		{
			Dictionary<object, Object> dictionary = new Dictionary<object, Object>();
			SerializeObject(obj, dictionary);
			object[] array = new object[dictionary.Count];
			Object[] array2 = new Object[dictionary.Count];
			object[] array3 = new object[dictionary.Count];
			foreach (KeyValuePair<object, Object> item in dictionary)
			{
				object key = item.Key;
				Object value = item.Value;
				srcDstRefs.TryGetValue(key, out var value2);
				if (array2[value.refId] != null)
				{
					throw new Exception("Objects with the same id: " + value.refId);
				}
				array[value.refId] = key;
				array2[value.refId] = value;
				array3[value.refId] = value2;
			}
			object[] array4 = new object[array2.Length];
			object result = DeserializeObject(0, array2, array4, array3);
			srcDstRefs.Clear();
			for (int i = 0; i < array.Length; i++)
			{
				if (!srcDstRefs.ContainsKey(array[i]))
				{
					srcDstRefs.Add(array[i], array4[i]);
				}
				else if (srcDstRefs[array[i]] != array4[i])
				{
					throw new Exception("Deserialized and srcDstRefs mismatch");
				}
			}
			return result;
		}

		private static void CopyObjectsToArray(Dictionary<object, Object> serializedDict, ref Object[] serialized)
		{
			if (serialized == null || serialized.Length != serializedDict.Count)
			{
				serialized = new Object[serializedDict.Count];
			}
			else
			{
				for (int i = 0; i < serialized.Length; i++)
				{
					serialized[i] = null;
				}
			}
			foreach (KeyValuePair<object, Object> item in serializedDict)
			{
				Object value = item.Value;
				if (serialized[value.refId] != null)
				{
					throw new Exception("Objects with the same id: " + value.refId);
				}
				serialized[value.refId] = value;
			}
		}

		private static Object SerializeObject(object obj, Dictionary<object, Object> serialized, bool skipNoCopyAttribute = false, Action<object, Object> onAfterSerialize = null)
		{
			if (obj == null)
			{
				return Object.Null;
			}
			if (serialized.TryGetValue(obj, out var value))
			{
				return value;
			}
			value = new Object();
			serialized.Add(obj, value);
			value.refId = serialized.Count - 1;
			Type type = obj.GetType();
			value.type = type.AssemblyQualifiedName;
			if (type == typeof(string))
			{
				value.special = (string)obj;
			}
			if (type == typeof(Guid))
			{
				value.special = ((Guid)obj/*cast due to .constrained prefix*/).ToString();
			}
			else if (type.IsSubclassOf(typeof(UnityEngine.Object)))
			{
				value.uniObj = (UnityEngine.Object)obj;
			}
			else
			{
				if (type == typeof(Object[]))
				{
					throw new Exception("Serializer is trying to serialize serializer objects. This is causing infinite loop.");
				}
				if (type.IsSubclassOf(typeof(MemberInfo)))
				{
					if (type.IsSubclassOf(typeof(Type)))
					{
						value.special = ((Type)obj).AssemblyQualifiedName;
					}
					else
					{
						MemberInfo memberInfo = (MemberInfo)obj;
						value.special = memberInfo.Name + ", " + memberInfo.DeclaringType.AssemblyQualifiedName;
					}
				}
				else if (type.IsPrimitive)
				{
					value.values = new Value[1];
					value.values[0].Set(obj, type);
				}
				else if (type == typeof(AnimationCurve))
				{
					AnimationCurve animationCurve = (AnimationCurve)obj;
					value.values = new Value[3];
					value.values[0] = SerializeObject(animationCurve.keys, serialized, skipNoCopyAttribute: false, onAfterSerialize).refId;
					value.values[1].Set((int)animationCurve.preWrapMode, typeof(int));
					value.values[2].Set((int)animationCurve.postWrapMode, typeof(int));
				}
				else if (type.IsArray)
				{
					Array array = (Array)obj;
					value.fields = null;
					value.values = new Value[array.Length];
					Type elementType = type.GetElementType();
					bool isPrimitive = elementType.IsPrimitive;
					for (int i = 0; i < array.Length; i++)
					{
						object value2 = array.GetValue(i);
						Value value3 = default(Value);
						if (isPrimitive)
						{
							value3.Set(value2, elementType);
						}
						else
						{
							value3 = SerializeObject(value2, serialized, skipNoCopyAttribute: false, onAfterSerialize).refId;
						}
						value.values[i] = value3;
					}
				}
				else if (type == typeof(UnityEngine.Object))
				{
					if ((IntPtr)type.GetField("m_CachedPtr", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(obj) == IntPtr.Zero)
					{
						return Object.Null;
					}
				}
				else
				{
					if (obj is ISerializationCallbackReceiver)
					{
						((ISerializationCallbackReceiver)obj).OnBeforeSerialize();
					}
					if (obj is IAlternativeType alternativeType)
					{
						value.altType = alternativeType.AlternativeSerializationType.AssemblyQualifiedName;
					}
					FieldInfo[] array2 = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (obj is IList || obj is IDictionary)
					{
						ArrayTools.Append(ref array2, type.BaseType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
					}
					List<FieldInfo> list = new List<FieldInfo>();
					foreach (FieldInfo fieldInfo in array2)
					{
						if (!fieldInfo.IsLiteral && !fieldInfo.FieldType.IsPointer && !fieldInfo.IsNotSerialized && (!skipNoCopyAttribute || fieldInfo.GetCustomAttributes(typeof(NoCopyAttribute), inherit: false).Length == 0))
						{
							list.Add(fieldInfo);
						}
					}
					int count = list.Count;
					value.fields = new string[count];
					value.values = new Value[count];
					for (int k = 0; k < count; k++)
					{
						FieldInfo fieldInfo2 = list[k];
						value.fields[k] = fieldInfo2.Name;
						object value4 = fieldInfo2.GetValue(obj);
						Value value5 = default(Value);
						if (fieldInfo2.FieldType.IsPrimitive)
						{
							value5.Set(value4, fieldInfo2.FieldType);
						}
						else
						{
							value5 = SerializeObject(value4, serialized, skipNoCopyAttribute: false, onAfterSerialize).refId;
						}
						value.values[k] = value5;
					}
					if (obj is ICustomSerialization customSerialization)
					{
						customSerialization.PostprocessAfterSerialize(value, serialized);
					}
				}
			}
			onAfterSerialize?.Invoke(obj, value);
			return value;
		}

		private static object DeserializeObject(int refId, Object[] serialized, object[] deserialized, object[] reuse = null, Action<Object> onBeforeDeserialize = null)
		{
			if (refId < 0)
			{
				return null;
			}
			if (deserialized[refId] != null)
			{
				return deserialized[refId];
			}
			Object obj = serialized[refId];
			onBeforeDeserialize?.Invoke(obj);
			if (obj.type == null || obj.refId < 0)
			{
				return null;
			}
			Type type = Type.GetType(obj.type);
			if (type == null && obj.altType != null)
			{
				type = Type.GetType(obj.altType);
			}
			if (type == null && obj.type.Contains(","))
			{
				string text = obj.type.Substring(0, obj.type.IndexOf(','));
				type = Type.GetType(text);
				if (type == null)
				{
					Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
					for (int i = 0; i < assemblies.Length; i++)
					{
						type = assemblies[i].GetType(text);
						if (type != null)
						{
							break;
						}
					}
				}
			}
			if (type == typeof(string))
			{
				return obj.special;
			}
			if (type == typeof(Guid) && obj.special.Length != 0)
			{
				return Guid.Parse(obj.special);
			}
			if (type == null)
			{
				throw new Exception("Could not find type for: " + obj.type);
			}
			if (type.IsSubclassOf(typeof(UnityEngine.Object)))
			{
				if (obj.uniObj.GetType() == typeof(UnityEngine.Object))
				{
					return null;
				}
				return obj.uniObj;
			}
			if (type.IsSubclassOf(typeof(MemberInfo)))
			{
				if (type.IsSubclassOf(typeof(Type)))
				{
					return Type.GetType(obj.special);
				}
				int num = obj.special.IndexOf(", ");
				string name = obj.special.Substring(0, num);
				return Type.GetType(obj.special.Substring(num + 2)).GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			}
			if (type.IsPrimitive)
			{
				return obj.values[0].Get();
			}
			if (type == typeof(AnimationCurve))
			{
				AnimationCurve animationCurve = (AnimationCurve)(deserialized[refId] = ((reuse != null && reuse[refId] != null) ? ((AnimationCurve)reuse[refId]) : new AnimationCurve()));
				animationCurve.keys = (Keyframe[])DeserializeObject(obj.values[0], serialized, deserialized, reuse, onBeforeDeserialize);
				animationCurve.preWrapMode = (WrapMode)obj.values[1].Get();
				animationCurve.postWrapMode = (WrapMode)obj.values[2].Get();
				return animationCurve;
			}
			if (type.IsArray)
			{
				Type elementType = type.GetElementType();
				bool isPrimitive = elementType.IsPrimitive;
				if (obj.values.Length != 0 && !obj.values[0].CheckType(elementType))
				{
					throw new Exception("Value was saved as a reference, but loading as a primitive (or vice versa)");
				}
				Array array = (Array)(deserialized[refId] = ((reuse != null && reuse[refId] != null) ? ((Array)reuse[refId]) : ((Array)Activator.CreateInstance(type, obj.values.Length))));
				for (int j = 0; j < array.Length; j++)
				{
					Value value = obj.values[j];
					if (isPrimitive)
					{
						array.SetValue(value.Get(), j);
					}
					else if ((int)value >= 0)
					{
						object value2 = DeserializeObject(value, serialized, deserialized, reuse, onBeforeDeserialize);
						array.SetValue(value2, j);
					}
				}
				return array;
			}
			object obj2 = ((reuse != null && reuse[refId] != null) ? reuse[refId] : Activator.CreateInstance(type));
			if (obj2 is ICustomSerialization customSerialization)
			{
				customSerialization.PreprocessBeforeDeserialize(obj, serialized, deserialized);
			}
			deserialized[refId] = obj2;
			if (obj.values == null)
			{
				return null;
			}
			for (int k = 0; k < obj.values.Length; k++)
			{
				FieldInfo field = type.GetField(obj.fields[k], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (field == null && (obj2 is IList || obj2 is IDictionary))
				{
					field = type.BaseType.GetField(obj.fields[k], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				}
				if (field == null || field.IsNotSerialized)
				{
					continue;
				}
				Value value3 = obj.values[k];
				if (field.FieldType.IsPrimitive)
				{
					if (value3.CheckType(field.FieldType))
					{
						field.SetValue(obj2, value3.Get());
					}
				}
				else if ((int)value3 >= 0)
				{
					object obj3 = DeserializeObject(value3, serialized, deserialized, reuse, onBeforeDeserialize);
					if (obj3 != null && obj3.GetType() != field.FieldType && !field.FieldType.IsAssignableFrom(obj3.GetType()))
					{
						Debug.LogWarning($"Serializer: Could not convert {obj3.GetType()} to {field.FieldType}. Using default value");
					}
					else
					{
						field.SetValue(obj2, obj3);
					}
				}
			}
			if (obj2 is ISerializationCallbackReceiver)
			{
				((ISerializationCallbackReceiver)obj2).OnAfterDeserialize();
			}
			return obj2;
		}

		public static bool CheckMatch(object src, object dst, HashSet<object> chkd, ClassMatch checkIfSameReference = ClassMatch.None)
		{
			if (chkd == null)
			{
				chkd = new HashSet<object>();
			}
			if (src == null && dst == null)
			{
				return true;
			}
			if (src == null && dst != null)
			{
				throw new Exception("Src is null while dst isn't");
			}
			if (src != null && dst == null)
			{
				throw new Exception("Dst is null while src isn't");
			}
			Type type = src.GetType();
			if (type.IsPrimitive)
			{
				if (!src.Equals(dst))
				{
					throw new Exception("Primitives not equal");
				}
			}
			else if (type == typeof(string))
			{
				if (src != dst)
				{
					throw new Exception("String not equal");
				}
			}
			else if (type.IsSubclassOf(typeof(Type)))
			{
				if (src != dst)
				{
					throw new Exception("Types not equal");
				}
			}
			else if (type.IsSubclassOf(typeof(FieldInfo)))
			{
				if (src != dst)
				{
					throw new Exception("Field Infos not equal");
				}
			}
			else
			{
				if (!type.IsSubclassOf(typeof(UnityEngine.Object)))
				{
					if (type == typeof(AnimationCurve))
					{
						if (checkIfSameReference == ClassMatch.ShouldDiffer && src == dst)
						{
							throw new Exception("Value references match");
						}
						if (checkIfSameReference == ClassMatch.ShouldMatch && src != dst)
						{
							throw new Exception("Value references differ");
						}
						AnimationCurve animationCurve = (AnimationCurve)src;
						AnimationCurve animationCurve2 = (AnimationCurve)dst;
						if (animationCurve.keys.Length != animationCurve2.keys.Length)
						{
							throw new Exception("Anim Curve length differ");
						}
						for (int i = 0; i < animationCurve.keys.Length; i++)
						{
							if (animationCurve.keys[i].time != animationCurve2.keys[i].time || animationCurve.keys[i].value != animationCurve2.keys[i].value)
							{
								return false;
							}
						}
						return true;
					}
					if (type.IsArray || type.BaseType.IsArray)
					{
						if (checkIfSameReference == ClassMatch.ShouldDiffer && src == dst)
						{
							throw new Exception("Value references match");
						}
						if (checkIfSameReference == ClassMatch.ShouldMatch && src != dst)
						{
							throw new Exception("Value references differ");
						}
						Array array = (Array)src;
						Array array2 = (Array)dst;
						if (chkd.Contains(array))
						{
							return true;
						}
						chkd.Add(array);
						if (array.Length != array2.Length)
						{
							return false;
						}
						for (int j = 0; j < array2.Length; j++)
						{
							if (!CheckMatch(array.GetValue(j), array2.GetValue(j), chkd, checkIfSameReference))
							{
								return false;
							}
						}
						return true;
					}
					if (checkIfSameReference == ClassMatch.ShouldDiffer && src == dst)
					{
						throw new Exception("Value references match");
					}
					if (checkIfSameReference == ClassMatch.ShouldMatch && !src.Equals(dst))
					{
						throw new Exception("Value references differ");
					}
					if (chkd.Contains(src))
					{
						return true;
					}
					chkd.Add(src);
					FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					for (int k = 0; k < fields.Length; k++)
					{
						if (!fields[k].IsLiteral && !fields[k].FieldType.IsPointer && !fields[k].IsNotSerialized)
						{
							object value = fields[k].GetValue(src);
							object value2 = fields[k].GetValue(dst);
							if (!CheckMatch(value, value2, chkd, checkIfSameReference))
							{
								return false;
							}
						}
					}
					return true;
				}
				if (src != dst)
				{
					throw new Exception("Unity objects not equal");
				}
			}
			return true;
		}
	}
}
