using System;
using System.Collections;
using System.Collections.Generic;
using Epic.OnlineServices;
using Epic.OnlineServices.P2P;
using UnityEngine;

namespace EpicTransport
{
	public abstract class Common
	{
		protected enum InternalMessages : byte
		{
			CONNECT = 0,
			ACCEPT_CONNECT = 1,
			DISCONNECT = 2
		}

		protected struct PacketKey
		{
			public ProductUserId productUserId;

			public byte channel;
		}

		private PacketReliability[] channels;

		private OnIncomingConnectionRequestCallback OnIncomingConnectionRequest;

		private ulong incomingNotificationId;

		private OnRemoteConnectionClosedCallback OnRemoteConnectionClosed;

		private ulong outgoingNotificationId;

		protected readonly EosTransport transport;

		protected List<string> deadSockets;

		public bool ignoreAllMessages;

		protected Dictionary<PacketKey, List<List<Packet>>> incomingPackets = new Dictionary<PacketKey, List<List<Packet>>>();

		protected readonly Dictionary<ProductUserId, SocketId> _lastSocketIdByPuid = new Dictionary<ProductUserId, SocketId>();

		protected readonly HashSet<string> _loggedUnknownPuids = new HashSet<string>();

		protected readonly HashSet<string> _loggedDeadSockets = new HashSet<string>();

		private readonly byte[] _receiveBuffer = new byte[1170];

		private bool _disposed;

		private int internal_ch => channels.Length;

		protected Common(EosTransport transport)
		{
			channels = transport.Channels;
			deadSockets = new List<string>();
			AddNotifyPeerConnectionRequestOptions options = new AddNotifyPeerConnectionRequestOptions
			{
				LocalUserId = EOSSDKComponent.LocalUserProductId,
				SocketId = null
			};
			OnIncomingConnectionRequest = (OnIncomingConnectionRequestCallback)Delegate.Combine(OnIncomingConnectionRequest, new OnIncomingConnectionRequestCallback(OnNewConnection));
			OnRemoteConnectionClosed = (OnRemoteConnectionClosedCallback)Delegate.Combine(OnRemoteConnectionClosed, new OnRemoteConnectionClosedCallback(OnConnectFail));
			incomingNotificationId = EOSSDKComponent.GetP2PInterface().AddNotifyPeerConnectionRequest(ref options, null, OnIncomingConnectionRequest);
			AddNotifyPeerConnectionClosedOptions options2 = new AddNotifyPeerConnectionClosedOptions
			{
				LocalUserId = EOSSDKComponent.LocalUserProductId,
				SocketId = null
			};
			outgoingNotificationId = EOSSDKComponent.GetP2PInterface().AddNotifyPeerConnectionClosed(ref options2, null, OnRemoteConnectionClosed);
			if (outgoingNotificationId == 0L || incomingNotificationId == 0L)
			{
				Debug.LogError("Couldn't bind notifications with P2P interface");
			}
			incomingPackets = new Dictionary<PacketKey, List<List<Packet>>>();
			this.transport = transport;
		}

		protected void Dispose()
		{
			if (_disposed)
			{
				return;
			}
			_disposed = true;
			P2PInterface p2PInterface = EOSSDKComponent.GetP2PInterface();
			if (p2PInterface != null)
			{
				if (incomingNotificationId != 0L)
				{
					p2PInterface.RemoveNotifyPeerConnectionRequest(incomingNotificationId);
					incomingNotificationId = 0uL;
				}
				if (outgoingNotificationId != 0L)
				{
					p2PInterface.RemoveNotifyPeerConnectionClosed(outgoingNotificationId);
					outgoingNotificationId = 0uL;
				}
			}
			_lastSocketIdByPuid.Clear();
			_loggedUnknownPuids.Clear();
			_loggedDeadSockets.Clear();
			transport.ResetIgnoreMessagesAtStartUpTimer();
		}

		protected abstract void OnNewConnection(ref OnIncomingConnectionRequestInfo result);

		private void OnConnectFail(ref OnRemoteConnectionClosedInfo result)
		{
			if (!ignoreAllMessages)
			{
				SocketId valueOrDefault = result.SocketId.GetValueOrDefault();
				OnConnectionFailed(result.RemoteUserId, valueOrDefault);
				Debug.LogWarning(string.Format("[EOS P2P] {0} (Reason: {1})", result.Reason switch
				{
					ConnectionClosedReason.ClosedByLocalUser => "Connection closed: closed by local user.", 
					ConnectionClosedReason.ClosedByPeer => "Connection closed: closed by remote user.", 
					ConnectionClosedReason.ConnectionClosed => "Connection closed: unexpectedly closed.", 
					ConnectionClosedReason.ConnectionFailed => "Connection failed: failed to establish connection.", 
					ConnectionClosedReason.InvalidData => "Connection failed: remote user sent invalid data.", 
					ConnectionClosedReason.InvalidMessage => "Connection failed: remote user sent invalid message.", 
					ConnectionClosedReason.NegotiationFailed => "Connection failed: negotiation failed.", 
					ConnectionClosedReason.TimedOut => "Connection failed: timeout.", 
					ConnectionClosedReason.TooManyConnections => "Connection failed: too many connections.", 
					ConnectionClosedReason.UnexpectedError => "Connection failed: unexpected error.", 
					_ => "Connection closed: unknown reason.", 
				}, result.Reason));
			}
		}

		protected void SendInternal(ProductUserId target, SocketId socketId, InternalMessages type)
		{
			SendPacketOptions options = new SendPacketOptions
			{
				AllowDelayedDelivery = true,
				Channel = (byte)internal_ch,
				Data = new ArraySegment<byte>(new byte[1] { (byte)type }),
				LocalUserId = EOSSDKComponent.LocalUserProductId,
				Reliability = PacketReliability.ReliableOrdered,
				RemoteUserId = target,
				SocketId = socketId
			};
			EOSSDKComponent.GetP2PInterface().SendPacket(ref options);
		}

		protected void Send(ProductUserId host, SocketId socketId, byte[] msgBuffer, byte channel)
		{
			SendPacketOptions options = new SendPacketOptions
			{
				AllowDelayedDelivery = true,
				Channel = channel,
				Data = new ArraySegment<byte>(msgBuffer),
				LocalUserId = EOSSDKComponent.LocalUserProductId,
				Reliability = channels[channel],
				RemoteUserId = host,
				SocketId = socketId
			};
			Result result = EOSSDKComponent.GetP2PInterface().SendPacket(ref options);
			if (result != Result.Success)
			{
				Debug.LogError("Send failed " + result);
			}
		}

		private bool Receive(out ProductUserId clientProductUserId, out SocketId socketId, out byte[] receiveBuffer, byte channel)
		{
			ReceivePacketOptions options = new ReceivePacketOptions
			{
				LocalUserId = EOSSDKComponent.LocalUserProductId,
				MaxDataSizeBytes = 1170u,
				RequestedChannel = channel
			};
			clientProductUserId = null;
			socketId = default(SocketId);
			ArraySegment<byte> outData = new ArraySegment<byte>(_receiveBuffer);
			P2PInterface p2PInterface = EOSSDKComponent.GetP2PInterface();
			if (p2PInterface == null)
			{
				receiveBuffer = null;
				clientProductUserId = null;
				return false;
			}
			if (p2PInterface.ReceivePacket(ref options, ref clientProductUserId, ref socketId, out var _, outData, out var outBytesWritten) == Result.Success)
			{
				receiveBuffer = new byte[outBytesWritten];
				Array.Copy(_receiveBuffer, receiveBuffer, outBytesWritten);
				return true;
			}
			receiveBuffer = null;
			clientProductUserId = null;
			return false;
		}

		protected virtual void CloseP2PSessionWithUser(ProductUserId clientUserID, SocketId socketId)
		{
			if (string.IsNullOrEmpty(socketId.SocketName))
			{
				Debug.LogWarning("Socket ID has no name | " + ignoreAllMessages);
			}
			else if (deadSockets == null)
			{
				Debug.LogWarning("DeadSockets == null");
			}
			else if (!deadSockets.Contains(socketId.SocketName))
			{
				deadSockets.Add(socketId.SocketName);
				if (clientUserID != null)
				{
					CloseConnectionOptions options = new CloseConnectionOptions
					{
						LocalUserId = EOSSDKComponent.LocalUserProductId,
						RemoteUserId = clientUserID,
						SocketId = socketId
					};
					EOSSDKComponent.GetP2PInterface().CloseConnection(ref options);
				}
				else
				{
					CloseConnectionsOptions options2 = new CloseConnectionsOptions
					{
						LocalUserId = EOSSDKComponent.LocalUserProductId,
						SocketId = socketId
					};
					EOSSDKComponent.GetP2PInterface().CloseConnections(ref options2);
				}
			}
		}

		protected void WaitForClose(ProductUserId clientUserID, SocketId socketId)
		{
			transport.StartCoroutine(DelayedClose(clientUserID, socketId));
		}

		private IEnumerator DelayedClose(ProductUserId clientUserID, SocketId socketId)
		{
			yield return null;
			CloseP2PSessionWithUser(clientUserID, socketId);
		}

		public void ReceiveData()
		{
			try
			{
				SocketId socketId = default(SocketId);
				ProductUserId clientProductUserId;
				byte[] receiveBuffer;
				while (transport.enabled && Receive(out clientProductUserId, out socketId, out receiveBuffer, (byte)internal_ch))
				{
					if (clientProductUserId != null)
					{
						_lastSocketIdByPuid[clientProductUserId] = socketId;
					}
					if (receiveBuffer.Length == 1)
					{
						OnReceiveInternalData((InternalMessages)receiveBuffer[0], clientProductUserId, socketId);
						return;
					}
					Debug.Log("Incorrect package length on internal channel.");
				}
				for (int i = 0; i < channels.Length; i++)
				{
					ProductUserId clientProductUserId2;
					byte[] receiveBuffer2;
					while (transport.enabled && Receive(out clientProductUserId2, out socketId, out receiveBuffer2, (byte)i))
					{
						if (clientProductUserId2 != null)
						{
							_lastSocketIdByPuid[clientProductUserId2] = socketId;
						}
						PacketKey key = new PacketKey
						{
							productUserId = clientProductUserId2,
							channel = (byte)i
						};
						Packet item = default(Packet);
						item.FromBytes(receiveBuffer2);
						if (!incomingPackets.ContainsKey(key))
						{
							incomingPackets.Add(key, new List<List<Packet>>());
						}
						int num = incomingPackets[key].Count;
						for (int j = 0; j < incomingPackets[key].Count; j++)
						{
							if (incomingPackets[key][j][0].id == item.id)
							{
								num = j;
								break;
							}
						}
						if (num == incomingPackets[key].Count)
						{
							incomingPackets[key].Add(new List<Packet>());
						}
						int num2 = -1;
						for (int k = 0; k < incomingPackets[key][num].Count; k++)
						{
							if (incomingPackets[key][num][k].fragment > item.fragment)
							{
								num2 = k;
								break;
							}
						}
						if (num2 >= 0)
						{
							incomingPackets[key][num].Insert(num2, item);
						}
						else
						{
							incomingPackets[key][num].Add(item);
						}
					}
				}
				List<List<Packet>> list = new List<List<Packet>>();
				foreach (KeyValuePair<PacketKey, List<List<Packet>>> incomingPacket in incomingPackets)
				{
					for (int l = 0; l < incomingPacket.Value.Count; l++)
					{
						bool flag = true;
						int num3 = 0;
						for (int m = 0; m < incomingPacket.Value[l].Count; m++)
						{
							Packet packet = incomingPacket.Value[l][m];
							if (packet.fragment != m || (m == incomingPacket.Value[l].Count - 1 && packet.moreFragments))
							{
								flag = false;
							}
							else
							{
								num3 += packet.data.Length;
							}
						}
						if (flag)
						{
							byte[] array = new byte[num3];
							int num4 = 0;
							for (int n = 0; n < incomingPacket.Value[l].Count; n++)
							{
								Array.Copy(incomingPacket.Value[l][n].data, 0, array, num4, incomingPacket.Value[l][n].data.Length);
								num4 += incomingPacket.Value[l][n].data.Length;
							}
							OnReceiveData(array, incomingPacket.Key.productUserId, incomingPacket.Key.channel);
							list.Add(incomingPacket.Value[l]);
						}
					}
					for (int num5 = 0; num5 < list.Count; num5++)
					{
						incomingPacket.Value.Remove(list[num5]);
					}
					list.Clear();
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}

		protected abstract void OnReceiveInternalData(InternalMessages type, ProductUserId clientUserID, SocketId socketId);

		protected abstract void OnReceiveData(byte[] data, ProductUserId clientUserID, int channel);

		protected abstract void OnConnectionFailed(ProductUserId remoteId, SocketId failedSocketId);
	}
}
