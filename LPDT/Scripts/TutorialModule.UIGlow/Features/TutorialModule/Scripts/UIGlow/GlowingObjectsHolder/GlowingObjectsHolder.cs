using System.Collections.Generic;
using System.Linq;

namespace Features.TutorialModule.Scripts.UIGlow.GlowingObjectsHolder
{
	public class GlowingObjectsHolder
	{
		private readonly Dictionary<GlowableObjectEnum, IGlowableCollection> _glowableObjects = new Dictionary<GlowableObjectEnum, IGlowableCollection>();

		private readonly GlowableObjectsConfiguration _glowableObjectsConfiguration;

		public GlowingObjectsHolder(GlowableObjectsConfiguration glowableObjectsConfiguration)
		{
			_glowableObjectsConfiguration = glowableObjectsConfiguration;
		}

		public IEnumerable<IGlowableObject> GetAllGlowableObjects()
		{
			return _glowableObjects.Values.SelectMany((IGlowableCollection glowable) => glowable.GetGlowableObjects());
		}

		public IEnumerable<IGlowableObject> GetGlowableObject(GlowableObjectEnum glowableObjectEnum)
		{
			if (!_glowableObjects.TryGetValue(glowableObjectEnum, out var value))
			{
				return Enumerable.Empty<GlowableObject>();
			}
			return value.GetGlowableObjects();
		}

		public void Register(IGlowableObject glowableObject)
		{
			if (!_glowableObjects.TryGetValue(glowableObject.GlowableObjectEnum, out var value))
			{
				IGlowableCollection glowableCollection2;
				if (!IsAllowedMultiple(glowableObject.GlowableObjectEnum))
				{
					IGlowableCollection glowableCollection = new SingleGlowableObject();
					glowableCollection2 = glowableCollection;
				}
				else
				{
					IGlowableCollection glowableCollection = new GlowableObjectList();
					glowableCollection2 = glowableCollection;
				}
				value = glowableCollection2;
				_glowableObjects.Add(glowableObject.GlowableObjectEnum, value);
			}
			value.Add(glowableObject);
		}

		public void Unregister(IGlowableObject glowableObject)
		{
			if (_glowableObjects.TryGetValue(glowableObject.GlowableObjectEnum, out var value))
			{
				value.Remove(glowableObject);
				if (!value.GetGlowableObjects().Any())
				{
					_glowableObjects.Remove(glowableObject.GlowableObjectEnum);
				}
			}
		}

		private bool IsAllowedMultiple(GlowableObjectEnum glowableObjectGlowableObjectEnum)
		{
			return _glowableObjectsConfiguration.IsAllowedMultipleGlowableObjects(glowableObjectGlowableObjectEnum);
		}
	}
}
