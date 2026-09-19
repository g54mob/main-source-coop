using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using MessagePack.Internal;

namespace MessagePack.Formatters
{
	[Preserve]
	public sealed class EnumAsStringFormatter<T> : IMessagePackFormatter<T>, IMessagePackFormatter where T : struct, Enum
	{
		private readonly bool ignoreCase;

		private readonly IReadOnlyDictionary<string, T> nameValueMapping;

		private readonly IReadOnlyDictionary<T, string> valueNameMapping;

		private readonly IReadOnlyDictionary<string, string>? clrToSerializationName;

		private readonly IReadOnlyDictionary<string, string>? serializationToClrName;

		private readonly bool isFlags;

		public EnumAsStringFormatter()
			: this(false)
		{
		}

		public EnumAsStringFormatter(bool ignoreCase)
		{
			this.ignoreCase = ignoreCase;
			StringComparer comparer = (ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
			isFlags = typeof(T).GetCustomAttribute<FlagsAttribute>() != null;
			FieldInfo[] fields = typeof(T).GetFields(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
			Dictionary<string, T> dictionary = new Dictionary<string, T>(fields.Length, ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
			Dictionary<T, string> dictionary2 = new Dictionary<T, string>();
			Dictionary<string, string> dictionary3 = null;
			Dictionary<string, string> dictionary4 = null;
			FieldInfo[] array = fields;
			foreach (FieldInfo obj in array)
			{
				string text = obj.Name;
				T val = (T)obj.GetValue(null);
				EnumMemberAttribute customAttribute = obj.GetCustomAttribute<EnumMemberAttribute>();
				if (customAttribute != null && customAttribute.IsValueSetExplicitly && customAttribute.Value != null)
				{
					if (dictionary3 == null)
					{
						dictionary3 = new Dictionary<string, string>(comparer);
					}
					if (dictionary4 == null)
					{
						dictionary4 = new Dictionary<string, string>(comparer);
					}
					dictionary3.Add(text, customAttribute.Value);
					dictionary4.Add(customAttribute.Value, text);
					text = customAttribute.Value;
				}
				dictionary[text] = val;
				dictionary2[val] = text;
			}
			nameValueMapping = dictionary;
			valueNameMapping = dictionary2;
			clrToSerializationName = dictionary3;
			serializationToClrName = dictionary4;
		}

		public void Serialize(ref MessagePackWriter writer, T value, MessagePackSerializerOptions options)
		{
			if (!valueNameMapping.TryGetValue(value, out string value2))
			{
				value2 = GetSerializedNames(value.ToString());
			}
			writer.Write(value2);
		}

		public T Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
		{
			string text = reader.ReadString();
			if (text == null)
			{
				MessagePackSerializationException.ThrowUnexpectedNilWhileDeserializing<T>();
			}
			if (!nameValueMapping.TryGetValue(text, out var value))
			{
				return (T)Enum.Parse(typeof(T), GetClrNames(text), ignoreCase);
			}
			return value;
		}

		private string GetClrNames(string serializedNames)
		{
			if (serializationToClrName != null && isFlags && serializedNames.IndexOf(", ", StringComparison.Ordinal) >= 0)
			{
				return Translate(serializedNames, serializationToClrName);
			}
			return serializedNames;
		}

		private string GetSerializedNames(string clrNames)
		{
			if (clrToSerializationName != null && isFlags && clrNames.IndexOf(", ", StringComparison.Ordinal) >= 0)
			{
				return Translate(clrNames, clrToSerializationName);
			}
			return clrNames;
		}

		private static string Translate(string items, IReadOnlyDictionary<string, string> mapping)
		{
			string[] array = items.Split(',');
			for (int i = 0; i < array.Length; i = checked(i + 1))
			{
				if (i > 0 && array[i].Length > 0 && array[i][0] == ' ')
				{
					array[i] = array[i].Substring(1);
				}
				if (mapping.TryGetValue(array[i], out string value))
				{
					array[i] = value;
				}
			}
			return string.Join(", ", array);
		}
	}
}
