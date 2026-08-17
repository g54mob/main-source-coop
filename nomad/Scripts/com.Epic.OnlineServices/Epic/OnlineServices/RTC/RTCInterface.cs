using System;
using Epic.OnlineServices.RTCAudio;

namespace Epic.OnlineServices.RTC
{
	public sealed class RTCInterface : Handle
	{
		public ulong AddNotifyParticipantStatusChanged(ref AddNotifyParticipantStatusChangedOptions options, object clientData, OnParticipantStatusChangedCallback completionDelegate)
		{
			if (completionDelegate == null)
			{
				throw new ArgumentNullException("completionDelegate");
			}
			AddNotifyParticipantStatusChangedOptionsInternal options2 = default(AddNotifyParticipantStatusChangedOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, completionDelegate);
			ulong num = Bindings.EOS_RTC_AddNotifyParticipantStatusChanged(base.InnerHandle, ref options2, clientDataPointer, OnParticipantStatusChangedCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
			Helper.AssignNotificationIdToCallback(clientDataPointer, num);
			return num;
		}

		public RTCAudioInterface GetAudioInterface()
		{
			Helper.Get(Bindings.EOS_RTC_GetAudioInterface(base.InnerHandle), out RTCAudioInterface to);
			return to;
		}

		public void RemoveNotifyParticipantStatusChanged(ulong notificationId)
		{
			Bindings.EOS_RTC_RemoveNotifyParticipantStatusChanged(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}
	}
}
