using System.Collections.Generic;

namespace Photon.Client
{
	internal class EnetChannel
	{
		public class ReceiveTrackingValues
		{
			internal bool ReceivedReliableCommandSincePreviousAck2;

			internal HashSet<int> receivedReliableSequenceNumbers = new HashSet<int>();

			internal int reliableSequencedNumbersCompletelyReceived;

			internal int reliableSequencedNumbersHighestReceived;
		}

		internal byte ChannelNumber;

		internal NonAllocDictionary<int, NCommand> incomingReliableCommandsList;

		internal NonAllocDictionary<int, NCommand> incomingUnreliableCommandsList;

		internal Queue<NCommand> incomingUnsequencedCommandsList;

		internal NonAllocDictionary<int, NCommand> incomingUnsequencedFragments;

		internal List<NCommand> outgoingReliableCommandsList;

		internal List<NCommand> outgoingUnreliableCommandsList;

		internal int incomingReliableSequenceNumber;

		internal int incomingUnreliableSequenceNumber;

		internal int outgoingReliableSequenceNumber;

		internal int outgoingUnreliableSequenceNumber;

		internal int outgoingReliableUnsequencedNumber;

		private int reliableUnsequencedNumbersCompletelyReceived;

		private HashSet<int> reliableUnsequencedNumbersReceived = new HashSet<int>();

		internal int highestReceivedAck;

		internal int reliableCommandsInFlight;

		internal int lowestUnacknowledgedSequenceNumber;

		private ReceiveTrackingValues SequencedReceived = new ReceiveTrackingValues();

		private ReceiveTrackingValues UnsequencedReceived = new ReceiveTrackingValues();

		public EnetChannel(byte channelNumber, int commandBufferSize)
		{
			ChannelNumber = channelNumber;
			incomingReliableCommandsList = new NonAllocDictionary<int, NCommand>((uint)commandBufferSize);
			incomingUnreliableCommandsList = new NonAllocDictionary<int, NCommand>((uint)commandBufferSize);
			incomingUnsequencedCommandsList = new Queue<NCommand>();
			incomingUnsequencedFragments = new NonAllocDictionary<int, NCommand>();
			outgoingReliableCommandsList = new List<NCommand>(commandBufferSize);
			outgoingUnreliableCommandsList = new List<NCommand>(commandBufferSize);
		}

		public bool ContainsUnreliableSequenceNumber(int unreliableSequenceNumber)
		{
			return incomingUnreliableCommandsList.ContainsKey(unreliableSequenceNumber);
		}

		public NCommand FetchUnreliableSequenceNumber(int unreliableSequenceNumber)
		{
			return incomingUnreliableCommandsList[unreliableSequenceNumber];
		}

		public bool ContainsReliableSequenceNumber(int reliableSequenceNumber)
		{
			return incomingReliableCommandsList.ContainsKey(reliableSequenceNumber);
		}

		public bool AddSequencedIfNew(NCommand command)
		{
			NonAllocDictionary<int, NCommand> nonAllocDictionary = (command.IsFlaggedReliable ? incomingReliableCommandsList : incomingUnreliableCommandsList);
			int key = (command.IsFlaggedReliable ? command.reliableSequenceNumber : command.unreliableSequenceNumber);
			if (nonAllocDictionary.ContainsKey(key))
			{
				return false;
			}
			nonAllocDictionary.Add(key, command);
			return true;
		}

		public NCommand FetchReliableSequenceNumber(int reliableSequenceNumber)
		{
			return incomingReliableCommandsList[reliableSequenceNumber];
		}

		public bool TryGetFragment(int reliableSequenceNumber, bool isSequenced, out NCommand fragment)
		{
			if (isSequenced)
			{
				return incomingReliableCommandsList.TryGetValue(reliableSequenceNumber, out fragment);
			}
			return incomingUnsequencedFragments.TryGetValue(reliableSequenceNumber, out fragment);
		}

		public void RemoveFragment(int reliableSequenceNumber, bool isSequenced)
		{
			if (isSequenced)
			{
				incomingReliableCommandsList.Remove(reliableSequenceNumber);
			}
			else
			{
				incomingUnsequencedFragments.Remove(reliableSequenceNumber);
			}
		}

		public void clearAll()
		{
			lock (this)
			{
				SequencedReceived = new ReceiveTrackingValues();
				UnsequencedReceived = new ReceiveTrackingValues();
				incomingReliableCommandsList.Clear();
				incomingUnreliableCommandsList.Clear();
				incomingUnsequencedCommandsList.Clear();
				incomingUnsequencedFragments.Clear();
				outgoingReliableCommandsList.Clear();
				outgoingUnreliableCommandsList.Clear();
			}
		}

		public bool QueueIncomingReliableUnsequenced(NCommand command)
		{
			if (command.reliableSequenceNumber <= reliableUnsequencedNumbersCompletelyReceived)
			{
				return false;
			}
			if (reliableUnsequencedNumbersReceived.Contains(command.reliableSequenceNumber))
			{
				return false;
			}
			if (command.reliableSequenceNumber == reliableUnsequencedNumbersCompletelyReceived + 1)
			{
				reliableUnsequencedNumbersCompletelyReceived++;
				while (reliableUnsequencedNumbersReceived.Contains(reliableUnsequencedNumbersCompletelyReceived + 1))
				{
					reliableUnsequencedNumbersCompletelyReceived++;
					reliableUnsequencedNumbersReceived.Remove(reliableUnsequencedNumbersCompletelyReceived);
				}
			}
			else
			{
				reliableUnsequencedNumbersReceived.Add(command.reliableSequenceNumber);
			}
			if (command.commandType == 15)
			{
				incomingUnsequencedFragments.Add(command.reliableSequenceNumber, command);
			}
			else
			{
				incomingUnsequencedCommandsList.Enqueue(command);
			}
			return true;
		}

		internal void ApplySequenceNumberModifier(int mod)
		{
			incomingReliableSequenceNumber += mod;
			outgoingReliableSequenceNumber += mod;
			highestReceivedAck += mod;
			outgoingReliableUnsequencedNumber += mod;
			reliableUnsequencedNumbersCompletelyReceived += mod;
			SequencedReceived.reliableSequencedNumbersCompletelyReceived += mod;
			UnsequencedReceived.reliableSequencedNumbersCompletelyReceived += mod;
		}

		public void Received(NCommand inCommand)
		{
			int reliableSequenceNumber = inCommand.reliableSequenceNumber;
			ReceiveTrackingValues receiveTrackingValues = ((!inCommand.IsFlaggedUnsequenced) ? SequencedReceived : UnsequencedReceived);
			lock (receiveTrackingValues)
			{
				receiveTrackingValues.ReceivedReliableCommandSincePreviousAck2 = true;
				if (reliableSequenceNumber > receiveTrackingValues.reliableSequencedNumbersHighestReceived)
				{
					receiveTrackingValues.reliableSequencedNumbersHighestReceived = reliableSequenceNumber;
				}
				if (reliableSequenceNumber == receiveTrackingValues.reliableSequencedNumbersCompletelyReceived + 1)
				{
					receiveTrackingValues.reliableSequencedNumbersCompletelyReceived++;
					while (receiveTrackingValues.receivedReliableSequenceNumbers.Contains(receiveTrackingValues.reliableSequencedNumbersCompletelyReceived + 1))
					{
						receiveTrackingValues.reliableSequencedNumbersCompletelyReceived++;
						receiveTrackingValues.receivedReliableSequenceNumbers.Remove(receiveTrackingValues.reliableSequencedNumbersCompletelyReceived);
					}
				}
				else if (reliableSequenceNumber > receiveTrackingValues.reliableSequencedNumbersCompletelyReceived)
				{
					receiveTrackingValues.receivedReliableSequenceNumbers.Add(reliableSequenceNumber);
				}
			}
		}

		public bool GetGapBlock(out int completeSequenceNumber, int[] blocks, bool isSequenced = true)
		{
			ReceiveTrackingValues receiveTrackingValues = (isSequenced ? SequencedReceived : UnsequencedReceived);
			lock (receiveTrackingValues)
			{
				completeSequenceNumber = receiveTrackingValues.reliableSequencedNumbersCompletelyReceived;
				bool receivedReliableCommandSincePreviousAck = receiveTrackingValues.ReceivedReliableCommandSincePreviousAck2;
				receiveTrackingValues.ReceivedReliableCommandSincePreviousAck2 = false;
				if (!receivedReliableCommandSincePreviousAck)
				{
					return false;
				}
				if (blocks == null)
				{
					blocks = new int[4];
				}
				int num = completeSequenceNumber + 1;
				int num2 = 0;
				for (int i = 0; i < blocks.Length; i++)
				{
					blocks[i] = 0;
					int num3 = 0;
					int num4 = num + 32 * i;
					for (int j = 0; j < 32; j++)
					{
						int num5 = num4 + j;
						if (receiveTrackingValues.receivedReliableSequenceNumbers.Contains(num5))
						{
							num3 |= 1 << j;
							num2++;
							if (num2 >= receiveTrackingValues.receivedReliableSequenceNumbers.Count || num5 > receiveTrackingValues.reliableSequencedNumbersHighestReceived)
							{
								break;
							}
						}
					}
					blocks[i] = num3;
				}
				return receivedReliableCommandSincePreviousAck;
			}
		}
	}
}
