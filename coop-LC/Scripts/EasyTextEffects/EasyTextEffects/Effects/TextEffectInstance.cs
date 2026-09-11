using EasyTextEffects.Editor.EditorDocumentation;
using EasyTextEffects.Editor.MyBoxCopy.Attributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace EasyTextEffects.Effects
{
	public abstract class TextEffectInstance : TextEffect_Base
	{
		public enum AnimationType
		{
			Loop = 0,
			LoopFixedDuration = 1,
			PingPong = 2,
			OneTime = 3
		}

		[Space(10f)]
		[Header("Type")]
		[FoldBox("", new string[] { "Packages/com.qiaozhilei.easy-text-effects/Documentation/Images/onetime.png, 200", "Packages/com.qiaozhilei.easy-text-effects/Documentation/Images/notonetime.png, 300", "Packages/com.qiaozhilei.easy-text-effects/Documentation/Images/fixed.png, 300", "Packages/com.qiaozhilei.easy-text-effects/Documentation/Images/loopvspingpong.png, 300" }, new FoldBoxAttribute.ContentType[] { FoldBoxAttribute.ContentType.Image }, true)]
		public AnimationType animationType = AnimationType.PingPong;

		[ConditionalField("animationType", false, new object[] { AnimationType.LoopFixedDuration })]
		public float fixedDuration;

		[Space(10f)]
		[FormerlySerializedAs("duration")]
		[Header("Timing")]
		[FoldBox("Timing Explained", new string[] { "Packages/com.qiaozhilei.easy-text-effects/Documentation/Images/time.png, 300" }, new FoldBoxAttribute.ContentType[] { FoldBoxAttribute.ContentType.Image }, true)]
		public float durationPerChar = 0.5f;

		[FoldBox("Timing Explained", new string[] { "Packages/com.qiaozhilei.easy-text-effects/Documentation/Images/time.png, 300" }, new FoldBoxAttribute.ContentType[] { FoldBoxAttribute.ContentType.Image }, true)]
		public float timeBetweenChars = 0.05f;

		[FoldBox("No Delay Explained", new string[] { "If enabled, the effect will start immediately for all characters, instead of waiting for the previous character to finish.", "Packages/com.qiaozhilei.easy-text-effects/Documentation/Images/nodelay.png, 300" }, new FoldBoxAttribute.ContentType[]
		{
			FoldBoxAttribute.ContentType.Text,
			FoldBoxAttribute.ContentType.Image
		}, true)]
		public bool noDelayForChars;

		[FoldBox("Reverse Char Order Explained", new string[] { "If enabled, the effect will start from the last character instead of the first. This is useful for exit animations.", "Packages/com.qiaozhilei.easy-text-effects/Documentation/Images/reverse.png, 300" }, new FoldBoxAttribute.ContentType[]
		{
			FoldBoxAttribute.ContentType.Text,
			FoldBoxAttribute.ContentType.Image
		}, true)]
		public bool reverseCharOrder;

		[Space(10f)]
		[FormerlySerializedAs("curve")]
		[Header("Curve")]
		public AnimationCurve easingCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		public bool clampBetween0And1;

		protected float startTime;

		internal bool started;

		protected bool isComplete;

		private TextEffectEntry currentEntry;

		public virtual bool IsComplete => isComplete;

		protected bool CheckCanApplyEffect(int _charIndex)
		{
			if (isComplete)
			{
				return false;
			}
			if (started && _charIndex >= startCharIndex)
			{
				return _charIndex < startCharIndex + charLength;
			}
			return false;
		}

		public virtual void StartEffect(TextEffectEntry entry)
		{
			currentEntry = entry;
			started = true;
			startTime = TimeUtil.GetTime();
			isComplete = false;
			if (animationType == AnimationType.OneTime || animationType == AnimationType.LoopFixedDuration)
			{
				easingCurve.preWrapMode = WrapMode.Once;
				easingCurve.postWrapMode = WrapMode.Once;
			}
			else if (animationType == AnimationType.Loop)
			{
				easingCurve.preWrapMode = ((!noDelayForChars) ? WrapMode.Once : WrapMode.Loop);
				easingCurve.postWrapMode = WrapMode.Loop;
			}
			else if (animationType == AnimationType.PingPong)
			{
				easingCurve.preWrapMode = ((!noDelayForChars) ? WrapMode.Once : WrapMode.PingPong);
				easingCurve.postWrapMode = WrapMode.PingPong;
			}
		}

		public virtual void StopEffect()
		{
			started = false;
			isComplete = true;
		}

		protected float Interpolate(float _start, float _end, int _charIndex)
		{
			float timeForChar = GetTimeForChar(_charIndex);
			float num = easingCurve.Evaluate(timeForChar / durationPerChar);
			if (clampBetween0And1)
			{
				num = Mathf.Clamp01(num);
			}
			return _start * (1f - num) + _end * num;
		}

		protected Vector2 Interpolate(Vector2 _start, Vector2 _end, int _charIndex)
		{
			float timeForChar = GetTimeForChar(_charIndex);
			float num = easingCurve.Evaluate(timeForChar / durationPerChar);
			if (clampBetween0And1)
			{
				num = Mathf.Clamp01(num);
			}
			return _start * (1f - num) + _end * num;
		}

		protected Color Interpolate(Color _start, Color _end, int _charIndex)
		{
			float timeForChar = GetTimeForChar(_charIndex);
			float num = easingCurve.Evaluate(timeForChar / durationPerChar);
			if (clampBetween0And1)
			{
				num = Mathf.Clamp01(num);
			}
			return _start * (1f - num) + _end * num;
		}

		private float GetTimeForChar(int _charIndex)
		{
			float time = TimeUtil.GetTime();
			if (animationType == AnimationType.LoopFixedDuration && time - startTime > fixedDuration)
			{
				startTime += fixedDuration;
				if (!isComplete)
				{
					isComplete = true;
					currentEntry?.InvokeCompleted();
				}
			}
			else if (animationType == AnimationType.OneTime && !isComplete)
			{
				float num = (noDelayForChars ? durationPerChar : (durationPerChar + timeBetweenChars * (float)(charLength - 1)));
				if (time - startTime > num)
				{
					isComplete = true;
					currentEntry?.InvokeCompleted();
				}
			}
			int num2 = _charIndex - startCharIndex;
			if (reverseCharOrder)
			{
				num2 = charLength - num2 - 1;
			}
			float num3 = startTime + timeBetweenChars * (float)num2;
			return time - num3;
		}

		public virtual TextEffectInstance Instantiate()
		{
			return Object.Instantiate(this);
		}
	}
}
