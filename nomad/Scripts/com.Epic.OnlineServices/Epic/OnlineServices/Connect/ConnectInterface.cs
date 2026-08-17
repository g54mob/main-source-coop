using System;

namespace Epic.OnlineServices.Connect
{
	public sealed class ConnectInterface : Handle
	{
		public ulong AddNotifyAuthExpiration(ref AddNotifyAuthExpirationOptions options, object clientData, OnAuthExpirationCallback notification)
		{
			if (notification == null)
			{
				throw new ArgumentNullException("notification");
			}
			AddNotifyAuthExpirationOptionsInternal options2 = default(AddNotifyAuthExpirationOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, notification);
			ulong num = Bindings.EOS_Connect_AddNotifyAuthExpiration(base.InnerHandle, ref options2, clientDataPointer, OnAuthExpirationCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
			Helper.AssignNotificationIdToCallback(clientDataPointer, num);
			return num;
		}

		public ulong AddNotifyLoginStatusChanged(ref AddNotifyLoginStatusChangedOptions options, object clientData, OnLoginStatusChangedCallback notification)
		{
			if (notification == null)
			{
				throw new ArgumentNullException("notification");
			}
			AddNotifyLoginStatusChangedOptionsInternal options2 = default(AddNotifyLoginStatusChangedOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, notification);
			ulong num = Bindings.EOS_Connect_AddNotifyLoginStatusChanged(base.InnerHandle, ref options2, clientDataPointer, OnLoginStatusChangedCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
			Helper.AssignNotificationIdToCallback(clientDataPointer, num);
			return num;
		}

		public void CreateDeviceId(ref CreateDeviceIdOptions options, object clientData, OnCreateDeviceIdCallback completionDelegate)
		{
			if (completionDelegate == null)
			{
				throw new ArgumentNullException("completionDelegate");
			}
			CreateDeviceIdOptionsInternal options2 = default(CreateDeviceIdOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, completionDelegate);
			Bindings.EOS_Connect_CreateDeviceId(base.InnerHandle, ref options2, clientDataPointer, OnCreateDeviceIdCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
		}

		public void CreateUser(ref CreateUserOptions options, object clientData, OnCreateUserCallback completionDelegate)
		{
			if (completionDelegate == null)
			{
				throw new ArgumentNullException("completionDelegate");
			}
			CreateUserOptionsInternal options2 = default(CreateUserOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, completionDelegate);
			Bindings.EOS_Connect_CreateUser(base.InnerHandle, ref options2, clientDataPointer, OnCreateUserCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
		}

		public void LinkAccount(ref LinkAccountOptions options, object clientData, OnLinkAccountCallback completionDelegate)
		{
			if (completionDelegate == null)
			{
				throw new ArgumentNullException("completionDelegate");
			}
			LinkAccountOptionsInternal options2 = default(LinkAccountOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, completionDelegate);
			Bindings.EOS_Connect_LinkAccount(base.InnerHandle, ref options2, clientDataPointer, OnLinkAccountCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
		}

		public void Login(ref LoginOptions options, object clientData, OnLoginCallback completionDelegate)
		{
			if (completionDelegate == null)
			{
				throw new ArgumentNullException("completionDelegate");
			}
			LoginOptionsInternal options2 = default(LoginOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, completionDelegate);
			Bindings.EOS_Connect_Login(base.InnerHandle, ref options2, clientDataPointer, OnLoginCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
		}

		public void RemoveNotifyAuthExpiration(ulong inId)
		{
			Bindings.EOS_Connect_RemoveNotifyAuthExpiration(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		public void RemoveNotifyLoginStatusChanged(ulong inId)
		{
			Bindings.EOS_Connect_RemoveNotifyLoginStatusChanged(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}

		public void TransferDeviceIdAccount(ref TransferDeviceIdAccountOptions options, object clientData, OnTransferDeviceIdAccountCallback completionDelegate)
		{
			if (completionDelegate == null)
			{
				throw new ArgumentNullException("completionDelegate");
			}
			TransferDeviceIdAccountOptionsInternal options2 = default(TransferDeviceIdAccountOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, completionDelegate);
			Bindings.EOS_Connect_TransferDeviceIdAccount(base.InnerHandle, ref options2, clientDataPointer, OnTransferDeviceIdAccountCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
		}
	}
}
