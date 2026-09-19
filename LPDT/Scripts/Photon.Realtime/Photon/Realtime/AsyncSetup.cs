using System;
using System.Threading;
using UnityEngine;

namespace Photon.Realtime
{
	public class AsyncSetup
	{
		[Obsolete("Replaced by Application.exitCancellationToken")]
		public static CancellationTokenSource GlobalCancellationSource = new CancellationTokenSource();

		public static CancellationTokenSource CreateLinkedSource(CancellationToken token)
		{
			return CancellationTokenSource.CreateLinkedTokenSource(Application.exitCancellationToken, token);
		}

		[RuntimeInitializeOnLoadMethod]
		public static void Startup()
		{
			AsyncConfig.InitForUnity();
			AsyncConfig.Global.CancellationToken = Application.exitCancellationToken;
		}
	}
}
