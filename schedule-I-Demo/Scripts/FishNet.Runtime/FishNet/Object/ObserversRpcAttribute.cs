using System;

namespace FishNet.Object
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class ObserversRpcAttribute : RpcAttribute
	{
		public bool ExcludeOwner;

		public bool ExcludeServer;

		public bool BufferLast;
	}
}
