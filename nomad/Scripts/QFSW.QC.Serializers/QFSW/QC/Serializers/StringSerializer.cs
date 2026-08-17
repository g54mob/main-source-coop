namespace QFSW.QC.Serializers
{
	public class StringSerializer : BasicQcSerializer<string>
	{
		public override int Priority => 2147483647;

		public override string SerializeFormatted(string value, QuantumTheme theme)
		{
			return value;
		}
	}
}
