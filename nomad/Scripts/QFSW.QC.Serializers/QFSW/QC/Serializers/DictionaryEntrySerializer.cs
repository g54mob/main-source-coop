using System.Collections;

namespace QFSW.QC.Serializers
{
	public class DictionaryEntrySerializer : BasicQcSerializer<DictionaryEntry>
	{
		public override string SerializeFormatted(DictionaryEntry value, QuantumTheme theme)
		{
			string text = SerializeRecursive(value.Key, theme);
			string text2 = SerializeRecursive(value.Value, theme);
			return text + ": " + text2;
		}
	}
}
