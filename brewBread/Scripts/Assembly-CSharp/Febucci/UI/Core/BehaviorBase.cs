using System;

namespace Febucci.UI.Core
{
	public abstract class BehaviorBase : EffectsBase
	{
		[Obsolete("This variable will be removed from next versions. Please use 'time.timeSinceStart' instead")]
		public float animatorTime => time.timeSinceStart;

		[Obsolete("This variable will be removed from next versions. Please use 'time.deltaTime' instead")]
		public float animatorDeltaTime => time.deltaTime;

		public TextAnimator.TimeData time { get; private set; }

		public abstract void SetDefaultValues(BehaviorDefaultValues data);

		internal void SetAnimatorData(in TextAnimator.TimeData time)
		{
			this.time = time;
		}
	}
}
