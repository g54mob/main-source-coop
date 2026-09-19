using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Features.ChunkSystem
{
	public abstract class ActiveChunksPoint : MonoBehaviour
	{
		private readonly Dictionary<string, HashSet<ChunkSystemMember>> _previouslyActivated = new Dictionary<string, HashSet<ChunkSystemMember>>();

		[SerializeField]
		[HideInInspector]
		protected List<OverridableLayerData> _affectedLayers = new List<OverridableLayerData>();

		protected ChunkSystemModel _chunkSystemModel;

		protected ChunkLayersConfiguration _chunkLayersConfiguration;

		protected virtual Vector3Int AxisMask => new Vector3Int(1, 1, 1);

		[Inject]
		public void InjectDependencies(ChunkSystemModel chunkSystemModel, ChunkLayersConfiguration chunkLayersConfiguration)
		{
			_chunkSystemModel = chunkSystemModel;
			_chunkLayersConfiguration = chunkLayersConfiguration;
		}

		private void OnEnable()
		{
			_chunkSystemModel.OnGridRegistered += HandleGridRegistered;
			_chunkSystemModel.OnMemberRegistered += HandleMemberRegistered;
			_chunkSystemModel.OnMemberUnregistered += HandleMemberUnregistered;
			_chunkSystemModel.OnMemberPositionUpdated += HandleMemberPositionUpdated;
		}

		private void OnDisable()
		{
			_chunkSystemModel.OnGridRegistered -= HandleGridRegistered;
			_chunkSystemModel.OnMemberRegistered -= HandleMemberRegistered;
			_chunkSystemModel.OnMemberUnregistered -= HandleMemberUnregistered;
			_chunkSystemModel.OnMemberPositionUpdated -= HandleMemberPositionUpdated;
			foreach (KeyValuePair<string, HashSet<ChunkSystemMember>> item in _previouslyActivated)
			{
				foreach (ChunkSystemMember item2 in item.Value)
				{
					item2.RemoveWatcher(item.Key);
				}
			}
			_previouslyActivated.Clear();
		}

		private void HandleGridRegistered(string layer, ChunkGrid grid)
		{
			if (_affectedLayers.FirstOrDefault((OverridableLayerData l) => l.Layer == layer).Layer != null)
			{
				ApplyChangePosition(base.transform.position);
			}
		}

		private void HandleMemberPositionUpdated(string layer, Vector3Int oldPosition, Vector3Int newPosition, ChunkSystemMember member)
		{
			if (_previouslyActivated.TryGetValue(layer, out var value))
			{
				bool flag = value.Contains(member);
				bool flag2 = IsMemberActive(layer, member);
				if (!flag && flag2)
				{
					member.AddWatcher(layer);
					value.Add(member);
				}
				else if (flag && !flag2)
				{
					value.Remove(member);
					member.RemoveWatcher(layer);
				}
			}
		}

		private void HandleMemberRegistered(string layer, Vector3Int position, ChunkSystemMember member)
		{
			if (_previouslyActivated.TryGetValue(layer, out var value) && IsMemberActive(layer, member))
			{
				member.AddWatcher(layer);
				value.Add(member);
			}
		}

		private void HandleMemberUnregistered(string layer, Vector3Int position, ChunkSystemMember member)
		{
			if (_previouslyActivated.TryGetValue(layer, out var value) && value.Remove(member))
			{
				member.RemoveWatcher(layer);
			}
		}

		private bool IsMemberActive(string layer, ChunkSystemMember member)
		{
			OverridableLayerData overridableLayerData = _affectedLayers.FirstOrDefault((OverridableLayerData l) => l.Layer == layer);
			if (overridableLayerData.Layer == null)
			{
				return false;
			}
			ChunkLayerEntry chunkLayerEntry = _chunkLayersConfiguration.Layers.First((ChunkLayerEntry l) => l.LayerName == layer);
			int radius = (overridableLayerData.Override ? overridableLayerData.ActiveChunksCount : chunkLayerEntry.ActiveChunksCount);
			Vector3Int gridPosition = GetGridPosition(layer, base.transform.position);
			return member.IsInActiveRange(layer, gridPosition, radius, AxisMask);
		}

		protected virtual void ApplyChangePosition(Vector3 position)
		{
			Vector3Int axisMask = AxisMask;
			foreach (OverridableLayerData layerData in _affectedLayers)
			{
				ChunkLayerEntry chunkLayerEntry = _chunkLayersConfiguration.Layers.First((ChunkLayerEntry l) => l.LayerName == layerData.Layer);
				int radius = (layerData.Override ? layerData.ActiveChunksCount : chunkLayerEntry.ActiveChunksCount);
				Vector3Int gridPosition = GetGridPosition(layerData.Layer, position);
				if (!_chunkSystemModel.Members.TryGetValue(layerData.Layer, out var value))
				{
					continue;
				}
				HashSet<ChunkSystemMember> hashSet = new HashSet<ChunkSystemMember>();
				HashSet<ChunkSystemMember> hashSet2 = _previouslyActivated.GetValueOrDefault(layerData.Layer) ?? new HashSet<ChunkSystemMember>();
				foreach (KeyValuePair<Vector3Int, List<ChunkSystemMember>> item in value)
				{
					foreach (ChunkSystemMember item2 in item.Value)
					{
						if (!hashSet.Contains(item2) && item2.IsInActiveRange(layerData.Layer, gridPosition, radius, axisMask))
						{
							if (!hashSet2.Contains(item2))
							{
								item2.AddWatcher(layerData.Layer);
							}
							hashSet.Add(item2);
						}
					}
				}
				foreach (ChunkSystemMember item3 in hashSet2)
				{
					if (!hashSet.Contains(item3))
					{
						item3.RemoveWatcher(layerData.Layer);
					}
				}
				_previouslyActivated[layerData.Layer] = hashSet;
			}
		}

		private Vector3Int GetGridPosition(string layer, Vector3 position)
		{
			if (_chunkSystemModel.TryGetGrid(layer, out var grid))
			{
				return grid.WorldToGrid(position);
			}
			ChunkLayerEntry chunkLayerEntry = _chunkLayersConfiguration.Layers.First((ChunkLayerEntry l) => l.LayerName == layer);
			return new Vector3Int(Mathf.FloorToInt(position.x / (float)chunkLayerEntry.CellSize), Mathf.FloorToInt(position.y / (float)chunkLayerEntry.CellSize), Mathf.FloorToInt(position.z / (float)chunkLayerEntry.CellSize));
		}

		private void OnDrawGizmosSelected()
		{
			Vector3Int axisMask = AxisMask;
			foreach (OverridableLayerData layerData in _affectedLayers)
			{
				if (_chunkLayersConfiguration.Layers == null)
				{
					continue;
				}
				ChunkLayerEntry chunkLayerEntry = _chunkLayersConfiguration.Layers.First((ChunkLayerEntry l) => l.LayerName == layerData.Layer);
				int num = (layerData.Override ? layerData.ActiveChunksCount : chunkLayerEntry.ActiveChunksCount);
				Vector3Int gridPosition = GetGridPosition(layerData.Layer, base.transform.position);
				if (!_chunkSystemModel.Members.TryGetValue(layerData.Layer, out var _))
				{
					continue;
				}
				int num2 = ((axisMask.x != 0) ? (-num) : 0);
				int num3 = ((axisMask.x != 0) ? num : 0);
				int num4 = ((axisMask.y != 0) ? (-num) : 0);
				int num5 = ((axisMask.y != 0) ? num : 0);
				int num6 = ((axisMask.z != 0) ? (-num) : 0);
				int num7 = ((axisMask.z != 0) ? num : 0);
				Gizmos.color = Color.red;
				for (int num8 = num2; num8 <= num3; num8++)
				{
					for (int num9 = num4; num9 <= num5; num9++)
					{
						for (int num10 = num6; num10 <= num7; num10++)
						{
							Vector3Int gridPosition2 = gridPosition + new Vector3Int(num8, num9, num10);
							Vector3 center;
							if (_chunkSystemModel.TryGetGrid(layerData.Layer, out var grid))
							{
								center = grid.GridToWorld(gridPosition2);
							}
							else
							{
								float num11 = chunkLayerEntry.CellSize;
								center = new Vector3(((float)gridPosition2.x + 0.5f) * num11, ((float)gridPosition2.y + 0.5f) * num11, ((float)gridPosition2.z + 0.5f) * num11);
							}
							Gizmos.DrawWireCube(center, Vector3.one * chunkLayerEntry.CellSize);
						}
					}
				}
			}
		}
	}
}
