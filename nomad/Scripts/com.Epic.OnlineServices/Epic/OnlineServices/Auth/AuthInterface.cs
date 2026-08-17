using System;

namespace Epic.OnlineServices.Auth
{
	public sealed class AuthInterface : Handle
	{
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
			ulong num = Bindings.EOS_Auth_AddNotifyLoginStatusChanged(base.InnerHandle, ref options2, clientDataPointer, OnLoginStatusChangedCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
			Helper.AssignNotificationIdToCallback(clientDataPointer, num);
			return num;
		}

		public Result CopyIdToken(ref CopyIdTokenOptions options, out IdToken? outIdToken)
		{
			CopyIdTokenOptionsInternal options2 = default(CopyIdTokenOptionsInternal);
			options2.Set(ref options);
			IntPtr outIdToken2 = IntPtr.Zero;
			Result result = Bindings.EOS_Auth_CopyIdToken(base.InnerHandle, ref options2, out outIdToken2);
			Helper.Dispose(ref options2);
			Helper.Get<IdTokenInternal, IdToken>(outIdToken2, out outIdToken);
			if (outIdToken2 != IntPtr.Zero)
			{
				Bindings.EOS_Auth_IdToken_Release(outIdToken2);
			}
			return result;
		}

		public Result CopyUserAuthToken(ref CopyUserAuthTokenOptions options, EpicAccountId localUserId, out Token? outUserAuthToken)
		{
			CopyUserAuthTokenOptionsInternal options2 = default(CopyUserAuthTokenOptionsInternal);
			options2.Set(ref options);
			IntPtr outUserAuthToken2 = IntPtr.Zero;
			Result result = Bindings.EOS_Auth_CopyUserAuthToken(base.InnerHandle, ref options2, localUserId.InnerHandle, out outUserAuthToken2);
			Helper.Dispose(ref options2);
			Helper.Get<TokenInternal, Token>(outUserAuthToken2, out outUserAuthToken);
			if (outUserAuthToken2 != IntPtr.Zero)
			{
				Bindings.EOS_Auth_Token_Release(outUserAuthToken2);
			}
			return result;
		}

		public void DeletePersistentAuth(ref DeletePersistentAuthOptions options, object clientData, OnDeletePersistentAuthCallback completionDelegate)
		{
			if (completionDelegate == null)
			{
				throw new ArgumentNullException("completionDelegate");
			}
			DeletePersistentAuthOptionsInternal options2 = default(DeletePersistentAuthOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, completionDelegate);
			Bindings.EOS_Auth_DeletePersistentAuth(base.InnerHandle, ref options2, clientDataPointer, OnDeletePersistentAuthCallbackInternalImplementation.Delegate);
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
			Bindings.EOS_Auth_LinkAccount(base.InnerHandle, ref options2, clientDataPointer, OnLinkAccountCallbackInternalImplementation.Delegate);
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
			Bindings.EOS_Auth_Login(base.InnerHandle, ref options2, clientDataPointer, OnLoginCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
		}

		public void Logout(ref LogoutOptions options, object clientData, OnLogoutCallback completionDelegate)
		{
			if (completionDelegate == null)
			{
				throw new ArgumentNullException("completionDelegate");
			}
			LogoutOptionsInternal options2 = default(LogoutOptionsInternal);
			options2.Set(ref options);
			IntPtr clientDataPointer = IntPtr.Zero;
			Helper.AddCallback(out clientDataPointer, clientData, completionDelegate);
			Bindings.EOS_Auth_Logout(base.InnerHandle, ref options2, clientDataPointer, OnLogoutCallbackInternalImplementation.Delegate);
			Helper.Dispose(ref options2);
		}

		public void RemoveNotifyLoginStatusChanged(ulong inId)
		{
			Bindings.EOS_Auth_RemoveNotifyLoginStatusChanged(base.InnerHandle, inId);
			Helper.RemoveCallbackByNotificationId(inId);
		}
	}
}
