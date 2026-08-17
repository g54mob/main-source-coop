using System;
using UnityEngine;

namespace Den.Tools.Matrices
{
	public class MatrixObject : MonoBehaviour
	{
		public enum DisplayGizmo
		{
			None = 0,
			Texture = 1,
			Height = 2,
			FacetedHeight = 3
		}

		[NonSerialized]
		public Matrix matrix = new Matrix(new CoordRect(0, 0, 1, 1));

		public Vector2D worldPosition;

		public Vector2D worldSize = new Vector2D(1000f, 1000f);

		public float worldHeight = 250f;

		[NonSerialized]
		public Texture2D preview;

		public MatrixAsset.Source source;

		public string rawPath;

		public Texture2D textureSource;

		public MatrixAsset.Channel channelSource;

		public int newRes = 128;

		public Coord newOffset;

		public DisplayGizmo displayGizmo;

		public bool centerCell = true;

		public FilterMode filterMode;

		[NonSerialized]
		public MatrixHeightGizmo heightGizmo;

		[NonSerialized]
		public MatrixTextureGizmo textureGizmo;

		public MatrixWorld MatrixWorld => new MatrixWorld(matrix, (Vector3)worldPosition, new Vector3(worldSize.x, worldHeight, worldSize.z));

		public void RefreshPreview(int size = 128)
		{
			if (this.matrix != null)
			{
				Matrix matrix = this.matrix;
				preview = new Texture2D(matrix.rect.size.x, matrix.rect.size.z);
				matrix.ExportTexture(preview);
			}
			else
			{
				preview = TextureExtensions.ColorTexture(2, 2, Color.black);
			}
		}

		public void RefreshGizmos()
		{
			switch (displayGizmo)
			{
			case DisplayGizmo.Texture:
				if (textureGizmo == null)
				{
					textureGizmo = new MatrixTextureGizmo();
				}
				textureGizmo.SetMatrix(matrix, centerCell, filterMode);
				break;
			case DisplayGizmo.Height:
				if (heightGizmo == null)
				{
					heightGizmo = new MatrixHeightGizmo();
				}
				heightGizmo.SetMatrix(matrix);
				break;
			case DisplayGizmo.FacetedHeight:
				if (heightGizmo == null)
				{
					heightGizmo = new MatrixHeightGizmo();
				}
				heightGizmo.SetMatrix(matrix, faceted: true);
				break;
			}
		}

		public void Reload()
		{
			switch (source)
			{
			case MatrixAsset.Source.Raw:
				if (rawPath != null)
				{
					MatrixAsset.ImportRaw(ref matrix, rawPath);
				}
				break;
			case MatrixAsset.Source.Texture:
				if (textureSource != null)
				{
					MatrixAsset.ImportTexture(ref matrix, textureSource, channelSource);
				}
				break;
			case MatrixAsset.Source.New:
				if (matrix == null || matrix.rect.size.x != newRes || matrix.rect.size.z != newRes)
				{
					matrix = new Matrix(new CoordRect(0, 0, newRes, newRes));
				}
				else
				{
					matrix.Fill(0f);
				}
				matrix.rect.offset = newOffset;
				break;
			}
			RefreshPreview();
			RefreshGizmos();
		}
	}
}
