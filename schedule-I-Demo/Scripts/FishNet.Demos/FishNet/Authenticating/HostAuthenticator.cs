using System;
using System.Security.Cryptography;
using System.Text;
using FishNet.Connection;
using FishNet.Example.Authenticating;
using FishNet.Managing;
using FishNet.Transporting;
using UnityEngine;

namespace FishNet.Authenticating
{
	public abstract class HostAuthenticator : Authenticator
	{
		[Tooltip("True to enable use of AuthenticateAsHost.")]
		[SerializeField]
		private bool _allowHostAuthentication;

		private static string _hostHash = string.Empty;

		public void SetAllowHostAuthentication(bool value)
		{
			_allowHostAuthentication = value;
		}

		public bool GetAllowHostAuthentication()
		{
			return _allowHostAuthentication;
		}

		public override void InitializeOnce(NetworkManager networkManager)
		{
			base.InitializeOnce(networkManager);
			base.NetworkManager.ServerManager.OnServerConnectionState += ServerManager_OnServerConnectionState;
			base.NetworkManager.ServerManager.RegisterBroadcast<HostPasswordBroadcast>(OnHostPasswordBroadcast, requireAuthentication: false);
		}

		private void ServerManager_OnServerConnectionState(ServerConnectionStateArgs obj)
		{
			int hostHash = ((obj.ConnectionState == LocalConnectionState.Started) ? 25 : 0);
			SetHostHash(hostHash);
		}

		private void OnHostPasswordBroadcast(NetworkConnection conn, HostPasswordBroadcast hpb)
		{
			if (!_allowHostAuthentication)
			{
				conn.Disconnect(immediately: true);
				return;
			}
			if (conn.Authenticated)
			{
				conn.Disconnect(immediately: true);
				return;
			}
			bool authenticated = hpb.Password == _hostHash;
			OnHostAuthenticationResult(conn, authenticated);
		}

		protected abstract void OnHostAuthenticationResult(NetworkConnection conn, bool authenticated);

		private void SetHostHash(int length)
		{
			if (length <= 0)
			{
				_hostHash = string.Empty;
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			using (RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider())
			{
				byte[] array = new byte[4];
				while (length-- > 0)
				{
					rNGCryptoServiceProvider.GetBytes(array);
					uint num = BitConverter.ToUInt32(array, 0);
					stringBuilder.Append("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()"[(int)(num % (uint)"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()".Length)]);
				}
			}
			_hostHash = stringBuilder.ToString();
		}

		protected bool AuthenticateAsHost()
		{
			if (!_allowHostAuthentication)
			{
				return false;
			}
			if (_hostHash == string.Empty)
			{
				return false;
			}
			HostPasswordBroadcast message = new HostPasswordBroadcast
			{
				Password = _hostHash
			};
			base.NetworkManager.ClientManager.Broadcast(message);
			return true;
		}
	}
}
