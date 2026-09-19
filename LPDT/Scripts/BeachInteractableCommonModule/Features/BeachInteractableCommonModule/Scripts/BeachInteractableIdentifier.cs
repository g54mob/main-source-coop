using System;
using System.Runtime.InteropServices;
using Features.LevelModule.Scripts;
using Fusion;

namespace Features.BeachInteractableCommonModule.Scripts
{
	[StructLayout(LayoutKind.Explicit, Size = 12)]
	[NetworkStructWeaved(3)]
	public struct BeachInteractableIdentifier : INetworkStruct, IEquatable<BeachInteractableIdentifier>
	{
		[FieldOffset(0)]
		private LevelType _levelType;

		[FieldOffset(4)]
		private BeachInteractableType _beachInteractableType;

		[FieldOffset(8)]
		private int _spawnMarker;

		public LevelType LevelType => _levelType;

		public BeachInteractableType BeachInteractableType => _beachInteractableType;

		public int SpawnMarker => _spawnMarker;

		public BeachInteractableIdentifier(LevelType levelType, BeachInteractableType beachInteractableType, int spawnMarker)
		{
			_levelType = levelType;
			_beachInteractableType = beachInteractableType;
			_spawnMarker = spawnMarker;
		}

		public bool Equals(BeachInteractableIdentifier other)
		{
			if (_levelType == other._levelType && _beachInteractableType == other._beachInteractableType)
			{
				return _spawnMarker == other._spawnMarker;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is BeachInteractableIdentifier other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(_levelType, _beachInteractableType, _spawnMarker);
		}
	}
}
