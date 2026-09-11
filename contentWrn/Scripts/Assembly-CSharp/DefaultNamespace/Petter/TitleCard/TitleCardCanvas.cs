using System.Collections.Generic;
using UnityEngine;
using _02Scripts.Charts.Computes;

namespace DefaultNamespace.Petter.TitleCard
{
	public class TitleCardCanvas : MonoBehaviour
	{
		public ComputeShader csClearChunk;

		public ComputeShader csRenderFigure;

		public ComputeShader csCopyRt;

		public Vector2Int resolution;

		public RenderTexture titleCardRt;

		public RenderTexture titleCardRtSnapshot;

		public List<MeshRenderer> meshRenderers;

		public List<Figure> figures = new List<Figure>();

		public bool bgBlackNotWhite = true;

		private Figure activeFigure;

		private RenderTexture activeTexture;

		private RenderTexture snapShot;

		public bool invertBrushColor;

		public Color BackgroundColor
		{
			get
			{
				if (!bgBlackNotWhite)
				{
					return Color.white;
				}
				return Color.black;
			}
		}

		public Color BrushColor
		{
			get
			{
				if (bgBlackNotWhite)
				{
					return Color.white;
				}
				return Color.black;
			}
		}

		public bool IsDirty { get; private set; }

		public bool BackgroundIsBlack
		{
			get
			{
				return bgBlackNotWhite;
			}
			set
			{
				if (value != bgBlackNotWhite)
				{
					bgBlackNotWhite = value;
					Clear();
				}
			}
		}

		private void Awake()
		{
			CrFillRenderTexture.csFillRenderTexture = csClearChunk;
			CrRenderFigure.csRenderFigure = csRenderFigure;
			CrCopyRT.csCopyRt = csCopyRt;
			titleCardRt = CreateTitleCardRT();
			titleCardRtSnapshot = CreateTitleCardRT();
			foreach (MeshRenderer meshRenderer in meshRenderers)
			{
				meshRenderer.material.mainTexture = titleCardRt;
			}
			Clear();
		}

		private void OnDrawGizmos()
		{
			List<Figure> list = new List<Figure>(figures);
			if (activeFigure != null)
			{
				list.Add(activeFigure);
			}
			foreach (Figure item in list)
			{
				foreach (Vector3 debugWorldPosition in item.debugWorldPositions)
				{
					Gizmos.DrawWireSphere(debugWorldPosition, item.radius / 100f);
				}
			}
		}

		public void SetBrushColorToWhite()
		{
			invertBrushColor = !bgBlackNotWhite;
		}

		public void SetBrushColorToBlack()
		{
			invertBrushColor = bgBlackNotWhite;
		}

		public void Clear()
		{
			CrFillRenderTexture.FillRenderTexture(titleCardRt, BackgroundColor);
			CrFillRenderTexture.FillRenderTexture(titleCardRtSnapshot, BackgroundColor);
			SetDirty();
		}

		public RenderTexture CreateTitleCardRT()
		{
			return new RenderTexture(resolution.x, resolution.y, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB)
			{
				enableRandomWrite = true,
				anisoLevel = 0,
				filterMode = FilterMode.Bilinear
			};
		}

		public void Draw(Vector2 uvCoord, Vector3 debugWorldPos)
		{
			if (activeFigure == null)
			{
				activeFigure = new Figure();
				activeFigure.color = (invertBrushColor ? BackgroundColor : BrushColor);
				activeFigure.points.Add(uvCoord);
				activeFigure.points.Add(uvCoord);
				activeFigure.debugWorldPositions.Add(debugWorldPos);
			}
			activeFigure.DrawPoint(uvCoord, debugWorldPos);
			CrCopyRT.CopyRenderTexture(titleCardRtSnapshot, titleCardRt);
			CrRenderFigure.DrawFigure(titleCardRt, activeFigure);
		}

		public void CopyActiveToSnapShot()
		{
			CrCopyRT.CopyRenderTexture(titleCardRt, titleCardRtSnapshot);
		}

		public void FinishDrawing()
		{
			if (activeFigure == null)
			{
				return;
			}
			activeFigure.FinishFigure();
			figures.Add(activeFigure);
			activeFigure = null;
			foreach (Figure figure in figures)
			{
				CrRenderFigure.DrawFigure(titleCardRt, figure);
			}
			CrCopyRT.CopyRenderTexture(titleCardRt, titleCardRtSnapshot);
			SetDirty();
			figures.Clear();
		}

		public void SetDirty()
		{
			IsDirty = true;
		}

		public void ClearDirty()
		{
			IsDirty = false;
		}
	}
}
