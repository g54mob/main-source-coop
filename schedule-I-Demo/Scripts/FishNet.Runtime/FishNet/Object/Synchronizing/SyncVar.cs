using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using FishNet.Documenting;
using FishNet.Object.Helping;
using FishNet.Object.Synchronizing.Internal;
using FishNet.Serializing;
using FishNet.Serializing.Helping;
using FishNet.Transporting;

namespace FishNet.Object.Synchronizing
{
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	[APIExclude]
	public class SyncVar<T> : SyncBase
	{
		private struct CachedOnChange
		{
			internal readonly T Previous;

			internal readonly T Next;

			public CachedOnChange(T previous, T next)
			{
				Previous = previous;
				Next = next;
			}
		}

		private CachedOnChange? _serverOnChange;

		private CachedOnChange? _clientOnChange;

		private T _initialValue;

		private T _previousClientValue;

		private T _value;

		public event Action<T, T, bool> OnChange;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public SyncVar(NetworkBehaviour nb, uint syncIndex, WritePermission writePermission, ReadPermission readPermission, float sendRate, Channel channel, T value)
		{
			SetInitialValues(value);
			InitializeInstance(nb, syncIndex, writePermission, readPermission, sendRate, channel, isSyncObject: false);
		}

		protected override void Registered()
		{
			base.Registered();
			_initialValue = _value;
		}

		private void SetInitialValues(T next)
		{
			_initialValue = next;
			UpdateValues(next);
		}

		private void UpdateValues(T next)
		{
			_previousClientValue = next;
			_value = next;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetValue(T nextValue, bool calledByUser)
		{
			if (!base.IsRegistered)
			{
				return;
			}
			bool isNetworkInitialized = base.IsNetworkInitialized;
			if (isNetworkInitialized && CodegenHelper.NetworkObject_Deinitializing(NetworkBehaviour))
			{
				return;
			}
			if (calledByUser)
			{
				if (!CanNetworkSetValues())
				{
					return;
				}
				bool asServer = !isNetworkInitialized || NetworkBehaviour.IsServer;
				T value = _value;
				if (!isNetworkInitialized)
				{
					if (Comparers.EqualityCompare(_value, nextValue))
					{
						return;
					}
					_value = nextValue;
					InvokeOnChange(value, _value, asServer: true);
					InvokeOnChange(value, _value, asServer: false);
				}
				else
				{
					if (Comparers.EqualityCompare(_value, nextValue))
					{
						return;
					}
					_value = nextValue;
					InvokeOnChange(value, _value, asServer);
				}
				TryDirty(asServer);
				return;
			}
			T previousClientValue = _previousClientValue;
			if (!Comparers.EqualityCompare(previousClientValue, nextValue))
			{
				if (!NetworkManager.IsServer)
				{
					UpdateValues(nextValue);
				}
				else
				{
					_previousClientValue = nextValue;
				}
				InvokeOnChange(previousClientValue, nextValue, calledByUser);
			}
			void TryDirty(bool flag)
			{
				if (isNetworkInitialized && flag)
				{
					Dirty();
				}
			}
		}

		private void InvokeOnChange(T prev, T next, bool asServer)
		{
			if (asServer)
			{
				if (NetworkBehaviour.OnStartServerCalled)
				{
					this.OnChange?.Invoke(prev, next, asServer);
					_serverOnChange = null;
				}
				else
				{
					_serverOnChange = new CachedOnChange(prev, next);
				}
			}
			else if (NetworkBehaviour.OnStartClientCalled)
			{
				this.OnChange?.Invoke(prev, next, asServer);
				_clientOnChange = null;
			}
			else
			{
				_clientOnChange = new CachedOnChange(prev, next);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override void OnStartCallback(bool asServer)
		{
			base.OnStartCallback(asServer);
			if (this.OnChange != null)
			{
				CachedOnChange? cachedOnChange = (asServer ? _serverOnChange : _clientOnChange);
				if (cachedOnChange.HasValue)
				{
					InvokeOnChange(cachedOnChange.Value.Previous, cachedOnChange.Value.Next, asServer);
				}
			}
			if (asServer)
			{
				_serverOnChange = null;
			}
			else
			{
				_clientOnChange = null;
			}
		}

		public override void WriteDelta(PooledWriter writer, bool resetSyncTick = true)
		{
			base.WriteDelta(writer, resetSyncTick);
			writer.Write(_value);
		}

		public override void WriteFull(PooledWriter obj0)
		{
			if (!Comparers.EqualityCompare(_initialValue, _value))
			{
				WriteDelta(obj0, resetSyncTick: false);
			}
		}

		public T GetValue(bool calledByUser)
		{
			if (!calledByUser)
			{
				return _previousClientValue;
			}
			return _value;
		}

		public override void ResetState()
		{
			base.ResetState();
			_value = _initialValue;
			_previousClientValue = _initialValue;
		}
	}
}
