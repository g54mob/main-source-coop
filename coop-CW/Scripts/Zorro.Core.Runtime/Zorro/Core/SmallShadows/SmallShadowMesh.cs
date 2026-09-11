using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Zorro.Core.SmallShadows
{
	public class SmallShadowMesh : MonoBehaviour
	{
		public enum Mode
		{
			MeshRenderer = 0,
			LODGroup = 1
		}

		private bool isOnlyShadowCaster;

		public bool alwaysRemoveShadows;

		public float maxDistance = 20f;

		public Mode mode;

		public MeshRenderer meshRenderer;

		public LODGroup lodGroup;

		private bool m_registered;

		private List<MeshRenderer> m_meshRenderers;

		private bool IsMeshMode()
		{
			return mode == Mode.MeshRenderer;
		}

		private bool IsLODMode()
		{
			return mode == Mode.LODGroup;
		}

		private void OnEnable()
		{
			m_meshRenderers = new List<MeshRenderer>();
			if (mode == Mode.MeshRenderer)
			{
				if (meshRenderer == null)
				{
					meshRenderer = GetComponentInChildren<MeshRenderer>();
				}
				isOnlyShadowCaster = meshRenderer.shadowCastingMode == ShadowCastingMode.ShadowsOnly;
				if (meshRenderer != null && meshRenderer.shadowCastingMode != ShadowCastingMode.Off)
				{
					m_meshRenderers.Add(meshRenderer);
					if (alwaysRemoveShadows)
					{
						SetShadowMode(ShadowCastingMode.Off);
						return;
					}
				}
			}
			else
			{
				if (lodGroup == null)
				{
					lodGroup = GetComponent<LODGroup>();
				}
				if (lodGroup != null)
				{
					LOD[] lODs = lodGroup.GetLODs();
					for (int i = 0; i < lODs.Length; i++)
					{
						Renderer[] renderers = lODs[i].renderers;
						foreach (Renderer renderer in renderers)
						{
							if (renderer is MeshRenderer item && renderer.shadowCastingMode != ShadowCastingMode.Off)
							{
								m_meshRenderers.Add(item);
							}
						}
					}
					if (alwaysRemoveShadows)
					{
						SetShadowMode(ShadowCastingMode.Off);
						return;
					}
				}
			}
			SetShadowMode(ShadowCastingMode.Off);
			SmallShadowHandler.RegisterSmallShadowMesh(this);
			m_registered = true;
		}

		public void SetShadowMode(ShadowCastingMode shadowCastingMode)
		{
			if (isOnlyShadowCaster && shadowCastingMode == ShadowCastingMode.On)
			{
				shadowCastingMode = ShadowCastingMode.ShadowsOnly;
			}
			foreach (MeshRenderer meshRenderer in m_meshRenderers)
			{
				meshRenderer.shadowCastingMode = shadowCastingMode;
			}
		}

		public bool ShowSettings()
		{
			return !alwaysRemoveShadows;
		}

		private void OnDisable()
		{
			if (m_registered)
			{
				SmallShadowHandler.UnregisterSmallShadowMesh(this);
			}
		}
	}
}
