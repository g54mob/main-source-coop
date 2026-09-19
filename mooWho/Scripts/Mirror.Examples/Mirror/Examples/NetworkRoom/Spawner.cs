using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;

namespace Mirror.Examples.NetworkRoom
{
	internal static class Spawner
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CRecycleReward_003Ed__13 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncVoidMethodBuilder _003C_003Et__builder;

			public GameObject reward;

			private TaskAwaiter _003C_003Eu__1;

			private void MoveNext()
			{
				int num = _003C_003E1__state;
				try
				{
					TaskAwaiter awaiter;
					if (num != 0)
					{
						NetworkServer.UnSpawn(reward);
						awaiter = DelayedSpawn().GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = awaiter;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
							return;
						}
					}
					else
					{
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter);
						num = (_003C_003E1__state = -1);
					}
					awaiter.GetResult();
				}
				catch (Exception exception)
				{
					_003C_003E1__state = -2;
					_003C_003Et__builder.SetException(exception);
					return;
				}
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetResult();
			}

			void IAsyncStateMachine.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				this.MoveNext();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_003C_003Et__builder.SetStateMachine(stateMachine);
			}

			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
				this.SetStateMachine(stateMachine);
			}
		}

		private static GameObject prefab;

		private static byte poolSize = 10;

		private static Pool<GameObject> pool;

		private static ushort counter;

		internal static void InitializePool(GameObject poolPrefab, byte count)
		{
			prefab = poolPrefab;
			poolSize = count;
			NetworkClient.RegisterPrefab(prefab, SpawnHandler, UnspawnHandler);
			pool = new Pool<GameObject>(CreateNew, poolSize);
		}

		internal static void ClearPool()
		{
			if (prefab == null)
			{
				return;
			}
			NetworkClient.UnregisterPrefab(prefab);
			if (pool != null)
			{
				while (pool.Count > 0)
				{
					UnityEngine.Object.Destroy(pool.Get());
				}
				counter = 0;
				pool = null;
			}
		}

		private static GameObject SpawnHandler(SpawnMessage msg)
		{
			return Get(msg.position, msg.rotation);
		}

		private static void UnspawnHandler(GameObject spawned)
		{
			Return(spawned);
		}

		private static GameObject CreateNew()
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(prefab);
			counter++;
			gameObject.name = $"{prefab.name}_pooled_{counter:00}";
			gameObject.SetActive(value: false);
			return gameObject;
		}

		public static GameObject Get(Vector3 position, Quaternion rotation)
		{
			GameObject gameObject = pool.Get();
			gameObject.transform.SetPositionAndRotation(position, rotation);
			gameObject.SetActive(value: true);
			return gameObject;
		}

		public static void Return(GameObject spawned)
		{
			spawned.SetActive(value: false);
			spawned.transform.position = new Vector3(0f, -1000f, 0f);
			pool.Return(spawned);
		}

		[ServerCallback]
		internal static void InitialSpawn()
		{
			if (NetworkServer.active)
			{
				for (byte b = 0; b < poolSize; b++)
				{
					SpawnReward();
				}
			}
		}

		[ServerCallback]
		internal static void SpawnReward()
		{
			if (NetworkServer.active)
			{
				NetworkServer.Spawn(Get(new Vector3(UnityEngine.Random.Range(-19, 20), 1f, UnityEngine.Random.Range(-19, 20)), Quaternion.identity));
			}
		}

		[AsyncStateMachine(typeof(_003CRecycleReward_003Ed__13))]
		[ServerCallback]
		internal static void RecycleReward(GameObject reward)
		{
			if (NetworkServer.active)
			{
				_003CRecycleReward_003Ed__13 stateMachine = default(_003CRecycleReward_003Ed__13);
				stateMachine._003C_003Et__builder = AsyncVoidMethodBuilder.Create();
				stateMachine.reward = reward;
				stateMachine._003C_003E1__state = -1;
				stateMachine._003C_003Et__builder.Start(ref stateMachine);
			}
		}

		private static async Task DelayedSpawn()
		{
			await Task.Delay(new TimeSpan(0, 0, 1));
			SpawnReward();
		}
	}
}
