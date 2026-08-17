using System;
using System.Net;
using Mirror.Discovery;
using UnityEngine;

namespace Mirror.Examples.AutoLANClientController
{
	[DisallowMultipleComponent]
	[AddComponentMenu("Network/Network Discovery")]
	public class AutoLANNetworkDiscovery : NetworkDiscoveryBase<ServerRequest, ServerResponse>
	{
		public CanvasHUD canvasHUD;

		protected override ServerResponse ProcessRequest(ServerRequest request, IPEndPoint endpoint)
		{
			try
			{
				return new ServerResponse
				{
					serverId = base.ServerId,
					uri = transport.ServerUri()
				};
			}
			catch (NotImplementedException)
			{
				Debug.LogError($"Transport {transport} does not support network discovery");
				throw;
			}
		}

		protected override ServerRequest GetRequest()
		{
			return default(ServerRequest);
		}

		protected override void ProcessResponse(ServerResponse response, IPEndPoint endpoint)
		{
			response.EndPoint = endpoint;
			UriBuilder uriBuilder = new UriBuilder(response.uri)
			{
				Host = response.EndPoint.Address.ToString()
			};
			response.uri = uriBuilder.Uri;
			if (canvasHUD == null)
			{
				canvasHUD = UnityEngine.Object.FindAnyObjectByType<CanvasHUD>();
			}
			canvasHUD.OnDiscoveredServer(response);
		}
	}
}
