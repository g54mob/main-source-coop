using System;
using System.Collections.Generic;
using Features.GrabModule.Scripts;
using Features.LineArmModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.VoiceSpeakersModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.BootstrapModule.Scripts.Systems
{
	public static class PlayerNetworkObjectCleanup
	{
		public static void AtomicDespawnPlayerObjects(NetworkRunner runner, PlayerRef playerRef)
		{
			if (runner == null || !runner.IsRunning || playerRef == PlayerRef.None)
			{
				return;
			}
			HashSet<NetworkId> hashSet = new HashSet<NetworkId>();
			List<NetworkObject> list = CollectPlayerOwnedObjects(runner, playerRef);
			list.Sort((NetworkObject a, NetworkObject b) => GetHierarchyDepth(b).CompareTo(GetHierarchyDepth(a)));
			foreach (NetworkObject item in list)
			{
				if (!(item == null) && item.IsValid && !hashSet.Contains(item.Id))
				{
					if (item.GetComponent<PlayerInitializer>() != null)
					{
						DropHeldItems(item);
					}
					DespawnHierarchy(runner, item, hashSet);
				}
			}
		}

		public static IReadOnlyCollection<PlayerRef> CollectDepartedOwners(NetworkRunner runner)
		{
			HashSet<PlayerRef> hashSet = new HashSet<PlayerRef>();
			if (runner == null || !runner.IsRunning)
			{
				return hashSet;
			}
			HashSet<PlayerRef> hashSet2 = new HashSet<PlayerRef>(runner.ActivePlayers);
			foreach (NetworkObject allNetworkObject in runner.GetAllNetworkObjects())
			{
				if (!(allNetworkObject == null) && allNetworkObject.IsValid)
				{
					PlayerRef inputAuthority = allNetworkObject.InputAuthority;
					if (!(inputAuthority == PlayerRef.None) && !hashSet2.Contains(inputAuthority) && IsPlayerOwnedObject(allNetworkObject, inputAuthority))
					{
						hashSet.Add(inputAuthority);
					}
				}
			}
			return hashSet;
		}

		public static void DespawnAuthoritativeObjectsOf(NetworkRunner runner, PlayerRef playerRef)
		{
			if (runner == null || !runner.IsRunning || playerRef == PlayerRef.None)
			{
				return;
			}
			HashSet<NetworkId> hashSet = new HashSet<NetworkId>();
			List<NetworkObject> list = CollectPlayerOwnedObjects(runner, playerRef);
			list.Sort((NetworkObject a, NetworkObject b) => GetHierarchyDepth(b).CompareTo(GetHierarchyDepth(a)));
			foreach (NetworkObject item in list)
			{
				if (!(item == null) && item.IsValid && !hashSet.Contains(item.Id) && item.HasStateAuthority)
				{
					if (item.GetComponent<PlayerInitializer>() != null)
					{
						DropHeldItems(item);
					}
					DespawnHierarchy(runner, item, hashSet);
				}
			}
		}

		public static void RequestAuthorityOverPlayerObjects(NetworkRunner runner, PlayerRef playerRef)
		{
			if (runner == null || !runner.IsRunning || playerRef == PlayerRef.None)
			{
				return;
			}
			foreach (NetworkObject item in CollectPlayerOwnedObjects(runner, playerRef))
			{
				if (!(item == null) && item.IsValid && !item.HasStateAuthority)
				{
					item.RequestStateAuthority();
				}
			}
		}

		public static bool HasAuthorityOverPlayerObjects(NetworkRunner runner, PlayerRef playerRef)
		{
			if (runner == null || !runner.IsRunning || playerRef == PlayerRef.None)
			{
				return false;
			}
			foreach (NetworkObject item in CollectPlayerOwnedObjects(runner, playerRef))
			{
				if (!(item == null) && item.IsValid && !item.HasStateAuthority)
				{
					return false;
				}
			}
			return true;
		}

		public static bool HasPlayerObjects(NetworkRunner runner, PlayerRef playerRef)
		{
			if (runner == null || !runner.IsRunning || playerRef == PlayerRef.None)
			{
				return false;
			}
			return CollectPlayerOwnedObjects(runner, playerRef).Count > 0;
		}

		public static void ReleaseLocalGrabsOnPlayerObjects(NetworkRunner runner, PlayerRef playerRef)
		{
			if (runner == null || !runner.IsRunning || playerRef == PlayerRef.None)
			{
				return;
			}
			HashSet<NetworkObject> avatarObjects = CollectAvatarHierarchyObjects(runner, playerRef);
			if (avatarObjects.Count == 0)
			{
				return;
			}
			LineArmControllerBase[] array = UnityEngine.Object.FindObjectsByType<LineArmControllerBase>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			foreach (LineArmControllerBase lineArmControllerBase in array)
			{
				if (!(lineArmControllerBase.Object == null) && lineArmControllerBase.Object.IsValid && !(lineArmControllerBase.Object.InputAuthority != runner.LocalPlayer))
				{
					lineArmControllerBase.UnJoinAllMatching((IPointGrabable grabbable) => grabbable.NetworkObject != null && avatarObjects.Contains(grabbable.NetworkObject));
				}
			}
		}

		private static HashSet<NetworkObject> CollectAvatarHierarchyObjects(NetworkRunner runner, PlayerRef playerRef)
		{
			HashSet<NetworkObject> hashSet = new HashSet<NetworkObject>();
			foreach (NetworkObject allNetworkObject in runner.GetAllNetworkObjects())
			{
				if (!(allNetworkObject == null) && allNetworkObject.IsValid && !(allNetworkObject.InputAuthority != playerRef) && !(allNetworkObject.GetComponent<PlayerInitializer>() == null))
				{
					NetworkObject[] componentsInChildren = allNetworkObject.GetComponentsInChildren<NetworkObject>(includeInactive: true);
					foreach (NetworkObject item in componentsInChildren)
					{
						hashSet.Add(item);
					}
				}
			}
			return hashSet;
		}

		private static List<NetworkObject> CollectPlayerOwnedObjects(NetworkRunner runner, PlayerRef playerRef)
		{
			List<NetworkObject> list = new List<NetworkObject>();
			foreach (NetworkObject allNetworkObject in runner.GetAllNetworkObjects())
			{
				if (!(allNetworkObject == null) && allNetworkObject.IsValid && IsPlayerOwnedObject(allNetworkObject, playerRef))
				{
					list.Add(allNetworkObject);
				}
			}
			return list;
		}

		private static bool IsPlayerOwnedObject(NetworkObject networkObject, PlayerRef playerRef)
		{
			if (networkObject.InputAuthority != playerRef)
			{
				return false;
			}
			if (networkObject.GetComponent<PlayerInitializer>() != null)
			{
				return true;
			}
			if (networkObject.GetComponent<PlayerSpeakerAutoRegister>() != null)
			{
				return true;
			}
			NetworkBehaviour[] components = networkObject.GetComponents<NetworkBehaviour>();
			foreach (NetworkBehaviour networkBehaviour in components)
			{
				if (!(networkBehaviour == null))
				{
					string text = networkBehaviour.GetType().FullName ?? string.Empty;
					if (text.StartsWith("Features.LineArmModule.") || text.StartsWith("Features.StrechArmsModule.") || text.Contains(".PhysGrabber"))
					{
						return true;
					}
				}
			}
			return false;
		}

		private static void DropHeldItems(NetworkObject playerAvatar)
		{
			LineArmControllerBase[] componentsInChildren = playerAvatar.GetComponentsInChildren<LineArmControllerBase>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].UnJoinAll(throwItem: false);
			}
		}

		private static void DespawnHierarchy(NetworkRunner runner, NetworkObject root, HashSet<NetworkId> despawnedIds)
		{
			NetworkObject[] componentsInChildren = root.GetComponentsInChildren<NetworkObject>(includeInactive: true);
			Array.Sort(componentsInChildren, (NetworkObject a, NetworkObject b) => GetHierarchyDepth(b).CompareTo(GetHierarchyDepth(a)));
			NetworkObject[] array = componentsInChildren;
			foreach (NetworkObject networkObject in array)
			{
				if (!(networkObject == null) && networkObject.IsValid && !despawnedIds.Contains(networkObject.Id))
				{
					despawnedIds.Add(networkObject.Id);
					runner.Despawn(networkObject);
				}
			}
		}

		private static int GetHierarchyDepth(NetworkObject networkObject)
		{
			if (networkObject == null)
			{
				return 0;
			}
			int num = 0;
			Transform transform = networkObject.transform;
			while (transform.parent != null)
			{
				num++;
				transform = transform.parent;
			}
			return num;
		}
	}
}
