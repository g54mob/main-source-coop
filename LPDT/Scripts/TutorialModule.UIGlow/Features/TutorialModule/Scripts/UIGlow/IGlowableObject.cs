namespace Features.TutorialModule.Scripts.UIGlow
{
	public interface IGlowableObject
	{
		GlowableObjectEnum GlowableObjectEnum { get; }

		void SetGlow();

		void UnSetGlow();
	}
}
