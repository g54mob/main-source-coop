using System;
using System.Collections.Generic;
using EvilCore.EvilPack.EvilLogger;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

namespace NomadDrive.Features.Interaction
{
	public class StaticInteractableManager : NetworkBehaviour, IStaticInteractableManager
	{
		private readonly SyncList<StaticInteractableState> _syncedStates = new SyncList<StaticInteractableState>();

		private readonly SyncList<StaticInteractableExtendedState> _syncedExtendedStates = new SyncList<StaticInteractableExtendedState>();

		private readonly Dictionary<int, StaticInteractable> _registeredInteractables = new Dictionary<int, StaticInteractable>();

		private readonly Dictionary<int, int> _stateIdToIndex = new Dictionary<int, int>();

		private readonly Dictionary<int, int> _extendedStateIdToIndex = new Dictionary<int, int>();

		private readonly Dictionary<int, StaticInteractableState> _pendingStates = new Dictionary<int, StaticInteractableState>();

		private readonly Dictionary<int, StaticInteractableExtendedState> _pendingExtendedStates = new Dictionary<int, StaticInteractableExtendedState>();

		public override void OnStartServer()
		{
			base.OnStartServer();
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			SyncList<StaticInteractableState> syncedStates = _syncedStates;
			syncedStates.Callback = (Action<SyncList<StaticInteractableState>.Operation, int, StaticInteractableState, StaticInteractableState>)Delegate.Combine(syncedStates.Callback, new Action<SyncList<StaticInteractableState>.Operation, int, StaticInteractableState, StaticInteractableState>(OnSyncedStatesChanged));
			SyncList<StaticInteractableExtendedState> syncedExtendedStates = _syncedExtendedStates;
			syncedExtendedStates.Callback = (Action<SyncList<StaticInteractableExtendedState>.Operation, int, StaticInteractableExtendedState, StaticInteractableExtendedState>)Delegate.Combine(syncedExtendedStates.Callback, new Action<SyncList<StaticInteractableExtendedState>.Operation, int, StaticInteractableExtendedState, StaticInteractableExtendedState>(OnSyncedExtendedStatesChanged));
			RebuildIndexMaps();
			ProcessExistingStatesForLateJoiner();
		}

		private void OnDestroy()
		{
			SyncList<StaticInteractableState> syncedStates = _syncedStates;
			syncedStates.Callback = (Action<SyncList<StaticInteractableState>.Operation, int, StaticInteractableState, StaticInteractableState>)Delegate.Remove(syncedStates.Callback, new Action<SyncList<StaticInteractableState>.Operation, int, StaticInteractableState, StaticInteractableState>(OnSyncedStatesChanged));
			SyncList<StaticInteractableExtendedState> syncedExtendedStates = _syncedExtendedStates;
			syncedExtendedStates.Callback = (Action<SyncList<StaticInteractableExtendedState>.Operation, int, StaticInteractableExtendedState, StaticInteractableExtendedState>)Delegate.Remove(syncedExtendedStates.Callback, new Action<SyncList<StaticInteractableExtendedState>.Operation, int, StaticInteractableExtendedState, StaticInteractableExtendedState>(OnSyncedExtendedStatesChanged));
		}

		public bool Register(StaticInteractable interactable)
		{
			int interactableId = interactable.InteractableId;
			if (!_registeredInteractables.TryAdd(interactableId, interactable))
			{
				StaticInteractable staticInteractable = _registeredInteractables[interactableId];
				if (!(staticInteractable == null))
				{
					EvilLogger.LogError($"[StaticInteractableManager] DUPLICATE InteractableId {interactableId}: incoming " + $"'{interactable.interactableName}' @ {interactable.transform.position} collides with " + $"already-registered '{staticInteractable.interactableName}' @ {staticInteractable.transform.position}. Interactions on one will affect the " + "other — give one a unique _identifierSuffix or fix the hierarchy.", "Register", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Interaction\\Scripts\\StaticInteraction\\StaticInteractableManager.cs", 73);
					return false;
				}
				_registeredInteractables[interactableId] = interactable;
			}
			if (_pendingStates.TryGetValue(interactableId, out var value))
			{
				interactable.ApplyStateFromManager(value.stateData, skipAnimation: true);
				_pendingStates.Remove(interactableId);
				return true;
			}
			if (_pendingExtendedStates.TryGetValue(interactableId, out var value2))
			{
				if (interactable is IExtendedStateInteractable extendedStateInteractable)
				{
					extendedStateInteractable.ApplyExtendedStateFromManager(value2.stateData, value2.GetNormalizedFloat(), skipAnimation: true);
				}
				_pendingExtendedStates.Remove(interactableId);
				return true;
			}
			if (_stateIdToIndex.TryGetValue(interactableId, out var value3))
			{
				interactable.ApplyStateFromManager(_syncedStates[value3].stateData, skipAnimation: true);
				return true;
			}
			if (_extendedStateIdToIndex.TryGetValue(interactableId, out var value4))
			{
				StaticInteractableExtendedState staticInteractableExtendedState = _syncedExtendedStates[value4];
				if (interactable is IExtendedStateInteractable extendedStateInteractable2)
				{
					extendedStateInteractable2.ApplyExtendedStateFromManager(staticInteractableExtendedState.stateData, staticInteractableExtendedState.GetNormalizedFloat(), skipAnimation: true);
				}
				return true;
			}
			return false;
		}

		public void Unregister(StaticInteractable interactable)
		{
			int interactableId = interactable.InteractableId;
			_registeredInteractables.Remove(interactableId);
		}

		public void RequestStateChange(int id, byte newState)
		{
			CmdRequestStateChange(id, newState);
		}

		public void RequestExtendedStateChange(int id, byte newState, byte normalizedValue)
		{
			CmdRequestExtendedStateChange(id, newState, normalizedValue);
		}

		[Command(requiresAuthority = false)]
		private void CmdRequestStateChange(int id, byte newState)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarInt(id);
			NetworkWriterExtensions.WriteByte(writer, newState);
			SendCommandInternal("System.Void NomadDrive.Features.Interaction.StaticInteractableManager::CmdRequestStateChange(System.Int32,System.Byte)", 888515486, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command(requiresAuthority = false)]
		private void CmdRequestExtendedStateChange(int id, byte newState, byte normalizedValue)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarInt(id);
			NetworkWriterExtensions.WriteByte(writer, newState);
			NetworkWriterExtensions.WriteByte(writer, normalizedValue);
			SendCommandInternal("System.Void NomadDrive.Features.Interaction.StaticInteractableManager::CmdRequestExtendedStateChange(System.Int32,System.Byte,System.Byte)", -312082704, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		public StaticInteractableState? GetState(int id)
		{
			if (_stateIdToIndex.TryGetValue(id, out var value))
			{
				return _syncedStates[value];
			}
			return null;
		}

		public StaticInteractableExtendedState? GetExtendedState(int id)
		{
			if (_extendedStateIdToIndex.TryGetValue(id, out var value))
			{
				return _syncedExtendedStates[value];
			}
			return null;
		}

		private void OnSyncedStatesChanged(SyncList<StaticInteractableState>.Operation op, int index, StaticInteractableState oldItem, StaticInteractableState newItem)
		{
			switch (op)
			{
			case SyncList<StaticInteractableState>.Operation.OP_ADD:
				_stateIdToIndex[newItem.id] = index;
				ApplyStateToInteractable(newItem);
				break;
			case SyncList<StaticInteractableState>.Operation.OP_SET:
				ApplyStateToInteractable(newItem);
				break;
			case SyncList<StaticInteractableState>.Operation.OP_REMOVEAT:
				_stateIdToIndex.Remove(oldItem.id);
				break;
			case SyncList<StaticInteractableState>.Operation.OP_CLEAR:
				_stateIdToIndex.Clear();
				break;
			case SyncList<StaticInteractableState>.Operation.OP_INSERT:
				break;
			}
		}

		private void OnSyncedExtendedStatesChanged(SyncList<StaticInteractableExtendedState>.Operation op, int index, StaticInteractableExtendedState oldItem, StaticInteractableExtendedState newItem)
		{
			switch (op)
			{
			case SyncList<StaticInteractableExtendedState>.Operation.OP_ADD:
				_extendedStateIdToIndex[newItem.id] = index;
				ApplyExtendedStateToInteractable(newItem);
				break;
			case SyncList<StaticInteractableExtendedState>.Operation.OP_SET:
				ApplyExtendedStateToInteractable(newItem);
				break;
			case SyncList<StaticInteractableExtendedState>.Operation.OP_REMOVEAT:
				_extendedStateIdToIndex.Remove(oldItem.id);
				break;
			case SyncList<StaticInteractableExtendedState>.Operation.OP_CLEAR:
				_extendedStateIdToIndex.Clear();
				break;
			case SyncList<StaticInteractableExtendedState>.Operation.OP_INSERT:
				break;
			}
		}

		private void ApplyStateToInteractable(StaticInteractableState state)
		{
			if (_registeredInteractables.TryGetValue(state.id, out var value))
			{
				value.ApplyStateFromManager(state.stateData, skipAnimation: false);
			}
			else
			{
				_pendingStates[state.id] = state;
			}
		}

		private void ApplyExtendedStateToInteractable(StaticInteractableExtendedState state)
		{
			if (_registeredInteractables.TryGetValue(state.id, out var value))
			{
				if (value is IExtendedStateInteractable extendedStateInteractable)
				{
					extendedStateInteractable.ApplyExtendedStateFromManager(state.stateData, state.GetNormalizedFloat(), skipAnimation: false);
				}
			}
			else
			{
				_pendingExtendedStates[state.id] = state;
			}
		}

		private void RebuildIndexMaps()
		{
			_stateIdToIndex.Clear();
			for (int i = 0; i < _syncedStates.Count; i++)
			{
				_stateIdToIndex[_syncedStates[i].id] = i;
			}
			_extendedStateIdToIndex.Clear();
			for (int j = 0; j < _syncedExtendedStates.Count; j++)
			{
				_extendedStateIdToIndex[_syncedExtendedStates[j].id] = j;
			}
		}

		private void ProcessExistingStatesForLateJoiner()
		{
			foreach (StaticInteractableState syncedState in _syncedStates)
			{
				if (!_registeredInteractables.ContainsKey(syncedState.id))
				{
					_pendingStates[syncedState.id] = syncedState;
				}
			}
			foreach (StaticInteractableExtendedState syncedExtendedState in _syncedExtendedStates)
			{
				if (!_registeredInteractables.ContainsKey(syncedExtendedState.id))
				{
					_pendingExtendedStates[syncedExtendedState.id] = syncedExtendedState;
				}
			}
		}

		public StaticInteractableManager()
		{
			InitSyncObject(_syncedStates);
			InitSyncObject(_syncedExtendedStates);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdRequestStateChange__Int32__Byte(int id, byte newState)
		{
			float time = Time.time;
			if (_stateIdToIndex.TryGetValue(id, out var value))
			{
				if (time >= _syncedStates[value].timestamp)
				{
					_syncedStates[value] = new StaticInteractableState(id, newState, time);
				}
			}
			else
			{
				StaticInteractableState item = new StaticInteractableState(id, newState, time);
				_syncedStates.Add(item);
				_stateIdToIndex[id] = _syncedStates.Count - 1;
			}
		}

		protected static void InvokeUserCode_CmdRequestStateChange__Int32__Byte(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdRequestStateChange called on client.");
			}
			else
			{
				((StaticInteractableManager)obj).UserCode_CmdRequestStateChange__Int32__Byte(reader.ReadVarInt(), NetworkReaderExtensions.ReadByte(reader));
			}
		}

		protected void UserCode_CmdRequestExtendedStateChange__Int32__Byte__Byte(int id, byte newState, byte normalizedValue)
		{
			float time = Time.time;
			if (_extendedStateIdToIndex.TryGetValue(id, out var value))
			{
				if (time >= _syncedExtendedStates[value].timestamp)
				{
					_syncedExtendedStates[value] = new StaticInteractableExtendedState(id, newState, normalizedValue, time);
				}
			}
			else
			{
				StaticInteractableExtendedState item = new StaticInteractableExtendedState(id, newState, normalizedValue, time);
				_syncedExtendedStates.Add(item);
				_extendedStateIdToIndex[id] = _syncedExtendedStates.Count - 1;
			}
		}

		protected static void InvokeUserCode_CmdRequestExtendedStateChange__Int32__Byte__Byte(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdRequestExtendedStateChange called on client.");
			}
			else
			{
				((StaticInteractableManager)obj).UserCode_CmdRequestExtendedStateChange__Int32__Byte__Byte(reader.ReadVarInt(), NetworkReaderExtensions.ReadByte(reader), NetworkReaderExtensions.ReadByte(reader));
			}
		}

		static StaticInteractableManager()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(StaticInteractableManager), "System.Void NomadDrive.Features.Interaction.StaticInteractableManager::CmdRequestStateChange(System.Int32,System.Byte)", InvokeUserCode_CmdRequestStateChange__Int32__Byte, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(StaticInteractableManager), "System.Void NomadDrive.Features.Interaction.StaticInteractableManager::CmdRequestExtendedStateChange(System.Int32,System.Byte,System.Byte)", InvokeUserCode_CmdRequestExtendedStateChange__Int32__Byte__Byte, requiresAuthority: false);
		}
	}
}
