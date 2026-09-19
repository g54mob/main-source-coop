using System;
using Google.Apis.Util;

namespace Google.Apis.Requests
{
	public static class ClientServiceRequestExtensions
	{
		public static TRequest Configure<TRequest>(this TRequest request, Action<TRequest> configurationAction) where TRequest : ClientServiceRequest
		{
			request.ThrowIfNull("request");
			configurationAction.ThrowIfNull("configurationAction");
			configurationAction(request);
			return request;
		}
	}
}
