using System;

namespace Fusion
{
	[Serializable]
	public struct RpcInvokeInfo
	{
		public RpcLocalInvokeResult LocalInvokeResult;

		public RpcSendMessageResult SendMessageResult;

		public int PayloadSize;

		[Obsolete("No longer used", true)]
		public readonly RpcSendCullResult SendCullResult => RpcSendCullResult.NotCulled;

		[Obsolete("No longer used", true)]
		public readonly RpcSendResult SendResult => default(RpcSendResult);

		public override readonly string ToString()
		{
			return $"[Local: {LocalInvokeResult}, Send: {SendMessageResult}, Payload Size: {PayloadSize}]";
		}

		public static RpcInvokeInfo Create(RpcLocalInvokeResult local, RpcSendMessageResult remote, int payloadSize)
		{
			return new RpcInvokeInfo
			{
				LocalInvokeResult = local,
				SendMessageResult = remote,
				PayloadSize = payloadSize
			};
		}
	}
}
