using System;

namespace Epic.OnlineServices.UI
{
	public sealed class UIInterface : Handle
	{
		public ulong AddNotifyDisplaySettingsUpdated(ref AddNotifyDisplaySettingsUpdatedOptions options, object clientData, OnDisplaySettingsUpdatedCallback notificationFn)
		{
			if (notificationFn == null)
			{
				throw new ArgumentNullException("notificationFn");
			}
			AddNotifyDisplaySettingsUpdatedOptionsInternal options2 = default(AddNotifyDisplaySettingsUpdatedOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, notificationFn);
			ulong num = Bindings.EOS_UI_AddNotifyDisplaySettingsUpdated(base.InnerHandle, ref options2, clientDataPointer, OnDisplaySettingsUpdatedCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
			Helper.AssignNotificationIdToCallback(clientDataPointer, num);
			return num;
		}

		public Result SetDisplayPreference(ref SetDisplayPreferenceOptions options)
		{
			SetDisplayPreferenceOptionsInternal options2 = default(SetDisplayPreferenceOptionsInternal);
			options2.Set(ref options);
			Result result = Bindings.EOS_UI_SetDisplayPreference(base.InnerHandle, ref options2);
			Helper.Dispose(ref options2);
			return result;
		}

		public Result SetToggleFriendsButton(ref SetToggleFriendsButtonOptions options)
		{
			SetToggleFriendsButtonOptionsInternal options2 = default(SetToggleFriendsButtonOptionsInternal);
			options2.Set(ref options);
			Result result = Bindings.EOS_UI_SetToggleFriendsButton(base.InnerHandle, ref options2);
			Helper.Dispose(ref options2);
			return result;
		}
	}
}
