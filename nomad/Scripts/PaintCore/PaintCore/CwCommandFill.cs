using System.Collections.Generic;
using CW.Common;
using UnityEngine;

namespace PaintCore
{
	public class CwCommandFill : CwCommand
	{
		public CwBlendMode Blend;

		public CwHashedTexture Texture;

		public Color Color;

		public float Opacity;

		public float Minimum;

		public static CwCommandFill Instance;

		private static Stack<CwCommandFill> pool;

		private static Material cachedMaterial;

		private static int cachedMaterialHash;

		private static int _Buffer;

		private static int _BufferSize;

		private static int _Texture;

		private static int _Color;

		private static int _Opacity;

		private static int _Minimum;

		public override bool RequireMesh => false;

		static CwCommandFill()
		{
			Instance = new CwCommandFill();
			pool = new Stack<CwCommandFill>();
			_Buffer = Shader.PropertyToID("_Buffer");
			_BufferSize = Shader.PropertyToID("_BufferSize");
			_Texture = Shader.PropertyToID("_Texture");
			_Color = Shader.PropertyToID("_Color");
			_Opacity = Shader.PropertyToID("_Opacity");
			_Minimum = Shader.PropertyToID("_Minimum");
			CwCommand.BuildMaterial(ref cachedMaterial, ref cachedMaterialHash, "Hidden/PaintCore/CwFill");
		}

		public static RenderTexture Blit(RenderTexture main, CwBlendMode blendMode, Texture texture, Color color, float opacity, float minimum)
		{
			RenderTexture swap = CwCommon.GetRenderTexture(main.descriptor, main);
			Blit(ref main, ref swap, blendMode, texture, color, opacity, minimum);
			CwCommon.ReleaseRenderTexture(swap);
			return main;
		}

		public static void Blit(ref RenderTexture main, ref RenderTexture swap, CwBlendMode blendMode, Texture texture, Color color, float opacity, float minimum)
		{
			Material material = Instance.SetMaterial(blendMode, texture, color, opacity, minimum);
			Instance.Apply(material);
			CwCommandReplace.Blit(swap, main, Color.white);
			material.SetTexture(_Buffer, swap);
			material.SetVector(_BufferSize, new Vector2(swap.width, swap.height));
			CwCommon.Blit(main, material, blendMode);
		}

		public override void Apply(Material material)
		{
			base.Apply(material);
			Blend.Apply(material);
			material.SetTexture(_Texture, Texture);
			material.SetColor(_Color, CwHelper.ToLinear(Color));
			material.SetFloat(_Opacity, Opacity);
			material.SetVector(_Minimum, new Vector4(Minimum, Minimum, Minimum, Minimum));
		}

		public override void Pool()
		{
			pool.Push(this);
		}

		public override void Transform(Matrix4x4 posMatrix, Matrix4x4 rotMatrix, Matrix4x4 rotMatrix2)
		{
		}

		public override CwCommand SpawnCopy()
		{
			CwCommandFill cwCommandFill = SpawnCopy(pool);
			cwCommandFill.Blend = Blend;
			cwCommandFill.Texture = Texture;
			cwCommandFill.Color = Color;
			cwCommandFill.Opacity = Opacity;
			cwCommandFill.Minimum = Minimum;
			return cwCommandFill;
		}

		public override void Apply(CwPaintableTexture paintableTexture)
		{
			base.Apply(paintableTexture);
			if (Blend.Index == 8 || Blend.Index == 15)
			{
				Blend.Color = paintableTexture.Color;
				Blend.Texture = paintableTexture.Texture;
			}
		}

		public Material SetMaterial(CwBlendMode blendMode, Texture texture, Color color, float opacity, float minimum)
		{
			Blend = blendMode;
			Material = new CwHashedMaterial(cachedMaterial, cachedMaterialHash);
			Pass = blendMode;
			Texture = texture;
			Color = color;
			Opacity = opacity;
			Minimum = minimum;
			return cachedMaterial;
		}
	}
}
