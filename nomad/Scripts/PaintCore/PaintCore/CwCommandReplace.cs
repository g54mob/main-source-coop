using System.Collections.Generic;
using CW.Common;
using UnityEngine;

namespace PaintCore
{
	public class CwCommandReplace : CwCommand
	{
		public CwHashedTexture Texture;

		public Color Color;

		public static CwCommandReplace Instance;

		private static Stack<CwCommandReplace> pool;

		private static Material cachedMaterial;

		private static int cachedMaterialHash;

		private static int _Texture;

		private static int _Color;

		public override bool RequireMesh => false;

		static CwCommandReplace()
		{
			Instance = new CwCommandReplace();
			pool = new Stack<CwCommandReplace>();
			_Texture = Shader.PropertyToID("_Texture");
			_Color = Shader.PropertyToID("_Color");
			CwCommand.BuildMaterial(ref cachedMaterial, ref cachedMaterialHash, "Hidden/PaintCore/CwReplace");
		}

		public static void Blit(RenderTexture renderTexture, Texture texture, Color tint)
		{
			Material material = Instance.SetMaterial(texture, tint);
			Instance.Apply(material);
			CwCommon.Blit(renderTexture, material, Instance.Pass);
		}

		public static void BlitFast(RenderTexture renderTexture, Texture texture, Color tint)
		{
			Material material = Instance.SetMaterial(texture, tint);
			Instance.Apply(material);
			Graphics.Blit(null, renderTexture, material);
		}

		public override void Apply(Material material)
		{
			base.Apply(material);
			material.SetTexture(_Texture, Texture);
			material.SetColor(_Color, CwHelper.ToLinear(Color));
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
			CwCommandReplace cwCommandReplace = SpawnCopy(pool);
			cwCommandReplace.Texture = Texture;
			cwCommandReplace.Color = Color;
			return cwCommandReplace;
		}

		public Material SetMaterial(Texture texture, Color color)
		{
			Material = new CwHashedMaterial(cachedMaterial, cachedMaterialHash);
			Texture = texture;
			Color = color;
			return cachedMaterial;
		}
	}
}
