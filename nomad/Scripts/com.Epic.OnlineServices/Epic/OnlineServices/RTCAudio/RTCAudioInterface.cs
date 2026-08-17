using System;

namespace Epic.OnlineServices.RTCAudio
{
	public sealed class RTCAudioInterface : Handle
	{
		public ulong AddNotifyAudioDevicesChanged(ref AddNotifyAudioDevicesChangedOptions options, object clientData, OnAudioDevicesChangedCallback completionDelegate)
		{
			if (completionDelegate == null)
			{
				throw new ArgumentNullException("completionDelegate");
			}
			AddNotifyAudioDevicesChangedOptionsInternal options2 = default(AddNotifyAudioDevicesChangedOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, completionDelegate);
			ulong num = Bindings.EOS_RTCAudio_AddNotifyAudioDevicesChanged(base.InnerHandle, ref options2, clientDataPointer, OnAudioDevicesChangedCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
			Helper.AssignNotificationIdToCallback(clientDataPointer, num);
			return num;
		}

		public ulong AddNotifyParticipantUpdated(ref AddNotifyParticipantUpdatedOptions options, object clientData, OnParticipantUpdatedCallback completionDelegate)
		{
			if (completionDelegate == null)
			{
				throw new ArgumentNullException("completionDelegate");
			}
			AddNotifyParticipantUpdatedOptionsInternal options2 = default(AddNotifyParticipantUpdatedOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, completionDelegate);
			ulong num = Bindings.EOS_RTCAudio_AddNotifyParticipantUpdated(base.InnerHandle, ref options2, clientDataPointer, OnParticipantUpdatedCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
			Helper.AssignNotificationIdToCallback(clientDataPointer, num);
			return num;
		}

		public Result CopyInputDeviceInformationByIndex(ref CopyInputDeviceInformationByIndexOptions options, out InputDeviceInformation? outInputDeviceInformation)
		{
			CopyInputDeviceInformationByIndexOptionsInternal options2 = default(CopyInputDeviceInformationByIndexOptionsInternal);
			options2.Set(ref options);
			IntPtr outInputDeviceInformation2 = IntPtr.Zero;
			Result result = Bindings.EOS_RTCAudio_CopyInputDeviceInformationByIndex(base.InnerHandle, ref options2, out outInputDeviceInformation2);
			Helper.Dispose(ref options2);
			Helper.Get<InputDeviceInformationInternal, InputDeviceInformation>(outInputDeviceInformation2, out outInputDeviceInformation);
			if (outInputDeviceInformation2 != IntPtr.Zero)
			{
				Bindings.EOS_RTCAudio_InputDeviceInformation_Release(outInputDeviceInformation2);
			}
			return result;
		}

		public Result CopyOutputDeviceInformationByIndex(ref CopyOutputDeviceInformationByIndexOptions options, out OutputDeviceInformation? outOutputDeviceInformation)
		{
			CopyOutputDeviceInformationByIndexOptionsInternal options2 = default(CopyOutputDeviceInformationByIndexOptionsInternal);
			options2.Set(ref options);
			IntPtr outOutputDeviceInformation2 = IntPtr.Zero;
			Result result = Bindings.EOS_RTCAudio_CopyOutputDeviceInformationByIndex(base.InnerHandle, ref options2, out outOutputDeviceInformation2);
			Helper.Dispose(ref options2);
			Helper.Get<OutputDeviceInformationInternal, OutputDeviceInformation>(outOutputDeviceInformation2, out outOutputDeviceInformation);
			if (outOutputDeviceInformation2 != IntPtr.Zero)
			{
				Bindings.EOS_RTCAudio_OutputDeviceInformation_Release(outOutputDeviceInformation2);
			}
			return result;
		}

		public uint GetInputDevicesCount(ref GetInputDevicesCountOptions options)
		{
			GetInputDevicesCountOptionsInternal options2 = default(GetInputDevicesCountOptionsInternal);
			options2.Set(ref options);
			uint result = Bindings.EOS_RTCAudio_GetInputDevicesCount(base.InnerHandle, ref options2);
			Helper.Dispose(ref options2);
			return result;
		}

		public uint GetOutputDevicesCount(ref GetOutputDevicesCountOptions options)
		{
			GetOutputDevicesCountOptionsInternal options2 = default(GetOutputDevicesCountOptionsInternal);
			options2.Set(ref options);
			uint result = Bindings.EOS_RTCAudio_GetOutputDevicesCount(base.InnerHandle, ref options2);
			Helper.Dispose(ref options2);
			return result;
		}

		public void QueryInputDevicesInformation(ref QueryInputDevicesInformationOptions options, object clientData, OnQueryInputDevicesInformationCallback completionDelegate)
		{
			if (completionDelegate == null)
			{
				throw new ArgumentNullException("completionDelegate");
			}
			QueryInputDevicesInformationOptionsInternal options2 = default(QueryInputDevicesInformationOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, completionDelegate);
			Bindings.EOS_RTCAudio_QueryInputDevicesInformation(base.InnerHandle, ref options2, clientDataPointer, OnQueryInputDevicesInformationCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
		}

		public void QueryOutputDevicesInformation(ref QueryOutputDevicesInformationOptions options, object clientData, OnQueryOutputDevicesInformationCallback completionDelegate)
		{
			if (completionDelegate == null)
			{
				throw new ArgumentNullException("completionDelegate");
			}
			QueryOutputDevicesInformationOptionsInternal options2 = default(QueryOutputDevicesInformationOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, completionDelegate);
			Bindings.EOS_RTCAudio_QueryOutputDevicesInformation(base.InnerHandle, ref options2, clientDataPointer, OnQueryOutputDevicesInformationCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
		}

		public void RemoveNotifyAudioDevicesChanged(ulong notificationId)
		{
			Bindings.EOS_RTCAudio_RemoveNotifyAudioDevicesChanged(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		public void RemoveNotifyParticipantUpdated(ulong notificationId)
		{
			Bindings.EOS_RTCAudio_RemoveNotifyParticipantUpdated(base.InnerHandle, notificationId);
			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		public void SetInputDeviceSettings(ref SetInputDeviceSettingsOptions options, object clientData, OnSetInputDeviceSettingsCallback completionDelegate)
		{
			if (completionDelegate == null)
			{
				throw new ArgumentNullException("completionDelegate");
			}
			SetInputDeviceSettingsOptionsInternal options2 = default(SetInputDeviceSettingsOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, completionDelegate);
			Bindings.EOS_RTCAudio_SetInputDeviceSettings(base.InnerHandle, ref options2, clientDataPointer, OnSetInputDeviceSettingsCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
		}

		public void SetOutputDeviceSettings(ref SetOutputDeviceSettingsOptions options, object clientData, OnSetOutputDeviceSettingsCallback completionDelegate)
		{
			if (completionDelegate == null)
			{
				throw new ArgumentNullException("completionDelegate");
			}
			SetOutputDeviceSettingsOptionsInternal options2 = default(SetOutputDeviceSettingsOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, completionDelegate);
			Bindings.EOS_RTCAudio_SetOutputDeviceSettings(base.InnerHandle, ref options2, clientDataPointer, OnSetOutputDeviceSettingsCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
		}

		public void UpdateReceiving(ref UpdateReceivingOptions options, object clientData, OnUpdateReceivingCallback completionDelegate)
		{
			if (completionDelegate == null)
			{
				throw new ArgumentNullException("completionDelegate");
			}
			UpdateReceivingOptionsInternal options2 = default(UpdateReceivingOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, completionDelegate);
			Bindings.EOS_RTCAudio_UpdateReceiving(base.InnerHandle, ref options2, clientDataPointer, OnUpdateReceivingCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
		}

		public void UpdateSending(ref UpdateSendingOptions options, object clientData, OnUpdateSendingCallback completionDelegate)
		{
			if (completionDelegate == null)
			{
				throw new ArgumentNullException("completionDelegate");
			}
			UpdateSendingOptionsInternal options2 = default(UpdateSendingOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, completionDelegate);
			Bindings.EOS_RTCAudio_UpdateSending(base.InnerHandle, ref options2, clientDataPointer, OnUpdateSendingCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
		}
	}
}
