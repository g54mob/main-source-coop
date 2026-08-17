using System;
using System.Collections.Generic;
using Ami.BroAudio.Runtime;
using Ami.Extension;
using UnityEngine;

namespace Ami.BroAudio
{
	[CreateAssetMenu(menuName = "BroAudio/Playback Group", fileName = "PlaybackGroup", order = 0)]
	public class DefaultPlaybackGroup : PlaybackGroup
	{
		public const float DefaultCombFilteringTime = 0.04f;

		[SerializeField]
		[Tooltip("The maximum number of sounds that can be played simultaneously in this group")]
		[ValueButton("Infinity", -1, -1f)]
		private MaxPlayableCountRule _maxPlayableCount = -1;

		[SerializeField]
		[ValueButton("Default", 0.04f, -1f)]
		[Tooltip("Time interval to prevent the Comb-Filtering effect")]
		private CombFilteringRule _combFilteringTime = 0.04f;

		[SerializeField]
		[DerivativeProperty(false)]
		[InspectorName("Ignore If Same Frame")]
		[Tooltip("Ignore Comb-filtering prevention if identical sounds are played within the same frame")]
		private bool _ignoreCombFilteringIfSameFrame;

		[SerializeField]
		[DerivativeProperty(false)]
		[InspectorName("Ignore If Distance Is Greater Than")]
		[Tooltip("Ignore Comb-filtering prevention if identical sounds are played farther apart than the specified distance")]
		private float _ignoreIfDistanceIsGreaterThan = 0.1f;

		[SerializeField]
		[DerivativeProperty(true)]
		[InspectorName("Log Warning When Occurs")]
		[Tooltip("Log a warning message when the Comb-Filtering occurs")]
		private bool _logCombFilteringWarning = true;

		private int _currentPlayingCount;

		private Action<SoundID> _decreasePlayingCountDelegate;

		protected override IEnumerable<IRule> InitializeRules()
		{
			yield return Initialize(_maxPlayableCount, IsPlayableLimitNotReached);
			yield return Initialize(_combFilteringTime, HasPassedCombFilteringRule);
		}

		public override void OnGetPlayer(IAudioPlayer player)
		{
			_currentPlayingCount++;
			if (_decreasePlayingCountDelegate == null)
			{
				_decreasePlayingCountDelegate = delegate
				{
					_currentPlayingCount--;
				};
			}
			player.OnEnd(_decreasePlayingCountDelegate);
		}

		protected virtual bool IsPlayableLimitNotReached(SoundID id, Vector3 position)
		{
			if ((int)_maxPlayableCount > 0)
			{
				return _currentPlayingCount < (int)_maxPlayableCount;
			}
			return true;
		}

		protected virtual bool HasPassedCombFilteringRule(SoundID id, Vector3 currentPlayPos)
		{
			if ((float)_combFilteringTime <= 0f || !SoundManager.Instance.TryGetPreviousPlayerFromCombFilteringPreventer(id, out var previousPlayer))
			{
				return true;
			}
			if (!HasPassedCombFilteringRule(previousPlayer, currentPlayPos))
			{
				if (_logCombFilteringWarning)
				{
					Debug.LogWarning("<b><color=#F3E9D7>[BroAudio] </color></b>One of the plays of Audio:" + id.ToString().ToWhiteBold() + " was rejected by the [Comb Filtering Time] rule.");
				}
				return false;
			}
			return true;
		}

		private bool HasPassedCombFilteringRule(AudioPlayer previousPlayer, Vector3 currentPlayPos)
		{
			int unscaledCurrentFrameBeganTime = TimeExtension.UnscaledCurrentFrameBeganTime;
			int playbackStartingTime = previousPlayer.PlaybackStartingTime;
			bool num = Mathf.Approximately(playbackStartingTime, 0f);
			float difference = unscaledCurrentFrameBeganTime - playbackStartingTime;
			bool flag = num || Mathf.Approximately(difference, 0f);
			if ((flag && _ignoreCombFilteringIfSameFrame) || (!flag && HasPassedCombFilteringTime()))
			{
				return true;
			}
			bool flag2 = Utility.IsPlayedGlobally(currentPlayPos);
			bool flag3 = Utility.IsPlayedGlobally(previousPlayer.PlayingPosition);
			if (!flag2 && !flag3 && (currentPlayPos - previousPlayer.PlayingPosition).sqrMagnitude > Mathf.Pow(_ignoreIfDistanceIsGreaterThan, 2f))
			{
				return true;
			}
			if (flag2 != flag3 && _ignoreIfDistanceIsGreaterThan > 0f)
			{
				return true;
			}
			return false;
			bool HasPassedCombFilteringTime()
			{
				return difference >= (float)TimeExtension.SecToMs((float)_combFilteringTime);
			}
		}

		private void OnEnable()
		{
			_currentPlayingCount = 0;
		}
	}
}
