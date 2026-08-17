using System;
using System.Collections.Generic;
using PaintCore;
using UnityEngine;
using UnityEngine.Events;

namespace PaintIn3D
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintIn3D#CwGenerateMask")]
	[AddComponentMenu("CW/Paint in 3D/CW Generate Mask")]
	public class CwGenerateMask : MonoBehaviour
	{
		public enum ApplyType
		{
			Manually = 0,
			Siblings = 1,
			SiblingsAndDescendants = 2
		}

		[Serializable]
		public class RenderTextureEvent : UnityEvent<RenderTexture>
		{
		}

		[SerializeField]
		private Mesh mesh;

		[SerializeField]
		private int submesh;

		[SerializeField]
		private CwCoord coord;

		[SerializeField]
		private Vector2Int size = new Vector2Int(512, 512);

		[SerializeField]
		private RenderTextureFormat format = RenderTextureFormat.R8;

		[SerializeField]
		private ApplyType applyTo = ApplyType.Siblings;

		[SerializeField]
		private RenderTextureEvent onGenerated;

		[NonSerialized]
		private RenderTexture generatedTexture;

		private static List<CwPaintableMeshTexture> tempPaintableTextures = new List<CwPaintableMeshTexture>();

		public Mesh Mesh
		{
			get
			{
				return mesh;
			}
			set
			{
				mesh = value;
			}
		}

		public int Submesh
		{
			get
			{
				return submesh;
			}
			set
			{
				submesh = value;
			}
		}

		public CwCoord Coord
		{
			get
			{
				return coord;
			}
			set
			{
				coord = value;
			}
		}

		public Vector2Int Size
		{
			get
			{
				return size;
			}
			set
			{
				size = value;
			}
		}

		public RenderTextureFormat Format
		{
			get
			{
				return format;
			}
			set
			{
				format = value;
			}
		}

		public ApplyType ApplyTo
		{
			get
			{
				return applyTo;
			}
			set
			{
				applyTo = value;
			}
		}

		public RenderTextureEvent OnGenerated
		{
			get
			{
				if (onGenerated == null)
				{
					onGenerated = new RenderTextureEvent();
				}
				return onGenerated;
			}
		}

		public RenderTexture GeneratedTexture => generatedTexture;

		[ContextMenu("Clear")]
		public void Clear()
		{
			UnityEngine.Object.DestroyImmediate(generatedTexture);
			generatedTexture = null;
		}

		[ContextMenu("Generate")]
		public RenderTexture Generate()
		{
			TryGenerate();
			return generatedTexture;
		}

		public bool TryGenerate()
		{
			Clear();
			if (size.x > 0 && size.y > 0)
			{
				generatedTexture = new RenderTexture(size.x, size.y, 0, format);
				generatedTexture.name = "Generated Mask";
				CwCommandReplace.Blit(generatedTexture, null, Color.black);
				CwBlit.White(generatedTexture, mesh, submesh, coord);
				if (applyTo != ApplyType.Manually)
				{
					if (applyTo == ApplyType.SiblingsAndDescendants)
					{
						GetComponentsInChildren(tempPaintableTextures);
					}
					else
					{
						GetComponents(tempPaintableTextures);
					}
					foreach (CwPaintableMeshTexture tempPaintableTexture in tempPaintableTextures)
					{
						tempPaintableTexture.LocalMaskTexture = generatedTexture;
					}
				}
				if (onGenerated != null)
				{
					onGenerated.Invoke(generatedTexture);
				}
				return true;
			}
			return false;
		}

		protected virtual void OnEnable()
		{
			Generate();
		}

		protected virtual void OnDisable()
		{
			Clear();
		}
	}
}
