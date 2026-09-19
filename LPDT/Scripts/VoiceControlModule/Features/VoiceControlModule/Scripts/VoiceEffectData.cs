using UnityEngine;

namespace Features.VoiceControlModule.Scripts
{
	public class VoiceEffectData
	{
		public string EffectName { get; set; }

		public Coroutine EffectCoroutine { get; set; }

		public bool IsRunning { get; set; }

		public VoiceEffectData(string effectName, Coroutine coroutine, bool isRunning)
		{
			EffectName = effectName;
			EffectCoroutine = coroutine;
			IsRunning = isRunning;
		}
	}
}
