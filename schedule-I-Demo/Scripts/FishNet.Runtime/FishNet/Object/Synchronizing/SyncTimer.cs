using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet.Documenting;
using FishNet.Object.Synchronizing.Internal;
using FishNet.Serializing;

namespace FishNet.Object.Synchronizing
{
	public class SyncTimer : SyncBase, ICustomSync
	{
		private struct ChangeData
		{
			public readonly SyncTimerOperation Operation;

			public readonly float Previous;

			public readonly float Next;

			public ChangeData(SyncTimerOperation operation, float previous, float next)
			{
				Operation = operation;
				Previous = previous;
				Next = next;
			}
		}

		public delegate void SyncTypeChanged(SyncTimerOperation op, float prev, float next, bool asServer);

		private List<ChangeData> _changed = new List<ChangeData>();

		private readonly List<ChangeData> _serverOnChanges = new List<ChangeData>();

		private readonly List<ChangeData> _clientOnChanges = new List<ChangeData>();

		public float Remaining { get; private set; }

		public float Elapsed => Duration - Remaining;

		public float Duration { get; private set; }

		public bool Paused { get; private set; }

		public event SyncTypeChanged OnChange;

		public void StartTimer(float remaining, bool sendRemainingOnStop = true)
		{
			if (CanNetworkSetValues())
			{
				if (Remaining > 0f)
				{
					StopTimer(sendRemainingOnStop);
				}
				Paused = false;
				Remaining = remaining;
				Duration = remaining;
				AddOperation(SyncTimerOperation.Start, -1f, remaining);
			}
		}

		public void PauseTimer(bool sendRemaining = false)
		{
			if (!(Remaining <= 0f) && !Paused && CanNetworkSetValues())
			{
				Paused = true;
				SyncTimerOperation operation = (sendRemaining ? SyncTimerOperation.PauseUpdated : SyncTimerOperation.Pause);
				AddOperation(operation, Remaining, Remaining);
			}
		}

		public void UnpauseTimer()
		{
			if (!(Remaining <= 0f) && Paused && CanNetworkSetValues())
			{
				Paused = false;
				AddOperation(SyncTimerOperation.Unpause, Remaining, Remaining);
			}
		}

		public void StopTimer(bool sendRemaining = false)
		{
			if (!(Remaining <= 0f) && CanNetworkSetValues())
			{
				bool asServer = true;
				float remaining = Remaining;
				StopTimer_Internal(asServer);
				SyncTimerOperation operation = (sendRemaining ? SyncTimerOperation.StopUpdated : SyncTimerOperation.Stop);
				AddOperation(operation, remaining, 0f);
			}
		}

		private void AddOperation(SyncTimerOperation operation, float prev, float next)
		{
			if (base.IsRegistered)
			{
				bool flag = !base.IsNetworkInitialized || NetworkBehaviour.IsServer;
				if (flag && Dirty())
				{
					ChangeData item = new ChangeData(operation, prev, next);
					_changed.Add(item);
				}
				this.OnChange?.Invoke(operation, prev, next, flag);
			}
		}

		public override void WriteDelta(PooledWriter writer, bool resetSyncTick = true)
		{
			base.WriteDelta(writer, resetSyncTick);
			writer.WriteInt32(_changed.Count);
			for (int i = 0; i < _changed.Count; i++)
			{
				ChangeData changeData = _changed[i];
				writer.WriteByte((byte)changeData.Operation);
				if (changeData.Operation == SyncTimerOperation.Start)
				{
					WriteStartTimer(writer, includeOperationByte: false);
				}
				else if (changeData.Operation == SyncTimerOperation.PauseUpdated || changeData.Operation == SyncTimerOperation.StopUpdated)
				{
					writer.WriteSingle(changeData.Next);
				}
			}
			_changed.Clear();
		}

		public override void WriteFull(PooledWriter writer)
		{
			if (!(Remaining <= 0f))
			{
				base.WriteDelta(writer, resetSyncTick: false);
				int value = ((!Paused) ? 1 : 2);
				writer.WriteInt32(value);
				WriteStartTimer(writer, includeOperationByte: true);
				if (Paused)
				{
					writer.WriteByte(2);
				}
			}
		}

		private void WriteStartTimer(Writer w, bool includeOperationByte)
		{
			if (includeOperationByte)
			{
				w.WriteByte(1);
			}
			w.WriteSingle(Remaining);
			w.WriteSingle(Duration);
		}

		private bool CanSetValues(bool asServer)
		{
			if (!asServer)
			{
				return !NetworkManager.IsServer;
			}
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[APIExclude]
		public override void Read(PooledReader reader, bool asServer)
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				SyncTimerOperation syncTimerOperation = (SyncTimerOperation)reader.ReadByte();
				switch (syncTimerOperation)
				{
				case SyncTimerOperation.Start:
				{
					float num2 = reader.ReadSingle();
					float duration = reader.ReadSingle();
					if (CanSetValues(asServer))
					{
						Paused = false;
						Remaining = num2;
						Duration = duration;
					}
					InvokeOnChange(syncTimerOperation, -1f, num2, asServer);
					break;
				}
				case SyncTimerOperation.Pause:
				case SyncTimerOperation.PauseUpdated:
				case SyncTimerOperation.Unpause:
					UpdatePauseState(syncTimerOperation);
					break;
				case SyncTimerOperation.Stop:
				{
					float remaining2 = Remaining;
					StopTimer_Internal(asServer);
					InvokeOnChange(syncTimerOperation, remaining2, 0f, asServer: false);
					break;
				}
				case SyncTimerOperation.StopUpdated:
				{
					float remaining = Remaining;
					float next = reader.ReadSingle();
					StopTimer_Internal(asServer);
					InvokeOnChange(syncTimerOperation, remaining, next, asServer);
					break;
				}
				}
			}
			if (num > 0)
			{
				InvokeOnChange(SyncTimerOperation.Complete, -1f, -1f, asServer: false);
			}
			void UpdatePauseState(SyncTimerOperation op)
			{
				bool paused = op == SyncTimerOperation.Pause || op == SyncTimerOperation.PauseUpdated;
				float remaining3 = Remaining;
				float num3;
				if (op == SyncTimerOperation.PauseUpdated)
				{
					num3 = reader.ReadSingle();
					if (CanSetValues(asServer))
					{
						Remaining = num3;
					}
				}
				else
				{
					num3 = Remaining;
				}
				if (CanSetValues(asServer))
				{
					Paused = paused;
				}
				InvokeOnChange(op, remaining3, num3, asServer);
			}
		}

		private void StopTimer_Internal(bool asServer)
		{
			if (CanSetValues(asServer))
			{
				Paused = false;
				Remaining = 0f;
			}
		}

		private void InvokeOnChange(SyncTimerOperation operation, float prev, float next, bool asServer)
		{
			if (asServer)
			{
				if (NetworkBehaviour.OnStartServerCalled)
				{
					this.OnChange?.Invoke(operation, prev, next, asServer);
				}
				else
				{
					_serverOnChanges.Add(new ChangeData(operation, prev, next));
				}
			}
			else if (NetworkBehaviour.OnStartClientCalled)
			{
				this.OnChange?.Invoke(operation, prev, next, asServer);
			}
			else
			{
				_clientOnChanges.Add(new ChangeData(operation, prev, next));
			}
		}

		public override void OnStartCallback(bool asServer)
		{
			base.OnStartCallback(asServer);
			List<ChangeData> list = (asServer ? _serverOnChanges : _clientOnChanges);
			if (this.OnChange != null)
			{
				foreach (ChangeData item in list)
				{
					this.OnChange(item.Operation, item.Previous, item.Next, asServer);
				}
			}
			list.Clear();
		}

		public void Update(float delta)
		{
			if (Remaining <= 0f || Paused)
			{
				return;
			}
			if (delta < 0f)
			{
				delta *= -1f;
			}
			float remaining = Remaining;
			Remaining -= delta;
			if (!(Remaining > 0f))
			{
				Remaining = 0f;
				if (NetworkManager.IsServer)
				{
					this.OnChange?.Invoke(SyncTimerOperation.Finished, remaining, 0f, asServer: true);
				}
				if (NetworkManager.IsClient)
				{
					this.OnChange?.Invoke(SyncTimerOperation.Finished, remaining, 0f, asServer: false);
				}
			}
		}

		public object GetSerializedType()
		{
			return null;
		}
	}
}
