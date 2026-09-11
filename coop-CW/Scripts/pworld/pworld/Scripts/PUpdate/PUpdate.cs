using System.Collections.Generic;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace pworld.Scripts.PUpdate
{
	public static class PUpdate
	{
		private static readonly HashSet<IPPreUpdate> _earlyUpdates = new HashSet<IPPreUpdate>();

		private static readonly HashSet<IPPostLateUpdate> _superLateUpdates = new HashSet<IPPostLateUpdate>();

		public static void RegisterPreUpdate(IPPreUpdate _IPPreUpdate)
		{
			_earlyUpdates.Add(_IPPreUpdate);
		}

		public static void RegisterSuperLateUpdate(IPPostLateUpdate _ipPostLateUpdate)
		{
			_superLateUpdates.Add(_ipPostLateUpdate);
		}

		public static void UnregisterPreUpdate(IPPreUpdate _IPPreUpdate)
		{
			_earlyUpdates.Remove(_IPPreUpdate);
		}

		public static void UnregisterSuperLateUpdate(IPPostLateUpdate _ipPostLateUpdate)
		{
			_superLateUpdates.Remove(_ipPostLateUpdate);
		}

		private static void Init()
		{
			PlayerLoopSystem loopSystem = PlayerLoop.GetDefaultPlayerLoop();
			PlayerLoopSystem systemToAdd = new PlayerLoopSystem
			{
				subSystemList = null,
				updateDelegate = OnSuperLateUpdate,
				type = typeof(MySuperLateUpdate)
			};
			PlayerLoopSystem systemToAdd2 = new PlayerLoopSystem
			{
				subSystemList = null,
				updateDelegate = OnEarlyUpdate,
				type = typeof(MyEarlyUpdate)
			};
			PlayerLoop.SetPlayerLoop(AddSystem<PreUpdate>(AddSystem<PreLateUpdate>(in loopSystem, systemToAdd), systemToAdd2));
			static PlayerLoopSystem AddSystem<T>(in PlayerLoopSystem reference, PlayerLoopSystem item2) where T : struct
			{
				PlayerLoopSystem result = new PlayerLoopSystem
				{
					loopConditionFunction = reference.loopConditionFunction,
					type = reference.type,
					updateDelegate = reference.updateDelegate,
					updateFunction = reference.updateFunction
				};
				List<PlayerLoopSystem> list = new List<PlayerLoopSystem>();
				PlayerLoopSystem[] subSystemList = reference.subSystemList;
				for (int i = 0; i < subSystemList.Length; i++)
				{
					PlayerLoopSystem item = subSystemList[i];
					list.Add(item);
					if (item.type == typeof(T))
					{
						list.Add(item2);
					}
				}
				result.subSystemList = list.ToArray();
				return result;
			}
		}

		private static void OnEarlyUpdate()
		{
			foreach (IPPreUpdate earlyUpdate in _earlyUpdates)
			{
				earlyUpdate?.PPreUpdate();
			}
		}

		private static void OnSuperLateUpdate()
		{
			foreach (IPPostLateUpdate superLateUpdate in _superLateUpdates)
			{
				superLateUpdate?.PPostLateUpdate();
			}
		}
	}
}
