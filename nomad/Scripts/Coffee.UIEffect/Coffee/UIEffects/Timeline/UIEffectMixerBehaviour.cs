using UnityEngine.Playables;

namespace Coffee.UIEffects.Timeline
{
	public abstract class UIEffectMixerBehaviour<T, TBehavior> : PlayableBehaviour where TBehavior : UIEffectBehaviour, IGetValue<T>, new()
	{
		private T _defaultValue;

		protected abstract T currentValue { get; set; }

		protected UIEffect effect { get; private set; }

		public override void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
			InitializeIfNeeded(playerData as UIEffect);
			if (!effect)
			{
				return;
			}
			int inputCount = playable.GetInputCount();
			T val = default(T);
			float num = GetTotalWeight(playable);
			for (int i = 0; i < inputCount; i++)
			{
				float inputWeight = playable.GetInputWeight(i);
				if (!(inputWeight <= 0f))
				{
					ScriptPlayable<TBehavior> playable2 = (ScriptPlayable<TBehavior>)playable.GetInput(i);
					TBehavior behaviour = playable2.GetBehaviour();
					float time = (float)(playable2.GetTime() / behaviour.clip.timelineClip.duration);
					val = Add(val, behaviour.Get(time), inputWeight / num);
					num += inputWeight;
				}
			}
			currentValue = Lerp(_defaultValue, val, num);
		}

		public override void OnPlayableDestroy(Playable playable)
		{
			if ((bool)effect)
			{
				currentValue = _defaultValue;
			}
		}

		private static float GetTotalWeight(Playable playable)
		{
			float num = 0f;
			int inputCount = playable.GetInputCount();
			for (int i = 0; i < inputCount; i++)
			{
				num += playable.GetInputWeight(i);
			}
			return num;
		}

		private void InitializeIfNeeded(UIEffect newEffect)
		{
			if (!(effect == newEffect))
			{
				if ((bool)effect)
				{
					currentValue = _defaultValue;
				}
				effect = newEffect;
				_defaultValue = (newEffect ? currentValue : default(T));
			}
		}

		protected abstract T Add(T current, T value, float weight);

		protected abstract T Lerp(T defaultValue, T value, float weight);
	}
}
