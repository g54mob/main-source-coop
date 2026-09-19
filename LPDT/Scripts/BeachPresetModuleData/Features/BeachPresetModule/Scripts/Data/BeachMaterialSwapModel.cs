using System.Collections.Generic;
using UnityEngine;

namespace Features.BeachPresetModule.Scripts.Data
{
	public class BeachMaterialSwapModel
	{
		private readonly List<Renderer> _registeredRenderers = new List<Renderer>();

		public List<Renderer> RegisteredRenderers => _registeredRenderers;

		public void Register(Renderer renderer)
		{
			if (!(renderer == null))
			{
				_registeredRenderers.Add(renderer);
			}
		}

		public void Unregister(Renderer renderer)
		{
			if (!(renderer == null))
			{
				_registeredRenderers.Remove(renderer);
			}
		}
	}
}
