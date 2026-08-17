using System;
using System.Collections.Generic;
using Ami.BroAudio;
using NomadDrive.Features.Player.PlayerStateMachine;
using UnityEngine;

namespace NomadDrive.Features.Player
{
	[CreateAssetMenu(menuName = "NomadDrive/Player/Surface Footstep Config", fileName = "SurfaceFootstepConfig")]
	public class SurfaceFootstepConfig : ScriptableObject
	{
		[Serializable]
		public class FootstepSet
		{
			public SoundID walk;

			public SoundID sprint;

			public SoundID crouch;

			public SoundID jump;

			public SoundID land;
		}

		[Serializable]
		public class SurfaceEntry
		{
			public SurfaceType surfaceType;

			public FootstepSet sounds = new FootstepSet();
		}

		[SerializeField]
		private List<SurfaceEntry> surfaceSounds = new List<SurfaceEntry>();

		[SerializeField]
		private FootstepSet fallback = new FootstepSet();

		private Dictionary<SurfaceType, FootstepSet> _cache;

		private void OnEnable()
		{
			RebuildCache();
		}

		private void OnValidate()
		{
			RebuildCache();
		}

		private void RebuildCache()
		{
			_cache = new Dictionary<SurfaceType, FootstepSet>();
			if (surfaceSounds == null)
			{
				return;
			}
			foreach (SurfaceEntry surfaceSound in surfaceSounds)
			{
				if (surfaceSound != null && surfaceSound.sounds != null)
				{
					_cache[surfaceSound.surfaceType] = surfaceSound.sounds;
				}
			}
		}

		public SoundID Resolve(SurfaceType surfaceType, PlayerState state)
		{
			if (_cache == null)
			{
				RebuildCache();
			}
			FootstepSet value;
			return ResolveFromSet((_cache.TryGetValue(surfaceType, out value) && value != null) ? value : fallback, state);
		}

		public SoundID ResolveJump(SurfaceType surfaceType)
		{
			if (_cache == null)
			{
				RebuildCache();
			}
			FootstepSet value;
			FootstepSet footstepSet = ((_cache.TryGetValue(surfaceType, out value) && value != null) ? value : fallback);
			if (footstepSet == null)
			{
				return default(SoundID);
			}
			if (!footstepSet.jump.IsValid())
			{
				if (fallback == null)
				{
					return default(SoundID);
				}
				return fallback.jump;
			}
			return footstepSet.jump;
		}

		public SoundID ResolveLand(SurfaceType surfaceType)
		{
			if (_cache == null)
			{
				RebuildCache();
			}
			FootstepSet value;
			FootstepSet footstepSet = ((_cache.TryGetValue(surfaceType, out value) && value != null) ? value : fallback);
			if (footstepSet == null)
			{
				return default(SoundID);
			}
			if (!footstepSet.land.IsValid())
			{
				if (fallback == null)
				{
					return default(SoundID);
				}
				return fallback.land;
			}
			return footstepSet.land;
		}

		private static SoundID ResolveFromSet(FootstepSet set, PlayerState state)
		{
			if (set == null)
			{
				return default(SoundID);
			}
			switch (state)
			{
			case PlayerState.Sprint:
			case PlayerState.CrouchedSprint:
				return set.sprint.IsValid() ? set.sprint : set.walk;
			case PlayerState.CrouchedWalk:
				return set.crouch.IsValid() ? set.crouch : set.walk;
			default:
				return set.walk;
			}
		}
	}
}
