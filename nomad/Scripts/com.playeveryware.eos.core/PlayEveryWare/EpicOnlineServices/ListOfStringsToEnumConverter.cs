using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlayEveryWare.EpicOnlineServices.Utility;
using UnityEngine;

namespace PlayEveryWare.EpicOnlineServices
{
	internal abstract class ListOfStringsToEnumConverter<TEnum> : JsonConverter where TEnum : struct, Enum
	{
		private readonly Type _targetType = typeof(TEnum);

		public override bool CanConvert(Type objectType)
		{
			return typeof(TEnum).IsEnum;
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JToken jToken = JToken.Load(reader);
			return jToken.Type switch
			{
				JTokenType.Array => FromStringArray(jToken as JArray), 
				JTokenType.String => Enum.Parse(_targetType, jToken.Value<string>()), 
				JTokenType.Integer => FromNumberValue(jToken), 
				JTokenType.Null => default(TEnum), 
				_ => throw new JsonSerializationException($"Unexpected token type '{jToken.Type}' when " + $"parsing to type '{_targetType}'."), 
			};
		}

		protected abstract TEnum FromStringArray(JArray array);

		protected TEnum FromNumberValue(JToken token)
		{
			Type underlyingType = Enum.GetUnderlyingType(typeof(TEnum));
			object obj = Convert.ChangeType(token, underlyingType);
			TEnum lowest = EnumUtility<TEnum>.GetLowest();
			TEnum highest = EnumUtility<TEnum>.GetHighest();
			object y = Convert.ChangeType(lowest, underlyingType);
			object y2 = Convert.ChangeType(highest, underlyingType);
			if (Comparer<object>.Default.Compare(obj, y) < 0 || Comparer<object>.Default.Compare(obj, y2) > 0)
			{
				Debug.LogWarning(string.Format("Value {0} is out of range for {1}, setting to 0.", obj, "TEnum"));
				return (TEnum)Enum.ToObject(typeof(TEnum), 0);
			}
			return (TEnum)obj;
		}

		protected TEnum FromStringArrayWithCustomMapping(JArray array, IDictionary<string, TEnum> customMappings)
		{
			List<string> list = new List<string>();
			foreach (JToken item in array)
			{
				list.Add(item.ToString());
			}
			EnumUtility<TEnum>.TryParse(list, customMappings, out var result);
			return result;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			serializer.Serialize(writer, value);
		}
	}
}
