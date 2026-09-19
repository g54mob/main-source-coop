using System.Collections.Generic;

namespace Features.TutorialModule.Scripts.UIGlow.GlowingObjectsHolder
{
	public class GlowableObjectList : IGlowableCollection
	{
		private readonly List<IGlowableObject> _glowableObjects = new List<IGlowableObject>();

		public IEnumerable<IGlowableObject> GetGlowableObjects()
		{
			return _glowableObjects;
		}

		public void Add(IGlowableObject glowableObject)
		{
			_glowableObjects.Add(glowableObject);
		}

		public void Remove(IGlowableObject glowableObject)
		{
			_glowableObjects.Remove(glowableObject);
		}
	}
}
