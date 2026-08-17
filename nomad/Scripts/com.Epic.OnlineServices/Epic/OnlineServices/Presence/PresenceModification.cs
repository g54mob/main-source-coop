namespace Epic.OnlineServices.Presence
{
	public sealed class PresenceModification : Handle
	{
		public Result SetRawRichText(ref PresenceModificationSetRawRichTextOptions options)
		{
			PresenceModificationSetRawRichTextOptionsInternal options2 = default(PresenceModificationSetRawRichTextOptionsInternal);
			options2.Set(ref options);
			Result result = Bindings.EOS_PresenceModification_SetRawRichText(base.InnerHandle, ref options2);
			Helper.Dispose(ref options2);
			return result;
		}

		public Result SetStatus(ref PresenceModificationSetStatusOptions options)
		{
			PresenceModificationSetStatusOptionsInternal options2 = default(PresenceModificationSetStatusOptionsInternal);
			options2.Set(ref options);
			Result result = Bindings.EOS_PresenceModification_SetStatus(base.InnerHandle, ref options2);
			Helper.Dispose(ref options2);
			return result;
		}
	}
}
