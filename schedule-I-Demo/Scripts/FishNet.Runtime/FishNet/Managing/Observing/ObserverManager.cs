using System;
using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Observing;
using UnityEngine;
using UnityEngine.Serialization;

namespace FishNet.Managing.Observing
{
	[DisallowMultipleComponent]
	[AddComponentMenu("FishNet/Manager/ObserverManager")]
	public sealed class ObserverManager : MonoBehaviour
	{
		[Tooltip("True to use the NetworkLOD system.")]
		[FormerlySerializedAs("_useNetworkLod")]
		[SerializeField]
		private bool _enableNetworkLod;

		[Tooltip("Distance for each level of detal.")]
		[SerializeField]
		private List<float> _levelOfDetailDistances = new List<float>();

		private List<float> _singleLevelOfDetailDistances = new List<float> { float.MaxValue };

		[Tooltip("True to update visibility for clientHost based on if they are an observer or not.")]
		[FormerlySerializedAs("_setHostVisibility")]
		[SerializeField]
		private bool _updateHostVisibility = true;

		[Tooltip("Default observer conditions for networked objects.")]
		[SerializeField]
		private List<ObserverCondition> _defaultConditions = new List<ObserverCondition>();

		private NetworkManager _networkManager;

		private uint[] _levelOfDetailIntervals;

		internal byte LevelOfDetailIndex { get; private set; }

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

		internal bool GetEnableNetworkLod()
		{
			return _enableNetworkLod;
		}

		internal List<float> GetLevelOfDetailDistances()
		{
			if (!_enableNetworkLod)
			{
				return _singleLevelOfDetailDistances;
			}
			return _levelOfDetailDistances;
		}

		internal void InitializeOnce_Internal(NetworkManager manager)
		{
			_networkManager = manager;
			ValidateLevelOfDetails();
		}

		public void SetUpdateHostVisibility(bool value, HostVisibilityUpdateTypes updateType)
		{
			if (value == UpdateHostVisibility)
			{
				return;
			}
			if (HostVisibilityUpdateContains(updateType, HostVisibilityUpdateTypes.Manager))
			{
				UpdateHostVisibility = value;
			}
			if (!_networkManager.IsServer || !HostVisibilityUpdateContains(updateType, HostVisibilityUpdateTypes.Spawned))
			{
				return;
			}
			NetworkConnection connection = _networkManager.ClientManager.Connection;
			foreach (NetworkObject value2 in _networkManager.ServerManager.Objects.Spawned.Values)
			{
				value2.NetworkObserver.SetUpdateHostVisibility(value);
				if (connection.IsActive)
				{
					value2.SetRenderersVisible(value2.Observers.Contains(connection), force: true);
				}
			}
			static bool HostVisibilityUpdateContains(HostVisibilityUpdateTypes whole, HostVisibilityUpdateTypes part)
			{
				return (whole & part) == part;
			}
		}

		internal NetworkObserver AddDefaultConditions(NetworkObject nob)
		{
			bool flag = nob.IsGlobal && !nob.IsSceneObject;
			bool flag2;
			if (!nob.TryGetComponent<NetworkObserver>(out var component))
			{
				flag2 = true;
				component = nob.gameObject.AddComponent<NetworkObserver>();
			}
			else
			{
				flag2 = false;
			}
			if (!flag2 && _defaultConditions.Count == 0)
			{
				return component;
			}
			if (flag2)
			{
				if (flag)
				{
					return component;
				}
				if (_defaultConditions.Count == 0)
				{
					return component;
				}
				component.OverrideType = NetworkObserver.ConditionOverrideType.UseManager;
			}
			else if (flag)
			{
				component.ObserverConditionsInternal.Clear();
				component.OverrideType = NetworkObserver.ConditionOverrideType.IgnoreManager;
			}
			if (component.OverrideType != NetworkObserver.ConditionOverrideType.IgnoreManager)
			{
				if (component.OverrideType == NetworkObserver.ConditionOverrideType.UseManager)
				{
					component.ObserverConditionsInternal.Clear();
					AddMissing(component);
				}
				else if (component.OverrideType == NetworkObserver.ConditionOverrideType.AddMissing)
				{
					AddMissing(component);
				}
			}
			return component;
			void AddMissing(NetworkObserver networkObserver)
			{
				int count = _defaultConditions.Count;
				for (int i = 0; i < count; i++)
				{
					ObserverCondition item = _defaultConditions[i];
					if (!networkObserver.ObserverConditionsInternal.Contains(item))
					{
						networkObserver.ObserverConditionsInternal.Add(item);
					}
				}
			}
		}

		public static byte GetLevelOfDetailInterval(byte lodIndex)
		{
			if (lodIndex == 0)
			{
				return 1;
			}
			return (byte)Math.Pow(2.0, (int)lodIndex);
		}

		internal void CalculateLevelOfDetail(uint tick)
		{
			LevelOfDetailIndex = 0;
		}

		private void ValidateLevelOfDetails()
		{
			_enableNetworkLod = false;
		}
	}
}
