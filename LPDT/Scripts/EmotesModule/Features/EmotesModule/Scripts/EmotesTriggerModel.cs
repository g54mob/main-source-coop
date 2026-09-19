using System;

namespace Features.EmotesModule.Scripts
{
	public class EmotesTriggerModel
	{
		public event Action<FaceEmoteType, float> OnFaceEmoteTriggered;

		public event Action<BodyEmoteType> OnBodyEmoteTriggered;

		public event Action<HandEmoteType> OnHandEmoteTriggered;

		public event Action<EmoteGroup> OnEmoteGroupViewCalled;

		public event Action<int> OnHotBarActivated;

		public event Action OnEmotingStarted;

		public event Action OnEmotingStopped;

		public event Action OnBodyEmoteInterruptRequested;

		public void RequestBodyEmoteInterrupt()
		{
			this.OnBodyEmoteInterruptRequested?.Invoke();
		}

		internal void TriggerFaceEmote(FaceEmoteType emoteType, float percent)
		{
			this.OnFaceEmoteTriggered?.Invoke(emoteType, percent);
		}

		internal void TriggerBodyEmote(BodyEmoteType emoteType)
		{
			this.OnBodyEmoteTriggered?.Invoke(emoteType);
		}

		internal void TriggerHandEmote(HandEmoteType emoteType)
		{
			this.OnHandEmoteTriggered?.Invoke(emoteType);
		}

		internal void CallEmoteView(EmoteGroup emoteGroup)
		{
			this.OnEmoteGroupViewCalled?.Invoke(emoteGroup);
		}

		internal void InvokeOnHotBarActivated(int hotBarIndex)
		{
			this.OnHotBarActivated?.Invoke(hotBarIndex);
		}

		internal void TriggerEmotingStarted()
		{
			this.OnEmotingStarted?.Invoke();
		}

		internal void TriggerEmotingStopped()
		{
			this.OnEmotingStopped?.Invoke();
		}
	}
}
