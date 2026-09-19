using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.ChunkSystem
{
	[Serializable]
	public class ChunkStatusUpdateRenderers : ChunkStatusUpdateBehaviour
	{
		[SerializeField]
		private List<Renderer> _renderers;

		public override void Batch(GameObject[] objects)
		{
			_renderers.Clear();
			for (int i = 0; i < objects.Length; i++)
			{
				if (objects[i].TryGetComponent<Renderer>(out var component) && component.enabled && (component is MeshRenderer || component is SkinnedMeshRenderer))
				{
					_renderers.Add(component);
				}
			}
		}

		public override void Enable()
		{
			foreach (Renderer renderer in _renderers)
			{
				if (!(renderer == null))
				{
					renderer.enabled = true;
				}
			}
		}

		public override void Disable()
		{
			foreach (Renderer renderer in _renderers)
			{
				if (!(renderer == null))
				{
					renderer.enabled = false;
				}
			}
		}
	}
}
