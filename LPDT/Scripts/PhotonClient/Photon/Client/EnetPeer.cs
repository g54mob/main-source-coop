#define DEBUG
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace Photon.Client
{
	internal class EnetPeer : PeerBase
	{
		internal struct GapBlock
		{
			internal byte Offset;

			internal int Block;
		}

		private const int CRC_LENGTH = 4;

		private const int EncryptedDataGramHeaderSize = 7;

		private const int EncryptedHeaderSize = 5;

		internal Pool<NCommand> nCommandPool = new Pool<NCommand>(() => new NCommand(), delegate(NCommand cmd)
		{
			cmd.Reset();
		}, 16);

		private List<NCommand> sentReliableCommands = new List<NCommand>();

		private int sendWindowUpdateRequiredBackValue = 0;

		private StreamBuffer outgoingAcknowledgementsPool;

		internal const int UnsequencedWindowSize = 128;

		internal readonly int[] unsequencedWindow = new int[4];

		internal int outgoingUnsequencedGroupNumber;

		internal int incomingUnsequencedGroupNumber;

		private byte udpCommandCount;

		private byte[] udpBuffer;

		private int udpBufferIndex;

		private byte[] bufferForEncryption;

		private int commandBufferSize = 100;

		internal int challenge;

		internal int serverSentTime;

		internal static readonly byte[] udpHeader0xF3 = new byte[2] { 243, 2 };

		private int datagramEncryptedConnectionBackValue = 0;

		private EnetChannel[] channelArray = new EnetChannel[0];

		private const byte ControlChannelNumber = byte.MaxValue;

		protected internal const short PeerIdForConnect = -1;

		protected internal const short PeerIdForConnectTrace = -2;

		private Queue<int> commandsToRemove = new Queue<int>();

		private ConcurrentQueue<NCommand> CommandQueue = new ConcurrentQueue<NCommand>();

		private int fragmentLength = 0;

		private int fragmentLengthDatagramEncrypt = 0;

		private int fragmentLengthMtuValue = 0;

		private readonly object datagramAccessLockObject = new object();

		private readonly HashSet<byte> channelsToUpdateLowestSent = new HashSet<byte>();

		private int[] lowestSentSequenceNumber;

		private int[] gapBlocks = new int[4];

		private List<NCommand> toRemove = new List<NCommand>(32);

		internal override int QueuedIncomingCommandsCount
		{
			get
			{
				int num = 0;
				lock (channelArray)
				{
					for (int i = 0; i < channelArray.Length; i++)
					{
						EnetChannel enetChannel = channelArray[i];
						num += enetChannel.incomingReliableCommandsList.Count;
						num += enetChannel.incomingUnreliableCommandsList.Count;
					}
				}
				return num;
			}
		}

		internal override int QueuedOutgoingCommandsCount
		{
			get
			{
				int num = 0;
				lock (channelArray)
				{
					for (int i = 0; i < channelArray.Length; i++)
					{
						EnetChannel enetChannel = channelArray[i];
						lock (enetChannel)
						{
							num += enetChannel.outgoingReliableCommandsList.Count;
							num += enetChannel.outgoingUnreliableCommandsList.Count;
						}
					}
				}
				return num;
			}
		}

		private bool SendWindowUpdateRequired
		{
			get
			{
				return Interlocked.CompareExchange(ref sendWindowUpdateRequiredBackValue, 1, 1) == 1;
			}
			set
			{
				if (value)
				{
					Interlocked.CompareExchange(ref sendWindowUpdateRequiredBackValue, 1, 0);
				}
				else
				{
					Interlocked.CompareExchange(ref sendWindowUpdateRequiredBackValue, 0, 1);
				}
			}
		}

		private bool DatagramEncryptedConnection
		{
			get
			{
				return Interlocked.CompareExchange(ref datagramEncryptedConnectionBackValue, 1, 1) == 1;
			}
			set
			{
				if (value)
				{
					Interlocked.CompareExchange(ref datagramEncryptedConnectionBackValue, 1, 0);
				}
				else
				{
					Interlocked.CompareExchange(ref datagramEncryptedConnectionBackValue, 0, 1);
				}
			}
		}

		private bool useAck2 => photonPeer.UseAck2 && base.serverFeatureAck2Available;

		internal EnetPeer()
		{
			messageHeader = udpHeader0xF3;
		}

		internal override bool IsTransportEncrypted()
		{
			return DatagramEncryptedConnection;
		}

		internal override void Reset()
		{
			base.Reset();
			if (photonPeer.PayloadEncryptionSecret != null && usedTransportProtocol == ConnectionProtocol.Udp)
			{
				InitEncryption(photonPeer.PayloadEncryptionSecret);
			}
			if (photonPeer.Encryptor != null)
			{
				isEncryptionAvailable = true;
			}
			peerID = (short)(photonPeer.EnableServerTracing ? (-2) : (-1));
			challenge = SupportClass.ThreadSafeRandom.Next();
			if (udpBuffer == null || udpBuffer.Length != base.mtu)
			{
				udpBuffer = new byte[base.mtu];
			}
			NCommand result = null;
			while (CommandQueue.TryDequeue(out result))
			{
			}
			timeoutInt = 0;
			outgoingUnsequencedGroupNumber = 0;
			incomingUnsequencedGroupNumber = 0;
			for (int i = 0; i < unsequencedWindow.Length; i++)
			{
				unsequencedWindow[i] = 0;
			}
			lock (channelArray)
			{
				EnetChannel[] array = channelArray;
				if (array.Length != base.ChannelCount + 1)
				{
					array = new EnetChannel[base.ChannelCount + 1];
				}
				for (byte b = 0; b < base.ChannelCount; b++)
				{
					array[b] = new EnetChannel(b, commandBufferSize);
				}
				array[base.ChannelCount] = new EnetChannel(byte.MaxValue, commandBufferSize);
				channelArray = array;
			}
			lock (sentReliableCommands)
			{
				sentReliableCommands.Clear();
			}
			outgoingAcknowledgementsPool = new StreamBuffer();
		}

		internal void ApplyRandomizedSequenceNumbers()
		{
			lock (channelArray)
			{
				for (int i = 0; i < channelArray.Length; i++)
				{
					EnetChannel enetChannel = channelArray[i];
					int mod = photonPeer.RandomizedSequenceNumbers[i % photonPeer.RandomizedSequenceNumbers.Length];
					enetChannel.ApplySequenceNumberModifier(mod);
				}
			}
		}

		private EnetChannel GetChannel(byte channelNumber)
		{
			return (channelNumber == byte.MaxValue) ? channelArray[channelArray.Length - 1] : channelArray[channelNumber];
		}

		internal override bool Connect(string ipport, string proxyServerAddress, string appID, object photonToken)
		{
			if (PhotonSocket.Connect())
			{
				base.peerConnectionState = ConnectionStateValue.Connecting;
				NCommand nCommand = nCommandPool.Acquire();
				nCommand.Initialize(this, 2, null, byte.MaxValue);
				QueueOutgoingReliableCommand(nCommand);
				return true;
			}
			return false;
		}

		internal override void Disconnect(bool queueStatusChangeCallback = true)
		{
			if (base.peerConnectionState == ConnectionStateValue.Disconnected || base.peerConnectionState == ConnectionStateValue.Disconnecting)
			{
				return;
			}
			ConnectionStateValue connectionStateForDisconnect = base.peerConnectionState;
			base.peerConnectionState = ConnectionStateValue.Disconnecting;
			if (sentReliableCommands != null)
			{
				lock (sentReliableCommands)
				{
					sentReliableCommands.Clear();
				}
			}
			lock (channelArray)
			{
				EnetChannel[] array = channelArray;
				foreach (EnetChannel enetChannel in array)
				{
					enetChannel.clearAll();
				}
			}
			bool isSimulationEnabled = base.NetworkSimulationSettings.IsSimulationEnabled;
			base.NetworkSimulationSettings.IsSimulationEnabled = false;
			NCommand nCommand = nCommandPool.Acquire();
			nCommand.Initialize(this, 4, null, byte.MaxValue, connectionStateForDisconnect);
			base.peerConnectionState = ConnectionStateValue.Disconnecting;
			QueueOutgoingReliableCommand(nCommand);
			photonPeer.SendOutgoingCommands();
			base.NetworkSimulationSettings.IsSimulationEnabled = isSimulationEnabled;
			if (PhotonSocket != null)
			{
				PhotonSocket.Disconnect();
			}
			DatagramEncryptedConnection = false;
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

		internal override void SimulateTimeoutDisconnect(bool queueStatusChangeCallback = true)
		{
			if (base.peerConnectionState == ConnectionStateValue.Disconnected || base.peerConnectionState == ConnectionStateValue.Disconnecting)
			{
				return;
			}
			if (sentReliableCommands != null)
			{
				lock (sentReliableCommands)
				{
					sentReliableCommands.Clear();
				}
			}
			lock (channelArray)
			{
				EnetChannel[] array = channelArray;
				foreach (EnetChannel enetChannel in array)
				{
					enetChannel.clearAll();
				}
			}
			base.peerConnectionState = ConnectionStateValue.Disconnecting;
			if (PhotonSocket != null)
			{
				PhotonSocket.Disconnect();
			}
			DatagramEncryptedConnection = false;
			base.peerConnectionState = ConnectionStateValue.Disconnected;
			if (queueStatusChangeCallback)
			{
				EnqueueStatusCallback(StatusCode.TimeoutDisconnect);
				EnqueueStatusCallback(StatusCode.Disconnect);
			}
			else
			{
				base.Listener.OnStatusChanged(StatusCode.TimeoutDisconnect);
				base.Listener.OnStatusChanged(StatusCode.Disconnect);
			}
		}

		internal override void FetchServerTimestamp()
		{
			if (base.peerConnectionState != ConnectionStateValue.Connected || !ApplicationIsInitialized)
			{
				if ((int)base.LogLevel >= 3)
				{
					EnqueueDebugReturn(LogLevel.Info, $"FetchServerTimestamp() was skipped, as the client is not connected. Current ConnectionState: {base.peerConnectionState}");
				}
			}
			else
			{
				CreateAndEnqueueCommand(12, null, byte.MaxValue);
			}
		}

		private void DispatchCommandQueue()
		{
			NCommand result = null;
			while (CommandQueue.TryDequeue(out result))
			{
				ExecuteCommand(result);
			}
		}

		internal override bool DispatchIncomingCommands()
		{
			DispatchCommandQueue();
			if (SendWindowUpdateRequired)
			{
				base.Stats.UdpReliableCommandsInFlight = sentReliableCommands.Count;
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
					goto IL_0077;
				}
				IL_0077:
				myAction();
			}
			NCommand val = null;
			lock (channelArray)
			{
				for (int i = 0; i < channelArray.Length; i++)
				{
					EnetChannel enetChannel = channelArray[i];
					if (enetChannel.incomingUnsequencedCommandsList.Count > 0)
					{
						val = enetChannel.incomingUnsequencedCommandsList.Dequeue();
						break;
					}
					if (enetChannel.incomingUnreliableCommandsList.Count > 0)
					{
						int num = int.MaxValue;
						foreach (int key2 in enetChannel.incomingUnreliableCommandsList.Keys)
						{
							NCommand nCommand = enetChannel.incomingUnreliableCommandsList[key2];
							if (key2 < enetChannel.incomingUnreliableSequenceNumber || nCommand.reliableSequenceNumber < enetChannel.incomingReliableSequenceNumber)
							{
								photonPeer.CountDiscarded++;
								commandsToRemove.Enqueue(key2);
							}
							else if (key2 < num && nCommand.reliableSequenceNumber <= enetChannel.incomingReliableSequenceNumber)
							{
								num = key2;
							}
						}
						NonAllocDictionary<int, NCommand> incomingUnreliableCommandsList = enetChannel.incomingUnreliableCommandsList;
						while (commandsToRemove.Count > 0)
						{
							int key = commandsToRemove.Dequeue();
							NCommand nCommand2 = incomingUnreliableCommandsList[key];
							incomingUnreliableCommandsList.Remove(key);
							nCommand2.FreePayload();
							nCommandPool.Release(nCommand2);
						}
						if (num < int.MaxValue)
						{
							photonPeer.DeltaUnreliableNumber = num - enetChannel.incomingUnreliableSequenceNumber;
							val = enetChannel.incomingUnreliableCommandsList[num];
						}
						if (val != null)
						{
							enetChannel.incomingUnreliableCommandsList.Remove(val.unreliableSequenceNumber);
							enetChannel.incomingUnreliableSequenceNumber = val.unreliableSequenceNumber;
							break;
						}
					}
					if (val != null || enetChannel.incomingReliableCommandsList.Count <= 0)
					{
						continue;
					}
					lock (enetChannel)
					{
						enetChannel.incomingReliableCommandsList.TryGetValue(enetChannel.incomingReliableSequenceNumber + 1, out val);
						if (val == null)
						{
							continue;
						}
						if (val.commandType != 8)
						{
							enetChannel.incomingReliableSequenceNumber = val.reliableSequenceNumber;
							enetChannel.incomingReliableCommandsList.Remove(val.reliableSequenceNumber);
						}
						else if (val.fragmentsRemaining > 0)
						{
							val = null;
						}
						else
						{
							enetChannel.incomingReliableSequenceNumber = val.reliableSequenceNumber + val.fragmentCount - 1;
							enetChannel.incomingReliableCommandsList.Remove(val.reliableSequenceNumber);
						}
						break;
					}
				}
			}
			if (val != null && val.Payload != null)
			{
				ByteCountCurrentDispatch = val.Size;
				CommandInCurrentDispatch = val;
				bool flag = DeserializeMessageAndCallback(val.Payload);
				CommandInCurrentDispatch = null;
				val.FreePayload();
				nCommandPool.Release(val);
				return true;
			}
			return false;
		}

		private int GetFragmentLength()
		{
			if (fragmentLength == 0 || base.mtu != fragmentLengthMtuValue)
			{
				fragmentLengthMtuValue = base.mtu;
				fragmentLength = base.mtu - 12 - 36;
				fragmentLengthDatagramEncrypt = ((photonPeer.Encryptor != null) ? photonPeer.Encryptor.CalculateFragmentLength() : 0);
			}
			return DatagramEncryptedConnection ? fragmentLengthDatagramEncrypt : fragmentLength;
		}

		private int CalculatePacketSize(int inSize)
		{
			if (DatagramEncryptedConnection)
			{
				return photonPeer.Encryptor.CalculateEncryptedSize(inSize + 7);
			}
			return inSize;
		}

		private int CalculateInitialOffset()
		{
			if (DatagramEncryptedConnection)
			{
				return 5;
			}
			int num = 12;
			if (photonPeer.CrcEnabled)
			{
				num += 4;
			}
			return num;
		}

		internal override bool SendAcksOnly()
		{
			return SendOutgoingCommands(sendAcksOnly: true);
		}

		internal override bool SendOutgoingCommands()
		{
			return SendOutgoingCommands(sendAcksOnly: false);
		}

		internal bool SendOutgoingCommands(bool sendAcksOnly)
		{
			if (base.peerConnectionState == ConnectionStateValue.Disconnected)
			{
				return false;
			}
			if (PhotonSocket == null || !PhotonSocket.Connected)
			{
				return false;
			}
			lock (datagramAccessLockObject)
			{
				int num = 0;
				udpBufferIndex = CalculateInitialOffset();
				udpCommandCount = 0;
				timeIntCurrentSend = base.timeInt;
				lock (outgoingAcknowledgementsPool)
				{
					if (outgoingAcknowledgementsPool.Length > 0 || useAck2)
					{
						num = SerializeAckToBuffer();
						base.Stats.LastSendAckTimestamp = timeIntCurrentSend;
					}
				}
				DispatchCommandQueue();
				if (timeIntCurrentSend > timeoutInt && sentReliableCommands.Count > 0)
				{
					int num2 = timeIntCurrentSend + 50;
					lock (sentReliableCommands)
					{
						int num3 = 0;
						for (int i = 0; i < sentReliableCommands.Count; i++)
						{
							NCommand nCommand = sentReliableCommands[i];
							int num4 = nCommand.commandSentTime + nCommand.roundTripTimeout;
							if (timeIntCurrentSend > num4)
							{
								if (!sendAcksOnly && (nCommand.commandSentCount > photonPeer.MaxResends || timeIntCurrentSend > nCommand.timeoutTime))
								{
									if ((int)base.LogLevel >= 3)
									{
										base.Listener.DebugReturn(LogLevel.Info, $"Timeout-disconnect! Command: {nCommand.ToString(full: true)} now: {timeIntCurrentSend} challenge: {Convert.ToString(challenge, 16)}");
									}
									base.peerConnectionState = ConnectionStateValue.Zombie;
									EnqueueStatusCallback(StatusCode.TimeoutDisconnect);
									Disconnect();
									nCommandPool.Release(nCommand);
									return false;
								}
								int commandSentTime = nCommand.commandSentTime;
								int roundTripTimeout = nCommand.roundTripTimeout;
								if (SerializeCommandToBuffer(nCommand, commandIsInSentQueue: true))
								{
									if ((int)base.LogLevel >= 4)
									{
										base.Listener.DebugReturn(LogLevel.Debug, $"Resending: {nCommand.ToString(full: true)}.  repeat after: {nCommand.roundTripTimeout} rtt(var): {base.rttVarString} last recv: {base.timeInt - photonPeer.Stats.LastReceiveTimestamp}  now: {timeIntCurrentSend}");
									}
									base.Stats.UdpReliableCommandsResent++;
								}
								else
								{
									num3++;
									num2 = timeoutInt;
									if (base.mtu - udpBufferIndex < 80)
									{
										break;
									}
								}
							}
							else if (num4 < num2)
							{
								num2 = num4;
							}
						}
						num += num3;
						timeoutInt = num2;
					}
				}
				if (!sendAcksOnly)
				{
					if (base.peerConnectionState == ConnectionStateValue.Connected && base.PingInterval > 0 && sentReliableCommands.Count == 0 && timeIntCurrentSend - timeLastAckReceive > base.PingInterval && CalculatePacketSize(udpBufferIndex + 12) <= base.mtu)
					{
						NCommand nCommand2 = nCommandPool.Acquire();
						nCommand2.Initialize(this, 5, null, byte.MaxValue);
						QueueOutgoingReliableCommand(nCommand2);
					}
					base.Stats.LastSendOutgoingTimestamp = base.timeInt;
					if (SendWindowUpdateRequired)
					{
						UpdateSendWindow();
					}
					lock (channelArray)
					{
						for (int j = 0; j < channelArray.Length; j++)
						{
							EnetChannel enetChannel = channelArray[j];
							lock (enetChannel)
							{
								int channelSequenceLimit = enetChannel.lowestUnacknowledgedSequenceNumber + photonPeer.SendWindowSize;
								num += SerializeToBuffer(enetChannel.outgoingReliableCommandsList, channelSequenceLimit);
								num += SerializeToBuffer(enetChannel.outgoingUnreliableCommandsList, channelSequenceLimit);
							}
						}
					}
				}
				if (udpCommandCount <= 0)
				{
					return false;
				}
				SendData(udpBuffer, udpBufferIndex);
				base.Stats.UdpReliableCommandsInFlight = sentReliableCommands.Count;
				return num > 0;
			}
		}

		private void UpdateSendWindow()
		{
			SendWindowUpdateRequired = false;
			if (photonPeer.SendWindowSize <= 0)
			{
				return;
			}
			if (sentReliableCommands.Count == 0)
			{
				lock (channelArray)
				{
					for (int i = 0; i < channelArray.Length; i++)
					{
						EnetChannel enetChannel = channelArray[i];
						enetChannel.reliableCommandsInFlight = 0;
						enetChannel.lowestUnacknowledgedSequenceNumber = enetChannel.highestReceivedAck + 1;
					}
					return;
				}
			}
			channelsToUpdateLowestSent.Clear();
			lock (channelArray)
			{
				for (int j = 0; j < channelArray.Length; j++)
				{
					EnetChannel enetChannel2 = channelArray[j];
					if (enetChannel2.ChannelNumber != byte.MaxValue && enetChannel2.reliableCommandsInFlight > 0)
					{
						channelsToUpdateLowestSent.Add(enetChannel2.ChannelNumber);
					}
				}
			}
			if (lowestSentSequenceNumber == null || lowestSentSequenceNumber.Length != channelArray.Length)
			{
				lowestSentSequenceNumber = new int[channelArray.Length];
			}
			else
			{
				for (int k = 0; k < lowestSentSequenceNumber.Length; k++)
				{
					lowestSentSequenceNumber[k] = 0;
				}
			}
			lock (sentReliableCommands)
			{
				for (int l = 0; l < sentReliableCommands.Count; l++)
				{
					NCommand nCommand = sentReliableCommands[l];
					if (nCommand.IsFlaggedUnsequenced || nCommand.commandChannelID == byte.MaxValue)
					{
						continue;
					}
					int commandChannelID = nCommand.commandChannelID;
					if (channelsToUpdateLowestSent.Contains(nCommand.commandChannelID))
					{
						if (lowestSentSequenceNumber[commandChannelID] == 0)
						{
							lowestSentSequenceNumber[commandChannelID] = nCommand.reliableSequenceNumber;
						}
						channelsToUpdateLowestSent.Remove(nCommand.commandChannelID);
						if (channelsToUpdateLowestSent.Count == 0)
						{
							break;
						}
					}
				}
			}
			lock (channelArray)
			{
				for (int m = 0; m < channelArray.Length; m++)
				{
					EnetChannel enetChannel3 = channelArray[m];
					enetChannel3.lowestUnacknowledgedSequenceNumber = ((lowestSentSequenceNumber[m] > 0) ? lowestSentSequenceNumber[m] : (enetChannel3.highestReceivedAck + 1));
				}
			}
		}

		internal override bool EnqueuePhotonMessage(StreamBuffer opBytes, SendOptions sendParams)
		{
			byte commandType = 7;
			if (sendParams.DeliveryMode == DeliveryMode.UnreliableUnsequenced)
			{
				commandType = 11;
			}
			else if (sendParams.DeliveryMode == DeliveryMode.ReliableUnsequenced)
			{
				commandType = 14;
			}
			else if (sendParams.DeliveryMode == DeliveryMode.Reliable)
			{
				commandType = 6;
			}
			return CreateAndEnqueueCommand(commandType, opBytes, sendParams.Channel);
		}

		internal bool CreateAndEnqueueCommand(byte commandType, StreamBuffer payload, byte channelNumber)
		{
			EnetChannel channel = GetChannel(channelNumber);
			ByteCountLastOperation = 0;
			int num = GetFragmentLength();
			if (num == 0)
			{
				num = 1000;
				EnqueueDebugReturn(LogLevel.Warning, "Value of currentFragmentSize should not be 0. Corrected to 1000.");
			}
			if (payload == null || payload.Length <= num)
			{
				NCommand nCommand = nCommandPool.Acquire();
				nCommand.Initialize(this, commandType, payload, channel.ChannelNumber);
				if (nCommand.IsFlaggedReliable)
				{
					QueueOutgoingReliableCommand(nCommand);
				}
				else
				{
					QueueOutgoingUnreliableCommand(nCommand);
				}
				ByteCountLastOperation = nCommand.Size;
			}
			else
			{
				lock (channel)
				{
					bool flag = commandType == 14 || commandType == 11;
					int fragmentCount = (payload.Length + num - 1) / num;
					int startSequenceNumber = (flag ? channel.outgoingReliableUnsequencedNumber : channel.outgoingReliableSequenceNumber) + 1;
					byte[] buffer = payload.GetBuffer();
					int num2 = 0;
					for (int i = 0; i < payload.Length; i += num)
					{
						if (payload.Length - i < num)
						{
							num = payload.Length - i;
						}
						StreamBuffer streamBuffer = PeerBase.MessageBufferPool.Acquire();
						streamBuffer.Write(buffer, i, num);
						NCommand nCommand2 = nCommandPool.Acquire();
						nCommand2.Initialize(this, (byte)(flag ? 15 : 8), streamBuffer, channel.ChannelNumber);
						nCommand2.fragmentNumber = num2;
						nCommand2.startSequenceNumber = startSequenceNumber;
						nCommand2.fragmentCount = fragmentCount;
						nCommand2.totalLength = payload.Length;
						nCommand2.fragmentOffset = i;
						QueueOutgoingReliableCommand(nCommand2);
						ByteCountLastOperation += nCommand2.Size;
						base.Stats.UdpFragmentsOut++;
						num2++;
					}
				}
				PeerBase.MessageBufferPool.Release(payload);
			}
			return true;
		}

		internal int SerializeAckToBuffer()
		{
			if (useAck2)
			{
				if (base.peerConnectionState != ConnectionStateValue.Connected)
				{
					return 0;
				}
				lock (channelArray)
				{
					for (int i = 0; i < channelArray.Length; i++)
					{
						EnetChannel enetChannel = channelArray[i];
						int completeSequenceNumber = 0;
						bool isSequenced = true;
						if (enetChannel.GetGapBlock(out completeSequenceNumber, gapBlocks, isSequenced))
						{
							for (int j = 0; j < gapBlocks.Length; j++)
							{
								int num = gapBlocks[j];
								if (num != 0 || j <= 0)
								{
									NCommand.CreateAck2(udpBuffer, udpBufferIndex, enetChannel.ChannelNumber, completeSequenceNumber, num, (byte)j, serverSentTime, isSequenced);
									udpBufferIndex += 20;
									udpCommandCount++;
								}
							}
						}
						isSequenced = false;
						if (!enetChannel.GetGapBlock(out completeSequenceNumber, gapBlocks, isSequenced))
						{
							continue;
						}
						for (int k = 0; k < gapBlocks.Length; k++)
						{
							int num2 = gapBlocks[k];
							if (num2 != 0 || k <= 0)
							{
								NCommand.CreateAck2(udpBuffer, udpBufferIndex, enetChannel.ChannelNumber, completeSequenceNumber, num2, (byte)k, serverSentTime, isSequenced);
								udpBufferIndex += 20;
								udpCommandCount++;
							}
						}
					}
				}
				return 0;
			}
			outgoingAcknowledgementsPool.Seek(0L, SeekOrigin.Begin);
			while (outgoingAcknowledgementsPool.Position + 20 <= outgoingAcknowledgementsPool.Length && CalculatePacketSize(udpBufferIndex + 20) <= base.mtu)
			{
				int offset;
				byte[] bufferAndAdvance = outgoingAcknowledgementsPool.GetBufferAndAdvance(20, out offset);
				Buffer.BlockCopy(bufferAndAdvance, offset, udpBuffer, udpBufferIndex, 20);
				udpBufferIndex += 20;
				udpCommandCount++;
			}
			outgoingAcknowledgementsPool.Compact();
			outgoingAcknowledgementsPool.Position = outgoingAcknowledgementsPool.Length;
			return outgoingAcknowledgementsPool.Length / 20;
		}

		internal int SerializeToBuffer(List<NCommand> commandList, int channelSequenceLimit)
		{
			if (commandList.Count == 0)
			{
				return 0;
			}
			int num = 0;
			int num2 = 0;
			while (num < commandList.Count)
			{
				NCommand nCommand = commandList[num];
				if (nCommand.IsFlaggedReliable && !nCommand.IsFlaggedUnsequenced && photonPeer.SendWindowSize > 0 && nCommand.commandChannelID != byte.MaxValue && ((nCommand.startSequenceNumber == 0 && nCommand.reliableSequenceNumber >= channelSequenceLimit) || (nCommand.startSequenceNumber > 0 && nCommand.startSequenceNumber >= channelSequenceLimit)))
				{
					num++;
					num2++;
					continue;
				}
				if (SerializeCommandToBuffer(nCommand))
				{
					commandList.RemoveAt(num);
					continue;
				}
				break;
			}
			throttledBySendWindow += num2;
			return commandList.Count - num2;
		}

		private bool SerializeCommandToBuffer(NCommand command, bool commandIsInSentQueue = false)
		{
			if (command == null)
			{
				return true;
			}
			if (CalculatePacketSize(udpBufferIndex + command.Size) > base.mtu)
			{
				return false;
			}
			command.SerializeHeader(udpBuffer, ref udpBufferIndex);
			if (command.SizeOfPayload > 0)
			{
				Buffer.BlockCopy(command.Serialize(), 0, udpBuffer, udpBufferIndex, command.SizeOfPayload);
				udpBufferIndex += command.SizeOfPayload;
			}
			udpCommandCount++;
			if (command.IsFlaggedReliable)
			{
				if (command.commandSentCount == 0)
				{
					base.Stats.UdpReliableCommandsSent++;
				}
				QueueSentCommand(command, commandIsInSentQueue);
			}
			else
			{
				base.Stats.UdpUnreliableCommandsSent++;
				command.FreePayload();
				nCommandPool.Release(command);
			}
			return true;
		}

		internal void SendData(byte[] data, int length)
		{
			try
			{
				if (DatagramEncryptedConnection)
				{
					SendDataEncrypted(data, length);
					return;
				}
				int targetOffset = 0;
				MessageProtocol.Serialize(peerID, data, ref targetOffset);
				data[2] = (byte)(photonPeer.CrcEnabled ? 204 : 0);
				data[3] = udpCommandCount;
				targetOffset = 4;
				MessageProtocol.Serialize(timeIntCurrentSend, data, ref targetOffset);
				MessageProtocol.Serialize(challenge, data, ref targetOffset);
				if (photonPeer.CrcEnabled)
				{
					MessageProtocol.Serialize(0, data, ref targetOffset);
					uint value = SupportClass.CalculateCrc(data, length);
					targetOffset -= 4;
					MessageProtocol.Serialize((int)value, data, ref targetOffset);
				}
				SendToSocket(data, length);
			}
			catch (Exception ex)
			{
				if ((int)base.LogLevel >= 1)
				{
					base.Listener.DebugReturn(LogLevel.Error, "SendData() caught exception: " + ex.ToString());
				}
				SupportClass.WriteStackTrace(ex);
			}
		}

		private void SendToSocket(byte[] data, int length)
		{
			photonPeer.TrafficRecorder?.Record(data, length, incoming: false, peerID, PhotonSocket);
			base.Stats.BytesOut = base.Stats.BytesOut + length;
			base.Stats.PackagesOut++;
			if (base.NetworkSimulationSettings.IsSimulationEnabled)
			{
				byte[] array = new byte[length];
				Buffer.BlockCopy(data, 0, array, 0, length);
				SendNetworkSimulated(array);
				return;
			}
			int num = base.timeInt;
			PhotonSocket.Send(data, length);
			int num2 = base.timeInt - num;
			if (num2 > longestSendCall)
			{
				longestSendCall = num2;
			}
		}

		private void SendDataEncrypted(byte[] data, int length)
		{
			if (bufferForEncryption == null || bufferForEncryption.Length != base.mtu)
			{
				bufferForEncryption = new byte[base.mtu];
			}
			byte[] array = bufferForEncryption;
			int targetOffset = 0;
			MessageProtocol.Serialize(peerID, array, ref targetOffset);
			array[2] = 1;
			targetOffset++;
			MessageProtocol.Serialize(challenge, array, ref targetOffset);
			data[0] = udpCommandCount;
			int targetOffset2 = 1;
			MessageProtocol.Serialize(timeIntCurrentSend, data, ref targetOffset2);
			int outSize = array.Length - targetOffset;
			photonPeer.Encryptor.Encrypt2(data, length, array, array, targetOffset, ref outSize);
			SendToSocket(array, outSize + targetOffset);
		}

		internal void QueueSentCommand(NCommand command, bool commandIsAlreadyInSentQueue = false)
		{
			command.commandSentTime = timeIntCurrentSend;
			if (command.roundTripTimeout == 0)
			{
				command.roundTripTimeout = (int)Math.Min(roundTripTime + (float)base.TimeoutVarianceCompensation, photonPeer.InitialResendTimeMax);
				command.timeoutTime = timeIntCurrentSend + base.DisconnectTimeout;
			}
			else if (command.commandSentCount >= photonPeer.QuickResendAttempts || sentReliableCommands.Count >= photonPeer.SendWindowSize)
			{
				int val = command.roundTripTimeout * 2;
				command.roundTripTimeout = Math.Min(val, photonPeer.InitialResendTimeMax * 2);
			}
			command.commandSentCount++;
			int num = command.commandSentTime + command.roundTripTimeout;
			if (num < timeoutInt)
			{
				timeoutInt = num;
			}
			if (!commandIsAlreadyInSentQueue)
			{
				EnetChannel channel = GetChannel(command.commandChannelID);
				channel.reliableCommandsInFlight++;
				lock (sentReliableCommands)
				{
					sentReliableCommands.Add(command);
				}
			}
		}

		internal void QueueOutgoingReliableCommand(NCommand command)
		{
			EnetChannel channel = GetChannel(command.commandChannelID);
			lock (channel)
			{
				if (command.reliableSequenceNumber == 0)
				{
					if (command.IsFlaggedUnsequenced)
					{
						command.reliableSequenceNumber = ++channel.outgoingReliableUnsequencedNumber;
					}
					else
					{
						command.reliableSequenceNumber = ++channel.outgoingReliableSequenceNumber;
					}
				}
				channel.outgoingReliableCommandsList.Add(command);
			}
		}

		internal void QueueOutgoingUnreliableCommand(NCommand command)
		{
			EnetChannel channel = GetChannel(command.commandChannelID);
			lock (channel)
			{
				if (command.IsFlaggedUnsequenced)
				{
					command.reliableSequenceNumber = 0;
					command.unsequencedGroupNumber = ++outgoingUnsequencedGroupNumber;
				}
				else
				{
					command.reliableSequenceNumber = channel.outgoingReliableSequenceNumber;
					command.unreliableSequenceNumber = ++channel.outgoingUnreliableSequenceNumber;
				}
				if (!photonPeer.SendInCreationOrder)
				{
					channel.outgoingUnreliableCommandsList.Add(command);
				}
				else
				{
					channel.outgoingReliableCommandsList.Add(command);
				}
			}
		}

		internal void QueueOutgoingAcknowledgement(NCommand readCommand, int sendTime)
		{
			if (useAck2)
			{
				lock (channelArray)
				{
					EnetChannel channel = GetChannel(readCommand.commandChannelID);
					if (channel != null)
					{
						lock (channel)
						{
							channel.Received(readCommand);
							return;
						}
					}
					return;
				}
			}
			lock (outgoingAcknowledgementsPool)
			{
				int offset;
				byte[] bufferAndAdvance = outgoingAcknowledgementsPool.GetBufferAndAdvance(20, out offset);
				NCommand.CreateAck(bufferAndAdvance, offset, readCommand, sendTime);
			}
		}

		internal override void ReceiveIncomingCommands(byte[] inBuff, int inDataLength)
		{
			int num = base.timeInt;
			photonPeer.Stats.LastReceiveTimestamp = num;
			base.Stats.BytesIn += inDataLength;
			base.Stats.PackagesIn++;
			if (base.peerConnectionState == ConnectionStateValue.Disconnected)
			{
				return;
			}
			try
			{
				int offset = 0;
				MessageProtocol.Deserialize(out short _, inBuff, ref offset);
				byte b = inBuff[offset++];
				int value2;
				byte b2;
				if (b == 1)
				{
					if (photonPeer.Encryptor == null)
					{
						return;
					}
					MessageProtocol.Deserialize(out value2, inBuff, ref offset);
					if (value2 != challenge)
					{
						packetLossByChallenge++;
						return;
					}
					inBuff = photonPeer.Encryptor.Decrypt2(inBuff, offset, inDataLength - offset, inBuff, out var _);
					if (!DatagramEncryptedConnection)
					{
						DatagramEncryptedConnection = true;
						fragmentLength = 0;
					}
					offset = 0;
					b2 = inBuff[offset++];
					MessageProtocol.Deserialize(out serverSentTime, inBuff, ref offset);
				}
				else
				{
					if (DatagramEncryptedConnection)
					{
						if ((int)base.LogLevel >= 2)
						{
							EnqueueDebugReturn(LogLevel.Warning, "Ignored received package. Connection requires Datagram Encryption but received unencrypted datagram.");
						}
						return;
					}
					b2 = inBuff[offset++];
					MessageProtocol.Deserialize(out serverSentTime, inBuff, ref offset);
					MessageProtocol.Deserialize(out value2, inBuff, ref offset);
					if (value2 != challenge)
					{
						packetLossByChallenge++;
						if (base.peerConnectionState != ConnectionStateValue.Disconnected && (int)base.LogLevel >= 4)
						{
							EnqueueDebugReturn(LogLevel.Debug, $"Ignored received package. Wrong challenge. Received: {value2} local: {challenge}");
						}
						return;
					}
					if (b == 204)
					{
						MessageProtocol.Deserialize(out int value3, inBuff, ref offset);
						offset -= 4;
						MessageProtocol.Serialize(0, inBuff, ref offset);
						uint num2 = SupportClass.CalculateCrc(inBuff, inDataLength);
						if (value3 != (int)num2)
						{
							packetLossByCrc++;
							if (base.peerConnectionState != ConnectionStateValue.Disconnected && (int)base.LogLevel >= 4)
							{
								EnqueueDebugReturn(LogLevel.Debug, $"Ignored received package. Wrong CRC. Incoming:  {(uint)value3:X} local: {num2:X}");
							}
							return;
						}
					}
				}
				if (b2 <= 0)
				{
					if ((int)base.LogLevel >= 4)
					{
						EnqueueDebugReturn(LogLevel.Debug, $"Ignored received package. No commands in package: {b2}.");
					}
					return;
				}
				for (int i = 0; i < b2; i++)
				{
					NCommand nCommand = nCommandPool.Acquire();
					nCommand.Initialize(this, inBuff, ref offset, num);
					if (nCommand.IsFlaggedReliable)
					{
						QueueOutgoingAcknowledgement(nCommand, serverSentTime);
					}
					CommandQueue.Enqueue(nCommand);
				}
			}
			catch (Exception ex)
			{
				if ((int)base.LogLevel >= 1)
				{
					EnqueueDebugReturn(LogLevel.Error, $"ReceiveIncomingCommands caught exception: {ex}");
				}
				SupportClass.WriteStackTrace(ex);
			}
		}

		internal void ExecuteCommand(NCommand command)
		{
			bool flag = false;
			switch (command.commandType)
			{
			case 2:
			case 5:
				nCommandPool.Release(command);
				break;
			case 4:
			{
				StatusCode statusValue = StatusCode.DisconnectByServerReasonUnknown;
				if (command.reservedByte == 1)
				{
					statusValue = StatusCode.DisconnectByServerLogic;
				}
				else if (command.reservedByte == 2)
				{
					statusValue = StatusCode.DisconnectByServerTimeout;
				}
				else if (command.reservedByte == 3)
				{
					statusValue = StatusCode.DisconnectByServerUserLimit;
				}
				if ((int)base.LogLevel >= 4)
				{
					base.Listener.DebugReturn(LogLevel.Debug, $"Disconnect received. Server: {base.ServerAddress} PeerId: {(ushort)peerID} rtt(var): {base.rttVarString} Reason byte: {command.reservedByte} peerConnectionState: {base.peerConnectionState}");
				}
				if (base.peerConnectionState != ConnectionStateValue.Disconnected && base.peerConnectionState != ConnectionStateValue.Disconnecting)
				{
					EnqueueStatusCallback(statusValue);
					Disconnect();
				}
				nCommandPool.Release(command);
				break;
			}
			case 1:
			case 16:
			{
				timeLastAckReceive = command.TimeOfReceive;
				SendWindowUpdateRequired = true;
				lastRoundTripTime = command.TimeOfReceive - command.ackReceivedSentTime;
				if (lastRoundTripTime < 0 || lastRoundTripTime > 10000)
				{
					if ((int)base.LogLevel >= 3)
					{
						EnqueueDebugReturn(LogLevel.Info, $"Measured lastRoundtripTime is suspicious: {lastRoundTripTime} for command: {command}");
					}
					lastRoundTripTime = (int)roundTripTime + base.TimeoutVarianceCompensation;
				}
				NCommand nCommand = RemoveSentReliableCommand(command.ackReceivedReliableSequenceNumber, command.commandChannelID, command.commandType == 16);
				nCommandPool.Release(command);
				if (nCommand == null)
				{
					break;
				}
				nCommand.FreePayload();
				EnetChannel channel2 = GetChannel(nCommand.commandChannelID);
				lock (channel2)
				{
					if (nCommand.reliableSequenceNumber > channel2.highestReceivedAck)
					{
						channel2.highestReceivedAck = nCommand.reliableSequenceNumber;
					}
					channel2.reliableCommandsInFlight--;
				}
				if (nCommand.commandType == 12)
				{
					if ((float)lastRoundTripTime <= roundTripTime)
					{
						serverTimeOffset = serverSentTime + (lastRoundTripTime >> 1) - base.timeInt;
						serverTimeOffsetIsAvailable = true;
					}
					else
					{
						FetchServerTimestamp();
					}
				}
				else
				{
					UpdateRoundTripTimeAndVariance(lastRoundTripTime);
					if (nCommand.commandType == 4 && base.peerConnectionState == ConnectionStateValue.Disconnecting)
					{
						if ((int)base.LogLevel >= 4)
						{
							EnqueueDebugReturn(LogLevel.Debug, "Server ACKd this client's Disconnect command.");
						}
						EnqueueActionForDispatch(delegate
						{
							PhotonSocket.Disconnect();
						});
					}
					else if (nCommand.commandType == 2 && lastRoundTripTime >= 0)
					{
						if (lastRoundTripTime <= 15)
						{
							roundTripTime = 15f;
							roundTripTimeVariance = 5f;
						}
						else
						{
							roundTripTime = lastRoundTripTime;
						}
					}
				}
				nCommandPool.Release(nCommand);
				break;
			}
			case 19:
				break;
			case 17:
			case 18:
			{
				timeLastAckReceive = command.TimeOfReceive;
				SendWindowUpdateRequired = true;
				lastRoundTripTime = command.TimeOfReceive - command.ackReceivedSentTime;
				if (lastRoundTripTime < 0 || lastRoundTripTime > 10000)
				{
					if ((int)base.LogLevel >= 3)
					{
						EnqueueDebugReturn(LogLevel.Info, $"Measured lastRoundtripTime is suspicious: {lastRoundTripTime} for command: {command}");
					}
					lastRoundTripTime = (int)roundTripTime + base.TimeoutVarianceCompensation;
				}
				UpdateRoundTripTimeAndVariance(lastRoundTripTime);
				int ackReceivedReliableSequenceNumber = command.ackReceivedReliableSequenceNumber;
				uint reliableSequenceNumber = (uint)command.reliableSequenceNumber;
				byte commandFlags = command.commandFlags;
				byte commandChannelID = command.commandChannelID;
				bool flag4 = command.commandType == 18;
				EnetChannel channel3 = GetChannel(command.commandChannelID);
				int highestReceivedAck = channel3.highestReceivedAck;
				nCommandPool.Release(command);
				int num2 = ackReceivedReliableSequenceNumber + 1 + commandFlags * 32;
				int num3 = num2;
				lock (sentReliableCommands)
				{
					toRemove.Clear();
					foreach (NCommand sentReliableCommand in sentReliableCommands)
					{
						if (sentReliableCommand.commandChannelID != commandChannelID || sentReliableCommand.IsFlaggedUnsequenced != flag4)
						{
							continue;
						}
						if (sentReliableCommand.reliableSequenceNumber <= ackReceivedReliableSequenceNumber)
						{
							toRemove.Add(sentReliableCommand);
							continue;
						}
						int num4 = sentReliableCommand.reliableSequenceNumber - num2;
						if (num4 < 0 || num4 >= 32)
						{
							continue;
						}
						if (((reliableSequenceNumber >> num4) & 1) == 1)
						{
							toRemove.Add(sentReliableCommand);
						}
						else if (sentReliableCommand.reliableSequenceNumber < num3)
						{
							if (sentReliableCommand.commandSentCount <= 3)
							{
								sentReliableCommand.roundTripTimeout = lowestRoundTripTime + base.TimeoutVarianceCompensation;
							}
							timeoutInt = 0;
						}
					}
					foreach (NCommand item in toRemove)
					{
						sentReliableCommands.Remove(item);
						if (item.commandType == 2 || item.commandType == 4 || item.commandType == 12)
						{
							Debug.WriteLine($"[{peerID}] TODO: Process removed command type: {item.commandType}");
						}
						item.FreePayload();
						nCommandPool.Release(item);
					}
					if (ackReceivedReliableSequenceNumber > channel3.highestReceivedAck)
					{
						channel3.highestReceivedAck = ackReceivedReliableSequenceNumber;
					}
					break;
				}
			}
			case 6:
			case 7:
			case 11:
			case 14:
				if (base.peerConnectionState != ConnectionStateValue.Connected || !QueueIncomingCommand(command))
				{
					nCommandPool.Release(command);
				}
				break;
			case 8:
			case 15:
			{
				if (base.peerConnectionState != ConnectionStateValue.Connected)
				{
					nCommandPool.Release(command);
					break;
				}
				if (command.fragmentNumber > command.fragmentCount || command.fragmentOffset >= command.totalLength || command.fragmentOffset + command.Payload.Length > command.totalLength)
				{
					if ((int)base.LogLevel >= 1)
					{
						base.Listener.DebugReturn(LogLevel.Error, $"Received fragment has bad size: {command}");
					}
					nCommandPool.Release(command);
					break;
				}
				bool flag2 = command.commandType == 8;
				EnetChannel channel = GetChannel(command.commandChannelID);
				NCommand fragment = null;
				lock (channel)
				{
					bool flag3 = channel.TryGetFragment(command.startSequenceNumber, flag2, out fragment);
					if (flag3 && fragment.fragmentsRemaining <= 0)
					{
						nCommandPool.Release(command);
						break;
					}
					if (!QueueIncomingCommand(command))
					{
						nCommandPool.Release(command);
						break;
					}
					base.Stats.UdpFragmentsIn++;
					if (command.reliableSequenceNumber != command.startSequenceNumber)
					{
						if (flag3)
						{
							fragment.fragmentsRemaining--;
						}
					}
					else
					{
						fragment = command;
						fragment.fragmentsRemaining--;
						NCommand fragment2 = null;
						int num = command.startSequenceNumber + 1;
						while (fragment.fragmentsRemaining > 0 && num < fragment.startSequenceNumber + fragment.fragmentCount)
						{
							if (channel.TryGetFragment(num++, flag2, out fragment2))
							{
								fragment.fragmentsRemaining--;
							}
						}
					}
					if (fragment == null || fragment.fragmentsRemaining > 0)
					{
						break;
					}
					StreamBuffer streamBuffer = PeerBase.MessageBufferPool.Acquire();
					streamBuffer.Position = 0;
					streamBuffer.SetCapacityMinimum(fragment.totalLength);
					byte[] buffer = streamBuffer.GetBuffer();
					for (int i = fragment.startSequenceNumber; i < fragment.startSequenceNumber + fragment.fragmentCount; i++)
					{
						if (channel.TryGetFragment(i, flag2, out var fragment3))
						{
							Buffer.BlockCopy(fragment3.Payload.GetBuffer(), 0, buffer, fragment3.fragmentOffset, fragment3.Payload.Length);
							fragment3.FreePayload();
							channel.RemoveFragment(fragment3.reliableSequenceNumber, flag2);
							if (fragment3.fragmentNumber > 0)
							{
								nCommandPool.Release(fragment3);
							}
							continue;
						}
						throw new Exception("startCommand.fragmentsRemaining was 0 but not all fragments were found to be combined!");
					}
					streamBuffer.SetLength(fragment.totalLength);
					fragment.FreePayload();
					fragment.Payload = streamBuffer;
					fragment.Size = 12 * fragment.fragmentCount + fragment.totalLength;
					if (flag2)
					{
						channel.incomingReliableCommandsList.Add(fragment.startSequenceNumber, fragment);
					}
					else
					{
						channel.incomingUnsequencedCommandsList.Enqueue(fragment);
					}
					break;
				}
			}
			case 3:
				if (TryUpdateConnectionState(ConnectionStateValue.Connecting, ConnectionStateValue.ConnectingHandleVerifyConnect))
				{
					if (base.serverFeatureSyncReliableQueue && base.ServerMaxQueueableReliableCommands != 0)
					{
						photonPeer.SendWindowSize = base.ServerMaxQueueableReliableCommands;
					}
					byte[] buf = WriteInitRequest();
					CreateAndEnqueueCommand(6, new StreamBuffer(buf), 0);
					if (photonPeer.RandomizeSequenceNumbers)
					{
						ApplyRandomizedSequenceNumbers();
					}
					base.peerConnectionState = ConnectionStateValue.Connected;
				}
				nCommandPool.Release(command);
				break;
			case 9:
			case 10:
			case 12:
			case 13:
				break;
			}
		}

		internal bool QueueIncomingCommand(NCommand command)
		{
			EnetChannel channel = GetChannel(command.commandChannelID);
			if (channel == null)
			{
				if ((int)base.LogLevel >= 1)
				{
					base.Listener.DebugReturn(LogLevel.Error, $"Received command for non-existing channel: {command.commandChannelID}");
				}
				return false;
			}
			if (command.IsFlaggedUnsequenced)
			{
				if (command.IsFlaggedReliable)
				{
					bool result;
					lock (channel)
					{
						result = channel.QueueIncomingReliableUnsequenced(command);
					}
					return result;
				}
				int unsequencedGroupNumber = command.unsequencedGroupNumber;
				int num = command.unsequencedGroupNumber % 128;
				if (unsequencedGroupNumber >= incomingUnsequencedGroupNumber + 128)
				{
					incomingUnsequencedGroupNumber = unsequencedGroupNumber - num;
					for (int i = 0; i < unsequencedWindow.Length; i++)
					{
						unsequencedWindow[i] = 0;
					}
				}
				else if (unsequencedGroupNumber < incomingUnsequencedGroupNumber || (unsequencedWindow[num / 32] & (1 << num % 32)) != 0)
				{
					return false;
				}
				unsequencedWindow[num / 32] |= 1 << num % 32;
				channel.incomingUnsequencedCommandsList.Enqueue(command);
				return true;
			}
			if (command.IsFlaggedReliable)
			{
				if (command.reliableSequenceNumber <= channel.incomingReliableSequenceNumber)
				{
					if ((int)base.LogLevel >= 4)
					{
						base.Listener.DebugReturn(LogLevel.Debug, $"Command {command} outdated. Sequence number is less than dispatched incomingReliableSequenceNumber: {channel.incomingReliableSequenceNumber}");
					}
					return false;
				}
				bool flag = false;
				lock (channel)
				{
					flag = channel.AddSequencedIfNew(command);
				}
				if (!flag)
				{
					if ((int)base.LogLevel >= 4)
					{
						base.Listener.DebugReturn(LogLevel.Debug, $"Command was received before! New: {command} inReliableSeq#: {channel.incomingReliableSequenceNumber}");
					}
					return false;
				}
			}
			else
			{
				if (command.reliableSequenceNumber < channel.incomingReliableSequenceNumber)
				{
					photonPeer.CountDiscarded++;
					if ((int)base.LogLevel >= 4)
					{
						base.Listener.DebugReturn(LogLevel.Debug, "Incoming reliable-seq# < Dispatched-rel-seq#. not saved.");
					}
					return false;
				}
				if (command.unreliableSequenceNumber <= channel.incomingUnreliableSequenceNumber)
				{
					photonPeer.CountDiscarded++;
					if ((int)base.LogLevel >= 4)
					{
						base.Listener.DebugReturn(LogLevel.Debug, "Incoming unreliable-seq# < Dispatched-unrel-seq#. not saved.");
					}
					return false;
				}
				bool flag2 = false;
				lock (channel)
				{
					flag2 = channel.AddSequencedIfNew(command);
				}
				if (!flag2)
				{
					if ((int)base.LogLevel >= 4)
					{
						base.Listener.DebugReturn(LogLevel.Debug, $"Command was received before! New: {command} inReliableSeq#: {channel.incomingReliableSequenceNumber}");
					}
					return false;
				}
			}
			return true;
		}

		internal NCommand RemoveSentReliableCommand(int ackReceivedReliableSequenceNumber, int ackReceivedChannel, bool isUnsequenced)
		{
			NCommand nCommand = null;
			lock (sentReliableCommands)
			{
				foreach (NCommand sentReliableCommand in sentReliableCommands)
				{
					if (sentReliableCommand != null && sentReliableCommand.reliableSequenceNumber == ackReceivedReliableSequenceNumber && sentReliableCommand.commandChannelID == ackReceivedChannel && sentReliableCommand.IsFlaggedUnsequenced == isUnsequenced)
					{
						nCommand = sentReliableCommand;
						break;
					}
				}
				if (nCommand != null)
				{
					sentReliableCommands.Remove(nCommand);
				}
				else if ((int)base.LogLevel >= 4 && base.peerConnectionState != ConnectionStateValue.Connected && base.peerConnectionState != ConnectionStateValue.Disconnecting)
				{
					EnqueueDebugReturn(LogLevel.Debug, $"No sent command for ACK (Ch: {ackReceivedReliableSequenceNumber} Sq#: {ackReceivedChannel}). PeerState: {base.peerConnectionState}.");
				}
			}
			return nCommand;
		}

		internal string CommandListToString(NCommand[] list)
		{
			if ((int)base.LogLevel < 4)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < list.Length; i++)
			{
				stringBuilder.Append(i + "=");
				stringBuilder.Append(list[i]);
				stringBuilder.Append(" # ");
			}
			return stringBuilder.ToString();
		}
	}
}
