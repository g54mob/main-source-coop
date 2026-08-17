using System;
using FishNet.Authenticating;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Transporting;
using UnityEngine;

namespace FishNet.Example.Authenticating
{
	public class PasswordAuthenticator : HostAuthenticator
	{
		[Tooltip("Password to authenticate.")]
		[SerializeField]
		private string _password = "HelloWorld";

		public override event Action<NetworkConnection, bool> OnAuthenticationResult;

		public override void InitializeOnce(NetworkManager networkManager)
		{
			base.InitializeOnce(networkManager);
			base.NetworkManager.ClientManager.OnClientConnectionState += ClientManager_OnClientConnectionState;
			base.NetworkManager.ServerManager.RegisterBroadcast<PasswordBroadcast>(OnPasswordBroadcast, requireAuthentication: false);
			base.NetworkManager.ClientManager.RegisterBroadcast<ResponseBroadcast>(OnResponseBroadcast);
		}

		private void ClientManager_OnClientConnectionState(ClientConnectionStateArgs args)
		{
			if (args.ConnectionState == LocalConnectionState.Started && !AuthenticateAsHost())
			{
				PasswordBroadcast message = new PasswordBroadcast
				{
					Password = _password
				};
				base.NetworkManager.ClientManager.Broadcast(message);
			}
		}

		private void OnPasswordBroadcast(NetworkConnection conn, PasswordBroadcast pb)
		{
			if (conn.Authenticated)
			{
				conn.Disconnect(immediately: true);
				return;
			}
			bool flag = pb.Password == _password;
			SendAuthenticationResponse(conn, flag);
			OnAuthenticationResult?.Invoke(conn, flag);
		}

		private void OnResponseBroadcast(ResponseBroadcast rb)
		{
			string value = (rb.Passed ? "Authentication complete." : "Authenitcation failed.");
			base.NetworkManager.Log(value);
		}

		private void SendAuthenticationResponse(NetworkConnection conn, bool authenticated)
		{
			ResponseBroadcast message = new ResponseBroadcast
			{
				Passed = authenticated
			};
			base.NetworkManager.ServerManager.Broadcast(conn, message, requireAuthenticated: false);
		}

		protected override void OnHostAuthenticationResult(NetworkConnection conn, bool authenticated)
		{
			SendAuthenticationResponse(conn, authenticated);
			OnAuthenticationResult?.Invoke(conn, authenticated);
		}
	}
}
