using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Nerdbank.Streams;

namespace MessagePack
{
	public class MessagePackStreamReader : IDisposable
	{
		private readonly Stream stream;

		private readonly bool leaveOpen;

		private SequencePool.Rental sequenceRental;

		private SequencePosition? endOfLastMessage;

		public ReadOnlySequence<byte> RemainingBytes
		{
			get
			{
				if (!endOfLastMessage.HasValue)
				{
					return ReadData.AsReadOnlySequence;
				}
				return ReadData.AsReadOnlySequence.Slice(endOfLastMessage.Value);
			}
		}

		private Sequence<byte> ReadData => sequenceRental.Value;

		public async ValueTask<int> ReadArrayHeaderAsync(CancellationToken cancellationToken)
		{
			RecycleLastMessage();
			cancellationToken.ThrowIfCancellationRequested();
			int length;
			while (!TryReadArrayHeader(out length))
			{
				if (!(await TryReadMoreDataAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false)))
				{
					throw new EndOfStreamException("The stream ended before a map header could be found.");
				}
			}
			return length;
		}

		public async IAsyncEnumerable<ReadOnlySequence<byte>> ReadArrayAsync([EnumeratorCancellation] CancellationToken cancellationToken)
		{
			RecycleLastMessage();
			cancellationToken.ThrowIfCancellationRequested();
			int length;
			while (!TryReadArrayHeader(out length))
			{
				if (!(await TryReadMoreDataAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false)))
				{
					throw new EndOfStreamException("The stream ended before an array header could be found.");
				}
			}
			for (int i = 0; i < length; i = checked(i + 1))
			{
				ReadOnlySequence<byte>? readOnlySequence = await ReadAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				if (!readOnlySequence.HasValue)
				{
					throw new EndOfStreamException("Stream ended before all elements were read.");
				}
				yield return readOnlySequence.Value;
			}
		}

		private bool TryReadArrayHeader(out int length)
		{
			if (ReadData.Length > 0)
			{
				MessagePackReader messagePackReader = new MessagePackReader((ReadOnlySequence<byte>)ReadData);
				if (messagePackReader.TryReadArrayHeader(out length))
				{
					endOfLastMessage = messagePackReader.Position;
					return true;
				}
			}
			length = 0;
			return false;
		}

		public MessagePackStreamReader(Stream stream)
			: this(stream, leaveOpen: false)
		{
		}

		public MessagePackStreamReader(Stream stream, bool leaveOpen)
			: this(stream, leaveOpen, SequencePool.Shared)
		{
		}

		public MessagePackStreamReader(Stream stream, bool leaveOpen, SequencePool sequencePool)
		{
			if (sequencePool == null)
			{
				throw new ArgumentNullException("sequencePool");
			}
			this.stream = stream ?? throw new ArgumentNullException("stream");
			this.leaveOpen = leaveOpen;
			sequenceRental = sequencePool.Rent();
		}

		public async ValueTask<ReadOnlySequence<byte>?> ReadAsync(CancellationToken cancellationToken)
		{
			RecycleLastMessage();
			do
			{
				cancellationToken.ThrowIfCancellationRequested();
				if (TryReadNextMessage(out var completeMessage))
				{
					return completeMessage;
				}
			}
			while (await TryReadMoreDataAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
			return null;
		}

		public void DiscardBufferedData()
		{
			sequenceRental.Value.Reset();
			endOfLastMessage = null;
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (!leaveOpen)
				{
					stream.Dispose();
				}
				sequenceRental.Dispose();
				sequenceRental = default(SequencePool.Rental);
			}
		}

		private void RecycleLastMessage()
		{
			if (endOfLastMessage.HasValue)
			{
				ReadData.AdvanceTo(endOfLastMessage.Value);
				endOfLastMessage = null;
			}
		}

		private async Task<bool> TryReadMoreDataAsync(CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			Memory<byte> memory = ReadData.GetMemory(0);
			int bytesRead = 0;
			try
			{
				bytesRead = await stream.ReadAsync(memory, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				return bytesRead > 0;
			}
			finally
			{
				ReadData.Advance(bytesRead);
			}
		}

		private bool TryReadNextMessage(out ReadOnlySequence<byte> completeMessage)
		{
			if (ReadData.Length > 0)
			{
				ReadOnlySequence<byte> readOnlySequence = ReadData;
				MessagePackReader messagePackReader = new MessagePackReader(in readOnlySequence);
				if (messagePackReader.TrySkip())
				{
					endOfLastMessage = messagePackReader.Position;
					readOnlySequence = messagePackReader.Sequence;
					completeMessage = readOnlySequence.Slice(0, messagePackReader.Position);
					return true;
				}
			}
			completeMessage = default(ReadOnlySequence<byte>);
			return false;
		}

		public async ValueTask<int> ReadMapHeaderAsync(CancellationToken cancellationToken)
		{
			RecycleLastMessage();
			cancellationToken.ThrowIfCancellationRequested();
			int count;
			while (!TryReadMapHeader(out count))
			{
				if (!(await TryReadMoreDataAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false)))
				{
					throw new EndOfStreamException("The stream ended before a map header could be found.");
				}
			}
			return count;
		}

		private bool TryReadMapHeader(out int count)
		{
			if (ReadData.Length > 0)
			{
				MessagePackReader messagePackReader = new MessagePackReader((ReadOnlySequence<byte>)ReadData);
				if (messagePackReader.TryReadMapHeader(out count))
				{
					endOfLastMessage = messagePackReader.Position;
					return true;
				}
			}
			count = 0;
			return false;
		}
	}
}
