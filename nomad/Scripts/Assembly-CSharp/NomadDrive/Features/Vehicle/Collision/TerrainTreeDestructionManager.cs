using System;
using System.Collections.Generic;
using Ami.BroAudio;
using EvilCore.Audio;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Particles;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.FloatingOrigin;
using NomadDrive.Features.WorldGeneration;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Vehicle.Collision
{
	public class TerrainTreeDestructionManager : NetworkBehaviour, ITerrainTreeDestructionManager
	{
		[Header("Configuration")]
		[SerializeField]
		private TerrainTreeDestructionConfig destructionConfig;

		[SerializeField]
		private float treeSearchRadius = 3f;

		[SerializeField]
		[Range(100f, 10000f)]
		private int maxOperations = 2000;

		[Inject]
		private IWorldGenerator _worldGenerator;

		[Inject]
		private IAudioManager _audioManager;

		[Inject]
		private IParticlesManager _particlesManager;

		private readonly SyncList<TreeDestructionOperation> _operations = new SyncList<TreeDestructionOperation>();

		private readonly Dictionary<Vector2Int, List<TreeDestructionOperation>> _pendingOperations = new Dictionary<Vector2Int, List<TreeDestructionOperation>>();

		private int _replayedCount;

		public TerrainTreeDestructionConfig Config => destructionConfig;

		public float TreeSearchRadius => treeSearchRadius;

		public override void OnStartClient()
		{
			base.OnStartClient();
			SyncList<TreeDestructionOperation> operations = _operations;
			operations.Callback = (Action<SyncList<TreeDestructionOperation>.Operation, int, TreeDestructionOperation, TreeDestructionOperation>)Delegate.Combine(operations.Callback, new Action<SyncList<TreeDestructionOperation>.Operation, int, TreeDestructionOperation, TreeDestructionOperation>(OnOperationsChanged));
			if (_worldGenerator != null)
			{
				_worldGenerator.OnTileFullyLoaded += OnTileFullyLoaded;
				ReplayAllOperations();
			}
			else
			{
				EvilLogger.LogError("[TerrainTreeDestructionManager] IWorldGenerator not injected", "OnStartClient", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Driving\\_Core\\Collision\\TerrainTreeDestructionManager.cs", 54);
			}
		}

		public override void OnStopClient()
		{
			base.OnStopClient();
			SyncList<TreeDestructionOperation> operations = _operations;
			operations.Callback = (Action<SyncList<TreeDestructionOperation>.Operation, int, TreeDestructionOperation, TreeDestructionOperation>)Delegate.Remove(operations.Callback, new Action<SyncList<TreeDestructionOperation>.Operation, int, TreeDestructionOperation, TreeDestructionOperation>(OnOperationsChanged));
			_replayedCount = 0;
			_pendingOperations.Clear();
			if (_worldGenerator != null)
			{
				_worldGenerator.OnTileFullyLoaded -= OnTileFullyLoaded;
			}
		}

		public void ReportDestruction(Vector3 worldPosition, Vector3 impactVelocity)
		{
			if (_worldGenerator == null)
			{
				EvilLogger.LogError("[TerrainTreeDestructionManager] WorldGenerator missing, cannot report", "ReportDestruction", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Driving\\_Core\\Collision\\TerrainTreeDestructionManager.cs", 74);
				return;
			}
			Vector3 worldPosition2 = FloatingOriginManager.ToTrueWorld(worldPosition);
			Vector2Int chunkCoordFromWorldPosition = _worldGenerator.GetChunkCoordFromWorldPosition(worldPosition2);
			TreeDestructionOperation op = new TreeDestructionOperation(worldPosition2, impactVelocity, chunkCoordFromWorldPosition.x, chunkCoordFromWorldPosition.y);
			ApplyOperation(op, isReplay: false);
			CmdRecordDestruction(worldPosition2, impactVelocity, chunkCoordFromWorldPosition.x, chunkCoordFromWorldPosition.y);
		}

		[Command(requiresAuthority = false)]
		private void CmdRecordDestruction(Vector3 worldPosition, Vector3 impactVelocity, int chunkX, int chunkZ)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(worldPosition);
			writer.WriteVector3(impactVelocity);
			writer.WriteVarInt(chunkX);
			writer.WriteVarInt(chunkZ);
			SendCommandInternal("System.Void NomadDrive.Features.Vehicle.Collision.TerrainTreeDestructionManager::CmdRecordDestruction(UnityEngine.Vector3,UnityEngine.Vector3,System.Int32,System.Int32)", -821211242, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		private void OnOperationsChanged(SyncList<TreeDestructionOperation>.Operation op, int index, TreeDestructionOperation oldItem, TreeDestructionOperation newItem)
		{
			switch (op)
			{
			case SyncList<TreeDestructionOperation>.Operation.OP_ADD:
				if (index >= _replayedCount)
				{
					ApplyOperation(newItem, isReplay: false);
				}
				break;
			case SyncList<TreeDestructionOperation>.Operation.OP_REMOVEAT:
				if (index < _replayedCount)
				{
					_replayedCount = Mathf.Max(0, _replayedCount - 1);
				}
				break;
			}
		}

		private void ReplayAllOperations()
		{
			_replayedCount = _operations.Count;
			for (int i = 0; i < _replayedCount; i++)
			{
				ApplyOperation(_operations[i], isReplay: true);
			}
		}

		private void ApplyOperation(TreeDestructionOperation op, bool isReplay)
		{
			if (!(destructionConfig == null))
			{
				if (_worldGenerator == null || !_worldGenerator.TryGetTerrainByChunkCoord(op.ChunkCoord, out var terrain))
				{
					QueuePendingOperation(op);
				}
				else
				{
					ApplyToTerrain(terrain, op, isReplay);
				}
			}
		}

		private void ApplyToTerrain(Terrain terrain, TreeDestructionOperation op, bool isReplay)
		{
			Vector3 worldPos = FloatingOriginManager.ToRenderWorld(op.worldPosition);
			if (!TerrainTreeCollisionHelper.FindNearestTreeInstance(terrain, worldPos, treeSearchRadius, out var treeIndex, out var treeWorldPos, out var prototypeIndex))
			{
				return;
			}
			GameObject treePrototypePrefab = TerrainTreeCollisionHelper.GetTreePrototypePrefab(terrain, prototypeIndex);
			if (!(treePrototypePrefab == null) && destructionConfig.TryGetEntry(treePrototypePrefab, out var entry) && entry.isDestructible)
			{
				Quaternion treeRotation = TerrainTreeCollisionHelper.CalculateTreeRotation(terrain.terrainData.treeInstances[treeIndex]);
				TerrainTreeCollisionHelper.RemoveTreeInstance(terrain, treeIndex);
				if (!isReplay)
				{
					SpawnDestructionEffects(treeWorldPos, treeRotation, entry, op.impactVelocity);
				}
			}
		}

		private void SpawnDestructionEffects(Vector3 treeWorldPos, Quaternion treeRotation, TerrainTreeDestructionEntry entry, Vector3 vehicleVelocity)
		{
			if (entry.destructionPrefab != null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(entry.destructionPrefab, treeWorldPos, treeRotation);
				gameObject.AddComponent<NonDetachingCollisionMarker>();
				DisableDebrisInterCollision(gameObject);
				Vector3 force = vehicleVelocity * entry.debrisPushFraction;
				Rigidbody[] componentsInChildren = gameObject.GetComponentsInChildren<Rigidbody>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].AddForce(force, ForceMode.VelocityChange);
				}
				UnityEngine.Object.Destroy(gameObject, entry.debrisLifetime);
			}
			if (entry.destructionParticle.IsValid)
			{
				_particlesManager?.PlayOneShot(entry.destructionParticle, treeWorldPos, Quaternion.identity);
			}
			if (entry.destructionSound.IsValid())
			{
				_audioManager?.PlayOneShot(entry.destructionSound, treeWorldPos);
			}
		}

		private void DisableDebrisInterCollision(GameObject debris)
		{
			Collider[] componentsInChildren = debris.GetComponentsInChildren<Collider>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				for (int j = i + 1; j < componentsInChildren.Length; j++)
				{
					Physics.IgnoreCollision(componentsInChildren[i], componentsInChildren[j]);
				}
			}
		}

		private void QueuePendingOperation(TreeDestructionOperation op)
		{
			if (!_pendingOperations.TryGetValue(op.ChunkCoord, out var value))
			{
				value = new List<TreeDestructionOperation>();
				_pendingOperations[op.ChunkCoord] = value;
			}
			value.Add(op);
		}

		private void OnTileFullyLoaded(Vector2Int chunkCoord, Terrain terrain)
		{
			if (terrain == null || !_pendingOperations.TryGetValue(chunkCoord, out var value))
			{
				return;
			}
			foreach (TreeDestructionOperation item in value)
			{
				ApplyToTerrain(terrain, item, isReplay: true);
			}
			_pendingOperations.Remove(chunkCoord);
		}

		public TerrainTreeDestructionManager()
		{
			InitSyncObject(_operations);
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdRecordDestruction__Vector3__Vector3__Int32__Int32(Vector3 worldPosition, Vector3 impactVelocity, int chunkX, int chunkZ)
		{
			TreeDestructionOperation item = new TreeDestructionOperation(worldPosition, impactVelocity, chunkX, chunkZ);
			_operations.Add(item);
			while (_operations.Count > maxOperations)
			{
				_operations.RemoveAt(0);
			}
		}

		protected static void InvokeUserCode_CmdRecordDestruction__Vector3__Vector3__Int32__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdRecordDestruction called on client.");
			}
			else
			{
				((TerrainTreeDestructionManager)obj).UserCode_CmdRecordDestruction__Vector3__Vector3__Int32__Int32(reader.ReadVector3(), reader.ReadVector3(), reader.ReadVarInt(), reader.ReadVarInt());
			}
		}

		static TerrainTreeDestructionManager()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(TerrainTreeDestructionManager), "System.Void NomadDrive.Features.Vehicle.Collision.TerrainTreeDestructionManager::CmdRecordDestruction(UnityEngine.Vector3,UnityEngine.Vector3,System.Int32,System.Int32)", InvokeUserCode_CmdRecordDestruction__Vector3__Vector3__Int32__Int32, requiresAuthority: false);
		}
	}
}
