using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine;

namespace Features.ItemsModule.Scripts
{
	public class ItemSpawnService : IItemSpawnService
	{
		private readonly MultiplayerModel _multiplayerModel;

		public ItemSpawnService(MultiplayerModel multiplayerModel)
		{
			_multiplayerModel = multiplayerModel;
		}

		public async UniTask<NetworkBehaviour> SpawnItem(NetworkBehaviour prefab, Vector3 position, Quaternion rotation, ItemData itemData = null, bool addRandomForce = false, float randomForce = 0f)
		{
			if (_multiplayerModel.NetworkRunner == null || !_multiplayerModel.NetworkRunner.IsRunning)
			{
				return null;
			}
			NetworkObject networkObject;
			try
			{
				networkObject = await _multiplayerModel.NetworkRunner.SpawnAsync(prefab, position, rotation, _multiplayerModel.NetworkRunner.LocalPlayer);
			}
			catch (NetworkObjectSpawnException)
			{
				return null;
			}
			if (networkObject == null)
			{
				return null;
			}
			if (networkObject.TryGetComponent<MonoItem>(out var component))
			{
				component.Initialize(itemData);
			}
			if (addRandomForce && randomForce > 0f && networkObject.TryGetComponent<Rigidbody>(out var component2))
			{
				Vector3 normalized = Random.insideUnitSphere.normalized;
				component2.AddForce(normalized * randomForce, ForceMode.Force);
			}
			return networkObject.GetComponent<NetworkBehaviour>();
		}

		public async UniTask SpawnItemsMultiple(NetworkBehaviour prefab, Vector3 centerPosition, int count, float positionRandomRadius, bool useSpread, ItemData itemData = null, bool addRandomForce = false, float randomForce = 0f)
		{
			if (_multiplayerModel.NetworkRunner == null || !_multiplayerModel.NetworkRunner.IsRunning || prefab == null || count <= 0)
			{
				return;
			}
			for (int i = 0; i < count; i++)
			{
				Vector3 position = centerPosition;
				if (useSpread)
				{
					Vector2 vector = Random.insideUnitCircle * positionRandomRadius;
					position += Vector3.up * 0.5f + new Vector3(vector.x, 0f, vector.y);
				}
				Quaternion identity = Quaternion.identity;
				await SpawnItem(prefab, position, identity, itemData, addRandomForce, randomForce);
			}
		}

		public async UniTask SpawnItemsMultiple(Dictionary<NetworkBehaviour, MultipleSpawnData> prefabs, Vector3 centerPosition, float positionRandomRadius, bool useSpread, bool addRandomForce = false, float randomForce = 0f)
		{
			if (_multiplayerModel.NetworkRunner == null || !_multiplayerModel.NetworkRunner.IsRunning || prefabs == null || prefabs.Count == 0)
			{
				return;
			}
			foreach (KeyValuePair<NetworkBehaviour, MultipleSpawnData> keyValuePair in prefabs)
			{
				for (int i = 0; i < keyValuePair.Value.Count; i++)
				{
					Vector3 position = centerPosition;
					if (useSpread)
					{
						Vector2 vector = Random.insideUnitCircle * positionRandomRadius;
						position += Vector3.up * 0.5f + new Vector3(vector.x, 0f, vector.y);
					}
					Quaternion identity = Quaternion.identity;
					await SpawnItem(keyValuePair.Key, position, identity, keyValuePair.Value.ItemData, addRandomForce, randomForce);
				}
			}
		}
	}
}
