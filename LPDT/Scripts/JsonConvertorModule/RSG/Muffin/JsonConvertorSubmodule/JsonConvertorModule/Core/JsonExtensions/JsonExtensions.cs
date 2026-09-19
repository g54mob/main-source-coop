using System;
using UnityEngine;

namespace RSG.Muffin.JsonConvertorSubmodule.JsonConvertorModule.Core.JsonExtensions
{
	public static class JsonExtensions
	{
		public static string ToJson(this object @object)
		{
			return JsonUtility.ToJson(@object);
		}

		public static object FromJson(this string data, Type type)
		{
			if (!string.IsNullOrEmpty(data))
			{
				return JsonUtility.FromJson(data, type);
			}
			return null;
		}

		public static T FromJson<T>(this string data)
		{
			return JsonUtility.FromJson<T>(data);
		}

		public static void FromJsonOverwrite(this string data, object objectToOverwrite)
		{
			JsonUtility.FromJsonOverwrite(data, objectToOverwrite);
		}
	}
}
