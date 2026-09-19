using System.Collections.Generic;
using FMOD;
using FMODUnity;
using UnityEngine;

namespace Features.EntitiesSoundOcclusionModule.Scripts
{
	[CreateAssetMenu(fileName = "EntitiesSoundOcclusionConfiguration_Default", menuName = "Configurations/EntitiesSoundOcclusion/EntitiesSoundOcclusionConfiguration")]
	public class EntitiesSoundOcclusionConfiguration : ScriptableObject
	{
		[SerializeField]
		[Tooltip("FMOD events that must NOT trigger sound-driven entity reactions (still play normally).")]
		private List<EventReference> _blacklistedSounds = new List<EventReference>();

		[SerializeField]
		[Tooltip("Forces an absolute audible radius (meters) for specific FMOD events, bypassing loudness metering. Use for short one-shots like run steps whose metered loudness is unreliable. Unlisted events fall back to metered loudness.")]
		private List<SoundOcclusionDistanceOverride> _distanceOverrides = new List<SoundOcclusionDistanceOverride>();

		private HashSet<GUID> _blacklistedGuids;

		private Dictionary<GUID, float> _distanceByGuid;

		[field: SerializeField]
		public float MaxHearingDistance { get; private set; } = 30f;

		[field: SerializeField]
		[field: Tooltip("Audible radius (meters) at full voice loudness; scales down with quieter speech. 0 disables voice occlusion.")]
		public float MaxVoiceHearingDistance { get; private set; } = 20f;

		[field: SerializeField]
		[field: Range(0f, 1f)]
		[field: Tooltip("Minimum normalized voice loudness required to emit an entity sound signal. Use this to filter background mic noise.")]
		public float MinVoiceLoudnessForEntityOcclusion { get; private set; } = 0.1f;

		public bool IsBlacklisted(GUID guid)
		{
			if (_blacklistedGuids == null)
			{
				_blacklistedGuids = BuildBlacklist();
			}
			return _blacklistedGuids.Contains(guid);
		}

		public bool TryGetAudibleDistance(GUID guid, out float audibleDistance)
		{
			if (_distanceByGuid == null)
			{
				_distanceByGuid = BuildDistances();
			}
			return _distanceByGuid.TryGetValue(guid, out audibleDistance);
		}

		public void SetMinVoiceLoudnessForEntityOcclusion(float value)
		{
			MinVoiceLoudnessForEntityOcclusion = Mathf.Clamp01(value);
		}

		private HashSet<GUID> BuildBlacklist()
		{
			HashSet<GUID> hashSet = new HashSet<GUID>();
			foreach (EventReference blacklistedSound in _blacklistedSounds)
			{
				if (!blacklistedSound.IsNull)
				{
					hashSet.Add(blacklistedSound.Guid);
				}
			}
			return hashSet;
		}

		private Dictionary<GUID, float> BuildDistances()
		{
			Dictionary<GUID, float> dictionary = new Dictionary<GUID, float>();
			foreach (SoundOcclusionDistanceOverride distanceOverride in _distanceOverrides)
			{
				if (!distanceOverride.Sound.IsNull)
				{
					dictionary[distanceOverride.Sound.Guid] = distanceOverride.AudibleDistance;
				}
			}
			return dictionary;
		}
	}
}
