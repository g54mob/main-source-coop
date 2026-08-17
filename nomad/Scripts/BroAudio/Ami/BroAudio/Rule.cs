using System;
using UnityEngine;

namespace Ami.BroAudio
{
	[Serializable]
	public abstract class Rule<T> : IRule
	{
		public static class NameOf
		{
			public const string IsOverride = "_isOverride";
		}

		public T Value;

		private PlaybackGroup.IsPlayableDelegate _ruleMethod;

		[SerializeField]
		private bool _isOverride = true;

		public PlaybackGroup.IsPlayableDelegate RuleMethod
		{
			get
			{
				if (_ruleMethod == null)
				{
					Debug.LogError($"{GetType()} is not initialized yet! As a result, a method that always passes will be returned.");
					return RuleExtension.EmptyRuleMethod;
				}
				return _ruleMethod;
			}
		}

		public override string ToString()
		{
			return base.ToString().Remove(0, "Ami.BroAudio.".Length) + " | Value: " + Value.ToString();
		}

		public Rule(T value)
		{
			Value = value;
		}

		internal IRule Initialize(PlaybackGroup.IsPlayableDelegate ruleMethod, Func<Type, IRule> onGetParentRule)
		{
			if (_isOverride)
			{
				_ruleMethod = ruleMethod;
				if (_ruleMethod == null)
				{
					_ruleMethod = RuleExtension.EmptyRuleMethod;
				}
				return this;
			}
			_ruleMethod = (onGetParentRule?.Invoke(GetType()))?.RuleMethod;
			if (_ruleMethod == null)
			{
				_ruleMethod = RuleExtension.EmptyRuleMethod;
			}
			return this;
		}

		public static implicit operator T(Rule<T> property)
		{
			if (property != null)
			{
				return property.Value;
			}
			return default(T);
		}
	}
}
