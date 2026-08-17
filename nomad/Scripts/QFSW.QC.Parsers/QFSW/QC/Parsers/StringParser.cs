namespace QFSW.QC.Parsers
{
	public class StringParser : BasicCachedQcParser<string>
	{
		public override int Priority => 2147483647;

		public override string Parse(string value)
		{
			return value.ReduceScope('"', '"').UnescapeText('"');
		}
	}
}
