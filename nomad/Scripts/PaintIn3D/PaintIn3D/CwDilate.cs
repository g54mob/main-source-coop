using System.Collections.Generic;
using CW.Common;
using PaintCore;
using UnityEngine;

namespace PaintIn3D
{
	public static class CwDilate
	{
		private static int _CwCoord;

		private static int _CwTexure;

		private static int _CwLookup;

		private static int _CwOffsets;

		private static int _CwSize;

		private static int _CwSamples;

		private static int _CwScale;

		private static Material dilateMaterial;

		private static Vector4[] offsets;

		private static List<Mesh> tempMeshes;

		static CwDilate()
		{
			_CwCoord = Shader.PropertyToID("_CwCoord");
			_CwTexure = Shader.PropertyToID("_CwTexure");
			_CwLookup = Shader.PropertyToID("_CwLookup");
			_CwOffsets = Shader.PropertyToID("_CwOffsets");
			_CwSize = Shader.PropertyToID("_CwSize");
			_CwSamples = Shader.PropertyToID("_CwSamples");
			_CwScale = Shader.PropertyToID("_CwScale");
			tempMeshes = new List<Mesh>();
			Vector2[] array = new Vector2[8]
			{
				new Vector2(0f, 1f),
				new Vector2(0f, -1f),
				new Vector2(1f, 0f),
				new Vector2(-1f, 0f),
				new Vector2(1f, 1f),
				new Vector2(-1f, -1f),
				new Vector2(-1f, 1f),
				new Vector2(1f, -1f)
			};
			List<Vector3> list = new List<Vector3>();
			int num = 32;
			list.Add(new Vector3(0f, 0f, 0f));
			for (int i = 1; i <= num; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					Vector2 vector = array[j] * i;
					float magnitude = vector.magnitude;
					if (vector.magnitude <= (float)num)
					{
						list.Add(new Vector3(vector.x, vector.y, magnitude));
					}
				}
			}
			list.Sort((Vector3 a, Vector3 b) => a.z.CompareTo(b.z));
			offsets = list.ConvertAll((Vector3 a) => new Vector4(a.x, a.y)).ToArray();
		}

		public static void Dilate(RenderTexture texture, Mesh mesh, int channel, int highQualitySteps)
		{
			tempMeshes.Clear();
			tempMeshes.Add(mesh);
			Dilate(texture, tempMeshes, channel, highQualitySteps);
		}

		public static void Dilate(RenderTexture texture, List<Mesh> meshes, int channel, int highQualitySteps)
		{
			if (dilateMaterial == null)
			{
				dilateMaterial = CwHelper.CreateTempMaterial("Dilate", "Hidden/PaintIn3D/CwDilate");
			}
			RenderTexture active = RenderTexture.active;
			RenderTextureFormat colorFormat = ((highQualitySteps > 0) ? RenderTextureFormat.ARGBInt : RenderTextureFormat.R8);
			RenderTexture renderTexture = PaintCore.CwCommon.GetRenderTexture(new RenderTextureDescriptor(texture.width, texture.height, colorFormat));
			RenderTexture renderTexture2 = PaintCore.CwCommon.GetRenderTexture(new RenderTextureDescriptor(texture.width, texture.height, colorFormat));
			RenderTexture renderTexture3 = PaintCore.CwCommon.GetRenderTexture(texture);
			renderTexture.filterMode = FilterMode.Point;
			renderTexture2.filterMode = FilterMode.Point;
			RenderTexture.active = renderTexture;
			dilateMaterial.SetVector(_CwCoord, PaintCore.CwCommon.IndexToVector(channel));
			dilateMaterial.SetVector(_CwSize, new Vector2(renderTexture.width, renderTexture.height));
			dilateMaterial.SetVectorArray(_CwOffsets, offsets);
			dilateMaterial.SetInt(_CwSamples, offsets.Length);
			dilateMaterial.SetVector(_CwScale, new Vector2(1f / (float)renderTexture.width, 1f / (float)renderTexture.height));
			if (dilateMaterial.SetPass(0) && meshes != null)
			{
				foreach (Mesh mesh in meshes)
				{
					if (mesh != null)
					{
						Graphics.DrawMeshNow(mesh, Matrix4x4.identity);
					}
				}
			}
			if (highQualitySteps > 0)
			{
				RenderTexture renderTexture4 = renderTexture2;
				RenderTexture renderTexture5 = renderTexture;
				for (int i = 0; i < highQualitySteps; i++)
				{
					RenderTexture renderTexture6 = renderTexture4;
					renderTexture4 = renderTexture5;
					renderTexture5 = renderTexture6;
					dilateMaterial.SetTexture(_CwTexure, renderTexture4);
					Graphics.Blit(null, renderTexture5, dilateMaterial, 3);
				}
				dilateMaterial.SetTexture(_CwTexure, texture);
				dilateMaterial.SetTexture(_CwLookup, renderTexture5);
				Graphics.Blit(null, renderTexture3, dilateMaterial, 4);
			}
			else
			{
				dilateMaterial.SetTexture(_CwTexure, renderTexture);
				Graphics.Blit(null, renderTexture2, dilateMaterial, 1);
				dilateMaterial.SetTexture(_CwTexure, texture);
				dilateMaterial.SetTexture(_CwLookup, renderTexture2);
				Graphics.Blit(null, renderTexture3, dilateMaterial, 2);
			}
			Graphics.Blit(renderTexture3, texture);
			PaintCore.CwCommon.ReleaseRenderTexture(renderTexture);
			PaintCore.CwCommon.ReleaseRenderTexture(renderTexture2);
			PaintCore.CwCommon.ReleaseRenderTexture(renderTexture3);
			RenderTexture.active = active;
		}
	}
}
