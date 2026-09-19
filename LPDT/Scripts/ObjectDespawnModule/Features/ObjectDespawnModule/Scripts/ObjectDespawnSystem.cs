using System;
using System.Collections;
using System.Collections.Generic;
using Features.CoroutineUtils.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using NetworkServices.ObjectsProvider;
using UnityEngine;
using Zenject;

namespace Features.ObjectDespawnModule.Scripts
{
	public class ObjectDespawnSystem : IInitializable, IDisposable
	{
		private readonly ObjectDespawnModel _objectDespawnModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly ICoroutineRunner _coroutineRunner;

		private readonly Dictionary<NetworkObject, Coroutine> _activeDespawnCoroutines = new Dictionary<NetworkObject, Coroutine>();

		public ObjectDespawnSystem(ObjectDespawnModel objectDespawnModel, MultiplayerModel multiplayerModel, ICoroutineRunner coroutineRunner)
		{
			_objectDespawnModel = objectDespawnModel;
			_multiplayerModel = multiplayerModel;
			_coroutineRunner = coroutineRunner;
		}

		public void Initialize()
		{
			_objectDespawnModel.OnObjectAddedToDespawn += DespawnObject;
		}

		public void Dispose()
		{
			_objectDespawnModel.OnObjectAddedToDespawn -= DespawnObject;
			foreach (Coroutine value in _activeDespawnCoroutines.Values)
			{
				if (value != null)
				{
					_coroutineRunner.StopCoroutine(value);
				}
			}
			_activeDespawnCoroutines.Clear();
		}

		private void DespawnObject(DespawnObjectData despawnObjectData)
		{
			if (_multiplayerModel.NetworkRunner == null || despawnObjectData.TargetObject == null || !despawnObjectData.TargetObject.IsValid || (!despawnObjectData.Reliable && !despawnObjectData.TargetObject.HasStateAuthority))
			{
				return;
			}
			if (despawnObjectData.Reliable && !despawnObjectData.TargetObject.HasStateAuthority)
			{
				if (!_activeDespawnCoroutines.ContainsKey(despawnObjectData.TargetObject))
				{
					Coroutine value = _coroutineRunner.StartCoroutine(RequestStateAuthorityAndDespawnCoroutine(despawnObjectData));
					_activeDespawnCoroutines[despawnObjectData.TargetObject] = value;
				}
			}
			else
			{
				DespawnObjectInternal(despawnObjectData);
			}
		}

		private IEnumerator RequestStateAuthorityAndDespawnCoroutine(DespawnObjectData despawnObjectData)
		{
			NetworkObject targetObject = despawnObjectData.TargetObject;
			if (targetObject == null || !targetObject.IsValid)
			{
				if (targetObject != null)
				{
					_activeDespawnCoroutines.Remove(targetObject);
				}
				yield break;
			}
			targetObject.RequestStateAuthority();
			float timeout = 2f;
			float elapsedTime = 0f;
			while (!targetObject.HasStateAuthority && elapsedTime < timeout)
			{
				if (targetObject == null || !targetObject.IsValid)
				{
					NetworkObject targetObject2 = despawnObjectData.TargetObject;
					if (targetObject2 != null)
					{
						_activeDespawnCoroutines.Remove(targetObject2);
					}
					yield break;
				}
				elapsedTime += Time.deltaTime;
				yield return null;
			}
			if (targetObject != null)
			{
				_activeDespawnCoroutines.Remove(targetObject);
			}
			if (targetObject != null && targetObject.HasStateAuthority && targetObject.IsValid)
			{
				DespawnObjectInternal(despawnObjectData);
			}
			else if (targetObject != null)
			{
				Debug.LogWarning("Failed to obtain StateAuthority for object " + targetObject.name + " within timeout period.");
			}
		}

		private void DespawnObjectInternal(DespawnObjectData despawnObjectData)
		{
			if (despawnObjectData.TargetObject == null || !despawnObjectData.TargetObject.IsValid)
			{
				return;
			}
			try
			{
				despawnObjectData.TargetObject.DespawnHierarchy();
				_objectDespawnModel.RemoveObject(despawnObjectData);
			}
			catch (Exception ex)
			{
				Debug.LogError("Error despawning network object: " + ex.Message);
			}
		}
	}
}
