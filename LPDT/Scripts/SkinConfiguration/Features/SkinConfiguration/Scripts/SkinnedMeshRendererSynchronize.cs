using System.Collections.Generic;
using UnityEngine;

namespace Features.SkinConfiguration.Scripts
{
	public class SkinnedMeshRendererSynchronize : MonoBehaviour
	{
		[SerializeField]
		private List<SkinnedMeshRenderer> _skinnedMeshRenderers = new List<SkinnedMeshRenderer>();

		[SerializeField]
		private List<SkinnedMeshRenderer> _skinnedMeshRenderersReference = new List<SkinnedMeshRenderer>();

		private void Update()
		{
			for (int i = 0; i < _skinnedMeshRenderers.Count; i++)
			{
				SkinnedMeshRenderer skinnedMeshRenderer = _skinnedMeshRenderers[i];
				SkinnedMeshRenderer skinnedMeshRenderer2 = _skinnedMeshRenderersReference[i];
				skinnedMeshRenderer.enabled = skinnedMeshRenderer2.enabled;
			}
		}
	}
}
