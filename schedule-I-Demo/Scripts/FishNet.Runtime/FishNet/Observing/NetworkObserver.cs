using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet.Connection;
using FishNet.Documenting;
using FishNet.Managing.Server;
using FishNet.Object;
using FishNet.Transporting;
using GameKit.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace FishNet.Observing
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(NetworkObject))]
	[AddComponentMenu("FishNet/Component/NetworkObserver")]
	public sealed class NetworkObserver : MonoBehaviour
	{
		public enum ConditionOverrideType
		{
			AddMissing = 1,
			UseManager = 2,
			IgnoreManager = 3
		}

		[Tooltip("How ObserverManager conditions are used.")]
		[SerializeField]
		private ConditionOverrideType _overrideType = ConditionOverrideType.IgnoreManager;

		[Tooltip("True to update visibility for clientHost based on if they are an observer or not.")]
		[FormerlySerializedAs("_setHostVisibility")]
		[SerializeField]
		private bool _updateHostVisibility = true;

		[Tooltip("Conditions connections must met to be added as an observer. Multiple conditions may be used.")]
		[SerializeField]
		internal List<ObserverCondition> _observerConditions = new List<ObserverCondition>();

		private List<ObserverCondition> _timedConditions;

		private HashSet<NetworkConnection> _nonTimedMet;

		private NetworkObject _networkObject;

		private bool _registeredAsTimed;

		private bool _initializedPreviously;

		private bool _lastParentVisible;

		private ServerManager _serverManager;

		private bool _hasNormalConditions;

		public ConditionOverrideType OverrideType
		{
			get
			{
				return _overrideType;
			}
			internal set
			{
				_overrideType = value;
			}
		}

		public bool UpdateHostVisibility
		{
			get
			{
				return _updateHostVisibility;
			}
			private set
			{
				_updateHostVisibility = value;
			}
		}

		public IReadOnlyList<ObserverCondition> ObserverConditions => _observerConditions;

		[APIExclude]
		internal List<ObserverCondition> ObserverConditionsInternal
		{
			get
			{
				return _observerConditions;
			}
			set
			{
				_observerConditions = value;
			}
		}

		internal void Deinitialize(bool destroyed)
		{
			_lastParentVisible = false;
			_nonTimedMet?.Clear();
			UnregisterTimedConditions();
			if (_serverManager != null)
			{
				_serverManager.OnRemoteConnectionState -= ServerManager_OnRemoteConnectionState;
			}
			if (_initializedPreviously)
			{
				_hasNormalConditions = false;
				foreach (ObserverCondition observerCondition in _observerConditions)
				{
					observerCondition.Deinitialize(destroyed);
					if (destroyed)
					{
						UnityEngine.Object.Destroy(observerCondition);
					}
				}
				if (destroyed)
				{
					CollectionCaches<ObserverCondition>.Store(_timedConditions);
					CollectionCaches<NetworkConnection>.Store(_nonTimedMet);
				}
			}
			_serverManager = null;
			_networkObject = null;
		}

		internal void Initialize(NetworkObject networkObject)
		{
			_networkObject = networkObject;
			_serverManager = _networkObject.ServerManager;
			_serverManager.OnRemoteConnectionState += ServerManager_OnRemoteConnectionState;
			if (!_initializedPreviously)
			{
				_initializedPreviously = true;
				if (OverrideType != ConditionOverrideType.IgnoreManager)
				{
					UpdateHostVisibility = networkObject.ObserverManager.UpdateHostVisibility;
				}
				List<ObserverCondition> list = CollectionCaches<ObserverCondition>.RetrieveList();
				_timedConditions = CollectionCaches<ObserverCondition>.RetrieveList();
				_nonTimedMet = CollectionCaches<NetworkConnection>.RetrieveHashSet();
				int num = 0;
				bool flag = false;
				for (int i = 0; i < _observerConditions.Count; i++)
				{
					if (_observerConditions[i] != null)
					{
						flag = true;
						ObserverCondition observerCondition = _observerConditions[i].Clone();
						_observerConditions[i] = observerCondition;
						ObserverConditionType observerConditionType = observerCondition.GetConditionType();
						if (observerCondition.Timed() || observerConditionType == ObserverConditionType.Timed)
						{
							observerConditionType = ObserverConditionType.Timed;
							list.Add(observerCondition);
						}
						else
						{
							_hasNormalConditions = true;
							list.Insert(num++, observerCondition);
						}
						if (observerConditionType == ObserverConditionType.Timed)
						{
							_timedConditions.Add(observerCondition);
						}
					}
					else
					{
						_observerConditions.RemoveAt(i);
						i--;
					}
				}
				CollectionCaches<ObserverCondition>.Store(_observerConditions);
				_observerConditions = list;
				if (!flag)
				{
					return;
				}
			}
			for (int j = 0; j < _observerConditions.Count; j++)
			{
				_observerConditions[j].Initialize(_networkObject);
			}
			RegisterTimedConditions();
		}

		public ObserverCondition GetObserverCondition<T>() where T : ObserverCondition
		{
			Type typeFromHandle = typeof(T);
			for (int i = 0; i < _observerConditions.Count; i++)
			{
				if (_observerConditions[i].GetType() == typeFromHandle)
				{
					return _observerConditions[i];
				}
			}
			return null;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal ObserverStateChange RebuildObservers(NetworkConnection connection, bool timedOnly)
		{
			bool flag = _networkObject.Observers.Contains(connection);
			bool flag2 = true;
			if (connection != _networkObject.Owner)
			{
				bool flag3 = true;
				if (_networkObject.CurrentParentNetworkObject != null)
				{
					flag3 = _networkObject.CurrentParentNetworkObject.Observers.Contains(connection);
				}
				if (flag3 && !_lastParentVisible)
				{
					timedOnly = false;
				}
				_lastParentVisible = flag3;
				if (!flag3)
				{
					flag2 = false;
				}
				else if (_observerConditions.Count > 0)
				{
					bool flag4 = !_hasNormalConditions || _nonTimedMet.Contains(connection);
					if (timedOnly && !flag4)
					{
						flag2 = false;
					}
					else
					{
						bool flag5 = true;
						List<ObserverCondition> list = (timedOnly ? _timedConditions : _observerConditions);
						for (int i = 0; i < list.Count; i++)
						{
							ObserverCondition observerCondition = list[i];
							bool notProcessed = false;
							bool flag6 = !observerCondition.GetIsEnabled() || observerCondition.ConditionMet(connection, flag, out notProcessed);
							if (notProcessed)
							{
								flag6 = flag;
							}
							if (!flag6)
							{
								flag2 = false;
								if (observerCondition.GetConditionType() != ObserverConditionType.Timed)
								{
									flag5 = false;
								}
								break;
							}
						}
						if (flag4 != flag5)
						{
							if (flag5)
							{
								_nonTimedMet.Add(connection);
							}
							else
							{
								_nonTimedMet.Remove(connection);
							}
						}
					}
				}
			}
			if (flag2)
			{
				return ReturnPassedConditions(flag);
			}
			return ReturnFailedCondition(flag);
		}

		private void RegisterTimedConditions()
		{
			if (_timedConditions != null && _timedConditions.Count != 0 && !_registeredAsTimed)
			{
				_registeredAsTimed = true;
				if (!(_serverManager == null))
				{
					_serverManager.Objects.AddTimedNetworkObserver(_networkObject);
				}
			}
		}

		private void UnregisterTimedConditions()
		{
			if (_timedConditions != null && _timedConditions.Count != 0 && _registeredAsTimed)
			{
				_registeredAsTimed = false;
				if (!(_serverManager == null))
				{
					_serverManager.Objects.RemoveTimedNetworkObserver(_networkObject);
				}
			}
		}

		private ObserverStateChange ReturnFailedCondition(bool currentlyAdded)
		{
			if (currentlyAdded)
			{
				return ObserverStateChange.Removed;
			}
			return ObserverStateChange.Unchanged;
		}

		private ObserverStateChange ReturnPassedConditions(bool currentlyAdded)
		{
			if (currentlyAdded)
			{
				return ObserverStateChange.Unchanged;
			}
			return ObserverStateChange.Added;
		}

		private void ServerManager_OnRemoteConnectionState(NetworkConnection conn, RemoteConnectionStateArgs arg2)
		{
			if (arg2.ConnectionState == RemoteConnectionState.Stopped)
			{
				_nonTimedMet.Remove(conn);
			}
		}

		public void SetUpdateHostVisibility(bool value)
		{
			if (value != UpdateHostVisibility)
			{
				UpdateHostVisibility = value;
			}
		}
	}
}
