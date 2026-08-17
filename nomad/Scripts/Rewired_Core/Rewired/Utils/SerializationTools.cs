using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using Rewired.Utils.Attributes;
using Rewired.Utils.Interfaces;
using Rewired.Utils.Libraries.TinyJson;
using UnityEngine;

namespace Rewired.Utils
{
	public static class SerializationTools
	{
		public static string SerializeObjectToXmlString<T>(T obj)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
			string empty = string.Empty;
			using StringWriter stringWriter = new StringWriter();
			xmlSerializer.Serialize(stringWriter, obj);
			return stringWriter.ToString();
		}

		public static void WriteXmlElement(XmlWriter writer, string name, object value)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}
			bool flag = false;
			if (value is IExportToXml && (value as IExportToXml).writesOwnElementTag)
			{
				flag = true;
			}
			if (flag)
			{
				ohCBEWHVXTrttFSjbGpiHhqxDSUM(writer, value);
				return;
			}
			writer.WriteStartElement(name);
			ohCBEWHVXTrttFSjbGpiHhqxDSUM(writer, value);
			writer.WriteEndElement();
		}

		public static void WriteXmlElement<T>(XmlWriter writer, string name, T value)
		{
			WriteXmlElement(writer, name, (object)value);
		}

		private static void ohCBEWHVXTrttFSjbGpiHhqxDSUM(XmlWriter P_0, object P_1)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("writer");
			}
			if (P_1 == null)
			{
				return;
			}
			Type type = P_1.GetType();
			if (ReflectionTools.DoesTypeImplement(type, typeof(IExportToXml)))
			{
				((IExportToXml)P_1).WriteXml(P_0);
				return;
			}
			if ((object)type == typeof(string))
			{
				P_0.WriteValue(CleanInvalidXmlChars((string)P_1));
				return;
			}
			if ((object)type == typeof(char))
			{
				P_0.WriteValue(CleanInvalidXmlChars(P_1.ToString()));
				return;
			}
			if ((object)type == typeof(byte))
			{
				P_0.WriteValue((int)P_1);
				return;
			}
			if ((object)type == typeof(sbyte))
			{
				P_0.WriteValue((int)P_1);
				return;
			}
			if ((object)type == typeof(short))
			{
				P_0.WriteValue((short)P_1);
				return;
			}
			if ((object)type == typeof(ushort))
			{
				P_0.WriteValue((ushort)P_1);
				return;
			}
			if ((object)type == typeof(int))
			{
				P_0.WriteValue((int)P_1);
				return;
			}
			if ((object)type == typeof(uint))
			{
				P_0.WriteValue((uint)P_1);
				return;
			}
			if ((object)type == typeof(long))
			{
				P_0.WriteValue((long)P_1);
				return;
			}
			if ((object)type == typeof(ulong))
			{
				P_0.WriteValue(((ulong)P_1).ToString());
				return;
			}
			if ((object)type == typeof(float))
			{
				P_0.WriteValue((float)P_1);
				return;
			}
			if ((object)type == typeof(double))
			{
				P_0.WriteValue((double)P_1);
				return;
			}
			if ((object)type == typeof(decimal))
			{
				P_0.WriteValue((decimal)P_1);
				return;
			}
			if ((object)type == typeof(bool))
			{
				P_0.WriteValue((bool)P_1);
				return;
			}
			if ((object)type == typeof(DateTime))
			{
				P_0.WriteValue((DateTime)P_1);
				return;
			}
			if ((object)type == typeof(Guid))
			{
				P_0.WriteValue(((Guid)P_1/*cast due to .constrained prefix*/).ToString());
				return;
			}
			if (ReflectionTools.DoesTypeImplement(type, typeof(Enum)))
			{
				Type underlyingType = Enum.GetUnderlyingType(type);
				P_0.WriteValue(Convert.ChangeType(P_1, underlyingType));
				return;
			}
			if ((object)type == typeof(AnimationCurve))
			{
				WriteXmlElement(P_0, "keys", ((AnimationCurve)P_1).keys);
				return;
			}
			if ((object)type == typeof(Keyframe))
			{
				Keyframe keyframe = (Keyframe)P_1;
				WriteXmlElement(P_0, "time", keyframe.time);
				WriteXmlElement(P_0, "value", keyframe.value);
				WriteXmlElement(P_0, "inTangent", keyframe.inTangent);
				WriteXmlElement(P_0, "outTangent", keyframe.outTangent);
				WriteXmlElement(P_0, "inWeight", keyframe.inWeight);
				WriteXmlElement(P_0, "outWeight", keyframe.outWeight);
				WriteXmlElement(P_0, "weightedMode", keyframe.weightedMode);
				return;
			}
			if (!ReflectionTools.IsDefined(type, typeof(SerializationTypeAttribute), inherit: true) || ReflectionTools.GetAttribute<SerializationTypeAttribute>(type, inherit: true).serializationType != SerializationTypeAttribute.SerializationType.Object)
			{
				if (ReflectionTools.DoesTypeImplement(type, typeof(IList)))
				{
					IList list = P_1 as IList;
					for (int i = 0; i < list.Count; i++)
					{
						WriteXmlElement(P_0, (list[i] != null) ? list[i].GetType().Name : "value", list[i]);
					}
					return;
				}
				if (ReflectionTools.DoesTypeImplement(type, typeof(IDictionary)))
				{
					IDictionary dictionary = P_1 as IDictionary;
					{
						foreach (object key in dictionary.Keys)
						{
							WriteXmlElement(P_0, key.ToString(), dictionary[key]);
						}
						return;
					}
				}
				if (ReflectionTools.DoesTypeImplement(type, typeof(IEnumerable)))
				{
					foreach (object item in P_1 as IEnumerable)
					{
						WriteXmlElement(P_0, (item != null) ? item.GetType().Name : "value", item);
					}
					return;
				}
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
			foreach (FieldInfo field in ReflectionTools.GetFields(type, ReflectionTools.BindingFlags.Instance | ReflectionTools.BindingFlags.Public | ReflectionTools.BindingFlags.NonPublic))
			{
				if (field.IsDefined(typeof(NonSerializedAttribute), inherit: true) || field.IsDefined(typeof(DoNotSerializeAttribute), inherit: true) || (!field.IsPublic && !field.IsDefined(typeof(SerializeAttribute), inherit: true) && !field.IsDefined(typeof(SerializeField), inherit: true)))
				{
					continue;
				}
				object value = field.GetValue(P_1);
				if (value != null)
				{
					string name;
					if (!field.IsDefined(typeof(SerializeAttribute), inherit: true) || string.IsNullOrEmpty(name = (CollectionTools.GetValue(field.GetCustomAttributes(typeof(SerializeAttribute), inherit: true), 0) as SerializeAttribute).Name))
					{
						name = field.Name;
					}
					WriteXmlElement(P_0, name, value);
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
					string name2;
					if (!property.IsDefined(typeof(SerializeAttribute), inherit: true) || string.IsNullOrEmpty(name2 = (CollectionTools.GetValue(property.GetCustomAttributes(typeof(SerializeAttribute), inherit: true), 0) as SerializeAttribute).Name))
					{
						name2 = property.Name;
					}
					WriteXmlElement(P_0, name2, value2);
				}
			}
		}

		public static string ReadXmlElement(XmlReader reader, string name)
		{
			string result = string.Empty;
			bool isEmptyElement = reader.IsEmptyElement;
			reader.ReadStartElement(name);
			if (!isEmptyElement)
			{
				result = reader.ReadContentAsString();
				reader.ReadEndElement();
			}
			return result;
		}

		public static T ReadXmlElement<T>(XmlReader reader, string name)
		{
			string text = ReadXmlElement(reader, name);
			Type typeFromHandle = typeof(T);
			if ((object)typeFromHandle == typeof(int))
			{
				if (int.TryParse(text, out var result))
				{
					return (T)(object)result;
				}
			}
			else if ((object)typeFromHandle == typeof(float))
			{
				if (float.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out var result2))
				{
					return (T)(object)result2;
				}
			}
			else if ((object)typeFromHandle == typeof(bool))
			{
				if (bool.TryParse(text, out var result3))
				{
					return (T)(object)result3;
				}
			}
			else
			{
				if ((object)typeFromHandle == typeof(string))
				{
					return (T)(object)text;
				}
				if ((object)typeFromHandle == typeof(short))
				{
					if (short.TryParse(text, out var result4))
					{
						return (T)(object)result4;
					}
				}
				else if ((object)typeFromHandle == typeof(byte))
				{
					if (byte.TryParse(text, out var result5))
					{
						return (T)(object)result5;
					}
				}
				else if ((object)typeFromHandle == typeof(ushort))
				{
					if (ushort.TryParse(text, out var result6))
					{
						return (T)(object)result6;
					}
				}
				else if ((object)typeFromHandle == typeof(uint))
				{
					if (uint.TryParse(text, out var result7))
					{
						return (T)(object)result7;
					}
				}
				else if ((object)typeFromHandle == typeof(long))
				{
					if (long.TryParse(text, out var result8))
					{
						return (T)(object)result8;
					}
				}
				else if ((object)typeFromHandle == typeof(ulong))
				{
					if (ulong.TryParse(text, out var result9))
					{
						return (T)(object)result9;
					}
				}
				else if ((object)typeFromHandle == typeof(double))
				{
					if (double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out var result10))
					{
						return (T)(object)result10;
					}
				}
				else
				{
					if ((object)typeFromHandle != typeof(decimal))
					{
						throw new NotImplementedException();
					}
					if (decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out var result11))
					{
						return (T)(object)result11;
					}
				}
			}
			return default(T);
		}

		public static bool TryReadXmlElement(XmlReader reader, string name, out string outValue)
		{
			outValue = string.Empty;
			bool isEmptyElement = reader.IsEmptyElement;
			try
			{
				reader.ReadStartElement(name);
			}
			catch
			{
				return false;
			}
			if (!isEmptyElement)
			{
				outValue = reader.ReadContentAsString();
				reader.ReadEndElement();
			}
			return true;
		}

		public static bool TryReadXmlElement<T>(XmlReader reader, string name, out T outValue)
		{
			outValue = default(T);
			Type typeFromHandle = typeof(T);
			if (!TryReadXmlElement(reader, name, out var outValue2))
			{
				return false;
			}
			if ((object)typeFromHandle == typeof(string))
			{
				outValue = (T)(object)outValue2;
				return true;
			}
			if ((object)typeFromHandle == typeof(byte))
			{
				if (byte.TryParse(outValue2, out var result))
				{
					outValue = (T)(object)result;
					return true;
				}
			}
			else if ((object)typeFromHandle == typeof(sbyte))
			{
				if (sbyte.TryParse(outValue2, out var result2))
				{
					outValue = (T)(object)result2;
					return true;
				}
			}
			else if ((object)typeFromHandle == typeof(short))
			{
				if (short.TryParse(outValue2, out var result3))
				{
					outValue = (T)(object)result3;
					return true;
				}
			}
			else if ((object)typeFromHandle == typeof(ushort))
			{
				if (ushort.TryParse(outValue2, out var result4))
				{
					outValue = (T)(object)result4;
					return true;
				}
			}
			else if ((object)typeFromHandle == typeof(int))
			{
				if (int.TryParse(outValue2, out var result5))
				{
					outValue = (T)(object)result5;
					return true;
				}
			}
			else if ((object)typeFromHandle == typeof(uint))
			{
				if (uint.TryParse(outValue2, out var result6))
				{
					outValue = (T)(object)result6;
					return true;
				}
			}
			else if ((object)typeFromHandle == typeof(float))
			{
				if (float.TryParse(outValue2, NumberStyles.Any, CultureInfo.InvariantCulture, out var result7))
				{
					outValue = (T)(object)result7;
					return true;
				}
			}
			else if ((object)typeFromHandle == typeof(double))
			{
				if (double.TryParse(outValue2, NumberStyles.Any, CultureInfo.InvariantCulture, out var result8))
				{
					outValue = (T)(object)result8;
					return true;
				}
			}
			else if ((object)typeFromHandle == typeof(decimal))
			{
				if (decimal.TryParse(outValue2, NumberStyles.Any, CultureInfo.InvariantCulture, out var result9))
				{
					outValue = (T)(object)result9;
					return true;
				}
			}
			else if ((object)typeFromHandle == typeof(bool))
			{
				if (bool.TryParse(outValue2, out var result10))
				{
					outValue = (T)(object)result10;
					return true;
				}
			}
			else
			{
				if (!ReflectionTools.DoesTypeImplement(typeFromHandle, typeof(Enum)))
				{
					throw new NotImplementedException();
				}
				if ((object)Enum.GetUnderlyingType(typeFromHandle) != typeof(int))
				{
					throw new NotImplementedException("Only INT enums are currently supported!");
				}
				if (int.TryParse(outValue2, out var result11))
				{
					outValue = (T)(object)result11;
					return true;
				}
			}
			return true;
		}

		public static bool TryReadXmlElement<T>(XmlReader reader, string name, out T outValue, T defaultValue)
		{
			if (!TryReadXmlElement(reader, name, out outValue))
			{
				outValue = defaultValue;
				return false;
			}
			return true;
		}

		public static bool TryReadXmlStartElement(XmlReader reader, string name, out bool isEmpty)
		{
			isEmpty = reader.IsEmptyElement;
			try
			{
				reader.ReadStartElement(name);
			}
			catch
			{
				return false;
			}
			return true;
		}

		public static bool TryReadXmlEndElement(XmlReader reader)
		{
			try
			{
				reader.ReadEndElement();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static string CleanInvalidXmlChars(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return string.Empty;
			}
			try
			{
				string pattern = "[^\\x09\\x0A\\x0D\\x20-\\xD7FF\\xE000-\\xFFFD\\x10000-x10FFFF]";
				return Regex.Replace(text, pattern, "");
			}
			catch
			{
				return string.Empty;
			}
		}
	}
}
