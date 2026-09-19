using System;
using UnityEngine.LowLevel;

namespace Fusion
{
	public static class FusionPlayerLoopSystemUtils
	{
		public static IDisposable AddToPlayerLoop(Type referenceSystemType, FusionPlayerLoopSystemAddMode addMode, Type ownerType, PlayerLoopSystem.UpdateFunction updateDelegate)
		{
			PlayerLoopSystem parentSystem = PlayerLoop.GetCurrentPlayerLoop();
			FusionPlayerLoopSystemHandle fusionPlayerLoopSystemHandle = parentSystem.AddToPlayerLoop(referenceSystemType, addMode, ownerType, updateDelegate);
			PlayerLoop.SetPlayerLoop(parentSystem);
			return fusionPlayerLoopSystemHandle;
		}

		public static FusionPlayerLoopSystemHandle AddToPlayerLoop(this ref PlayerLoopSystem parentSystem, Type referenceSystemType, FusionPlayerLoopSystemAddMode addMode, Type ownerType, PlayerLoopSystem.UpdateFunction updateDelegate)
		{
			if (!AddToPlayerLoopInternal(ref parentSystem, referenceSystemType, addMode, ownerType, updateDelegate))
			{
				throw new ArgumentException("Failed to register a system, reference system " + referenceSystemType?.FullName + " not found", "referenceSystemType");
			}
			return new FusionPlayerLoopSystemHandle(ownerType);
		}

		private static bool AddToPlayerLoopInternal(ref PlayerLoopSystem parentSystem, Type referenceSystemType, FusionPlayerLoopSystemAddMode addMode, Type ownerType, PlayerLoopSystem.UpdateFunction updateDelegate)
		{
			ref PlayerLoopSystem[] subSystemList = ref parentSystem.subSystemList;
			PlayerLoopSystem[] obj = subSystemList;
			int num = ((obj != null) ? obj.Length : 0);
			if (parentSystem.type == referenceSystemType)
			{
				switch (addMode)
				{
				case FusionPlayerLoopSystemAddMode.FirstChild:
					InsertSystem(ref subSystemList, 0, ownerType, updateDelegate);
					break;
				case FusionPlayerLoopSystemAddMode.LastChild:
					InsertSystem(ref subSystemList, num, ownerType, updateDelegate);
					break;
				default:
					throw new InvalidOperationException($"Unable to add with a mode {addMode} once a system has been entered");
				}
				return true;
			}
			for (int i = 0; i < num; i++)
			{
				PlayerLoopSystem playerLoopSystem = subSystemList[i];
				if (playerLoopSystem.type == referenceSystemType)
				{
					switch (addMode)
					{
					case FusionPlayerLoopSystemAddMode.Before:
						InsertSystem(ref subSystemList, i, ownerType, updateDelegate);
						return true;
					case FusionPlayerLoopSystemAddMode.After:
						InsertSystem(ref subSystemList, i + 1, ownerType, updateDelegate);
						return true;
					}
				}
				if (AddToPlayerLoopInternal(ref subSystemList[i], referenceSystemType, addMode, ownerType, updateDelegate))
				{
					return true;
				}
			}
			return false;
		}

		public static bool RemoveFromPlayerLoop(Type type)
		{
			PlayerLoopSystem parentSystem = PlayerLoop.GetCurrentPlayerLoop();
			if (!parentSystem.RemoveFromPlayerLoop(type))
			{
				return false;
			}
			PlayerLoop.SetPlayerLoop(parentSystem);
			return true;
		}

		public static bool RemoveFromPlayerLoop(this ref PlayerLoopSystem parentSystem, Type type)
		{
			ref PlayerLoopSystem[] subSystemList = ref parentSystem.subSystemList;
			if (subSystemList == null)
			{
				return false;
			}
			for (int i = 0; i < subSystemList.Length; i++)
			{
				PlayerLoopSystem playerLoopSystem = subSystemList[i];
				if (playerLoopSystem.type == type)
				{
					for (int j = i + 1; j < subSystemList.Length; j++)
					{
						subSystemList[j - 1] = subSystemList[j];
					}
					Array.Resize(ref subSystemList, subSystemList.Length - 1);
					return true;
				}
				if (subSystemList[i].RemoveFromPlayerLoop(type))
				{
					return true;
				}
			}
			return false;
		}

		private static void InsertSystem(ref PlayerLoopSystem[] systems, int position, Type ownerType, PlayerLoopSystem.UpdateFunction updateDelegate)
		{
			PlayerLoopSystem[] obj = systems;
			int num = ((obj != null) ? obj.Length : 0);
			if (position < 0 || position > num)
			{
				throw new ArgumentOutOfRangeException("position");
			}
			PlayerLoopSystem playerLoopSystem = new PlayerLoopSystem
			{
				type = ownerType,
				updateDelegate = updateDelegate
			};
			Array.Resize(ref systems, num + 1);
			if (position < num)
			{
				Array.Copy(systems, position, systems, position + 1, systems.Length - position - 1);
			}
			systems[position] = playerLoopSystem;
		}
	}
}
