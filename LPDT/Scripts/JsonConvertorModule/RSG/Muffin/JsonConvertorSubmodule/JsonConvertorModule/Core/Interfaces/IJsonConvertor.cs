using System;

namespace RSG.Muffin.JsonConvertorSubmodule.JsonConvertorModule.Core.Interfaces
{
	public interface IJsonConvertor
	{
		string Convert(object dataObject);

		object Convert(string dataString, Type type);

		T Convert<T>(string dataString);

		void ConvertDataFromStringOverwrite(string dataString, object objectToOverwrite);
	}
}
