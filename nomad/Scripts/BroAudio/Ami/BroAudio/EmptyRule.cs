using System;
using UnityEngine;

namespace Ami.BroAudio
{
	public class EmptyRule : IRule
	{
		public PlaybackGroup.IsPlayableDelegate RuleMethod => RuleExtension.EmptyRuleMethod;

		public EmptyRule(Type ruleType)
		{
			Debug.LogError($"Can't find a valid rule instance of {ruleType}, It might not be initialized, or there's no default rule available when the override option is off. As a result, a method that always passes is returned");
		}
	}
}
