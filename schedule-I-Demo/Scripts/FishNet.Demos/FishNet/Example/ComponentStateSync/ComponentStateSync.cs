using System.Runtime.CompilerServices;
using FishNet.Documenting;
using FishNet.Object.Synchronizing;
using FishNet.Object.Synchronizing.Internal;
using FishNet.Serializing;
using UnityEngine;

namespace FishNet.Example.ComponentStateSync
{
	public class ComponentStateSync<T> : SyncBase, ICustomSync where T : MonoBehaviour
	{
		public delegate void StateChanged(T component, bool prevState, bool nextState, bool asServer);

		public bool Enabled
		{
			get
			{
				if (!(Component == null))
				{
					return GetState();
				}
				return false;
			}
			set
			{
				SetState(value);
			}
		}

		public T Component { get; private set; }

		public event StateChanged OnChange;

		public void Initialize(T component)
		{
			Component = component;
		}

		private void SetState(bool enabled)
		{
			if (!(NetworkManager == null))
			{
				if (Component == null)
				{
					NetworkManager.LogError("State cannot be changed as Initialize has not been called with a valid component.");
				}
				bool state = GetState();
				if (enabled != state)
				{
					Component.enabled = enabled;
					AddOperation(Component, state, enabled);
				}
			}
		}

		private bool GetState()
		{
			return Component.enabled;
		}

		private void AddOperation(T component, bool prev, bool next)
		{
			if (base.IsRegistered)
			{
				if (NetworkManager != null && !NetworkBehaviour.IsServer)
				{
					NetworkManager.LogWarning("Cannot complete operation as server when server is not active.");
					return;
				}
				Dirty();
				bool asServer = true;
				this.OnChange?.Invoke(component, prev, next, asServer);
			}
		}

		public override void WriteDelta(PooledWriter writer, bool resetSyncTick = true)
		{
			base.WriteDelta(writer, resetSyncTick);
			writer.WriteBoolean(Component.enabled);
		}

		public override void WriteFull(PooledWriter writer)
		{
			WriteDelta(writer, resetSyncTick: false);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[APIExclude]
		public override void Read(PooledReader reader, bool asServer)
		{
			bool flag = reader.ReadBoolean();
			if (!(NetworkManager == null))
			{
				bool state = GetState();
				if (asServer || !NetworkManager.IsServer)
				{
					Component.enabled = flag;
				}
				this.OnChange?.Invoke(Component, state, flag, asServer);
			}
		}

		public object GetSerializedType()
		{
			return typeof(bool);
		}
	}
}
