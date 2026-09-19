using System;
using System.Collections.Generic;
using RSG.Muffin.JsonConvertorSubmodule.JsonConvertorModule.Core.Implementation.JsonConvertor;
using RSG.Muffin.JsonConvertorSubmodule.JsonConvertorModule.Core.Interfaces;
using UnityEngine;

namespace RSG.Muffin.JsonConvertorSubmodule.JsonConvertorModule.Core.Implementation
{
	[HelpURL("https://coda.io/d/Muffin_drQAArgV92o/FileConvertorFactory_suv1E#_lu3G0")]
	public class FileConvertorFactory
	{
		private Dictionary<FileConvertionType, Func<IJsonConvertor>> _converterInitializerByFileConvertionTypeDictionary;

		public FileConvertorFactory()
		{
			InitInitializers();
		}

		public IJsonConvertor GetConverterByFileConvertionType(FileConvertionType type)
		{
			return _converterInitializerByFileConvertionTypeDictionary[type]?.Invoke();
		}

		private void InitInitializers()
		{
			JsonConvertorInitializer jsonConvertorInitializer = new JsonConvertorInitializer();
			_converterInitializerByFileConvertionTypeDictionary = new Dictionary<FileConvertionType, Func<IJsonConvertor>> { [FileConvertionType.Json] = jsonConvertorInitializer.Initialize };
		}
	}
}
