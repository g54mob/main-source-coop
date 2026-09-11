using System;
using System.Collections;

namespace Portningsbolaget.Photon
{
	public class SwitchPhotonAuthManager : IPhotonAuth
	{
		public IEnumerator Authenticate(Action onAuthenticated)
		{
			yield return null;
		}

		public void Update()
		{
		}

		public void TearDown()
		{
		}
	}
}
