using System.Collections.Generic;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Collision
{
	[CreateAssetMenu(menuName = "NomadDrive/Vehicle/Terrain Tree Destruction")]
	public class TerrainTreeDestructionConfig : ScriptableObject
	{
		[SerializeField]
		private List<TerrainTreeDestructionEntry> entries = new List<TerrainTreeDestructionEntry>();

		private Dictionary<GameObject, TerrainTreeDestructionEntry> _lookup;

		private void OnEnable()
		{
			RebuildLookup();
		}

		private void RebuildLookup()
		{
			_lookup = new Dictionary<GameObject, TerrainTreeDestructionEntry>();
			foreach (TerrainTreeDestructionEntry entry in entries)
			{
				if (entry.treePrefab != null && !_lookup.ContainsKey(entry.treePrefab))
				{
					_lookup[entry.treePrefab] = entry;
				}
			}
		}

		public bool TryGetEntry(GameObject treePrefab, out TerrainTreeDestructionEntry entry)
		{
			if (_lookup == null)
			{
				RebuildLookup();
			}
			return _lookup.TryGetValue(treePrefab, out entry);
		}
	}
}
