namespace Photon.Client
{
	public class OperationResponse
	{
		public byte OperationCode;

		public short ReturnCode;

		public string DebugMessage;

		public ParameterDictionary Parameters;

		public object this[byte parameterCode]
		{
			get
			{
				Parameters.TryGetValue(parameterCode, out var value);
				return value;
			}
			set
			{
				Parameters.Add(parameterCode, value);
			}
		}

		public override string ToString()
		{
			if (string.IsNullOrEmpty(DebugMessage))
			{
				return $"OperationResponse {OperationCode}: ReturnCode: {ReturnCode}.";
			}
			return $"OperationResponse {OperationCode}: ReturnCode: {ReturnCode}. Msg: \"{DebugMessage}\"";
		}

		public string ToStringFull()
		{
			return string.Format("OperationResponse {0}: ReturnCode: {1} ({3}). Parameters: {2}", OperationCode, ReturnCode, SupportClass.DictionaryToString(Parameters), DebugMessage);
		}
	}
}
