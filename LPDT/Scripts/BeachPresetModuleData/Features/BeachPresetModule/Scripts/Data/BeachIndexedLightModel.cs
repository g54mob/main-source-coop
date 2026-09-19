using System.Collections.Generic;
using UnityEngine;

namespace Features.BeachPresetModule.Scripts.Data
{
	public class BeachIndexedLightModel
	{
		private readonly Dictionary<int, Light> _lightsByIndex = new Dictionary<int, Light>();

		public bool TryGetLight(int index, out Light light)
		{
			return _lightsByIndex.TryGetValue(index, out light);
		}

		public void Register(int index, Light light)
		{
			if (!(light == null) && index >= 0)
			{
				_lightsByIndex[index] = light;
			}
		}

		public void Unregister(int index)
		{
			_lightsByIndex.Remove(index);
		}
	}
}
