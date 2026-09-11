using System;
using EasyTextEffects.Effects;
using UnityEngine.Events;

namespace EasyTextEffects
{
	[Serializable]
	public class TextEffectEntry
	{
		public enum TriggerWhen
		{
			OnStart = 0,
			Manual = 1
		}

		public TriggerWhen triggerWhen;

		public TextEffectInstance effect;

		public UnityEvent onEffectCompleted = new UnityEvent();

		public void StartEffect()
		{
			effect.StartEffect(this);
		}

		internal void InvokeCompleted()
		{
			onEffectCompleted?.Invoke();
		}
	}
}
