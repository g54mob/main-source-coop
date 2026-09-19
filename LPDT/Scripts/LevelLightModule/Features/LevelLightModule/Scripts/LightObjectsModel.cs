using System.Collections.Generic;
using UnityEngine;

namespace Features.LevelLightModule.Scripts
{
	public class LightObjectsModel
	{
		private readonly Dictionary<LightObjectGroup, List<Light>> _lightObjects = new Dictionary<LightObjectGroup, List<Light>>();

		private readonly Dictionary<LightObjectGroup, bool> _groupEnabledState = new Dictionary<LightObjectGroup, bool>();

		public IReadOnlyDictionary<LightObjectGroup, List<Light>> LightObjectsGroup => _lightObjects;

		public void SetGroupEnabledState(LightObjectGroup group, bool state)
		{
			_groupEnabledState[group] = state;
		}

		public void AddLightObject(List<LightObjectGroup> lightObjectType, Light lightObjectAutoRegister)
		{
			lightObjectType.Add(LightObjectGroup.All);
			foreach (LightObjectGroup item in lightObjectType)
			{
				if (_lightObjects.TryGetValue(item, out var value))
				{
					value.Add(lightObjectAutoRegister);
				}
				else
				{
					_lightObjects.Add(item, new List<Light> { lightObjectAutoRegister });
				}
				if (_groupEnabledState.TryGetValue(item, out var value2))
				{
					lightObjectAutoRegister.enabled = value2;
				}
			}
		}

		public void RemoveLightObject(List<LightObjectGroup> lightObjectType, Light lightObjectAutoRegister)
		{
			lightObjectType.Add(LightObjectGroup.All);
			foreach (LightObjectGroup item in lightObjectType)
			{
				if (_lightObjects.TryGetValue(item, out var value))
				{
					value.Remove(lightObjectAutoRegister);
					if (value.Count == 0)
					{
						_lightObjects.Remove(item);
					}
				}
			}
		}
	}
}
