using System;
using System.Threading;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace VContainer.Unity
{
	internal static class PlayerLoopHelper
	{
		private static readonly PlayerLoopRunner[] Runners = new PlayerLoopRunner[10];

		private static long initialized;

		public static void EnsureInitialized()
		{
			if (Interlocked.CompareExchange(ref initialized, 1L, 0L) == 0L)
			{
				for (int i = 0; i < Runners.Length; i++)
				{
					Runners[i] = new PlayerLoopRunner();
				}
				PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
				PlayerLoopSystem[] subSystemList = currentPlayerLoop.subSystemList;
				InsertSubsystem(ref FindSubSystem(typeof(Initialization), subSystemList), null, new PlayerLoopSystem
				{
					type = typeof(VContainerInitialization),
					updateDelegate = Runners[0].Run
				}, new PlayerLoopSystem
				{
					type = typeof(VContainerPostInitialization),
					updateDelegate = Runners[1].Run
				});
				InsertSubsystem(ref FindSubSystem(typeof(EarlyUpdate), subSystemList), typeof(EarlyUpdate.ScriptRunDelayedStartupFrame), new PlayerLoopSystem
				{
					type = typeof(VContainerStartup),
					updateDelegate = Runners[2].Run
				}, new PlayerLoopSystem
				{
					type = typeof(VContainerPostStartup),
					updateDelegate = Runners[3].Run
				});
				InsertSubsystem(ref FindSubSystem(typeof(FixedUpdate), subSystemList), typeof(FixedUpdate.ScriptRunBehaviourFixedUpdate), new PlayerLoopSystem
				{
					type = typeof(VContainerFixedUpdate),
					updateDelegate = Runners[4].Run
				}, new PlayerLoopSystem
				{
					type = typeof(VContainerPostFixedUpdate),
					updateDelegate = Runners[5].Run
				});
				InsertSubsystem(ref FindSubSystem(typeof(Update), subSystemList), typeof(Update.ScriptRunBehaviourUpdate), new PlayerLoopSystem
				{
					type = typeof(VContainerUpdate),
					updateDelegate = Runners[6].Run
				}, new PlayerLoopSystem
				{
					type = typeof(VContainerPostUpdate),
					updateDelegate = Runners[7].Run
				});
				InsertSubsystem(ref FindSubSystem(typeof(PreLateUpdate), subSystemList), typeof(PreLateUpdate.ScriptRunBehaviourLateUpdate), new PlayerLoopSystem
				{
					type = typeof(VContainerLateUpdate),
					updateDelegate = Runners[8].Run
				}, new PlayerLoopSystem
				{
					type = typeof(VContainerPostLateUpdate),
					updateDelegate = Runners[9].Run
				});
				currentPlayerLoop.subSystemList = subSystemList;
				PlayerLoop.SetPlayerLoop(currentPlayerLoop);
			}
		}

		public static void Dispatch(PlayerLoopTiming timing, IPlayerLoopItem item)
		{
			EnsureInitialized();
			Runners[(int)timing].Dispatch(item);
		}

		private static ref PlayerLoopSystem FindSubSystem(Type targetType, PlayerLoopSystem[] systems)
		{
			for (int i = 0; i < systems.Length; i++)
			{
				if (systems[i].type == targetType)
				{
					return ref systems[i];
				}
			}
			throw new InvalidOperationException(targetType.FullName + " not in systems");
		}

		private static void InsertSubsystem(ref PlayerLoopSystem parentSystem, Type beforeType, PlayerLoopSystem newSystem, PlayerLoopSystem newPostSystem)
		{
			PlayerLoopSystem[] subSystemList = parentSystem.subSystemList;
			int num = -1;
			if (beforeType == null)
			{
				num = 0;
			}
			for (int i = 0; i < subSystemList.Length; i++)
			{
				if (subSystemList[i].type == beforeType)
				{
					num = i;
				}
			}
			if (num < 0)
			{
				throw new ArgumentException($"{beforeType.FullName} not in system {parentSystem} {parentSystem.type.FullName}");
			}
			PlayerLoopSystem[] array = new PlayerLoopSystem[subSystemList.Length + 2];
			for (int j = 0; j < array.Length; j++)
			{
				if (j == num)
				{
					array[j] = newSystem;
				}
				else if (j == array.Length - 1)
				{
					array[j] = newPostSystem;
				}
				else if (j < num)
				{
					array[j] = subSystemList[j];
				}
				else
				{
					array[j] = subSystemList[j - 1];
				}
			}
			parentSystem.subSystemList = array;
		}
	}
}
