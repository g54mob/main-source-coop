using System;
using System.Collections;
using System.Collections.Generic;

namespace Portningsbolaget.Photon
{
	public class XboxPhotonAuthManager : IPhotonAuth
	{
		private struct AuthenticatedObj
		{
			public Action callback;
		}

		private object m_OnAuthenticatedLock = new object();

		private Queue<AuthenticatedObj> m_OnAuthenticatedQueue = new Queue<AuthenticatedObj>();

		public IEnumerator Authenticate(Action onAuthenticated)
		{
			yield return null;
		}

		public void Update()
		{
			lock (m_OnAuthenticatedLock)
			{
				while (m_OnAuthenticatedQueue.Count > 0)
				{
					m_OnAuthenticatedQueue.Dequeue().callback?.Invoke();
				}
			}
		}

		public void TearDown()
		{
		}
	}
}
