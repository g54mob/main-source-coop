using System.Collections.Generic;
using Mimicraft.Settings;
using Mimicraft.UI;
using UnityEngine;
using UnityEngine.Audio;

namespace Mimicraft.Gameplay
{
	public static class SpatialAudio
	{
		public const float MinDistance = 3f;

		public const float MaxDistance = 45f;

		public const float MinimumRange = 30f;

		private const float Spread = 25f;

		private static readonly List<AudioSource> pendingRoutes = new List<AudioSource>();

		private static AnimationCurve rolloffCurve;

		private static AnimationCurve RolloffCurve
		{
			get
			{
				if (rolloffCurve != null)
				{
					return rolloffCurve;
				}
				rolloffCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f / 15f, 1f), new Keyframe(0.35f, 0.8f), new Keyframe(0.65f, 0.5f), new Keyframe(0.88f, 0.2f), new Keyframe(1f, 0f));
				for (int i = 0; i < rolloffCurve.length; i++)
				{
					rolloffCurve.SmoothTangents(i, 0f);
				}
				return rolloffCurve;
			}
		}

		public static void Apply(AudioSource source)
		{
			if (!(source == null))
			{
				source.spatialBlend = 1f;
				ApplyRange(source, 3f, 45f);
				source.spread = 25f;
				source.dopplerLevel = 0f;
				Route(source);
			}
		}

		public static void Route(AudioSource source)
		{
			if (!(source == null))
			{
				AudioMixerGroup audioMixerGroup = ResolveSfxGroup();
				if (audioMixerGroup != null)
				{
					source.outputAudioMixerGroup = audioMixerGroup;
				}
				else if (!pendingRoutes.Contains(source))
				{
					pendingRoutes.Add(source);
				}
			}
		}

		private static AudioMixerGroup ResolveSfxGroup()
		{
			AudioMixerGroup sfxGroup = AudioLibrary.SfxGroup;
			if (!(sfxGroup != null))
			{
				return SettingsApplier.SfxGroup;
			}
			return sfxGroup;
		}

		public static void FlushPendingRoutes()
		{
			AudioMixerGroup audioMixerGroup = ResolveSfxGroup();
			if (audioMixerGroup == null)
			{
				return;
			}
			for (int i = 0; i < pendingRoutes.Count; i++)
			{
				if (pendingRoutes[i] != null)
				{
					pendingRoutes[i].outputAudioMixerGroup = audioMixerGroup;
				}
			}
			pendingRoutes.Clear();
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			pendingRoutes.Clear();
		}

		public static void Apply(AudioSource source, float rangeMultiplier)
		{
			Apply(source);
			if (!(source == null))
			{
				float num = Mathf.Max(0.01f, rangeMultiplier);
				float num2 = Mathf.Max(30f, 45f * num);
				ApplyRange(source, num2 * (1f / 15f), num2);
			}
		}

		private static void ApplyRange(AudioSource source, float minRange, float maxRange)
		{
			source.rolloffMode = AudioRolloffMode.Custom;
			source.minDistance = minRange;
			source.maxDistance = maxRange;
			source.SetCustomCurve(AudioSourceCurveType.CustomRolloff, RolloffCurve);
		}
	}
}
