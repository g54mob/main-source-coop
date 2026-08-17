using Ami.BroAudio.Data;
using Ami.Extension;
using UnityEngine;

namespace Ami.BroAudio.Runtime
{
	public struct PlaybackPreference
	{
		public readonly IAudioEntity Entity;

		private readonly Vector3? _position;

		private readonly Transform _followTarget;

		private FadeData _fadeInData;

		private FadeData _fadeOutData;

		private int _contextValue;

		public double ScheduledStartTime { get; set; }

		public double ScheduledEndTime { get; set; }

		public bool SkipLoopRegionIntro { get; set; }

		public PlaybackStage ChainedModeStage
		{
			get
			{
				return (PlaybackStage)_contextValue;
			}
			set
			{
				_contextValue = (int)value;
			}
		}

		public Vector3 Position
		{
			get
			{
				if (!_position.HasValue)
				{
					if (!(_followTarget != null))
					{
						return Utility.GloballyPlayedPosition;
					}
					return _followTarget.position;
				}
				return _position.Value;
			}
		}

		public PlaybackPreference(IAudioEntity entity, Vector3 position)
			: this(entity)
		{
			_position = position;
		}

		public PlaybackPreference(IAudioEntity entity, Transform followTarget)
			: this(entity)
		{
			_followTarget = followTarget;
		}

		public PlaybackPreference(IAudioEntity entity)
		{
			Entity = entity;
			_fadeInData = new FadeData(entity.GetFadeInEase(), SoundManager.FadeInEase);
			_fadeOutData = new FadeData(entity.GetFadeOutEase(), SoundManager.FadeOutEase);
			ScheduledStartTime = 0.0;
			ScheduledEndTime = 0.0;
			SkipLoopRegionIntro = false;
			_position = Utility.GloballyPlayedPosition;
			_followTarget = null;
			_contextValue = GetContextValue(entity);
		}

		public void SetFadeInEase(Ease ease)
		{
			_fadeInData.SetEase(ease);
		}

		public void SetFadeOutEase(Ease ease)
		{
			_fadeOutData.SetEase(ease);
		}

		public bool HasFadeIn(float clipFade, out float fadeIn, out Ease ease)
		{
			return HasFading(clipFade, SoundManager.FadeInEase, ref _fadeInData, out fadeIn, out ease);
		}

		public bool HasFadeOut(float clipFade, out float fadeOut, out Ease ease)
		{
			return HasFading(clipFade, SoundManager.FadeOutEase, ref _fadeOutData, out fadeOut, out ease);
		}

		private static bool HasFading(float clipFade, Ease clipEase, ref FadeData overrideData, out float fadeIn, out Ease ease)
		{
			fadeIn = clipFade;
			ease = clipEase;
			if (overrideData.TryGetOrConsumeOverride(out var fade, out var ease2))
			{
				fadeIn = fade;
				ease = ease2;
			}
			return fadeIn > 0f;
		}

		public IBroAudioClip PickNewClip()
		{
			return Entity.PickNewClip(_contextValue);
		}

		public void ApplySeamlessFade()
		{
			if (Entity.HasLoop(out var loopType, out var transitionTime) && loopType == LoopType.SeamlessLoop)
			{
				_fadeInData.Base = transitionTime;
				_fadeOutData.Base = transitionTime;
			}
		}

		public PlaybackPreference SetNextFadeIn(float fadeTime)
		{
			_fadeInData.Next = fadeTime;
			return this;
		}

		public PlaybackPreference SetNextFadeOut(float fadeTime)
		{
			_fadeOutData.Next = fadeTime;
			return this;
		}

		public void SetVelocity(int velocity)
		{
			if (Entity.PlayMode != MulticlipsPlayMode.Velocity)
			{
				Debug.LogError($"Cannot set velocity on [{Entity}] because it's not using VelocityPlayMode. (current : {Entity.PlayMode})");
			}
			else
			{
				_contextValue = velocity;
			}
		}

		public bool HasFollowTarget(out Transform target)
		{
			target = _followTarget;
			return target != null;
		}

		public bool HasSpecifiedPosition(out Vector3 position)
		{
			position = _position.GetValueOrDefault();
			if (_position.HasValue)
			{
				return !Utility.IsPlayedGlobally(position);
			}
			return false;
		}

		public bool CanHandoverToLoop()
		{
			if (IsChainedMode() && (ChainedModeStage == PlaybackStage.End || ChainedModeStage == PlaybackStage.None))
			{
				return false;
			}
			if (!IsLoop(LoopType.SeamlessLoop))
			{
				if (IsChainedMode())
				{
					return ChainedModeStage == PlaybackStage.Start;
				}
				return false;
			}
			return true;
		}

		public bool CanHandoverToEnd()
		{
			if (!IsChainedMode() || (Entity is AudioEntity audioEntity && audioEntity.Clips.Length < 3))
			{
				return false;
			}
			if (ChainedModeStage != PlaybackStage.End)
			{
				return ChainedModeStage != PlaybackStage.None;
			}
			return false;
		}

		public bool IsChainedMode()
		{
			return Entity.PlayMode == MulticlipsPlayMode.Chained;
		}

		public bool IsLoop(LoopType targetType)
		{
			if (Entity.HasLoop(out var loopType, out var _))
			{
				return loopType == targetType;
			}
			return false;
		}

		private static int GetContextValue(IAudioEntity entity)
		{
			if (entity.PlayMode == MulticlipsPlayMode.Chained)
			{
				return 1;
			}
			return 0;
		}
	}
}
