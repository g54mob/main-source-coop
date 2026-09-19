using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Mirror.Examples.TopDownShooter
{
	public class NetworkTopDown : NetworkBehaviour
	{
		public CanvasTopDown canvasTopDown;

		public GameObject[] enemyPrefabs;

		public Vector2 enemySpawnRangeX;

		public Vector2 enemySpawnRangeZ;

		[SyncVar(hook = "OnEnemyCounterChanged")]
		public int enemyCounter;

		public Action<int, int> _Mirror_SyncVarHookDelegate_enemyCounter;

		public int NetworkenemyCounter
		{
			get
			{
				return enemyCounter;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref enemyCounter, 1uL, _Mirror_SyncVarHookDelegate_enemyCounter);
			}
		}

		public override void OnStartServer()
		{
			canvasTopDown.ResetUI();
			SpawnEnemy();
		}

		public override void OnStartClient()
		{
			canvasTopDown.ResetUI();
		}

		[ServerCallback]
		public void SpawnEnemy()
		{
			if (NetworkServer.active)
			{
				if (!base.isServer)
				{
					MonoBehaviour.print("Only server can spawn enemies, or clients via cmd request.");
					return;
				}
				GameObject obj = UnityEngine.Object.Instantiate(enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)]);
				obj.transform.position = new Vector3(UnityEngine.Random.Range(enemySpawnRangeX.x, enemySpawnRangeX.y), 0f, UnityEngine.Random.Range(enemySpawnRangeZ.x, enemySpawnRangeZ.y));
				NetworkServer.Spawn(obj);
				NetworkenemyCounter = enemyCounter + 1;
				canvasTopDown.UpdateEnemyUI(enemyCounter);
			}
		}

		private void OnEnemyCounterChanged(int _Old, int _New)
		{
			canvasTopDown.UpdateEnemyUI(enemyCounter);
		}

		public NetworkTopDown()
		{
			_Mirror_SyncVarHookDelegate_enemyCounter = OnEnemyCounterChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteVarInt(enemyCounter);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				writer.WriteVarInt(enemyCounter);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref enemyCounter, _Mirror_SyncVarHookDelegate_enemyCounter, reader.ReadVarInt());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref enemyCounter, _Mirror_SyncVarHookDelegate_enemyCounter, reader.ReadVarInt());
			}
		}
	}
}
