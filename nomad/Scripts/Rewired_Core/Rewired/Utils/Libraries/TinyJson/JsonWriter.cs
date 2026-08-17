using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;
using Rewired.Utils.Interfaces;
using UnityEngine;

namespace Rewired.Utils.Libraries.TinyJson
{
	public static class JsonWriter
	{
		private static Action<StringBuilder, object> qnwPBEMMlGOtDSeZxYkJQdxcxXSv;

		private static Action<StringBuilder, object> nxuBlUjULRhnuMrphqVsIDApXVZw => VbpdSLhkUmsoiaZqJZghdHzZvqgR;

		public static string ToJson(object item)
		{
			StringBuilder stringBuilder = new StringBuilder();
			VbpdSLhkUmsoiaZqJZghdHzZvqgR(stringBuilder, item);
			return stringBuilder.ToString();
		}

		private static void VbpdSLhkUmsoiaZqJZghdHzZvqgR(StringBuilder P_0, object P_1)
		{
			if (P_1 == null)
			{
				P_0.Append("null");
				return;
			}
			if (P_1 is ISerializationCallbackReceiver serializationCallbackReceiver)
			{
				try
				{
					serializationCallbackReceiver.OnBeforeSerialize();
				}
				catch (Exception ex)
				{
					Logger.LogError(ex.ToString(), requiredThreadSafety: true);
				}
			}
			Type type = P_1.GetType();
			if (ReflectionTools.DoesTypeImplement(type, typeof(IExportToJson)))
			{
				((IExportToJson)P_1).WriteJson(P_0, nxuBlUjULRhnuMrphqVsIDApXVZw);
				return;
			}
			if ((object)type == typeof(string))
			{
				P_0.Append('"');
				LsydhSxWaeNXWSNTptBZlkhQttlB(P_0, (string)P_1);
				P_0.Append('"');
				return;
			}
			if ((object)type == typeof(int) || (object)type == typeof(uint) || (object)type == typeof(long) || (object)type == typeof(ulong) || (object)type == typeof(short) || (object)type == typeof(ushort) || (object)type == typeof(byte) || (object)type == typeof(sbyte))
			{
				P_0.Append(P_1.ToString());
				return;
			}
			if ((object)type == typeof(float))
			{
				P_0.Append(((float)P_1).ToString(CultureInfo.InvariantCulture));
				return;
			}
			if ((object)type == typeof(double))
			{
				P_0.Append(((double)P_1).ToString(CultureInfo.InvariantCulture));
				return;
			}
			if ((object)type == typeof(decimal))
			{
				P_0.Append(((decimal)P_1).ToString(CultureInfo.InvariantCulture));
				return;
			}
			if ((object)type == typeof(bool))
			{
				P_0.Append(((bool)P_1) ? "true" : "false");
				return;
			}
			if ((object)type == typeof(Guid))
			{
				VbpdSLhkUmsoiaZqJZghdHzZvqgR(P_0, P_1.ToString());
				return;
			}
			if (ReflectionTools.IsEnum(type))
			{
				Type underlyingType = Enum.GetUnderlyingType(type);
				VbpdSLhkUmsoiaZqJZghdHzZvqgR(P_0, Convert.ChangeType(P_1, underlyingType));
				return;
			}
			if ((object)type == typeof(AnimationCurve))
			{
				P_0.Append('{');
				P_0.Append("\"keys\":");
				VbpdSLhkUmsoiaZqJZghdHzZvqgR(P_0, ((AnimationCurve)P_1).keys);
				P_0.Append('}');
				return;
			}
			if ((object)type == typeof(Keyframe))
			{
				Keyframe keyframe = (Keyframe)P_1;
				P_0.Append('{');
				qYqkpmOwrxKGuzhCgfgcAGBljgud(P_0, "time", keyframe.time);
				P_0.Append(',');
				qYqkpmOwrxKGuzhCgfgcAGBljgud(P_0, "value", keyframe.value);
				P_0.Append(',');
				qYqkpmOwrxKGuzhCgfgcAGBljgud(P_0, "inTangent", keyframe.inTangent);
				P_0.Append(',');
				qYqkpmOwrxKGuzhCgfgcAGBljgud(P_0, "outTangent", keyframe.outTangent);
				P_0.Append(',');
				qYqkpmOwrxKGuzhCgfgcAGBljgud(P_0, "inWeight", keyframe.inWeight);
				P_0.Append(',');
				qYqkpmOwrxKGuzhCgfgcAGBljgud(P_0, "outWeight", keyframe.outWeight);
				P_0.Append(',');
				qYqkpmOwrxKGuzhCgfgcAGBljgud(P_0, "weightedMode", keyframe.weightedMode);
				P_0.Append('}');
				return;
			}
			if (ReflectionTools.DoesTypeImplement(type, typeof(IList)))
			{
				P_0.Append('[');
				bool flag = true;
				IList list = P_1 as IList;
				for (int i = 0; i < list.Count; i++)
				{
					if (flag)
					{
						flag = false;
					}
					else
					{
						P_0.Append(',');
					}
					VbpdSLhkUmsoiaZqJZghdHzZvqgR(P_0, list[i]);
				}
				P_0.Append(']');
				return;
			}
			if (ReflectionTools.IsGenericType(type) && ReflectionTools.DoesTypeImplement(type, typeof(IDictionary)))
			{
				Type type2 = ReflectionTools.GetGenericArguments(type)[0];
				bool flag2 = false;
				Type conversionType = null;
				if (ReflectionTools.IsEnum(type2))
				{
					flag2 = true;
					conversionType = ReflectionTools.GetUnderlyingEnumType(type2);
				}
				P_0.Append('{');
				IDictionary dictionary = P_1 as IDictionary;
				bool flag3 = true;
				foreach (object key in dictionary.Keys)
				{
					if (flag3)
					{
						flag3 = false;
					}
					else
					{
						P_0.Append(',');
					}
					P_0.Append('"');
					P_0.Append(flag2 ? Convert.ChangeType(key, conversionType).ToString() : key.ToString());
					P_0.Append("\":");
					VbpdSLhkUmsoiaZqJZghdHzZvqgR(P_0, dictionary[key]);
				}
				P_0.Append('}');
				return;
			}
			P_0.Append('{');
			bool flag4 = true;
			foreach (FieldInfo field in ReflectionTools.GetFields(type, ReflectionTools.BindingFlags.Instance | ReflectionTools.BindingFlags.Public | ReflectionTools.BindingFlags.NonPublic))
			{
				if (field.IsDefined(typeof(NonSerializedAttribute), inherit: true) || field.IsDefined(typeof(DoNotSerializeAttribute), inherit: true) || (!field.IsPublic && !field.IsDefined(typeof(SerializeAttribute), inherit: true) && !field.IsDefined(typeof(SerializeField), inherit: true)))
				{
					continue;
				}
				object value = field.GetValue(P_1);
				if (value != null)
				{
					if (flag4)
					{
						flag4 = false;
					}
					else
					{
						P_0.Append(',');
					}
					P_0.Append('"');
					string name;
					if (!field.IsDefined(typeof(SerializeAttribute), inherit: true) || string.IsNullOrEmpty(name = (CollectionTools.GetValue(field.GetCustomAttributes(typeof(SerializeAttribute), inherit: true), 0) as SerializeAttribute).Name))
					{
						name = field.Name;
					}
					P_0.Append(name);
					P_0.Append("\":");
					VbpdSLhkUmsoiaZqJZghdHzZvqgR(P_0, value);
				}
			}
			foreach (PropertyInfo property in ReflectionTools.GetProperties(type, ReflectionTools.BindingFlags.Instance | ReflectionTools.BindingFlags.Public | ReflectionTools.BindingFlags.NonPublic))
			{
				if (!property.CanWrite || !property.IsDefined(typeof(SerializeAttribute), inherit: true) || property.IsDefined(typeof(DoNotSerializeAttribute), inherit: true) || !property.CanRead)
				{
					continue;
				}
				object value2 = property.GetValue(P_1, null);
				if (value2 != null)
				{
					if (flag4)
					{
						flag4 = false;
					}
					else
					{
						P_0.Append(',');
					}
					P_0.Append('"');
					string name2;
					if (!property.IsDefined(typeof(SerializeAttribute), inherit: true) || string.IsNullOrEmpty(name2 = (CollectionTools.GetValue(property.GetCustomAttributes(typeof(SerializeAttribute), inherit: true), 0) as SerializeAttribute).Name))
					{
						name2 = property.Name;
					}
					P_0.Append(name2);
					P_0.Append("\":");
					VbpdSLhkUmsoiaZqJZghdHzZvqgR(P_0, value2);
				}
			}
			P_0.Append('}');
		}

		private static void qYqkpmOwrxKGuzhCgfgcAGBljgud(StringBuilder P_0, string P_1, object P_2)
		{
			if (!string.IsNullOrEmpty(P_1))
			{
				P_0.Append('"');
				P_0.Append(P_1);
				P_0.Append("\":");
			}
			VbpdSLhkUmsoiaZqJZghdHzZvqgR(P_0, P_2);
		}

		private static void LsydhSxWaeNXWSNTptBZlkhQttlB(StringBuilder P_0, string P_1)
		{
			if (string.IsNullOrEmpty(P_1))
			{
				return;
			}
			int length = P_1.Length;
			for (int i = 0; i < length; i++)
			{
				if (P_1[i] == '"' && (i == 0 || P_1[i - 1] != '\\'))
				{
					P_0.Append('\\');
				}
				P_0.Append(P_1[i]);
			}
		}
	}
}
