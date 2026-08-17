using System.Collections.Generic;
using EvilCore.Particles;
using NomadDrive.Features.Player;
using UnityEngine;

namespace NomadDrive.Features.ObjectPlacement
{
	[CreateAssetMenu(menuName = "NomadDrive/ObjectPlacement/Drop Surface Effect Config", fileName = "DropSurfaceEffectConfig")]
	public class DropSurfaceEffectConfig : ScriptableObject
	{
		[SerializeField]
		private ParticleKey defaultParticleKey;

		[SerializeField]
		private List<SurfaceEffectEntry> surfaceEntries = new List<SurfaceEffectEntry>();

		private Dictionary<SurfaceType, ParticleKey> _lookup;

		public ParticleKey DefaultParticleKey => defaultParticleKey;

		public bool HasExplicitEntry(SurfaceType surfaceType)
		{
			BuildLookupIfNeeded();
			if (_lookup.TryGetValue(surfaceType, out var value))
			{
				return value.IsValid;
			}
			return false;
		}

		public ParticleKey GetParticleKey(SurfaceType surfaceType)
		{
			BuildLookupIfNeeded();
			if (_lookup.TryGetValue(surfaceType, out var value) && value.IsValid)
			{
				return value;
			}
			return defaultParticleKey;
		}

		private void BuildLookupIfNeeded()
		{
			if (_lookup != null)
			{
				return;
			}
			_lookup = new Dictionary<SurfaceType, ParticleKey>(surfaceEntries.Count);
			foreach (SurfaceEffectEntry surfaceEntry in surfaceEntries)
			{
				_lookup[surfaceEntry.surfaceType] = surfaceEntry.particleKey;
			}
		}

		private void OnValidate()
		{
			_lookup = null;
		}
	}
}
