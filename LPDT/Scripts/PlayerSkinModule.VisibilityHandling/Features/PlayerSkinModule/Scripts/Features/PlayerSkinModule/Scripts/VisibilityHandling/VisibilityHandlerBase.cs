using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;
using UnityEngine.Rendering;

namespace Features.PlayerSkinModule.Scripts.Features.PlayerSkinModule.Scripts.VisibilityHandling
{
	[NetworkBehaviourWeaved(0)]
	public class VisibilityHandlerBase : NetworkBehaviour
	{
		[SerializeField]
		protected List<Renderer> _renderers;

		public bool IsRenderObjectEnabled
		{
			get
			{
				foreach (Renderer renderer in _renderers)
				{
					if (renderer != null)
					{
						return renderer.gameObject.activeSelf;
					}
				}
				return false;
			}
		}

		public void EnableVisibility()
		{
			foreach (Renderer renderer in _renderers)
			{
				renderer.shadowCastingMode = ShadowCastingMode.On;
			}
		}

		public void DisableVisibility()
		{
			foreach (Renderer renderer in _renderers)
			{
				renderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
			}
		}

		public bool IsMainVisible()
		{
			return _renderers.First().shadowCastingMode == ShadowCastingMode.On;
		}

		public void EnableRenderObject()
		{
			foreach (Renderer renderer in _renderers)
			{
				renderer.gameObject.SetActive(value: true);
			}
		}

		public void DisableRenderObject()
		{
			foreach (Renderer renderer in _renderers)
			{
				renderer.gameObject.SetActive(value: false);
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
