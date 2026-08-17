using System.Collections.Generic;
using UnityEngine;

namespace PaintCore
{
	public class CwCommandReplaceChannels : CwCommand
	{
		public CwHashedTexture TextureR;

		public CwHashedTexture TextureG;

		public CwHashedTexture TextureB;

		public CwHashedTexture TextureA;

		public Vector4 ChannelR;

		public Vector4 ChannelG;

		public Vector4 ChannelB;

		public Vector4 ChannelA;

		public static CwCommandReplaceChannels Instance;

		private static Stack<CwCommandReplaceChannels> pool;

		private static Material cachedMaterial;

		private static int cachedMaterialHash;

		private static int _TextureR;

		private static int _TextureG;

		private static int _TextureB;

		private static int _TextureA;

		private static int _ChannelR;

		private static int _ChannelG;

		private static int _ChannelB;

		private static int _ChannelA;

		public override bool RequireMesh => false;

		static CwCommandReplaceChannels()
		{
			Instance = new CwCommandReplaceChannels();
			pool = new Stack<CwCommandReplaceChannels>();
			_TextureR = Shader.PropertyToID("_TextureR");
			_TextureG = Shader.PropertyToID("_TextureG");
			_TextureB = Shader.PropertyToID("_TextureB");
			_TextureA = Shader.PropertyToID("_TextureA");
			_ChannelR = Shader.PropertyToID("_ChannelR");
			_ChannelG = Shader.PropertyToID("_ChannelG");
			_ChannelB = Shader.PropertyToID("_ChannelB");
			_ChannelA = Shader.PropertyToID("_ChannelA");
			CwCommand.BuildMaterial(ref cachedMaterial, ref cachedMaterialHash, "Hidden/Paint Core/CwReplaceChannels");
		}

		public static void Blit(RenderTexture renderTexture, Texture textureR, Texture textureG, Texture textureB, Texture textureA, Vector4 channelR, Vector4 channelG, Vector4 channelB, Vector4 channelA, Vector4 channels)
		{
			Material material = Instance.SetMaterial(textureR, textureG, textureB, textureA, channelR, channelG, channelB, channelA);
			Instance.Apply(material);
			CwCommon.Blit(renderTexture, material, Instance.Pass);
		}

		public static void BlitFast(RenderTexture renderTexture, Texture textureR, Texture textureG, Texture textureB, Texture textureA, Vector4 channelR, Vector4 channelG, Vector4 channelB, Vector4 channelA)
		{
			Material material = Instance.SetMaterial(textureR, textureG, textureB, textureA, channelR, channelG, channelB, channelA);
			Instance.Apply(material);
			Graphics.Blit(null, renderTexture, material);
		}

		public override void Apply(Material material)
		{
			base.Apply(material);
			material.SetTexture(_TextureR, TextureR);
			material.SetTexture(_TextureG, TextureG);
			material.SetTexture(_TextureB, TextureB);
			material.SetTexture(_TextureA, TextureA);
			material.SetVector(_ChannelR, ChannelR);
			material.SetVector(_ChannelG, ChannelG);
			material.SetVector(_ChannelB, ChannelB);
			material.SetVector(_ChannelA, ChannelA);
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
			CwCommandReplaceChannels cwCommandReplaceChannels = SpawnCopy(pool);
			cwCommandReplaceChannels.TextureR = TextureR;
			cwCommandReplaceChannels.TextureG = TextureG;
			cwCommandReplaceChannels.TextureB = TextureB;
			cwCommandReplaceChannels.TextureA = TextureA;
			cwCommandReplaceChannels.ChannelR = ChannelR;
			cwCommandReplaceChannels.ChannelG = ChannelG;
			cwCommandReplaceChannels.ChannelB = ChannelB;
			cwCommandReplaceChannels.ChannelA = ChannelA;
			return cwCommandReplaceChannels;
		}

		public Material SetMaterial(Texture textureR, Texture textureG, Texture textureB, Texture textureA, Vector4 channelR, Vector4 channelG, Vector4 channelB, Vector4 channelA)
		{
			Material = new CwHashedMaterial(cachedMaterial, cachedMaterialHash);
			TextureR = textureR;
			TextureG = textureG;
			TextureB = textureB;
			TextureA = textureA;
			ChannelR = channelR;
			ChannelG = channelG;
			ChannelB = channelB;
			ChannelA = channelA;
			return cachedMaterial;
		}
	}
}
