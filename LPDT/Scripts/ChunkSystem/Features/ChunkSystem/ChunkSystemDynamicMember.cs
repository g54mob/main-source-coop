using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features.ChunkSystem
{
	public class ChunkSystemDynamicMember : ChunkSystemMember
	{
		[SerializeField]
		[Min(0.01f)]
		private float _updateInterval = 0.1f;

		private Vector3 _lastPosition;

		private float _timer;

		private void OnEnable()
		{
			_chunkSystemModel.OnGridRegistered += OnGridRegistered;
			foreach (ChunkMemberLayerEntry layer in _layers)
			{
				foreach (ChunkStatusUpdateBehaviour activator in layer.Activators)
				{
					activator.Disable();
				}
				Vector3Int gridPosition = GetGridPosition(layer.Layer, base.transform.position);
				_layersData[layer.Layer] = new ChunkLayerData(gridPosition, ChunkStatus.Disabled, layer.Activators);
				_chunkSystemModel.RegisterMember(layer.Layer, gridPosition, this);
			}
		}

		private void OnDisable()
		{
			_chunkSystemModel.OnGridRegistered -= OnGridRegistered;
			ResetWatchers();
			foreach (KeyValuePair<string, ChunkLayerData> layersDatum in _layersData)
			{
				_chunkSystemModel.UnregisterMember(layersDatum.Key, layersDatum.Value.CurrentPosition, this);
			}
			_layersData.Clear();
		}

		private void OnGridRegistered(string layer, ChunkGrid grid)
		{
			if (_layersData.TryGetValue(layer, out var value))
			{
				Vector3Int vector3Int = grid.WorldToGrid(base.transform.position);
				if (!(value.CurrentPosition == vector3Int))
				{
					_chunkSystemModel.UnregisterMember(layer, value.CurrentPosition, this);
					value.CurrentPosition = vector3Int;
					_layersData[layer] = value;
					_chunkSystemModel.RegisterMember(layer, vector3Int, this);
					ResetWatchers();
				}
			}
		}

		private void Update()
		{
			_timer += Time.deltaTime;
			if (!(_timer < _updateInterval))
			{
				_timer = 0f;
				CheckPositionChanged();
			}
		}

		private void CheckPositionChanged()
		{
			if (_lastPosition == base.transform.position)
			{
				return;
			}
			bool flag = false;
			foreach (string item in _layersData.Keys.ToList())
			{
				Vector3Int gridPosition = GetGridPosition(item, base.transform.position);
				ChunkLayerData value = _layersData[item];
				if (!(value.CurrentPosition == gridPosition))
				{
					flag = true;
					_chunkSystemModel.UpdatePosition(item, value.CurrentPosition, gridPosition, this);
					value.CurrentPosition = gridPosition;
					_layersData[item] = value;
				}
			}
			if (flag)
			{
				ResetWatchers();
			}
			_lastPosition = base.transform.position;
		}

		private void ResetWatchers()
		{
			foreach (KeyValuePair<string, int> watcher in _watchers)
			{
				if (watcher.Value > 0)
				{
					_watchers[watcher.Key] = 0;
					OnDisableChunk(watcher.Key);
				}
			}
			_watchers.Clear();
		}
	}
}
