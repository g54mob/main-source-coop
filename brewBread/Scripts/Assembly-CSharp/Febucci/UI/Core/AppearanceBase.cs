using System;

namespace Febucci.UI.Core
{
	public abstract class AppearanceBase : EffectsBase
	{
		public float effectDuration = 0.3f;

		[Obsolete("This variable will be removed from next versions. Please use 'effectDuration' instead")]
		protected float showDuration => effectDuration;

		public abstract void SetDefaultValues(AppearanceDefaultValues data);

		public virtual bool CanShowAppearanceOn(float timePassed)
		{
			return timePassed <= effectDuration;
		}

		public override void SetModifier(string modifierName, string modifierValue)
		{
			if (modifierName != null && modifierName == "d")
			{
				ApplyModifierTo(ref effectDuration, modifierValue);
			}
		}
	}
}
