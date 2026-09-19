using System;
using Fusion.Matchmaking;
using Fusion.Photon.Realtime;
using Fusion.Protocol;
using Photon.Client;
using Photon.Realtime;

namespace Fusion
{
	internal class CloudCommunicator : CommunicatorBase, IDisposable, IOnEventCallback
	{
		private readonly byte[] _buffer = new byte[65536];

		public RealtimeClient Client { get; private set; }

		public override int CommunicatorID => (Client != null) ? Client.LocalPlayer.ActorNumber : (-1);

		public bool WasExtracted { get; set; }

		public CloudCommunicator(RealtimeClient realtimeClient = null, FusionAppSettings appSettings = null)
		{
			Client = (realtimeClient ?? new RealtimeClient()).SetupForFusion(appSettings);
			Client.AddCallbackTarget(this);
		}

		public override void Service()
		{
			if (Client != null)
			{
				try
				{
					Client.Service();
				}
				catch (Exception error)
				{
					InternalLogStreams.LogError?.Log(error);
				}
				base.Service();
			}
		}

		public unsafe override bool SendPackage(byte code, int targetActor, bool reliable, byte* buffer, int bufferLength)
		{
			return Client != null && Client.SendEvent(targetActor, code, buffer, bufferLength, reliable);
		}

		protected override void ConvertData(object data, out byte[] dataBuffer, out int maxLength)
		{
			dataBuffer = null;
			maxLength = -1;
			if (data is ByteArraySlice { Count: >0 } byteArraySlice)
			{
				Assert.Always(byteArraySlice.Count <= _buffer.Length, "Array slice to large for the buffer {0} {1}", _buffer.Length, byteArraySlice.Count);
				maxLength = byteArraySlice.Count;
				Array.Copy(byteArraySlice.Buffer, _buffer, maxLength);
				dataBuffer = _buffer;
				byteArraySlice.Release();
			}
		}

		public void Reset()
		{
			MessageSendQueue.Clear();
			RecvQueue.Clear();
			Callbacks.Clear();
		}

		public void OnEvent(EventData evt)
		{
			PushPackage(evt.Sender, evt.Code, evt.CustomData);
		}

		public void Dispose()
		{
			if (!WasExtracted)
			{
				Client = null;
			}
		}
	}
}
