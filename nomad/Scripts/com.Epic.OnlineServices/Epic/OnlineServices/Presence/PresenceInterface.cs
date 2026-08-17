using System;

namespace Epic.OnlineServices.Presence
{
	public sealed class PresenceInterface : Handle
	{
		public static readonly Utf8String KEY_PLATFORM_PRESENCE = "EOS_PlatformPresence";

		public Result CreatePresenceModification(ref CreatePresenceModificationOptions options, out PresenceModification outPresenceModificationHandle)
		{
			CreatePresenceModificationOptionsInternal options2 = default(CreatePresenceModificationOptionsInternal);
			options2.Set(ref options);
			IntPtr outPresenceModificationHandle2 = IntPtr.Zero;
			Result result = Bindings.EOS_Presence_CreatePresenceModification(base.InnerHandle, ref options2, out outPresenceModificationHandle2);
			Helper.Dispose(ref options2);
			Helper.Get(outPresenceModificationHandle2, out outPresenceModificationHandle);
			return result;
		}

		public void SetPresence(ref SetPresenceOptions options, object clientData, SetPresenceCompleteCallback completionDelegate)
		{
			if (completionDelegate == null)
			{
				throw new ArgumentNullException("completionDelegate");
			}
			SetPresenceOptionsInternal options2 = default(SetPresenceOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, completionDelegate);
			Bindings.EOS_Presence_SetPresence(base.InnerHandle, ref options2, clientDataPointer, SetPresenceCompleteCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
		}
	}
}
