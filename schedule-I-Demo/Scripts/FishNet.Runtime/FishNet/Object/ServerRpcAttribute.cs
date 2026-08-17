using System;

namespace FishNet.Object
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class ServerRpcAttribute : RpcAttribute
	{
		public bool RequireOwnership = true;
	}
}
