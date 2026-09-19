using System;
using System.Collections.Generic;
using System.Reflection;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using NetworkServices.ObjectsProvider;
using UnityEngine;

namespace Features.AIModule.Scripts
{
	public class EnemyForceDespawnService : IEnemyForceDespawnService
	{
		private readonly MultiplayerModel _multiplayerModel;

		public EnemyForceDespawnService(MultiplayerModel multiplayerModel)
		{
			_multiplayerModel = multiplayerModel;
		}

		public int ForceDespawnAllLiveEnemies()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning)
			{
				Debug.LogWarning("Force enemy despawn debug action requires an active network session.");
				return 0;
			}
			if (!networkRunner.IsSharedModeMasterClient)
			{
				Debug.LogWarning("Force enemy despawn debug action is host-only.");
				return 0;
			}
			IReadOnlyList<IEnemyBehaviour> readOnlyList = FindLiveEnemyBehaviours();
			int num = 0;
			for (int i = 0; i < readOnlyList.Count; i++)
			{
				IEnemyBehaviour enemyBehaviour = readOnlyList[i];
				NetworkObject networkObject = enemyBehaviour.NetworkObject;
				if (!(networkObject == null) && networkObject.IsValid && networkObject.HasStateAuthority)
				{
					if (!TryRunCustomForcedDespawn(enemyBehaviour))
					{
						networkObject.DespawnHierarchy();
					}
					num++;
				}
			}
			return num;
		}

		private static IReadOnlyList<IEnemyBehaviour> FindLiveEnemyBehaviours()
		{
			NetworkBehaviour[] array = UnityEngine.Object.FindObjectsByType<NetworkBehaviour>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
			List<IEnemyBehaviour> list = new List<IEnemyBehaviour>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] is IEnemyBehaviour item)
				{
					list.Add(item);
				}
			}
			return list;
		}

		private static bool TryRunCustomForcedDespawn(IEnemyBehaviour enemyBehaviour)
		{
			MethodInfo method = enemyBehaviour.GetType().GetMethod("DespawnSwarm", Type.EmptyTypes);
			if (method == null)
			{
				return false;
			}
			method.Invoke(enemyBehaviour, Array.Empty<object>());
			return true;
		}
	}
}
