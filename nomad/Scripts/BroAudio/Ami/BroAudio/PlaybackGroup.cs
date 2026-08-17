using System;
using System.Collections.Generic;
using Ami.BroAudio.Runtime;
using UnityEngine;

namespace Ami.BroAudio
{
	public abstract class PlaybackGroup : ScriptableObject, IPlayableValidator
	{
		public delegate bool IsPlayableDelegate(SoundID id, Vector3 position);

		private PlaybackGroup _parent;

		private List<IRule> _rules;

		protected PlaybackGroup Parent
		{
			get
			{
				PlaybackGroup globalPlaybackGroup = SoundManager.Instance.Setting.GlobalPlaybackGroup;
				if (this == globalPlaybackGroup)
				{
					return null;
				}
				if (!_parent && _parent != globalPlaybackGroup)
				{
					_parent = globalPlaybackGroup;
				}
				return _parent;
			}
		}

		protected abstract IEnumerable<IRule> InitializeRules();

		public virtual void OnGetPlayer(IAudioPlayer player)
		{
		}

		public bool IsPlayable(SoundID id, Vector3 position)
		{
			if (_rules == null)
			{
				_rules = new List<IRule>(InitializeRules());
			}
			foreach (IRule rule in _rules)
			{
				if (!rule.RuleMethod(id, position))
				{
					return false;
				}
			}
			return true;
		}

		protected IRule Initialize<T>(Rule<T> rule, IsPlayableDelegate ruleMethod)
		{
			Func<Type, IRule> onGetParentRule = null;
			if ((bool)Parent)
			{
				onGetParentRule = Parent.GetRule;
			}
			return rule.Initialize(ruleMethod, onGetParentRule);
		}

		internal IRule GetRule(Type ruleType)
		{
			if (_rules == null)
			{
				_rules = new List<IRule>(InitializeRules());
			}
			foreach (IRule rule in _rules)
			{
				if (rule.GetType() == ruleType)
				{
					return rule;
				}
			}
			return new EmptyRule(ruleType);
		}

		internal void SetParent(PlaybackGroup parent)
		{
			_parent = parent;
		}
	}
}
