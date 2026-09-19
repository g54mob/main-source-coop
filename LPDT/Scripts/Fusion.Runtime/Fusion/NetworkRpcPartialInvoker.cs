using System;

namespace Fusion
{
	[AttributeUsage(AttributeTargets.Method)]
	public class NetworkRpcPartialInvoker : RpcAttributeBase
	{
		public uint Key => _003Ckey_003EP;

		public NetworkRpcPartialInvoker(uint key)
		{
			_003Ckey_003EP = key;
			base._002Ector();
		}
	}
}
