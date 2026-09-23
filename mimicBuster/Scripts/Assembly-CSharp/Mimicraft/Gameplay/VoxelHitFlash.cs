using System.Collections.Generic;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	public class VoxelHitFlash : MonoBehaviour
	{
		private static readonly int FlashColorId = Shader.PropertyToID("_FlashColor");

		private static readonly int FlashAmountId = Shader.PropertyToID("_FlashAmount");

		[Tooltip("Colour the body blends toward when hit.")]
		[SerializeField]
		private Color flashColor = Color.red;

		[Tooltip("How long the flash takes to fade out.")]
		[SerializeField]
		[Min(0.01f)]
		private float duration = 0.18f;

		[Tooltip("How far toward the flash colour it goes at its peak. 1 is fully solid.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float peak = 0.85f;

		private MaterialPropertyBlock block;

		private readonly List<Renderer> renderers = new List<Renderer>();

		private float remaining;

		public void Flash()
		{
			remaining = duration;
			Apply(peak);
		}

		private void Update()
		{
			if (!(remaining <= 0f))
			{
				remaining -= Time.deltaTime;
				if (remaining > 0f)
				{
					Apply(peak * (remaining / duration));
				}
				else
				{
					Clear();
				}
			}
		}

		private void OnDisable()
		{
			Clear();
		}

		private void Apply(float amount)
		{
			if (block == null)
			{
				block = new MaterialPropertyBlock();
			}
			block.Clear();
			block.SetColor(FlashColorId, flashColor);
			block.SetFloat(FlashAmountId, amount);
			foreach (Renderer item in CollectRenderers())
			{
				if (item != null)
				{
					item.SetPropertyBlock(block);
				}
			}
		}

		private void Clear()
		{
			remaining = 0f;
			foreach (Renderer item in CollectRenderers())
			{
				if (item != null)
				{
					item.SetPropertyBlock(null);
				}
			}
		}

		private List<Renderer> CollectRenderers()
		{
			renderers.Clear();
			GetComponentsInChildren(includeInactive: true, renderers);
			return renderers;
		}
	}
}
