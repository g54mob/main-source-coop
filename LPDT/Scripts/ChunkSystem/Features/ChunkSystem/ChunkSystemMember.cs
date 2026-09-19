using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Features.ChunkSystem
{
	public abstract class ChunkSystemMember : MonoBehaviour
	{
		protected readonly Dictionary<string, ChunkLayerData> _layersData = new Dictionary<string, ChunkLayerData>();

		protected readonly Dictionary<string, int> _watchers = new Dictionary<string, int>();

		[SerializeField]
		[HideInInspector]
		protected List<ChunkMemberLayerEntry> _layers;

		[Header("Activation")]
		[Tooltip("Points used to decide when this chunk should be active. If empty, the chunk transform is used.")]
		[SerializeField]
		private List<Transform> _activationPoints = new List<Transform>();

		[Header("Batch picker")]
		[Tooltip("Objects on these Unity layers are skipped when collecting hierarchy/grid for Batch.")]
		[SerializeField]
		private LayerMask _batchPickerIgnoreLayers;

		protected ChunkSystemModel _chunkSystemModel;

		protected ChunkLayersConfiguration _layersConfiguration;

		public IReadOnlyDictionary<string, ChunkLayerData> LayersData => _layersData;

		[Inject]
		public void InjectDependencies(ChunkSystemModel chunkSystemModel, ChunkLayersConfiguration layersConfiguration)
		{
			_chunkSystemModel = chunkSystemModel;
			_layersConfiguration = layersConfiguration;
		}

		public bool ShouldExcludeFromChunkBatch(GameObject go)
		{
			if (go == null)
			{
				return true;
			}
			if (go.TryGetComponent<DontIncludeInBatchChunkObjects>(out var _))
			{
				return true;
			}
			int value = _batchPickerIgnoreLayers.value;
			if (value != 0 && (value & (1 << go.layer)) != 0)
			{
				return true;
			}
			return false;
		}

		protected Vector3Int GetGridPosition(string layer, Vector3 position)
		{
			if (_chunkSystemModel.TryGetGrid(layer, out var grid))
			{
				return grid.WorldToGrid(position);
			}
			ChunkLayerEntry chunkLayerEntry = _layersConfiguration.Layers.First((ChunkLayerEntry l) => l.LayerName == layer);
			return new Vector3Int(Mathf.FloorToInt(position.x / (float)chunkLayerEntry.CellSize), Mathf.FloorToInt(position.y / (float)chunkLayerEntry.CellSize), Mathf.FloorToInt(position.z / (float)chunkLayerEntry.CellSize));
		}

		public bool IsInActiveRange(string layer, Vector3Int centerPosition, int radius, Vector3Int axisMask)
		{
			bool flag = false;
			foreach (Transform activationPoint in _activationPoints)
			{
				if (!(activationPoint == null))
				{
					flag = true;
					if (IsGridPositionInRange(GetGridPosition(layer, activationPoint.position), centerPosition, radius, axisMask))
					{
						return true;
					}
				}
			}
			if (!flag)
			{
				return IsGridPositionInRange(GetGridPosition(layer, base.transform.position), centerPosition, radius, axisMask);
			}
			return false;
		}

		private static bool IsGridPositionInRange(Vector3Int position, Vector3Int centerPosition, int radius, Vector3Int axisMask)
		{
			bool num = axisMask.x == 0 || Mathf.Abs(position.x - centerPosition.x) <= radius;
			bool flag = axisMask.y == 0 || Mathf.Abs(position.y - centerPosition.y) <= radius;
			bool flag2 = axisMask.z == 0 || Mathf.Abs(position.z - centerPosition.z) <= radius;
			return num && flag && flag2;
		}

		public void AddWatcher(string layer)
		{
			if (!_watchers.TryAdd(layer, 1))
			{
				_watchers[layer]++;
			}
			if (_watchers[layer] == 1)
			{
				OnEnableChunk(layer);
			}
		}

		public void RemoveWatcher(string layer)
		{
			if (_watchers.TryGetValue(layer, out var value))
			{
				_watchers[layer] = value - 1;
				if (_watchers[layer] <= 0)
				{
					_watchers[layer] = 0;
					OnDisableChunk(layer);
				}
			}
		}

		public virtual void OnEnableChunk(string layer)
		{
			foreach (ChunkStatusUpdateBehaviour activator in _layersData[layer].Activators)
			{
				activator.Enable();
			}
		}

		public virtual void OnDisableChunk(string layer)
		{
			foreach (ChunkStatusUpdateBehaviour activator in _layersData[layer].Activators)
			{
				activator.Disable();
			}
		}
	}
}
