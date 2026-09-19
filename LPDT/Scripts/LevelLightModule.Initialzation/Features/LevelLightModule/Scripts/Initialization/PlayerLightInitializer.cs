using System.Collections.Generic;
using Fusion;
using Obi;
using UnityEngine;

namespace Features.LevelLightModule.Scripts.Initialization
{
	public class PlayerLightInitializer : MonoBehaviour
	{
		private const string AuthorityPlayerRenderLayerName = "AuthorityPlayer";

		private const string NonAuthorityPlayerRenderLayerName = "NonAuthorityPlayer";

		[SerializeField]
		private NetworkObject _networkObject;

		[SerializeField]
		private List<Renderer> _affectedRenderers;

		[SerializeField]
		private List<ObiRopeExtrudedRenderer> _obiRopeExtrudedRenderers;

		private uint? _authorityRenderingLayerMask;

		private uint? _nonAuthorityRenderingLayerMask;

		private void Start()
		{
			InitializeLight(_affectedRenderers);
			InitializeLight(_obiRopeExtrudedRenderers);
		}

		public void InitializeLight(List<Renderer> renderers)
		{
			InitializeLayers();
			foreach (Renderer renderer in renderers)
			{
				renderer.renderingLayerMask = (_networkObject.HasInputAuthority ? _authorityRenderingLayerMask.Value : _nonAuthorityRenderingLayerMask.Value);
			}
		}

		public void InitializeLight(List<ObiRopeExtrudedRenderer> renderers)
		{
			InitializeLayers();
			foreach (ObiRopeExtrudedRenderer renderer in renderers)
			{
				renderer.renderParameters.renderingLayerMask = (_networkObject.HasInputAuthority ? _authorityRenderingLayerMask.Value : _nonAuthorityRenderingLayerMask.Value);
			}
		}

		private void InitializeLayers()
		{
			uint valueOrDefault = _authorityRenderingLayerMask.GetValueOrDefault();
			if (!_authorityRenderingLayerMask.HasValue)
			{
				valueOrDefault = RenderingLayerMask.GetMask("AuthorityPlayer");
				_authorityRenderingLayerMask = valueOrDefault;
			}
			valueOrDefault = _nonAuthorityRenderingLayerMask.GetValueOrDefault();
			if (!_nonAuthorityRenderingLayerMask.HasValue)
			{
				valueOrDefault = RenderingLayerMask.GetMask("NonAuthorityPlayer");
				_nonAuthorityRenderingLayerMask = valueOrDefault;
			}
		}
	}
}
