using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Den.Tools.Serialization
{
	public class Serializer
	{
		internal class Builder : IDisposable
		{
			private StringBuilder builder;

			public Builder(string name, object val, Dictionary<object, int> serializedObjsIds, string offset = "", bool addId = true)
			{
				int num = 0;
				if (addId)
				{
					num = serializedObjsIds.Count + 1;
					serializedObjsIds.Add(val, num);
				}
				if (val is ISerializationCallbackReceiver serializationCallbackReceiver)
				{
					serializationCallbackReceiver.OnBeforeSerialize();
				}
				builder = new StringBuilder();
				builder.Append($"{offset}< \"{name}\", \"{TypeName(val.GetType())}\", id{num}");
			}

			public void Dispose()
			{
				builder.Append(" >");
			}

			public void AddLine(string s)
			{
				builder.AppendLine(",");
				builder.Append(s);
			}

			public void Add(string s)
			{
				builder.Append(",");
				builder.Append(s);
			}

			public void AddSpace()
			{
				builder.Append(" ");
			}

			public override string ToString()
			{
				return builder.ToString();
			}
		}

		public static object Deserialize(string str, IList<UnityEngine.Object> unityObjs = null)
		{
			if (str == "null")
			{
				return null;
			}
			return DeserializeRecursive((object[])((object[])Tools.SplitBlock(str, ',', '<', '>'))[0], new Dictionary<int, object>(), unityObjs).val;
		}

		private static (string name, object val) DeserializeRecursive(object[] splitString, Dictionary<int, object> deserializedIdsObjs, IList<UnityEngine.Object> unityObjs)
		{
			string item = ((string)splitString[0]).Trim('"');
			Type type = null;
			string text = (string)splitString[1];
			text = text.Trim('"');
			type = Type.GetType(text);
			if (type == null)
			{
				Debug.LogError("Could not find type for: " + text);
				return (name: item, val: null);
			}
			int num = 0;
			if (!type.IsPrimitive)
			{
				num = int.Parse(((string)splitString[2]).TrimStart('i', 'd'));
			}
			if (splitString.Length <= 3 && deserializedIdsObjs.TryGetValue(num, out var value))
			{
				return (name: item, val: value);
			}
			if (type == typeof(float))
			{
				return (name: item, val: float.Parse((string)splitString[3], CultureInfo.InvariantCulture.NumberFormat));
			}
			if (type == typeof(int))
			{
				return (name: item, val: int.Parse((string)splitString[3]));
			}
			if (type == typeof(Guid))
			{
				Guid guid = Guid.Parse((string)splitString[3]);
				return (name: item, val: guid);
			}
			if (type.IsPrimitive)
			{
				object item2 = Convert.ChangeType((string)splitString[3], Type.GetTypeCode(type));
				return (name: item, val: item2);
			}
			if (splitString.Length == 4 && splitString[3] is string text2 && text2 == "null")
			{
				return (name: item, val: null);
			}
			if (type == typeof(string))
			{
				if (deserializedIdsObjs.TryGetValue(num, out var value2))
				{
					return (name: item, val: value2);
				}
				string text3 = ((string)splitString[3]).Trim('"');
				deserializedIdsObjs.Add(num, text3);
				return (name: item, val: text3);
			}
			if (type.IsSubclassOf(typeof(UnityEngine.Object)))
			{
				if (deserializedIdsObjs.TryGetValue(num, out var value3))
				{
					return (name: item, val: value3);
				}
				string text4 = (string)splitString[3];
				if (text4 == "null")
				{
					return (name: item, val: null);
				}
				int index = int.Parse(text4);
				return (name: item, val: unityObjs[index]);
			}
			if (type.IsSubclassOf(typeof(Type)))
			{
				Type type2 = Type.GetType(((string)splitString[3]).Trim('"'));
				if (num != 0)
				{
					deserializedIdsObjs.Add(num, type2);
				}
				return (name: item, val: type2);
			}
			if (type.IsSubclassOf(typeof(MemberInfo)))
			{
				MemberInfo field = Type.GetType(((string)splitString[4]).Trim('"')).GetField(((string)splitString[3]).Trim('"'), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (num != 0)
				{
					deserializedIdsObjs.Add(num, field);
				}
				return (name: item, val: field);
			}
			if (type.IsArray)
			{
				if (deserializedIdsObjs.TryGetValue(num, out var value4))
				{
					return (name: item, val: value4);
				}
				Array array = (Array)Activator.CreateInstance(type, splitString.Length - 3);
				deserializedIdsObjs.Add(num, array);
				TypeCode typeCode = Type.GetTypeCode(type.GetElementType());
				for (int i = 0; i < array.Length; i++)
				{
					object obj = splitString[i + 3];
					if (obj is string value5)
					{
						object value6 = Convert.ChangeType(value5, typeCode);
						array.SetValue(value6, i);
					}
					else if (obj is object[] splitString2)
					{
						object item3 = DeserializeRecursive(splitString2, deserializedIdsObjs, unityObjs).val;
						array.SetValue(item3, i);
					}
				}
				return (name: item, val: array);
			}
			if (!deserializedIdsObjs.TryGetValue(num, out var value7))
			{
				value7 = Activator.CreateInstance(type);
				if (num != 0)
				{
					deserializedIdsObjs.Add(num, value7);
				}
			}
			for (int j = 3; j < splitString.Length; j++)
			{
				(string name, object val) tuple = DeserializeRecursive((object[])splitString[j], deserializedIdsObjs, unityObjs);
				string text5 = tuple.name;
				object item4 = tuple.val;
				MemberInfo memberInfo = type.GetField(text5, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (memberInfo == null)
				{
					memberInfo = type.BaseType.GetField(text5, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				}
				if (memberInfo == null)
				{
					memberInfo = type.GetProperty(text5, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				}
				if (memberInfo == null)
				{
					switch (text5)
					{
					case "buckets":
					case "entries":
					case "count":
					case "comparer":
					case "version":
						text5 = "_" + text5;
						break;
					}
					memberInfo = type.GetField(text5, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (memberInfo == null)
					{
						memberInfo = type.BaseType.GetField(text5, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					}
				}
				if (memberInfo == null)
				{
					continue;
				}
				if (memberInfo is FieldInfo fieldInfo)
				{
					if (!fieldInfo.IsNotSerialized)
					{
						fieldInfo.SetValue(value7, item4);
					}
				}
				else if (memberInfo is PropertyInfo propertyInfo)
				{
					propertyInfo.SetValue(value7, item4);
				}
			}
			return (name: item, val: value7);
		}

		public static string Serialize(object val)
		{
			if (val == null)
			{
				return "null";
			}
			return SerializeRecursive("root", val, new Dictionary<object, int>(), null);
		}

		public static string Serialize(object val, out UnityEngine.Object[] unityObjs)
		{
			if (val == null)
			{
				unityObjs = new UnityEngine.Object[0];
				return "null";
			}
			Dictionary<object, int> serializedObjsIds = new Dictionary<object, int>();
			List<UnityEngine.Object> list = new List<UnityEngine.Object>();
			string result = SerializeRecursive("root", val, serializedObjsIds, list);
			unityObjs = list.ToArray();
			return result;
		}

		private static string SerializeRecursive(string name, object val, Dictionary<object, int> serializedObjsIds, List<UnityEngine.Object> unityObjs, string offset = "")
		{
			if (val == null)
			{
				string text = "System.Object";
				return offset + "< \"" + name + "\", " + text + ", id0, null >";
			}
			if (!(val is float num))
			{
				if (!(val is double num2))
				{
					if (val is int num3)
					{
						return offset + "< \"" + name + "\", System.Int32, id0, " + num3 + " >";
					}
					Type type = val.GetType();
					if (serializedObjsIds != null && serializedObjsIds.TryGetValue(val, out var value))
					{
						return $"{offset}< \"{name}\", \"{TypeName(type)}\", id{value} >";
					}
					if (val is string)
					{
						int num4 = serializedObjsIds.Count + 1;
						serializedObjsIds.Add(val, num4);
						return $"{offset}< \"{name}\", System.String, id{num4}, \"{val}\" >";
					}
					if (val is Guid guid)
					{
						return offset + "< \"" + name + "\", System.Guid, id0, " + guid.ToString() + " >";
					}
					if (type.IsPrimitive)
					{
						return $"{offset}< \"{name}\", {type.FullName}, id0, {val} >";
					}
					if (val is UnityEngine.Object)
					{
						UnityEngine.Object obj = (UnityEngine.Object)val;
						if (unityObjs == null)
						{
							return offset + "<  \"" + name + "\", \"" + TypeName(type) + "\", id0, null >";
						}
						if (obj == null)
						{
							return offset + "<  \"" + name + "\", \"" + TypeName(type) + "\", id0, null >";
						}
						if (unityObjs.Contains(obj, out var index))
						{
							return $"{offset}<  \"{name}\", \"{TypeName(type)}\", id0, {index} >";
						}
						unityObjs.Add(obj);
						return $"{offset}<  \"{name}\", \"{TypeName(type)}\", id0, {unityObjs.Count - 1} >";
					}
					if (val is MemberInfo memberInfo)
					{
						int num5 = serializedObjsIds.Count + 1;
						serializedObjsIds.Add(val, num5);
						if (val is Type type2)
						{
							return $"{offset}< \"{name}\", \"{TypeName(type)}\", id{num5}, \"{TypeName(type2)}\" >";
						}
						return $"{offset}< \"{name}\", \"{TypeName(type)}\", id{num5}, \"{memberInfo.Name}\", \"{TypeName(memberInfo.DeclaringType)}\" >";
					}
					if (val is Array array)
					{
						Builder builder = new Builder(name, val, serializedObjsIds, offset);
						using (builder)
						{
							Type elementType = type.GetElementType();
							if (elementType.IsPrimitive)
							{
								builder.AddSpace();
								if (elementType == typeof(float))
								{
									for (int i = 0; i < array.Length; i++)
									{
										builder.Add(((float)array.GetValue(i)).ToString("R"));
									}
								}
								else
								{
									for (int j = 0; j < array.Length; j++)
									{
										builder.Add(array.GetValue(j).ToString());
									}
								}
							}
							else
							{
								for (int k = 0; k < array.Length; k++)
								{
									object value2 = array.GetValue(k);
									value2?.GetType();
									string s = SerializeRecursive(k.ToString(), value2, serializedObjsIds, unityObjs, offset + "  ");
									builder.AddLine(s);
								}
							}
						}
						return builder.ToString();
					}
					Builder builder2 = new Builder(name, val, serializedObjsIds, offset, !type.IsValueType);
					using (builder2)
					{
						if (val is ISerializationCallbackReceiver serializationCallbackReceiver)
						{
							serializationCallbackReceiver.OnBeforeSerialize();
						}
						foreach (MemberInfo item in Fields(type, val))
						{
							object val2 = null;
							if (item is FieldInfo fieldInfo)
							{
								val2 = fieldInfo.GetValue(val);
								_ = fieldInfo.FieldType;
							}
							else if (item is PropertyInfo propertyInfo)
							{
								val2 = propertyInfo.GetValue(val);
								_ = propertyInfo.PropertyType;
							}
							string s2 = SerializeRecursive(item.Name, val2, serializedObjsIds, unityObjs, offset + "  ");
							builder2.AddLine(s2);
						}
					}
					return builder2.ToString();
				}
				return offset + "< \"" + name + "\", System.Double, id0, " + num2.ToString("R") + " >";
			}
			return offset + "< \"" + name + "\", System.Single, id0, " + num.ToString("R") + " >";
		}

		private static IEnumerable<MemberInfo> Fields(Type type, object val)
		{
			Type baseType = type.BaseType;
			if (baseType != typeof(object) && baseType != typeof(ValueType))
			{
				foreach (FieldInfo item in Fields(baseType, val))
				{
					yield return item;
				}
			}
			BindingFlags bind = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			if (val is IList && baseType == typeof(object))
			{
				yield return type.GetField("_items", bind);
				yield return type.GetField("_size", bind);
				yield return type.GetField("_version", bind);
				yield break;
			}
			if (val is IDictionary && baseType == typeof(object))
			{
				if (type.GetField("_buckets", bind) != null)
				{
					yield return type.GetField("_buckets", bind);
					yield return type.GetField("_entries", bind);
					yield return type.GetField("_count", bind);
					yield return type.GetField("_comparer", bind);
					yield return type.GetField("_version", bind);
				}
				else
				{
					yield return type.GetField("buckets", bind);
					yield return type.GetField("entries", bind);
					yield return type.GetField("count", bind);
					yield return type.GetField("comparer", bind);
					yield return type.GetField("version", bind);
				}
				yield break;
			}
			if (val is AnimationCurve && baseType == typeof(object))
			{
				yield return type.GetProperty("keys", bind);
				yield return type.GetProperty("preWrapMode", bind);
				yield return type.GetProperty("postWrapMode", bind);
				yield break;
			}
			FieldInfo[] fields = type.GetFields(bind);
			FieldInfo[] array = fields;
			foreach (FieldInfo fieldInfo in array)
			{
				if (!fieldInfo.IsNotSerialized && !fieldInfo.IsLiteral && !fieldInfo.FieldType.IsPointer)
				{
					yield return fieldInfo;
				}
			}
		}

		private static string TypeName(Type type, char div = ',', bool withAssembly = true)
		{
			string text = type.ToString();
			if (type.IsArray)
			{
				text = TypeName(type.GetElementType(), div, withAssembly: false) + "[]";
				if (withAssembly)
				{
					text = $"{text}{div} {type.Assembly.GetName().Name}";
				}
				return text;
			}
			if (type.IsGenericType)
			{
				text = text.Substring(0, text.IndexOf('[', 0) + 1);
				Type[] genericTypeArguments = type.GenericTypeArguments;
				for (int i = 0; i < genericTypeArguments.Length; i++)
				{
					text = text + "[" + TypeName(genericTypeArguments[i]) + "]";
					if (i != genericTypeArguments.Length - 1)
					{
						text += div;
					}
				}
				text += "]";
			}
			if (withAssembly)
			{
				text = $"{text}{div} {type.Assembly.GetName().Name}";
			}
			return text;
		}
	}
}
