namespace Features.UIAnimationsModule.Scripts
{
	public interface IUIAnimationPresetResolver
	{
		UIAnimationPreset Get(UIAnimationKey key);

		bool TryGet(UIAnimationKey key, out UIAnimationPreset preset);
	}
}
