using System;
using System.Collections;

namespace Portningsbolaget.Photon
{
	public interface IPhotonAuth
	{
		IEnumerator Authenticate(Action onAuthenticated)
		{
			yield break;
		}

		void TearDown();

		void Update();
	}
}
