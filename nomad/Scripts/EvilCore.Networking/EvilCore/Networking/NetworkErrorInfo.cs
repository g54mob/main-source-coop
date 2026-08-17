namespace EvilCore.Networking
{
	public readonly struct NetworkErrorInfo
	{
		public readonly NetworkErrorType Type;

		public readonly string TechnicalDetail;

		public NetworkErrorInfo(NetworkErrorType type, string technicalDetail = null)
		{
			Type = type;
			TechnicalDetail = technicalDetail;
		}
	}
}
