using System;
using System.Collections;
using UnityEngine;

namespace Portningsbolaget.Photon
{
	public class DebugPhotonAuthManager : IPhotonAuth
	{
		public DebugPhotonAuthManager()
		{
			Debug.LogError("DebugPhotonAuthManager is not implemented!");
		}

		public IEnumerator Authenticate(Action onAuthenticated)
		{
			Debug.LogError("DebugPhotonAuthManager.Authenticate is not implemented!");
			yield return null;
			onAuthenticated();
		}

		public void Update()
		{
		}

		public void TearDown()
		{
			Debug.LogError("DebugPhotonAuthManager.TearDown is not implemented!");
		}
	}
}
