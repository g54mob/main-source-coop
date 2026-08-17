using System.Collections.Generic;
using CW.Common;
using PaintCore;
using UnityEngine;

namespace PaintIn3D
{
	public class CwCommandSphere : CwCommand
	{
		public CwBlendMode Blend;

		public bool In3D;

		public Vector3 Position;

		public Vector3 EndPosition;

		public Vector3 Position2;

		public Vector3 EndPosition2;

		public int Extrusions;

		public bool Clip;

		public Matrix4x4 Matrix;

		public Color Color;

		public float Opacity;

		public float Hardness;

		public CwHashedTexture TileTexture;

		public Matrix4x4 TileMatrix;

		public float TileOpacity;

		public float TileTransition;

		public Matrix4x4 MaskMatrix;

		public CwHashedTexture MaskShape;

		public Vector4 MaskChannel;

		public Vector3 MaskStretch;

		public Vector2 MaskInvert;

		public CwRenderDepth DepthMask;

		public static CwCommandSphere Instance;

		private static Stack<CwCommandSphere> pool;

		private static Material cachedSpotMaterial;

		private static Material cachedLineMaterial;

		private static Material cachedQuadMaterial;

		private static Material cachedLineClipMaterial;

		private static Material cachedQuadClipMaterial;

		private static int cachedSpotMaterialHash;

		private static int cachedLineMaterialHash;

		private static int cachedQuadMaterialHash;

		private static int cachedLineClipMaterialHash;

		private static int cachedQuadClipMaterialHash;

		private static int _In3D;

		private static int _Position;

		private static int _EndPosition;

		private static int _Position2;

		private static int _EndPosition2;

		private static int _Matrix;

		private static int _Color;

		private static int _Opacity;

		private static int _Hardness;

		private static int _TileTexture;

		private static int _TileMatrix;

		private static int _TileOpacity;

		private static int _TileTransition;

		private static int _MaskMatrix;

		private static int _MaskTexture;

		private static int _MaskChannel;

		private static int _MaskStretch;

		private static int _MaskInvert;

		private static int _DepthMatrix;

		private static int _DepthTexture;

		private static int _DepthData;

		public override bool RequireMesh => true;

		static CwCommandSphere()
		{
			Instance = new CwCommandSphere();
			pool = new Stack<CwCommandSphere>();
			_In3D = Shader.PropertyToID("_In3D");
			_Position = Shader.PropertyToID("_Position");
			_EndPosition = Shader.PropertyToID("_EndPosition");
			_Position2 = Shader.PropertyToID("_Position2");
			_EndPosition2 = Shader.PropertyToID("_EndPosition2");
			_Matrix = Shader.PropertyToID("_Matrix");
			_Color = Shader.PropertyToID("_Color");
			_Opacity = Shader.PropertyToID("_Opacity");
			_Hardness = Shader.PropertyToID("_Hardness");
			_TileTexture = Shader.PropertyToID("_TileTexture");
			_TileMatrix = Shader.PropertyToID("_TileMatrix");
			_TileOpacity = Shader.PropertyToID("_TileOpacity");
			_TileTransition = Shader.PropertyToID("_TileTransition");
			_MaskMatrix = Shader.PropertyToID("_MaskMatrix");
			_MaskTexture = Shader.PropertyToID("_MaskTexture");
			_MaskChannel = Shader.PropertyToID("_MaskChannel");
			_MaskStretch = Shader.PropertyToID("_MaskStretch");
			_MaskInvert = Shader.PropertyToID("_MaskInvert");
			_DepthMatrix = Shader.PropertyToID("_DepthMatrix");
			_DepthTexture = Shader.PropertyToID("_DepthTexture");
			_DepthData = Shader.PropertyToID("_DepthData");
			CwCommand.BuildMaterial(ref cachedSpotMaterial, ref cachedSpotMaterialHash, "Hidden/PaintCore/CwSphere");
			CwCommand.BuildMaterial(ref cachedLineMaterial, ref cachedLineMaterialHash, "Hidden/PaintCore/CwSphere", "CW_LINE");
			CwCommand.BuildMaterial(ref cachedQuadMaterial, ref cachedQuadMaterialHash, "Hidden/PaintCore/CwSphere", "CW_QUAD");
			CwCommand.BuildMaterial(ref cachedLineClipMaterial, ref cachedLineClipMaterialHash, "Hidden/PaintCore/CwSphere", "CW_LINE_CLIP");
			CwCommand.BuildMaterial(ref cachedQuadClipMaterial, ref cachedQuadClipMaterialHash, "Hidden/PaintCore/CwSphere", "CW_QUAD_CLIP");
		}

		public override void Apply(Material material)
		{
			base.Apply(material);
			Blend.Apply(material);
			Matrix4x4 inverse = Matrix.inverse;
			material.SetFloat(_In3D, In3D ? 1f : 0f);
			material.SetVector(_Position, inverse.MultiplyPoint(Position));
			material.SetVector(_EndPosition, inverse.MultiplyPoint(EndPosition));
			material.SetVector(_Position2, inverse.MultiplyPoint(Position2));
			material.SetVector(_EndPosition2, inverse.MultiplyPoint(EndPosition2));
			material.SetMatrix(_Matrix, inverse);
			material.SetColor(_Color, CwHelper.ToLinear(Color));
			material.SetFloat(_Opacity, Opacity);
			material.SetFloat(_Hardness, Hardness);
			material.SetTexture(_TileTexture, TileTexture);
			material.SetMatrix(_TileMatrix, TileMatrix);
			material.SetFloat(_TileOpacity, TileOpacity);
			material.SetFloat(_TileTransition, TileTransition);
			material.SetMatrix(_MaskMatrix, MaskMatrix);
			material.SetTexture(_MaskTexture, MaskShape);
			material.SetVector(_MaskChannel, MaskChannel);
			material.SetVector(_MaskStretch, MaskStretch);
			material.SetVector(_MaskInvert, MaskInvert);
			if (DepthMask != null)
			{
				material.SetTexture(_DepthTexture, DepthMask.DepthTexture);
				material.SetVector(_DepthData, new Vector4(DepthMask.DepthTexture.width, DepthMask.DepthTexture.height, DepthMask.TapCount, DepthMask.Bias));
				material.SetMatrix(_DepthMatrix, DepthMask.SourceMatrix);
			}
			else
			{
				material.SetTexture(_DepthTexture, null);
				material.SetVector(_DepthData, new Vector4(0f, 0f, 0f, -1f));
				material.SetMatrix(_DepthMatrix, Matrix4x4.identity);
			}
		}

		public override void Pool()
		{
			pool.Push(this);
		}

		public override void Transform(Matrix4x4 posMatrix, Matrix4x4 rotMatrix, Matrix4x4 rotMatrix2)
		{
			Position = posMatrix.MultiplyPoint(Position);
			EndPosition = posMatrix.MultiplyPoint(EndPosition);
			Position2 = posMatrix.MultiplyPoint(Position2);
			EndPosition2 = posMatrix.MultiplyPoint(EndPosition2);
			Matrix = rotMatrix * Matrix * rotMatrix2;
		}

		public override CwCommand SpawnCopy()
		{
			CwCommandSphere cwCommandSphere = SpawnCopy(pool);
			cwCommandSphere.Blend = Blend;
			cwCommandSphere.In3D = In3D;
			cwCommandSphere.Position = Position;
			cwCommandSphere.EndPosition = EndPosition;
			cwCommandSphere.Position2 = Position2;
			cwCommandSphere.EndPosition2 = EndPosition2;
			cwCommandSphere.Extrusions = Extrusions;
			cwCommandSphere.Clip = Clip;
			cwCommandSphere.Matrix = Matrix;
			cwCommandSphere.Color = Color;
			cwCommandSphere.Opacity = Opacity;
			cwCommandSphere.Hardness = Hardness;
			cwCommandSphere.TileTexture = TileTexture;
			cwCommandSphere.TileMatrix = TileMatrix;
			cwCommandSphere.TileOpacity = TileOpacity;
			cwCommandSphere.TileTransition = TileTransition;
			cwCommandSphere.MaskMatrix = MaskMatrix;
			cwCommandSphere.MaskShape = MaskShape;
			cwCommandSphere.MaskChannel = MaskChannel;
			cwCommandSphere.MaskStretch = MaskStretch;
			cwCommandSphere.MaskInvert = MaskInvert;
			cwCommandSphere.DepthMask = DepthMask;
			return cwCommandSphere;
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

		public void SetLocation(Vector3 position, bool in3D = true)
		{
			In3D = in3D;
			Extrusions = 0;
			Clip = false;
			Position = position;
		}

		public void SetLocation(Vector3 position, Vector3 endPosition, bool in3D = true, bool clip = false)
		{
			In3D = in3D;
			Extrusions = 1;
			Clip = clip;
			Position = position;
			EndPosition = endPosition;
		}

		public void SetLocation(Vector3 positionA, Vector3 positionB, Vector3 positionC, bool in3D = true)
		{
			In3D = in3D;
			Extrusions = 2;
			Clip = false;
			Position = positionA;
			EndPosition = positionB;
			Position2 = positionC;
			EndPosition2 = positionA;
		}

		public void SetLocation(Vector3 position, Vector3 endPosition, Vector3 position2, Vector3 endPosition2, bool in3D = true, bool clip = false)
		{
			In3D = in3D;
			Extrusions = 2;
			Clip = clip;
			Position = position;
			EndPosition = endPosition;
			Position2 = position2;
			EndPosition2 = endPosition2;
		}

		public void ClearMask()
		{
			MaskShape = null;
			MaskChannel = Vector3.one;
			MaskInvert = new Vector2(0f, 1f);
		}

		public void SetMask(Matrix4x4 matrix, Texture shape, CwChannel channel, bool invert, Vector3 stretch)
		{
			MaskMatrix = matrix;
			MaskShape = shape;
			MaskChannel = PaintCore.CwCommon.IndexToVector((int)channel);
			MaskStretch = new Vector3(2f / stretch.x, 2f / stretch.y, 2f);
			MaskInvert = (invert ? new Vector2(1f, -1f) : new Vector2(0f, 1f));
		}

		public void ApplyAspect(Texture texture)
		{
			if (texture != null)
			{
				int width = texture.width;
				int height = texture.height;
				if (width > height)
				{
					Matrix.m00 *= (float)height / (float)width;
				}
				else
				{
					Matrix.m00 *= (float)width / (float)height;
				}
			}
		}

		public void SetShape(float radius)
		{
			Matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * radius);
		}

		public void SetShape(Quaternion rotation, Vector3 size, float angle)
		{
			if (In3D)
			{
				Matrix = Matrix4x4.TRS(Vector3.zero, rotation * Quaternion.Euler(0f, 0f, angle), size);
			}
			else
			{
				Matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, angle), size);
			}
		}

		public void SetMaterial(CwBlendMode blendMode, float hardness, Color color, float opacity, Texture tileTexture, Matrix4x4 tileMatrix, float tileOpacity, float tileTransition)
		{
			switch (Extrusions)
			{
			case 0:
				Material = new CwHashedMaterial(cachedSpotMaterial, cachedSpotMaterialHash);
				break;
			case 1:
				if (Clip)
				{
					Material = new CwHashedMaterial(cachedLineClipMaterial, cachedLineClipMaterialHash);
				}
				else
				{
					Material = new CwHashedMaterial(cachedLineMaterial, cachedLineMaterialHash);
				}
				break;
			case 2:
				if (Clip)
				{
					Material = new CwHashedMaterial(cachedQuadClipMaterial, cachedQuadClipMaterialHash);
				}
				else
				{
					Material = new CwHashedMaterial(cachedQuadMaterial, cachedQuadMaterialHash);
				}
				break;
			}
			Blend = blendMode;
			Pass = blendMode;
			Hardness = hardness;
			Color = color;
			Opacity = opacity;
			TileTexture = tileTexture;
			TileMatrix = tileMatrix;
			TileOpacity = tileOpacity;
			TileTransition = tileTransition;
		}
	}
}
