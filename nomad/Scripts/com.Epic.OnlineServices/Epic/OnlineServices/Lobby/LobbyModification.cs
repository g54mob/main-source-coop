namespace Epic.OnlineServices.Lobby
{
	public sealed class LobbyModification : Handle
	{
		public Result AddAttribute(ref LobbyModificationAddAttributeOptions options)
		{
			LobbyModificationAddAttributeOptionsInternal options2 = default(LobbyModificationAddAttributeOptionsInternal);
			options2.Set(ref options);
			Result result = Bindings.EOS_LobbyModification_AddAttribute(base.InnerHandle, ref options2);
			Helper.Dispose(ref options2);
			return result;
		}

		public Result AddMemberAttribute(ref LobbyModificationAddMemberAttributeOptions options)
		{
			LobbyModificationAddMemberAttributeOptionsInternal options2 = default(LobbyModificationAddMemberAttributeOptionsInternal);
			options2.Set(ref options);
			Result result = Bindings.EOS_LobbyModification_AddMemberAttribute(base.InnerHandle, ref options2);
			Helper.Dispose(ref options2);
			return result;
		}

		public void Release()
		{
			Bindings.EOS_LobbyModification_Release(base.InnerHandle);
		}
	}
}
