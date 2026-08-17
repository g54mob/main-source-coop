using System;
using Ami.BroAudio.Runtime;
using Ami.Extension;
using UnityEngine;
using UnityEngine.Audio;

namespace Ami.BroAudio.Data
{
	public class RuntimeSetting : ScriptableObject
	{
		public class FactorySettings
		{
			public const float CombFilteringPreventionInSeconds = 0.04f;

			public const Ease DefaultFadeInEase = Ease.InCubic;

			public const Ease DefaultFadeOutEase = Ease.OutSine;

			public const Ease SeamlessFadeInEase = Ease.OutCubic;

			public const Ease SeamlessFadeOutEase = Ease.OutSine;

			public const FilterSlope AudioFilterSlope = FilterSlope.FourPole;

			public const AudioMixerUpdateMode UpdateMode = AudioMixerUpdateMode.Normal;

			public const int DefaultAudioPlayerPoolSize = 5;

			public const PitchShiftingSetting PitchShifting = PitchShiftingSetting.AudioSource;

			public const Transition DefaultBGMTransition = Transition.CrossFade;

			public const float DefaultBGMTransitionTime = 2f;

			public const LoopType DefaultChainedPlayModeLoop = LoopType.SeamlessLoop;

			public const float DefaultChainedPlayModeSeamlessTransitionTime = 0.1f;

			public const bool AutomaticallyLoadAddressableAudioClips = false;

			public const float AutomaticallyUnloadUnusedAddressableAudioClipsAfter = 60f;

			public const LogType AddressablesNonPreloadedLogLevel = LogType.Error;
		}

		[Obsolete("This feature has been moved to PlaybackGroup")]
		[HideInInspector]
		public float CombFilteringPreventionInSeconds = 0.04f;

		public bool LogAccessRecycledPlayerWarning = true;

		public Ease DefaultFadeInEase = Ease.InCubic;

		public Ease DefaultFadeOutEase = Ease.OutSine;

		public Ease SeamlessFadeInEase = Ease.OutCubic;

		public Ease SeamlessFadeOutEase = Ease.OutSine;

		public FilterSlope AudioFilterSlope = FilterSlope.FourPole;

		public AudioMixerUpdateMode UpdateMode;

		public int DefaultAudioPlayerPoolSize = 5;

		public PitchShiftingSetting PitchSetting = PitchShiftingSetting.AudioSource;

		public bool AlwaysPlayMusicAsBGM = true;

		public Transition DefaultBGMTransition = Transition.CrossFade;

		public float DefaultBGMTransitionTime = 2f;

		public LoopType DefaultChainedPlayModeLoop = LoopType.SeamlessLoop;

		public float DefaultChainedPlayModeTransitionTime = 0.1f;

		public PlaybackGroup GlobalPlaybackGroup;

		public bool AutomaticallyLoadAddressableAudioClips;

		public float AutomaticallyUnloadUnusedAddressableAudioClipsAfter = 60f;

		public LogType AddressablesNonPreloadedLogLevel;
	}
}
