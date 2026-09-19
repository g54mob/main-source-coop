using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Google.Apis.Logging;
using Google.Apis.Util;

namespace Google.Apis.Requests.Parameters
{
	public static class ParameterUtils
	{
		private static readonly ILogger Logger = ApplicationContext.Logger.ForType(typeof(ParameterUtils));

		public static FormUrlEncodedContent CreateFormUrlEncodedContent(object request)
		{
			IList<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
			IterateParameters(request, delegate(RequestParameterType type, string name, object value)
			{
				list.Add(new KeyValuePair<string, string>(name, value.ToString()));
			});
			return new FormUrlEncodedContent(list);
		}

		public static IDictionary<string, object> CreateParameterDictionary(object request)
		{
			Dictionary<string, object> dict = new Dictionary<string, object>();
			IterateParameters(request, delegate(RequestParameterType type, string name, object value)
			{
				if (dict.TryGetValue(name, out var value2))
				{
					if (value2 == null && value != null)
					{
						dict[name] = value;
					}
					else if (value != null)
					{
						throw new InvalidOperationException("The query parameter '" + name + "' is set by multiple properties. For repeated enum query parameters, ensure that only one property is set to a non-null value.");
					}
				}
				else
				{
					dict.Add(name, value);
				}
			});
			return dict;
		}

		public static void InitParameters(RequestBuilder builder, object request)
		{
			IterateParameters(request, delegate(RequestParameterType type, string name, object value)
			{
				builder.AddParameter(type, name, value.ToString());
			});
		}

		private static void IterateParameters(object request, Action<RequestParameterType, string, object> action)
		{
			PropertyInfo[] properties = request.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (!(propertyInfo.GetCustomAttributes(typeof(RequestParameterAttribute), inherit: false).FirstOrDefault() is RequestParameterAttribute requestParameterAttribute))
				{
					continue;
				}
				string arg = requestParameterAttribute.Name ?? propertyInfo.Name.ToLower();
				Type propertyType = propertyInfo.PropertyType;
				object value = propertyInfo.GetValue(request, null);
				if (!propertyType.GetTypeInfo().IsValueType && value == null)
				{
					continue;
				}
				if (requestParameterAttribute.Type == RequestParameterType.UserDefinedQueries)
				{
					if (typeof(IEnumerable<KeyValuePair<string, string>>).IsAssignableFrom(value.GetType()))
					{
						foreach (KeyValuePair<string, string> item in (IEnumerable<KeyValuePair<string, string>>)value)
						{
							action(RequestParameterType.Query, item.Key, item.Value);
						}
					}
					else
					{
						Logger.Warning("Parameter marked with RequestParameterType.UserDefinedQueries attribute was not of type IEnumerable<KeyValuePair<string, string>> and will be skipped.");
					}
				}
				else
				{
					action(requestParameterAttribute.Type, arg, value);
				}
			}
		}
	}
}
