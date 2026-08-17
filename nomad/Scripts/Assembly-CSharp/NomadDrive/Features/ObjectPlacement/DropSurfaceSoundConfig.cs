using System.Collections.Generic;
using Ami.BroAudio;
using NomadDrive.Features.Player;
using UnityEngine;

namespace NomadDrive.Features.ObjectPlacement
{
	[CreateAssetMenu(menuName = "NomadDrive/ObjectPlacement/Drop Surface Sound Config", fileName = "DropSurfaceSoundConfig")]
	public class DropSurfaceSoundConfig : ScriptableObject
	{
		[SerializeField]
		private SoundID defaultEvent;

		[SerializeField]
		private List<SurfaceSoundEntry> surfaceEntries = new List<SurfaceSoundEntry>();

		[SerializeField]
		private List<MaterialSurfaceSoundEntry> materialSurfaceEntries = new List<MaterialSurfaceSoundEntry>();

		private Dictionary<SurfaceType, SoundID> _surfaceLookup;

		private Dictionary<(ItemMaterial material, SurfaceType surface), SoundID> _materialSurfaceLookup;

		public SoundID DefaultEvent => defaultEvent;

		public bool HasExplicitEntry(SurfaceType surfaceType)
		{
			BuildLookupIfNeeded();
			if (_surfaceLookup.TryGetValue(surfaceType, out var value))
			{
				return value.IsValid();
			}
			return false;
		}

		public SoundID GetEvent(SurfaceType surfaceType)
		{
			return GetEvent(surfaceType, ItemMaterial.Generic);
		}

		public SoundID GetEvent(SurfaceType surfaceType, ItemMaterial material)
		{
			BuildLookupIfNeeded();
			if (material != ItemMaterial.Generic && _materialSurfaceLookup.TryGetValue((material, surfaceType), out var value) && value.IsValid())
			{
				return value;
			}
			if (_surfaceLookup.TryGetValue(surfaceType, out var value2) && value2.IsValid())
			{
				return value2;
			}
			return defaultEvent;
		}

		private void BuildLookupIfNeeded()
		{
			if (_surfaceLookup != null && _materialSurfaceLookup != null)
			{
				return;
			}
			_surfaceLookup = new Dictionary<SurfaceType, SoundID>(surfaceEntries.Count);
			foreach (SurfaceSoundEntry surfaceEntry in surfaceEntries)
			{
				_surfaceLookup[surfaceEntry.surfaceType] = surfaceEntry.soundId;
			}
			_materialSurfaceLookup = new Dictionary<(ItemMaterial, SurfaceType), SoundID>(materialSurfaceEntries.Count);
			foreach (MaterialSurfaceSoundEntry materialSurfaceEntry in materialSurfaceEntries)
			{
				_materialSurfaceLookup[(materialSurfaceEntry.material, materialSurfaceEntry.surfaceType)] = materialSurfaceEntry.soundId;
			}
		}

		private void OnValidate()
		{
			_surfaceLookup = null;
			_materialSurfaceLookup = null;
		}
	}
}
