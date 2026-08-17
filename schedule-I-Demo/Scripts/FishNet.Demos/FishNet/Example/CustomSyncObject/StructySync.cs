using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet.Documenting;
using FishNet.Object.Synchronizing;
using FishNet.Object.Synchronizing.Internal;
using FishNet.Serializing;

namespace FishNet.Example.CustomSyncObject
{
	public class StructySync : SyncBase, ICustomSync
	{
		private struct ChangeData
		{
			internal CustomOperation Operation;

			internal Structy Data;

			public ChangeData(CustomOperation operation, Structy data)
			{
				Operation = operation;
				Data = data;
			}
		}

		public enum CustomOperation : byte
		{
			Full = 0,
			Name = 1,
			Age = 2
		}

		public delegate void CustomChanged(CustomOperation op, Structy oldItem, Structy newItem, bool asServer);

		public Structy Value;

		private Structy _initialValue;

		private readonly List<ChangeData> _changed = new List<ChangeData>();

		private bool _valuesChanged;

		private Structy _lastDirtied;

		public event CustomChanged OnChange;

		protected override void Registered()
		{
			base.Registered();
			_initialValue = Value;
		}

		private void AddOperation(CustomOperation operation, Structy prev, Structy next)
		{
			if (base.IsRegistered)
			{
				if (NetworkManager != null && !NetworkBehaviour.IsServer)
				{
					NetworkManager.LogWarning("Cannot complete operation as server when server is not active.");
					return;
				}
				_valuesChanged = true;
				Dirty();
				bool asServer = true;
				ChangeData item = new ChangeData(operation, next);
				_changed.Add(item);
				this.OnChange?.Invoke(operation, prev, next, asServer);
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
				if (changeData.Operation == CustomOperation.Age)
				{
					writer.WriteUInt16(changeData.Data.Age);
				}
				else if (changeData.Operation == CustomOperation.Name)
				{
					writer.WriteString(changeData.Data.Name);
				}
			}
			_changed.Clear();
		}

		public override void WriteFull(PooledWriter writer)
		{
			if (_valuesChanged)
			{
				base.WriteHeader(writer, resetSyncTick: false);
				writer.WriteInt32(1);
				writer.WriteByte(0);
				writer.Write(Value);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[APIExclude]
		public override void Read(PooledReader reader, bool asServer)
		{
			bool flag = !asServer && NetworkManager.IsServer;
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				CustomOperation customOperation = (CustomOperation)reader.ReadByte();
				Structy value = Value;
				Structy structy = value;
				switch (customOperation)
				{
				case CustomOperation.Full:
					structy = reader.Read<Structy>();
					break;
				case CustomOperation.Name:
					structy.Name = reader.ReadString();
					break;
				case CustomOperation.Age:
					structy.Age = reader.ReadUInt16();
					break;
				}
				this.OnChange?.Invoke(customOperation, value, structy, asServer);
				if (!flag)
				{
					Value = structy;
				}
			}
		}

		public void ValuesChanged()
		{
			Structy lastDirtied = _lastDirtied;
			Structy value = Value;
			if (lastDirtied.Name != value.Name)
			{
				AddOperation(CustomOperation.Name, lastDirtied, value);
			}
			if (lastDirtied.Age != value.Age)
			{
				AddOperation(CustomOperation.Age, lastDirtied, value);
			}
			_lastDirtied = Value;
		}

		public override void ResetState()
		{
			base.ResetState();
			_changed.Clear();
			Value = _initialValue;
			_valuesChanged = false;
		}

		public object GetSerializedType()
		{
			return typeof(Structy);
		}
	}
}
