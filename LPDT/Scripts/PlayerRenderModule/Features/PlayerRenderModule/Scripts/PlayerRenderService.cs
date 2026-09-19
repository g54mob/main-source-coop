using System.Collections.Generic;
using UnityEngine;

namespace Features.PlayerRenderModule.Scripts
{
	public class PlayerRenderService : IPlayerRenderService
	{
		private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

		private static readonly int ColorId = Shader.PropertyToID("_Color");

		private static readonly int SurfaceId = Shader.PropertyToID("_Surface");

		private static readonly int BlendId = Shader.PropertyToID("_Blend");

		private static readonly int SrcBlendId = Shader.PropertyToID("_SrcBlend");

		private static readonly int DstBlendId = Shader.PropertyToID("_DstBlend");

		private static readonly int SrcBlendAlphaId = Shader.PropertyToID("_SrcBlendAlpha");

		private static readonly int DstBlendAlphaId = Shader.PropertyToID("_DstBlendAlpha");

		private static readonly int ZWriteId = Shader.PropertyToID("_ZWrite");

		private static readonly int ZWriteControlId = Shader.PropertyToID("_ZWriteControl");

		private static readonly int QueueControlId = Shader.PropertyToID("_QueueControl");

		private static readonly int TransparencyId = Shader.PropertyToID("_Transparency");

		private const float QUEUE_CONTROL_USER_OVERRIDE = 1f;

		private readonly PlayerRenderModel _playerRenderModel;

		public PlayerRenderService(PlayerRenderModel playerRenderModel)
		{
			_playerRenderModel = playerRenderModel;
		}

		public void SetPlayerAlpha(float alpha)
		{
			SetBasePlayerAlpha(alpha);
			SetCustomPlayerAlpha(alpha);
		}

		public void SetCustomPlayerAlpha(float alpha)
		{
			SetCustomRenderersAlpha(_playerRenderModel.CustomPlayerRenderers, alpha);
		}

		public void SetBasePlayerAlpha(float alpha)
		{
			SetRenderersAlpha(_playerRenderModel.BasePlayerRenderers, alpha);
		}

		public void SetPlayerSurfaceType(PlayerRendererSurfaceType surfaceType)
		{
			SetBasePlayerSurfaceType(surfaceType);
			SetCustomPlayerSurfaceType(surfaceType);
		}

		public void SetCustomPlayerSurfaceType(PlayerRendererSurfaceType surfaceType)
		{
			SetRenderersSurfaceType(_playerRenderModel.CustomPlayerRenderers, surfaceType);
		}

		public void SetBasePlayerSurfaceType(PlayerRendererSurfaceType surfaceType)
		{
			SetRenderersSurfaceType(_playerRenderModel.BasePlayerRenderers, surfaceType);
		}

		public void SetPlayerSurfaceRenderQueue(int renderQueue)
		{
			List<Renderer> list = new List<Renderer>();
			list.AddRange(_playerRenderModel.BasePlayerRenderers);
			list.AddRange(_playerRenderModel.CustomPlayerRenderers);
			foreach (Renderer item in list)
			{
				Material[] materials = item.materials;
				foreach (Material material in materials)
				{
					SetMaterialRenderQueue(material, renderQueue);
				}
			}
		}

		private void SetMaterialRenderQueue(Material material, int renderQueue)
		{
			if (material.HasProperty(QueueControlId))
			{
				material.SetFloat(QueueControlId, 1f);
			}
			material.renderQueue = renderQueue;
		}

		private void SetRenderersAlpha(IReadOnlyList<Renderer> renderers, float alpha)
		{
			float alpha2 = Mathf.Clamp01(alpha);
			foreach (Renderer renderer in renderers)
			{
				Material[] materials = renderer.materials;
				foreach (Material material in materials)
				{
					SetMaterialAlpha(material, alpha2);
				}
			}
		}

		private void SetCustomRenderersAlpha(IReadOnlyList<Renderer> renderers, float alpha)
		{
			float value = Mathf.Clamp01(alpha);
			foreach (Renderer renderer in renderers)
			{
				Material[] materials = renderer.materials;
				for (int i = 0; i < materials.Length; i++)
				{
					materials[i].SetFloat(TransparencyId, value);
				}
			}
		}

		private void SetMaterialAlpha(Material material, float alpha)
		{
			if (material.HasProperty(BaseColorId))
			{
				SetColorAlpha(material, BaseColorId, alpha);
			}
			if (material.HasProperty(ColorId))
			{
				SetColorAlpha(material, ColorId, alpha);
			}
		}

		private void SetColorAlpha(Material material, int colorPropertyId, float alpha)
		{
			Color color = material.GetColor(colorPropertyId);
			color.a = alpha;
			material.SetColor(colorPropertyId, color);
		}

		private void SetRenderersSurfaceType(IReadOnlyList<Renderer> renderers, PlayerRendererSurfaceType surfaceType)
		{
			foreach (Renderer renderer in renderers)
			{
				Material[] materials = renderer.materials;
				foreach (Material material in materials)
				{
					SetMaterialSurfaceType(material, surfaceType);
				}
			}
		}

		private void SetMaterialSurfaceType(Material material, PlayerRendererSurfaceType surfaceType)
		{
			if (material.HasProperty(SurfaceId))
			{
				material.SetFloat(SurfaceId, (float)surfaceType);
			}
			switch (surfaceType)
			{
			case PlayerRendererSurfaceType.Opaque:
				ApplyOpaqueRenderState(material);
				break;
			case PlayerRendererSurfaceType.Transparent:
				ApplyTransparentRenderState(material);
				break;
			}
		}

		private void ApplyOpaqueRenderState(Material material)
		{
			material.SetOverrideTag("RenderType", "Opaque");
			if (material.HasProperty(SrcBlendId))
			{
				material.SetFloat(SrcBlendId, 1f);
			}
			if (material.HasProperty(DstBlendId))
			{
				material.SetFloat(DstBlendId, 0f);
			}
			if (material.HasProperty(SrcBlendAlphaId))
			{
				material.SetFloat(SrcBlendAlphaId, 1f);
			}
			if (material.HasProperty(DstBlendAlphaId))
			{
				material.SetFloat(DstBlendAlphaId, 0f);
			}
			material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
			material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			SetMaterialRenderQueue(material, 2000);
		}

		private void ApplyTransparentRenderState(Material material)
		{
			material.SetOverrideTag("RenderType", "Transparent");
			if (material.HasProperty(BlendId))
			{
				material.SetFloat(BlendId, 0f);
			}
			if (material.HasProperty(SrcBlendId))
			{
				material.SetFloat(SrcBlendId, 1f);
			}
			if (material.HasProperty(DstBlendId))
			{
				material.SetFloat(DstBlendId, 10f);
			}
			if (material.HasProperty(SrcBlendAlphaId))
			{
				material.SetFloat(SrcBlendAlphaId, 1f);
			}
			if (material.HasProperty(DstBlendAlphaId))
			{
				material.SetFloat(DstBlendAlphaId, 10f);
			}
			material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
			material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
			SetMaterialRenderQueue(material, 3000);
		}
	}
}
