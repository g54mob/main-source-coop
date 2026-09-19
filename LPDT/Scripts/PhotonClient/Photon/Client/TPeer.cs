using System;
using System.Collections.Generic;

namespace Photon.Client
{
	internal class TPeer : PeerBase
	{
		internal const int TCP_HEADER_BYTES = 7;

		internal const int MSG_HEADER_BYTES = 2;

		public const int ALL_HEADER_BYTES = 9;

		private Queue<StreamBuffer> incomingList = new Queue<StreamBuffer>(32);

		internal List<StreamBuffer> outgoingStream;

		private int lastPingActivity;

		private readonly byte[] pingRequest = new byte[5] { 240, 0, 0, 0, 0 };

		private readonly ParameterDictionary pingParamDict = new ParameterDictionary();

		internal static readonly byte[] tcpFramedMessageHead = new byte[9] { 251, 0, 0, 0, 0, 0, 0, 243, 2 };

		internal static readonly byte[] tcpMsgHead = new byte[2] { 243, 2 };

		protected internal bool DoFraming = true;

		internal override int QueuedIncomingCommandsCount => incomingList.Count;

		internal override int QueuedOutgoingCommandsCount => outgoingStream.Count;

		internal TPeer()
		{
		}

		internal override bool IsTransportEncrypted()
		{
			return usedTransportProtocol == ConnectionProtocol.WebSocketSecure;
		}

		internal override void Reset()
		{
			base.Reset();
			peerID = (short)(SupportClass.ThreadSafeRandom.Next() % 32767);
			if (photonPeer.PayloadEncryptionSecret != null && usedTransportProtocol != ConnectionProtocol.WebSocketSecure)
			{
				InitEncryption(photonPeer.PayloadEncryptionSecret);
			}
			incomingList = new Queue<StreamBuffer>(32);
			base.Stats.LastReceiveTimestamp = base.timeInt;
		}

		internal override bool Connect(string serverAddress, string proxyServerAddress, string appID, object photonToken)
		{
			outgoingStream = new List<StreamBuffer>(8);
			messageHeader = (DoFraming ? tcpFramedMessageHead : tcpMsgHead);
			if (usedTransportProtocol == ConnectionProtocol.WebSocket || usedTransportProtocol == ConnectionProtocol.WebSocketSecure)
			{
				PhotonSocket.ConnectAddress = PrepareWebSocketUrl(serverAddress, appID, photonToken);
			}
			if (PhotonSocket.Connect())
			{
				base.peerConnectionState = ConnectionStateValue.Connecting;
				lastPingActivity = base.timeInt;
				if (DoFraming || PhotonToken != null)
				{
					byte[] initRequestBytes = WriteInitRequest();
					EnqueueInit(initRequestBytes);
				}
				return true;
			}
			return false;
		}

		private void Disconnect()
		{
			Disconnect(true);
		}

		internal override void Disconnect(bool queueStatusChangeCallback = true)
		{
			if (base.peerConnectionState != ConnectionStateValue.Disconnected && base.peerConnectionState != ConnectionStateValue.Disconnecting)
			{
				if ((int)base.LogLevel >= 4)
				{
					base.Listener.DebugReturn(LogLevel.Debug, "TPeer.Disconnect()");
				}
				base.peerConnectionState = ConnectionStateValue.Disconnecting;
				if (PhotonSocket != null)
				{
					PhotonSocket.Disconnect();
				}
				lock (incomingList)
				{
					incomingList.Clear();
				}
				base.peerConnectionState = ConnectionStateValue.Disconnected;
				if (queueStatusChangeCallback)
				{
					EnqueueStatusCallback(StatusCode.Disconnect);
				}
				else
				{
					base.Listener.OnStatusChanged(StatusCode.Disconnect);
				}
			}
		}

		internal override void SimulateTimeoutDisconnect(bool queueStatusChangeCallback = true)
		{
			if (base.peerConnectionState != ConnectionStateValue.Disconnected && base.peerConnectionState != ConnectionStateValue.Disconnecting)
			{
				if ((int)base.LogLevel >= 4)
				{
					base.Listener.DebugReturn(LogLevel.Debug, "TPeer.Disconnect()");
				}
				base.peerConnectionState = ConnectionStateValue.Disconnecting;
				if (PhotonSocket != null)
				{
					PhotonSocket.Disconnect();
				}
				lock (incomingList)
				{
					incomingList.Clear();
				}
				base.peerConnectionState = ConnectionStateValue.Disconnected;
				if (queueStatusChangeCallback)
				{
					EnqueueStatusCallback(StatusCode.TimeoutDisconnect);
				}
				else
				{
					base.Listener.OnStatusChanged(StatusCode.TimeoutDisconnect);
				}
			}
		}

		internal override void FetchServerTimestamp()
		{
			if (base.peerConnectionState != ConnectionStateValue.Connected || !ApplicationIsInitialized)
			{
				if ((int)base.LogLevel >= 3)
				{
					base.Listener.DebugReturn(LogLevel.Info, $"FetchServerTimestamp() skipped. Client is not connected. Current ConnectionState: {base.peerConnectionState}");
				}
			}
			else
			{
				SendPing();
				serverTimeOffsetIsAvailable = false;
			}
		}

		private void EnqueueInit(byte[] initRequestBytes)
		{
			StreamBuffer streamBuffer = new StreamBuffer(initRequestBytes.Length + 32);
			byte[] array = new byte[7] { 251, 0, 0, 0, 0, 0, 1 };
			int targetOffset = 1;
			MessageProtocol.Serialize(initRequestBytes.Length + array.Length, array, ref targetOffset);
			streamBuffer.Write(array, 0, array.Length);
			streamBuffer.Write(initRequestBytes, 0, initRequestBytes.Length);
			EnqueueMessageAsPayload(DeliveryMode.Reliable, streamBuffer, 0);
		}

		internal override bool DispatchIncomingCommands()
		{
			if (base.peerConnectionState == ConnectionStateValue.Connected && base.timeInt - base.Stats.LastReceiveTimestamp > base.DisconnectTimeout)
			{
				EnqueueStatusCallback(StatusCode.TimeoutDisconnect);
				EnqueueActionForDispatch(Disconnect);
			}
			while (true)
			{
				MyAction myAction;
				lock (ActionQueue)
				{
					if (ActionQueue.Count <= 0)
					{
						break;
					}
					myAction = ActionQueue.Dequeue();
					goto IL_009b;
				}
				IL_009b:
				myAction();
			}
			StreamBuffer streamBuffer;
			lock (incomingList)
			{
				if (incomingList.Count <= 0)
				{
					return false;
				}
				streamBuffer = incomingList.Dequeue();
			}
			ByteCountCurrentDispatch = streamBuffer.Length + 3;
			bool result = DeserializeMessageAndCallback(streamBuffer);
			PeerBase.MessageBufferPool.Release(streamBuffer);
			return result;
		}

		internal override bool SendOutgoingCommands()
		{
			if (base.peerConnectionState == ConnectionStateValue.Disconnected)
			{
				return false;
			}
			if (!PhotonSocket.Connected)
			{
				return false;
			}
			base.Stats.LastSendOutgoingTimestamp = base.timeInt;
			timeIntCurrentSend = base.timeInt;
			if (base.timeInt - lastPingActivity > base.PingInterval)
			{
				SendPing();
			}
			lock (outgoingStream)
			{
				int num = 0;
				int num2 = 0;
				PhotonSocketError photonSocketError = PhotonSocketError.Success;
				for (int i = 0; i < outgoingStream.Count; i++)
				{
					StreamBuffer streamBuffer = outgoingStream[i];
					photonSocketError = SendData(streamBuffer.GetBuffer(), streamBuffer.Length);
					if (photonSocketError == PhotonSocketError.Busy)
					{
						break;
					}
					num2 += streamBuffer.Length;
					num++;
					if (photonSocketError != PhotonSocketError.PendingSend)
					{
						PeerBase.MessageBufferPool.Release(streamBuffer);
					}
					if (num2 >= base.mtu || photonSocketError == PhotonSocketError.PendingSend)
					{
						break;
					}
				}
				outgoingStream.RemoveRange(0, num);
				if (photonSocketError == PhotonSocketError.Busy || photonSocketError == PhotonSocketError.PendingSend)
				{
					return false;
				}
				return outgoingStream.Count > 0;
			}
		}

		internal override bool SendAcksOnly()
		{
			if (PhotonSocket == null || !PhotonSocket.Connected)
			{
				return false;
			}
			if (base.peerConnectionState == ConnectionStateValue.Connected && base.timeInt - lastPingActivity > base.PingInterval)
			{
				SendPing(sendImmediately: true);
			}
			return false;
		}

		internal override bool EnqueuePhotonMessage(StreamBuffer opBytes, SendOptions sendParams)
		{
			return EnqueueMessageAsPayload(sendParams.DeliveryMode, opBytes, sendParams.Channel);
		}

		internal bool EnqueueMessageAsPayload(DeliveryMode deliveryMode, StreamBuffer opMessage, byte channelId)
		{
			if (opMessage == null)
			{
				return false;
			}
			if (DoFraming)
			{
				byte[] buffer = opMessage.GetBuffer();
				int targetOffset = 1;
				MessageProtocol.Serialize(opMessage.Length, buffer, ref targetOffset);
				buffer[5] = channelId;
				switch (deliveryMode)
				{
				case DeliveryMode.Unreliable:
					buffer[6] = 0;
					break;
				case DeliveryMode.Reliable:
					buffer[6] = 1;
					break;
				case DeliveryMode.UnreliableUnsequenced:
					buffer[6] = 2;
					break;
				case DeliveryMode.ReliableUnsequenced:
					buffer[6] = 3;
					break;
				default:
					throw new ArgumentOutOfRangeException("DeliveryMode", deliveryMode, null);
				}
			}
			lock (outgoingStream)
			{
				outgoingStream.Add(opMessage);
			}
			ByteCountLastOperation = opMessage.Length;
			return true;
		}

		internal void SendPing(bool sendImmediately = false)
		{
			int num = (lastPingActivity = base.timeInt);
			StreamBuffer streamBuffer;
			if (!DoFraming)
			{
				lock (pingParamDict)
				{
					pingParamDict[1] = num;
					streamBuffer = SerializeOperationToMessage(PhotonCodes.Ping, pingParamDict, EgMessageType.InternalOperationRequest, encrypt: false);
				}
			}
			else
			{
				int targetOffset = 1;
				MessageProtocol.Serialize(num, pingRequest, ref targetOffset);
				streamBuffer = PeerBase.MessageBufferPool.Acquire();
				streamBuffer.Write(pingRequest, 0, pingRequest.Length);
			}
			if (!sendImmediately)
			{
				EnqueuePhotonMessage(streamBuffer, SendOptions.SendReliable);
			}
			else if (SendData(streamBuffer.GetBuffer(), streamBuffer.Length) == PhotonSocketError.Success)
			{
				PeerBase.MessageBufferPool.Release(streamBuffer);
			}
		}

		internal PhotonSocketError SendData(byte[] data, int length)
		{
			PhotonSocketError photonSocketError = PhotonSocketError.Success;
			try
			{
				if (base.NetworkSimulationSettings.IsSimulationEnabled)
				{
					byte[] array = new byte[length];
					Buffer.BlockCopy(data, 0, array, 0, length);
					SendNetworkSimulated(array);
					base.Stats.BytesOut += length;
					base.Stats.PackagesOut++;
					return photonSocketError;
				}
				int num = base.timeInt;
				photonSocketError = PhotonSocket.Send(data, length);
				int num2 = base.timeInt - num;
				if (num2 > longestSendCall)
				{
					longestSendCall = num2;
				}
				if (photonSocketError == PhotonSocketError.Success)
				{
					base.Stats.BytesOut += length;
					base.Stats.PackagesOut++;
				}
			}
			catch (Exception arg)
			{
				if ((int)base.LogLevel >= 1)
				{
					base.Listener.DebugReturn(LogLevel.Error, $"Caught exception in TPeer.SendData(): {arg}");
				}
			}
			return photonSocketError;
		}

		internal override void ReceiveIncomingCommands(byte[] inbuff, int dataLength)
		{
			if (inbuff == null)
			{
				if ((int)base.LogLevel >= 1)
				{
					EnqueueDebugReturn(LogLevel.Error, "checkAndQueueIncomingCommands() inBuff: null");
				}
				return;
			}
			base.Stats.LastReceiveTimestamp = base.timeInt;
			base.Stats.BytesIn += dataLength;
			base.Stats.PackagesIn++;
			if (inbuff[0] == 243)
			{
				if (DoFraming)
				{
					base.Stats.BytesIn += 7L;
				}
				byte b = (byte)(inbuff[1] & 0x7F);
				byte b2 = inbuff[2];
				if (b != 7 || b2 != PhotonCodes.Ping)
				{
					StreamBuffer streamBuffer = PeerBase.MessageBufferPool.Acquire();
					streamBuffer.Write(inbuff, 0, dataLength);
					streamBuffer.Position = 0;
					lock (incomingList)
					{
						incomingList.Enqueue(streamBuffer);
						return;
					}
				}
				DeserializeMessageAndCallback(new StreamBuffer(inbuff));
			}
			else if (inbuff[0] == 240)
			{
				ReadPingResult(inbuff);
			}
			else if ((int)base.LogLevel >= 1 && dataLength > 0)
			{
				EnqueueDebugReturn(LogLevel.Error, $"ReceiveIncomingCommands MagicNumber should be 0xF0 or 0xF3. Is: {inbuff[0]} dataLength: {dataLength}");
			}
		}

		private void ReadPingResult(byte[] inbuff)
		{
			int value = 0;
			int value2 = 0;
			int offset = 1;
			MessageProtocol.Deserialize(out value, inbuff, ref offset);
			MessageProtocol.Deserialize(out value2, inbuff, ref offset);
			lastRoundTripTime = base.timeInt - value2;
			if (!serverTimeOffsetIsAvailable)
			{
				roundTripTime = lastRoundTripTime;
			}
			UpdateRoundTripTimeAndVariance(lastRoundTripTime);
			if (!serverTimeOffsetIsAvailable)
			{
				serverTimeOffset = value + (lastRoundTripTime >> 1) - base.timeInt;
				serverTimeOffsetIsAvailable = true;
			}
		}

		protected internal void ReadPingResult(OperationResponse operationResponse)
		{
			int num = (int)operationResponse.Parameters[2];
			int num2 = (int)operationResponse.Parameters[1];
			lastRoundTripTime = base.timeInt - num2;
			if (!serverTimeOffsetIsAvailable)
			{
				roundTripTime = lastRoundTripTime;
			}
			UpdateRoundTripTimeAndVariance(lastRoundTripTime);
			if (!serverTimeOffsetIsAvailable)
			{
				serverTimeOffset = num + (lastRoundTripTime >> 1) - base.timeInt;
				serverTimeOffsetIsAvailable = true;
			}
		}
	}
}
