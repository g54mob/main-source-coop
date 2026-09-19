using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Features.PlayerSkinModule.Scripts.Features.PlayerSkinModule.Scripts.VisibilityHandling
{
	[NetworkBehaviourWeaved(0)]
	public class MultipleVisibilityHandler : VisibilityHandlerBase
	{
		[SerializeField]
		private List<Renderer> _hatPartRenderers;

		[FormerlySerializedAs("_topPartRenderers")]
		[SerializeField]
		private List<Renderer> _torsoPartRenderers;

		[SerializeField]
		private List<Renderer> _bottomPartRenderers;

		public void EnableHatVisibility()
		{
			foreach (Renderer hatPartRenderer in _hatPartRenderers)
			{
				hatPartRenderer.shadowCastingMode = ShadowCastingMode.On;
			}
		}

		public void DisableHatVisibility()
		{
			foreach (Renderer hatPartRenderer in _hatPartRenderers)
			{
				hatPartRenderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
			}
		}

		public void EnableTorsoVisibility()
		{
			foreach (Renderer torsoPartRenderer in _torsoPartRenderers)
			{
				torsoPartRenderer.shadowCastingMode = ShadowCastingMode.On;
			}
		}

		public void DisableTorsoVisibility()
		{
			foreach (Renderer torsoPartRenderer in _torsoPartRenderers)
			{
				torsoPartRenderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
			}
		}

		public void EnableTopVisibility()
		{
			EnableHatVisibility();
			EnableTorsoVisibility();
		}

		public void DisableTopVisibility()
		{
			DisableHatVisibility();
			DisableTorsoVisibility();
		}

		public void EnableBottomVisibility()
		{
			foreach (Renderer bottomPartRenderer in _bottomPartRenderers)
			{
				bottomPartRenderer.shadowCastingMode = ShadowCastingMode.On;
			}
		}

		public void DisableBottomVisibility()
		{
			foreach (Renderer bottomPartRenderer in _bottomPartRenderers)
			{
				bottomPartRenderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
			}
		}

		public void EnableHatRenderObjectOnly()
		{
			foreach (Renderer renderer in _renderers)
			{
				renderer.gameObject.SetActive(value: false);
			}
			foreach (Renderer hatPartRenderer in _hatPartRenderers)
			{
				hatPartRenderer.gameObject.SetActive(value: true);
			}
		}

		public void EnableTorsoRenderObjectOnly()
		{
			foreach (Renderer renderer in _renderers)
			{
				renderer.gameObject.SetActive(value: false);
			}
			foreach (Renderer torsoPartRenderer in _torsoPartRenderers)
			{
				torsoPartRenderer.gameObject.SetActive(value: true);
			}
		}

		public void EnableTopRenderObjectOnly()
		{
			foreach (Renderer renderer in _renderers)
			{
				renderer.gameObject.SetActive(value: false);
			}
			foreach (Renderer hatPartRenderer in _hatPartRenderers)
			{
				hatPartRenderer.gameObject.SetActive(value: true);
			}
			foreach (Renderer torsoPartRenderer in _torsoPartRenderers)
			{
				torsoPartRenderer.gameObject.SetActive(value: true);
			}
		}

		public void EnableBottomRenderObjectOnly()
		{
			foreach (Renderer renderer in _renderers)
			{
				renderer.gameObject.SetActive(value: false);
			}
			foreach (Renderer bottomPartRenderer in _bottomPartRenderers)
			{
				bottomPartRenderer.gameObject.SetActive(value: true);
			}
		}

		public void EnableHatRenderObject()
		{
			foreach (Renderer hatPartRenderer in _hatPartRenderers)
			{
				hatPartRenderer.gameObject.SetActive(value: true);
			}
		}

		public void DisableHatRenderObject()
		{
			foreach (Renderer hatPartRenderer in _hatPartRenderers)
			{
				hatPartRenderer.gameObject.SetActive(value: false);
			}
		}

		public void DisableTorsoRenderObject()
		{
			foreach (Renderer torsoPartRenderer in _torsoPartRenderers)
			{
				torsoPartRenderer.gameObject.SetActive(value: false);
			}
		}

		public void DisableTopRenderObject()
		{
			DisableHatRenderObject();
			DisableTorsoRenderObject();
		}

		public void DisableBottomRenderObject()
		{
			foreach (Renderer bottomPartRenderer in _bottomPartRenderers)
			{
				bottomPartRenderer.gameObject.SetActive(value: false);
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}
	}
}
