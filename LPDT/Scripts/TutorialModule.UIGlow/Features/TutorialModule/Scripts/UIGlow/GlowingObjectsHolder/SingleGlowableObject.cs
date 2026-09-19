using System.Collections.Generic;
using UnityEngine;

namespace Features.TutorialModule.Scripts.UIGlow.GlowingObjectsHolder
{
	public class SingleGlowableObject : IGlowableCollection
	{
		private IGlowableObject _glowableObject;

		public IEnumerable<IGlowableObject> GetGlowableObjects()
		{
			if (_glowableObject != null)
			{
				yield return _glowableObject;
			}
		}

		public void Add(IGlowableObject glowableObject)
		{
			if (_glowableObject != null)
			{
				Debug.LogError("Single object slot is already occupied." + glowableObject.GlowableObjectEnum);
			}
			else
			{
				_glowableObject = glowableObject;
			}
		}

		public void Remove(IGlowableObject glowableObject)
		{
			if (_glowableObject == glowableObject)
			{
				_glowableObject = null;
			}
		}
	}
}
