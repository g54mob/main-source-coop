using System;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

namespace FishNet.Observing
{
	public abstract class ObserverCondition : ScriptableObject
	{
		[HideInInspector]
		public NetworkObject NetworkObject;

		private bool _isEnabled = true;

		public bool GetIsEnabled()
		{
			return _isEnabled;
		}

		public void SetIsEnabled(bool value)
		{
			if (value != GetIsEnabled())
			{
				_isEnabled = value;
				if (!(NetworkObject == null))
				{
					(NetworkObject?.ServerManager?.Objects)?.RebuildObservers(NetworkObject);
				}
			}
		}

		public virtual void Initialize(NetworkObject networkObject)
		{
			NetworkObject = networkObject;
		}

		public virtual void Deinitialize(bool destroyed)
		{
			NetworkObject = null;
		}

		public abstract bool ConditionMet(NetworkConnection connection, bool currentlyAdded, out bool notProcessed);

		[Obsolete("Use GetConditionType()")]
		public virtual bool Timed()
		{
			return false;
		}

		public virtual ObserverConditionType GetConditionType()
		{
			return ObserverConditionType.Normal;
		}

		public abstract ObserverCondition Clone();
	}
}
