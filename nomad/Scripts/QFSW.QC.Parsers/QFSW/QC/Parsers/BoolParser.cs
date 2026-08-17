namespace QFSW.QC.Parsers
{
	public class BoolParser : BasicCachedQcParser<bool>
	{
		public override bool Parse(string value)
		{
			value = value.ToLower().Trim();
			return value switch
			{
				"true" => true, 
				"on" => true, 
				"1" => true, 
				"yes" => true, 
				"false" => false, 
				"off" => false, 
				"0" => false, 
				"no" => false, 
				_ => throw new ParserInputException("Cannot parse '" + value + "' to a bool."), 
			};
		}
	}
}
