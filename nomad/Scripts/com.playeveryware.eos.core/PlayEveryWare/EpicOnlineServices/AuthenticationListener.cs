using System;
using Epic.OnlineServices;
using Epic.OnlineServices.Auth;
using Epic.OnlineServices.Connect;
using UnityEngine;

namespace PlayEveryWare.EpicOnlineServices
{
	public class AuthenticationListener : IAuthInterfaceEventListener, IEOSOnAuthLogin, IEOSOnAuthLogout, IConnectInterfaceEventListener, IEOSOnConnectLogin, IDisposable
	{
		public enum LoginChangeKind
		{
			Auth = 0,
			Connect = 1
		}

		public delegate void AuthenticationChangedEventHandler(bool authenticated, LoginChangeKind changeType);

		private static readonly Lazy<AuthenticationListener> s_LazyInstance = new Lazy<AuthenticationListener>(() => new AuthenticationListener());

		private bool? _isAuthenticated;

		public static AuthenticationListener Instance => s_LazyInstance.Value;

		public bool IsAuthenticated
		{
			get
			{
				if (_isAuthenticated.HasValue)
				{
					return _isAuthenticated.Value;
				}
				return false;
			}
		}

		public event AuthenticationChangedEventHandler AuthenticationChanged;

		private AuthenticationListener()
		{
			EOSManager.Instance.AddAuthLoginListener(this);
			EOSManager.Instance.AddAuthLogoutListener(this);
			EOSManager.Instance.AddConnectLoginListener(this);
		}

		private void TriggerAuthenticationChangedEvent(bool attemptedState, Result attemptResult, LoginChangeKind changeType)
		{
			if (attemptResult != Result.Success)
			{
				Debug.LogWarning($"Authentication change attempt failed with following result code: {attemptResult}");
				return;
			}
			_isAuthenticated = attemptedState;
			this.AuthenticationChanged?.Invoke(attemptedState, changeType);
		}

		public void OnAuthLogin(Epic.OnlineServices.Auth.LoginCallbackInfo loginCallbackInfo)
		{
			TriggerAuthenticationChangedEvent(attemptedState: true, loginCallbackInfo.ResultCode, LoginChangeKind.Auth);
		}

		public void OnAuthLogout(LogoutCallbackInfo logoutCallbackInfo)
		{
			TriggerAuthenticationChangedEvent(attemptedState: false, logoutCallbackInfo.ResultCode, LoginChangeKind.Auth);
		}

		public void OnConnectLogin(Epic.OnlineServices.Connect.LoginCallbackInfo loginCallbackInfo)
		{
			TriggerAuthenticationChangedEvent(attemptedState: true, loginCallbackInfo.ResultCode, LoginChangeKind.Connect);
		}

		public void Dispose()
		{
			EOSManager.Instance.RemoveAuthLoginListener(this);
			EOSManager.Instance.RemoveAuthLogoutListener(this);
			EOSManager.Instance.RemoveConnectLoginListener(this);
		}
	}
}
