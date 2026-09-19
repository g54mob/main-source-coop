using System.Collections.Generic;
using UnityEngine;

namespace Features.ChunkSystem
{
	public class ChunkSystemStaticMember : ChunkSystemMember
	{
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
				}
			}
		}
	}
}
