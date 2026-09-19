using System;

namespace Global.Modules.LocalizationModule.Scripts.Generated
{
	public class LanguageMapper
	{
		public string GetStringByLanguage(ILocalizationDataHolder dataHolder, Language language)
		{
			return language switch
			{
				Language.English => dataHolder.English, 
				Language.Ukrainian => dataHolder.Ukrainian, 
				Language.French => dataHolder.French, 
				Language.Italian => dataHolder.Italian, 
				Language.German => dataHolder.German, 
				Language.SpanishSpain => dataHolder.SpanishSpain, 
				Language.Polish => dataHolder.Polish, 
				Language.Swedish => dataHolder.Swedish, 
				Language.PortugueseBrazil => dataHolder.PortugueseBrazil, 
				Language.TraditionalChinese => dataHolder.TraditionalChinese, 
				Language.SimplifiedChinese => dataHolder.SimplifiedChinese, 
				Language.Korean => dataHolder.Korean, 
				Language.Japanese => dataHolder.Japanese, 
				Language.Turkish => dataHolder.Turkish, 
				Language.Hungarian => dataHolder.Hungarian, 
				Language.Thai => dataHolder.Thai, 
				Language.Arabic => dataHolder.Arabic, 
				Language.Indonesian => dataHolder.Indonesian, 
				Language.Vietnamese => dataHolder.Vietnamese, 
				Language.LatinAmericaSpain => dataHolder.LatinAmericaSpain, 
				_ => throw new Exception("There is no such binded language"), 
			};
		}
	}
}
