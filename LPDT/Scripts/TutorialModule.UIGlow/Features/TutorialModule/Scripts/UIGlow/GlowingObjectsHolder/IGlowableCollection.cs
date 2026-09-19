using System.Collections.Generic;

namespace Features.TutorialModule.Scripts.UIGlow.GlowingObjectsHolder
{
	public interface IGlowableCollection
	{
		IEnumerable<IGlowableObject> GetGlowableObjects();

		void Add(IGlowableObject glowableObject);

		void Remove(IGlowableObject glowableObject);
	}
}
