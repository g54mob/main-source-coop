using Features.TutorialModule.Scripts.UIGlow.GlowingObjectsHolder;

namespace Features.TutorialModule.Scripts.UIGlow.GlowingService
{
	public class GlowingService : IGlowingService
	{
		private readonly Features.TutorialModule.Scripts.UIGlow.GlowingObjectsHolder.GlowingObjectsHolder _glowingObjectsHolder;

		public GlowingService(Features.TutorialModule.Scripts.UIGlow.GlowingObjectsHolder.GlowingObjectsHolder glowingObjectsHolder)
		{
			_glowingObjectsHolder = glowingObjectsHolder;
		}

		public void SetGlow(GlowableObjectEnum glowableObjectType)
		{
			foreach (IGlowableObject item in _glowingObjectsHolder.GetGlowableObject(glowableObjectType))
			{
				item.SetGlow();
			}
		}

		public void UnSetGlow(GlowableObjectEnum glowableObjectType)
		{
			foreach (IGlowableObject item in _glowingObjectsHolder.GetGlowableObject(glowableObjectType))
			{
				item.UnSetGlow();
			}
		}

		public void DisableAllGlow()
		{
			foreach (IGlowableObject allGlowableObject in _glowingObjectsHolder.GetAllGlowableObjects())
			{
				allGlowableObject.UnSetGlow();
			}
		}
	}
}
