using System;

namespace Fusion
{
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
	public class NetworkRpcWeavedInvokerAttribute : RpcAttributeBase
	{
		public uint Key { get; }

		public bool HasPartialInvoker { get; set; }

		public NetworkRpcWeavedInvokerAttribute(uint key)
		{
			Key = key;
		}
	}
}
