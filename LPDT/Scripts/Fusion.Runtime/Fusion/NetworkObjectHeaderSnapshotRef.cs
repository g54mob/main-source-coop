#define DEBUG
using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Fusion
{
	internal readonly ref struct NetworkObjectHeaderSnapshotRef
	{
		private readonly NetworkObjectHeaderSnapshot _snapshot;

		public bool Valid
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return _snapshot != null;
			}
		}

		public Tick Tick
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return _snapshot.Tick;
			}
		}

		public ref NetworkObjectHeader Header
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return ref _snapshot.Header;
			}
		}

		public ulong SnapshotCRC
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return _snapshot.BuildCRC();
			}
		}

		internal Span<int> Raw
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return _snapshot.Raw;
			}
		}

		public NetworkObjectHeaderSnapshotRef(NetworkObjectHeaderSnapshot snapshot)
		{
			_snapshot = snapshot;
		}

		public static implicit operator NetworkObjectHeaderSnapshotRef(NetworkObjectHeaderSnapshot snapshot)
		{
			return new NetworkObjectHeaderSnapshotRef(snapshot);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyFrom(NetworkObjectMeta source)
		{
			_snapshot.CopyFrom(source);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void CopyFrom(NetworkObjectMeta source, [CanBeNull] OffsetSet ignoredOffsets)
		{
			if (ignoredOffsets == null || ignoredOffsets.IsEmpty)
			{
				_snapshot.CopyFrom(source);
				return;
			}
			ReadOnlySpan<int> readOnlySpan = source.Raw;
			Span<int> raw = _snapshot.Raw;
			int num = 0;
			ReadOnlySpan<int> values = ignoredOffsets.Values;
			for (int i = 0; i < values.Length; i++)
			{
				int num2 = values[i];
				Assert.Check(num2 >= num, "offset >= start");
				if (num2 > num)
				{
					FusionUnsafe.Copy(raw.Slice(num, num2 - num), readOnlySpan.Slice(num, num2 - num));
				}
				num = num2 + 1;
			}
			if (num < readOnlySpan.Length)
			{
				FusionUnsafe.Copy(raw.Slice(num, readOnlySpan.Length - num), readOnlySpan.Slice(num, readOnlySpan.Length - num));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyFrom(NetworkObjectHeaderSnapshotRef source)
		{
			_snapshot.CopyFrom(source._snapshot);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTo(NetworkObjectMeta destination)
		{
			_snapshot.CopyTo(destination);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTo(int[] destination)
		{
			_snapshot.CopyTo(destination);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTo(NetworkObjectHeaderSnapshotRef destination)
		{
			_snapshot.CopyTo(destination._snapshot);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe int* GetBehaviourPtr(NetworkBehaviour behaviour)
		{
			return _snapshot.GetBehaviourPtr(behaviour);
		}
	}
}
