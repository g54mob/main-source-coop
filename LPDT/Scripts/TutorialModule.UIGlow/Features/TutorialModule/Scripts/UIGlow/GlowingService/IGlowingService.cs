namespace Features.TutorialModule.Scripts.UIGlow.GlowingService
{
	public interface IGlowingService
	{
		void SetGlow(GlowableObjectEnum glowableObjectType);

		void UnSetGlow(GlowableObjectEnum glowableObjectType);

		void DisableAllGlow();
	}
}
