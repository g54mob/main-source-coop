using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Photon.Pun;
using Photon.Realtime;
using Steamworks;
using UnityEngine;

namespace Portningsbolaget.Photon
{
	public class SteamPhotonAuthManager : IPhotonAuth
	{
		private struct AuthenticatedObj
		{
			public Action Callback;
		}

		private object m_OnAuthenticatedLock = new object();

		private Queue<AuthenticatedObj> m_OnAuthenticatedQueue = new Queue<AuthenticatedObj>();

		public HAuthTicket AuthTicket { get; private set; }

		public IEnumerator Authenticate(Action onAuthenticated)
		{
			HAuthTicket ticket;
			string steamAuthTicket = GetSteamAuthTicket(out ticket);
			AuthTicket = ticket;
			PhotonNetwork.AuthValues = new AuthenticationValues
			{
				UserId = SteamUser.GetSteamID().ToString(),
				AuthType = CustomAuthenticationType.Steam
			};
			Debug.Log("Steam auth ticket received, adding it to PhotonNetwork");
			PhotonNetwork.AuthValues.AddAuthParameter("ticket", steamAuthTicket);
			lock (m_OnAuthenticatedLock)
			{
				m_OnAuthenticatedQueue.Enqueue(new AuthenticatedObj
				{
					Callback = onAuthenticated
				});
			}
			yield return null;
		}

		public void Update()
		{
			lock (m_OnAuthenticatedLock)
			{
				while (m_OnAuthenticatedQueue.Count > 0)
				{
					m_OnAuthenticatedQueue.Dequeue().Callback?.Invoke();
				}
			}
		}

		public void TearDown()
		{
		}

		public static string GetSteamAuthTicket(out HAuthTicket ticket)
		{
			byte[] array = new byte[1024];
			CSteamID steamID = SteamUser.GetSteamID();
			SteamNetworkingIdentity pSteamNetworkingIdentity = default(SteamNetworkingIdentity);
			pSteamNetworkingIdentity.SetSteamID(steamID);
			ticket = SteamUser.GetAuthSessionTicket(array, array.Length, out var pcbTicket, ref pSteamNetworkingIdentity);
			Array.Resize(ref array, (int)pcbTicket);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < pcbTicket; i++)
			{
				stringBuilder.AppendFormat("{0:x2}", array[i]);
			}
			return stringBuilder.ToString();
		}

		public static void CancelAuthTicket(HAuthTicket ticket)
		{
			SteamUser.CancelAuthTicket(ticket);
		}
	}
}
