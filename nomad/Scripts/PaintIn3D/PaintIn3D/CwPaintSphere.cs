using System.Collections.Generic;
using CW.Common;
using PaintCore;
using UnityEngine;
using UnityEngine.Serialization;

namespace PaintIn3D
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintIn3D#CwPaintSphere")]
	[AddComponentMenu("CW/Paint Core/Hit/CW Paint Sphere")]
	public class CwPaintSphere : MonoBehaviour, IHitPoint, IHit, IHitLine, IHitTriangle, IHitQuad, IHitCoord
	{
		[SerializeField]
		private LayerMask layers = -1;

		[SerializeField]
		private CwGroup group;

		[SerializeField]
		private CwMeshModel targetModel;

		[SerializeField]
		private CwPaintableTexture targetTexture;

		[SerializeField]
		private CwBlendMode blendMode = CwBlendMode.AlphaBlend(Vector4.one);

		[SerializeField]
		private Color color = Color.white;

		[Range(0f, 1f)]
		[SerializeField]
		private float opacity = 1f;

		[Range(-180f, 180f)]
		[SerializeField]
		private float angle;

		[SerializeField]
		private Vector3 scale = Vector3.one;

		[SerializeField]
		private float radius = 0.1f;

		[SerializeField]
		private float hardness = 5f;

		[SerializeField]
		private Texture tileTexture;

		[SerializeField]
		private Transform tileTransform;

		[FormerlySerializedAs("tileBlend")]
		[Range(0f, 1f)]
		[SerializeField]
		private float tileOpacity = 1f;

		[Range(1f, 200f)]
		[SerializeField]
		private float tileTransition = 4f;

		[SerializeField]
		private bool findMask = true;

		[SerializeField]
		private bool findDepthMask = true;

		[SerializeField]
		private CwModifierList modifiers;

		public LayerMask Layers
		{
			get
			{
				return layers;
			}
			set
			{
				layers = value;
			}
		}

		public CwGroup Group
		{
			get
			{
				return group;
			}
			set
			{
				group = value;
			}
		}

		public CwMeshModel TargetModel
		{
			get
			{
				return targetModel;
			}
			set
			{
				targetModel = value;
			}
		}

		public CwPaintableTexture TargetTexture
		{
			get
			{
				return targetTexture;
			}
			set
			{
				targetTexture = value;
			}
		}

		public CwBlendMode BlendMode
		{
			get
			{
				return blendMode;
			}
			set
			{
				blendMode = value;
			}
		}

		public Color Color
		{
			get
			{
				return color;
			}
			set
			{
				color = value;
			}
		}

		public float Opacity
		{
			get
			{
				return opacity;
			}
			set
			{
				opacity = value;
			}
		}

		public float Angle
		{
			get
			{
				return angle;
			}
			set
			{
				angle = value;
			}
		}

		public Vector3 Scale
		{
			get
			{
				return scale;
			}
			set
			{
				scale = value;
			}
		}

		public float Radius
		{
			get
			{
				return radius;
			}
			set
			{
				radius = value;
			}
		}

		public float Hardness
		{
			get
			{
				return hardness;
			}
			set
			{
				hardness = value;
			}
		}

		public Texture TileTexture
		{
			get
			{
				return tileTexture;
			}
			set
			{
				tileTexture = value;
			}
		}

		public Transform TileTransform
		{
			get
			{
				return tileTransform;
			}
			set
			{
				tileTransform = value;
			}
		}

		public float TileOpacity
		{
			get
			{
				return tileOpacity;
			}
			set
			{
				tileOpacity = value;
			}
		}

		public float TileTransition
		{
			get
			{
				return tileTransition;
			}
			set
			{
				tileTransition = value;
			}
		}

		public bool FindMask
		{
			get
			{
				return findMask;
			}
			set
			{
				findMask = value;
			}
		}

		public bool FindDepthMask
		{
			get
			{
				return findDepthMask;
			}
			set
			{
				findDepthMask = value;
			}
		}

		public CwModifierList Modifiers
		{
			get
			{
				if (modifiers == null)
				{
					modifiers = new CwModifierList();
				}
				return modifiers;
			}
		}

		public void IncrementAngle(float degrees)
		{
			angle = Mathf.Repeat(angle + 180f + degrees, 360f) - 180f;
		}

		public void MultiplyOpacity(float multiplier)
		{
			opacity = Mathf.Clamp01(opacity * multiplier);
		}

		public void IncrementOpacity(float delta)
		{
			opacity = Mathf.Clamp01(opacity + delta);
		}

		public void MultiplyRadius(float multiplier)
		{
			radius *= multiplier;
		}

		public void IncrementRadius(float delta)
		{
			radius += delta;
		}

		public void MultiplyScale(float multiplier)
		{
			scale *= multiplier;
		}

		public void IncrementScale(float multiplier)
		{
			scale += Vector3.one * multiplier;
		}

		public void HandleHitPoint(bool preview, int priority, float pressure, int seed, Vector3 position, Quaternion rotation)
		{
			if (modifiers != null && modifiers.Count > 0)
			{
				CwHelper.BeginSeed(seed);
				modifiers.ModifyPosition(ref position, preview, pressure);
				CwHelper.EndSeed();
			}
			CwCommandSphere.Instance.SetState(preview, priority);
			CwCommandSphere.Instance.SetLocation(position);
			float num = PaintCore.CwCommon.GetRadius(HandleHitCommon(preview, pressure, seed, rotation));
			Vector3 vector = position;
			HandleMaskCommon(vector);
			CwPaintableManager.SubmitAll(CwCommandSphere.Instance, vector, num, layers, group, targetModel, targetTexture);
		}

		public void HandleHitLine(bool preview, int priority, float pressure, int seed, Vector3 position, Vector3 endPosition, Quaternion rotation, bool clip)
		{
			CwCommandSphere.Instance.SetState(preview, priority);
			CwCommandSphere.Instance.SetLocation(position, endPosition, in3D: true, clip);
			float num = PaintCore.CwCommon.GetRadius(HandleHitCommon(preview, pressure, seed, rotation), position, endPosition);
			Vector3 position2 = PaintCore.CwCommon.GetPosition(position, endPosition);
			HandleMaskCommon(position2);
			CwPaintableManager.SubmitAll(CwCommandSphere.Instance, position2, num, layers, group, targetModel, targetTexture);
		}

		public void HandleHitTriangle(bool preview, int priority, float pressure, int seed, Vector3 positionA, Vector3 positionB, Vector3 positionC, Quaternion rotation)
		{
			CwCommandSphere.Instance.SetState(preview, priority);
			CwCommandSphere.Instance.SetLocation(positionA, positionB, positionC);
			float num = PaintCore.CwCommon.GetRadius(HandleHitCommon(preview, pressure, seed, rotation), positionA, positionB, positionC);
			Vector3 position = PaintCore.CwCommon.GetPosition(positionA, positionB, positionC);
			HandleMaskCommon(position);
			CwPaintableManager.SubmitAll(CwCommandSphere.Instance, position, num, layers, group, targetModel, targetTexture);
		}

		public void HandleHitQuad(bool preview, int priority, float pressure, int seed, Vector3 position, Vector3 endPosition, Vector3 position2, Vector3 endPosition2, Quaternion rotation, bool clip)
		{
			CwCommandSphere.Instance.SetState(preview, priority);
			CwCommandSphere.Instance.SetLocation(position, endPosition, position2, endPosition2, in3D: true, clip);
			float num = PaintCore.CwCommon.GetRadius(HandleHitCommon(preview, pressure, seed, rotation), position, endPosition, position2, endPosition2);
			Vector3 position3 = PaintCore.CwCommon.GetPosition(position, endPosition, position2, endPosition2);
			HandleMaskCommon(position3);
			CwPaintableManager.SubmitAll(CwCommandSphere.Instance, position3, num, layers, group, targetModel, targetTexture);
		}

		public void HandleHitCoord(bool preview, int priority, float pressure, int seed, CwHit hit, Quaternion rotation)
		{
			CwPaintableMeshAtlas component = hit.Transform.GetComponent<CwPaintableMeshAtlas>();
			if (!(component != null))
			{
				return;
			}
			List<CwPaintableTexture> list = component.FindPaintableTextures(group);
			for (int num = list.Count - 1; num >= 0; num--)
			{
				CwPaintableTexture cwPaintableTexture = list[num];
				Vector2 vector = cwPaintableTexture.GetCoord(ref hit);
				if (modifiers != null && modifiers.Count > 0)
				{
					Vector3 position = vector;
					CwHelper.BeginSeed(seed);
					modifiers.ModifyPosition(ref position, preview, pressure);
					CwHelper.EndSeed();
					vector = position;
				}
				CwCommandSphere.Instance.SetState(preview, priority);
				CwCommandSphere.Instance.SetLocation(vector, in3D: false);
				HandleHitCommon(preview, pressure, seed, rotation);
				CwCommandSphere.Instance.ClearMask();
				CwCommandSphere.Instance.ApplyAspect(cwPaintableTexture.Current);
				CwPaintableManager.Submit(CwCommandSphere.Instance, component, cwPaintableTexture);
			}
		}

		private Vector3 HandleHitCommon(bool preview, float pressure, int seed, Quaternion rotation)
		{
			float num = opacity;
			float num2 = radius;
			Vector3 vector = scale;
			float num3 = hardness;
			Color color = this.color;
			float num4 = angle;
			Matrix4x4 tileMatrix = ((tileTransform != null) ? tileTransform.localToWorldMatrix : Matrix4x4.identity);
			if (modifiers != null && modifiers.Count > 0)
			{
				CwHelper.BeginSeed(seed);
				modifiers.ModifyColor(ref color, preview, pressure);
				modifiers.ModifyAngle(ref num4, preview, pressure);
				modifiers.ModifyOpacity(ref num, preview, pressure);
				modifiers.ModifyRadius(ref num2, preview, pressure);
				modifiers.ModifyScale(ref vector, preview, pressure);
				modifiers.ModifyHardness(ref num3, preview, pressure);
				CwHelper.EndSeed();
			}
			Vector3 vector2 = vector * num2;
			CwCommandSphere.Instance.SetShape(rotation, vector2, num4);
			CwCommandSphere.Instance.SetMaterial(BlendMode, num3, color, num, tileTexture, tileMatrix, tileOpacity, tileTransition);
			return vector2;
		}

		private void HandleMaskCommon(Vector3 worldPosition)
		{
			if (findMask)
			{
				CwMask cwMask = CwMask.Find(worldPosition, layers);
				if (cwMask != null)
				{
					CwCommandSphere.Instance.SetMask(cwMask.Matrix, cwMask.Texture, cwMask.Channel, cwMask.Invert, cwMask.Stretch);
				}
				else
				{
					CwCommandSphere.Instance.ClearMask();
				}
			}
			else
			{
				CwCommandSphere.Instance.ClearMask();
			}
			if (findDepthMask)
			{
				CwCommandSphere.Instance.DepthMask = CwRenderDepth.Find();
			}
			else
			{
				CwCommandSphere.Instance.DepthMask = null;
			}
		}
	}
}
