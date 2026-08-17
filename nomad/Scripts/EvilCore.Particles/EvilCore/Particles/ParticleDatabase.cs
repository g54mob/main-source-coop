using System.Collections.Generic;
using UnityEngine;

namespace EvilCore.Particles
{
	[CreateAssetMenu(fileName = "ParticleDatabase", menuName = "EvilCore/Particles/Particle Database")]
	public class ParticleDatabase : ScriptableObject
	{
		public const string PARTICLE_LABEL = "Particle";

		[SerializeField]
		private List<ParticleEntry> _entries = new List<ParticleEntry>();

		private Dictionary<string, ParticleEntry> _lookup;

		public IReadOnlyList<ParticleEntry> Entries => _entries;

		public int Count => _entries.Count;

		public bool TryGetEntry(string key, out ParticleEntry entry)
		{
			BuildLookupIfNeeded();
			if (_lookup.TryGetValue(key, out entry))
			{
				return true;
			}
			entry = null;
			return false;
		}

		private void BuildLookupIfNeeded()
		{
			if (_lookup != null)
			{
				return;
			}
			_lookup = new Dictionary<string, ParticleEntry>(_entries.Count);
			foreach (ParticleEntry entry in _entries)
			{
				if (!string.IsNullOrEmpty(entry.key))
				{
					_lookup[entry.key] = entry;
				}
			}
		}

		private void OnEnable()
		{
			_lookup = null;
		}
	}
}
