using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet.Documenting;
using FishNet.Object.Synchronizing.Internal;
using FishNet.Serializing;

namespace FishNet.Object.Synchronizing
{
	public class SyncStopwatch : SyncBase, ICustomSync
	{
		private struct ChangeData
		{
			public readonly SyncStopwatchOperation Operation;

			public readonly float Previous;

			public ChangeData(SyncStopwatchOperation operation, float previous)
			{
				Operation = operation;
				Previous = previous;
			}
		}

		public delegate void SyncTypeChanged(SyncStopwatchOperation op, float prev, bool asServer);

		private List<ChangeData> _changed = new List<ChangeData>();

		private readonly List<ChangeData> _serverOnChanges = new List<ChangeData>();

		private readonly List<ChangeData> _clientOnChanges = new List<ChangeData>();

		public float Elapsed { get; private set; } = -1f;

		public bool Paused { get; private set; }

		public event SyncTypeChanged OnChange;

		public void StartStopwatch(bool sendElapsedOnStop = true)
		{
			if (CanNetworkSetValues())
			{
				if (Elapsed > 0f)
				{
					StopStopwatch(sendElapsedOnStop);
				}
				Elapsed = 0f;
				AddOperation(SyncStopwatchOperation.Start, 0f);
			}
		}

		public void PauseStopwatch(bool sendElapsed = false)
		{
			if (!(Elapsed < 0f) && !Paused && CanNetworkSetValues())
			{
				Paused = true;
				float prev;
				SyncStopwatchOperation operation;
				if (sendElapsed)
				{
					prev = Elapsed;
					operation = SyncStopwatchOperation.PauseUpdated;
				}
				else
				{
					prev = -1f;
					operation = SyncStopwatchOperation.Pause;
				}
				AddOperation(operation, prev);
			}
		}

		public void UnpauseStopwatch()
		{
			if (!(Elapsed < 0f) && Paused && CanNetworkSetValues())
			{
				Paused = false;
				AddOperation(SyncStopwatchOperation.Unpause, -1f);
			}
		}

		public void StopStopwatch(bool sendElapsed = false)
		{
			if (!(Elapsed < 0f) && CanNetworkSetValues())
			{
				float prev = (sendElapsed ? (-1f) : Elapsed);
				StopStopwatch_Internal(asServer: true);
				SyncStopwatchOperation operation = (sendElapsed ? SyncStopwatchOperation.StopUpdated : SyncStopwatchOperation.Stop);
				AddOperation(operation, prev);
			}
		}

		private void AddOperation(SyncStopwatchOperation operation, float prev)
		{
			if (base.IsRegistered)
			{
				bool flag = !base.IsNetworkInitialized || NetworkBehaviour.IsServer;
				if (flag && Dirty())
				{
					ChangeData item = new ChangeData(operation, prev);
					_changed.Add(item);
				}
				this.OnChange?.Invoke(operation, prev, flag);
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
				if (changeData.Operation == SyncStopwatchOperation.Start)
				{
					WriteStartStopwatch(writer, 0f, includeOperationByte: false);
				}
				else if (changeData.Operation == SyncStopwatchOperation.PauseUpdated || changeData.Operation == SyncStopwatchOperation.StopUpdated)
				{
					writer.WriteSingle(changeData.Previous);
				}
			}
			_changed.Clear();
		}

		public override void WriteFull(PooledWriter writer)
		{
			if (!(Elapsed < 0f))
			{
				base.WriteDelta(writer, resetSyncTick: false);
				int value = ((!Paused) ? 1 : 2);
				writer.WriteInt32(value);
				WriteStartStopwatch(writer, Elapsed, includeOperationByte: true);
				if (Paused)
				{
					writer.WriteByte(2);
				}
			}
		}

		private void WriteStartStopwatch(Writer w, float elapsed, bool includeOperationByte)
		{
			if (includeOperationByte)
			{
				w.WriteByte(1);
			}
			w.WriteSingle(elapsed);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[APIExclude]
		public override void Read(PooledReader reader, bool asServer)
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				SyncStopwatchOperation syncStopwatchOperation = (SyncStopwatchOperation)reader.ReadByte();
				switch (syncStopwatchOperation)
				{
				case SyncStopwatchOperation.Start:
				{
					float num2 = reader.ReadSingle();
					if (CanSetValues(asServer))
					{
						Elapsed = num2;
					}
					InvokeOnChange(syncStopwatchOperation, num2, asServer);
					break;
				}
				case SyncStopwatchOperation.Pause:
					if (CanSetValues(asServer))
					{
						Paused = true;
					}
					InvokeOnChange(syncStopwatchOperation, -1f, asServer);
					break;
				case SyncStopwatchOperation.PauseUpdated:
				{
					float prev2 = reader.ReadSingle();
					if (CanSetValues(asServer))
					{
						Paused = true;
					}
					InvokeOnChange(syncStopwatchOperation, prev2, asServer);
					break;
				}
				case SyncStopwatchOperation.Unpause:
					if (CanSetValues(asServer))
					{
						Paused = false;
					}
					InvokeOnChange(syncStopwatchOperation, -1f, asServer);
					break;
				case SyncStopwatchOperation.Stop:
					StopStopwatch_Internal(asServer);
					InvokeOnChange(syncStopwatchOperation, -1f, asServer: false);
					break;
				case SyncStopwatchOperation.StopUpdated:
				{
					float prev = reader.ReadSingle();
					StopStopwatch_Internal(asServer);
					InvokeOnChange(syncStopwatchOperation, prev, asServer);
					break;
				}
				}
			}
			if (num > 0)
			{
				InvokeOnChange(SyncStopwatchOperation.Complete, -1f, asServer);
			}
		}

		private bool CanSetValues(bool asServer)
		{
			if (!asServer)
			{
				return !NetworkManager.IsServer;
			}
			return true;
		}

		private void StopStopwatch_Internal(bool asServer)
		{
			if (CanSetValues(asServer))
			{
				Paused = false;
				Elapsed = -1f;
			}
		}

		private void InvokeOnChange(SyncStopwatchOperation operation, float prev, bool asServer)
		{
			if (asServer)
			{
				if (NetworkBehaviour.OnStartServerCalled)
				{
					this.OnChange?.Invoke(operation, prev, asServer);
				}
				else
				{
					_serverOnChanges.Add(new ChangeData(operation, prev));
				}
			}
			else if (NetworkBehaviour.OnStartClientCalled)
			{
				this.OnChange?.Invoke(operation, prev, asServer);
			}
			else
			{
				_clientOnChanges.Add(new ChangeData(operation, prev));
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
					this.OnChange(item.Operation, item.Previous, asServer);
				}
			}
			list.Clear();
		}

		public void Update(float delta)
		{
			if (Elapsed != -1f && !Paused)
			{
				Elapsed += delta;
			}
		}

		public object GetSerializedType()
		{
			return null;
		}
	}
}
