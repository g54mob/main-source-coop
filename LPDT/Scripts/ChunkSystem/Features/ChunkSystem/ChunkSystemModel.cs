using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.ChunkSystem
{
	public class ChunkSystemModel
	{
		private readonly Dictionary<string, Dictionary<Vector3Int, List<ChunkSystemMember>>> _members = new Dictionary<string, Dictionary<Vector3Int, List<ChunkSystemMember>>>();

		private readonly Dictionary<string, ChunkGrid> _grids = new Dictionary<string, ChunkGrid>();

		public IReadOnlyDictionary<string, Dictionary<Vector3Int, List<ChunkSystemMember>>> Members => _members;

		public IReadOnlyDictionary<string, ChunkGrid> Grids => _grids;

		public event Action<string, Vector3Int, ChunkSystemMember> OnMemberRegistered;

		public event Action<string, Vector3Int, ChunkSystemMember> OnMemberUnregistered;

		public event Action<string, Vector3Int, Vector3Int, ChunkSystemMember> OnMemberPositionUpdated;

		public event Action<string, ChunkGrid> OnGridRegistered;

		public event Action<string> OnGridUnregistered;

		public void RegisterGrid(string layer, ChunkGrid grid)
		{
			_grids[layer] = grid;
			this.OnGridRegistered?.Invoke(layer, grid);
		}

		public void UnregisterGrid(string layer)
		{
			_grids.Remove(layer);
			this.OnGridUnregistered?.Invoke(layer);
		}

		public bool TryGetGrid(string layer, out ChunkGrid grid)
		{
			return _grids.TryGetValue(layer, out grid);
		}

		public void RegisterMember(string layer, Vector3Int position, ChunkSystemMember member)
		{
			if (!_members.TryGetValue(layer, out var value))
			{
				value = new Dictionary<Vector3Int, List<ChunkSystemMember>>();
				_members[layer] = value;
			}
			if (!value.TryGetValue(position, out var value2))
			{
				value2 = (value[position] = new List<ChunkSystemMember>());
			}
			value2.Add(member);
			this.OnMemberRegistered?.Invoke(layer, position, member);
		}

		public void UnregisterMember(string layer, Vector3Int position, ChunkSystemMember member)
		{
			if (_members.TryGetValue(layer, out var value))
			{
				if (value.TryGetValue(position, out var value2))
				{
					value2.Remove(member);
				}
				this.OnMemberUnregistered?.Invoke(layer, position, member);
			}
		}

		public void UpdatePosition(string layer, Vector3Int oldPosition, Vector3Int newPosition, ChunkSystemMember member)
		{
			if (_members.TryGetValue(layer, out var value))
			{
				if (value.TryGetValue(oldPosition, out var value2))
				{
					value2.Remove(member);
				}
				if (!value.TryGetValue(newPosition, out var value3))
				{
					value3 = (value[newPosition] = new List<ChunkSystemMember>());
				}
				value3.Add(member);
				this.OnMemberPositionUpdated?.Invoke(layer, oldPosition, newPosition, member);
			}
		}
	}
}
