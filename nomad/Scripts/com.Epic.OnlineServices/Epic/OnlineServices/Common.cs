namespace Epic.OnlineServices
{
	public sealed class Common
	{
		public static bool IsOperationComplete(Result result)
		{
			Helper.Get(Bindings.EOS_EResult_IsOperationComplete(result), out bool to);
			return to;
		}
	}
}
