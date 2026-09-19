using System;
using RSG.Muffin.JsonConvertorSubmodule.JsonConvertorModule.Core.Interfaces;
using RSG.Muffin.JsonConvertorSubmodule.JsonConvertorModule.Core.JsonExtensions;

namespace RSG.Muffin.JsonConvertorSubmodule.JsonConvertorModule.Core.Implementation.JsonConvertor
{
	public class JsonConvertor : IJsonConvertor
	{
		public string Convert(object dataObject)
		{
			return dataObject.ToJson();
		}

		public object Convert(string dataString, Type type)
		{
			return dataString.FromJson(type);
		}

		public T Convert<T>(string dataString)
		{
			return dataString.FromJson<T>();
		}

		public void ConvertDataFromStringOverwrite(string dataString, object objectToOverwrite)
		{
			dataString.FromJsonOverwrite(objectToOverwrite);
		}
	}
}
