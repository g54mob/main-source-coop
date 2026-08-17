using System;
using Den.Tools;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Core;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Initial", name = "Import", iconName = "GeneratorIcons/Import", disengageable = true, disabled = false, helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/Import")]
	public class Import200 : Generator, IOutlet<MatrixWorld>, IUnit, ISceneGizmo
	{
		[Val("Map", priority = 10, type = typeof(MatrixAsset))]
		public MatrixAsset matrixAsset;

		[Val("Wrap Mode", priority = 4)]
		public CoordRect.TileMode wrapMode;

		[Val("Scale", priority = 3)]
		public float scale = 1f;

		[Val("Offset", priority = 2)]
		public Vector2 offset;

		public bool drawOffsetScaleGizmo;

		public bool hideDefaultToolGizmo { get; set; }

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\MatrixInitial.cs", 327);
		}

		static Import200()
		{
			MatrixAsset.OnReloaded = (Action<MatrixAsset>)Delegate.Combine(MatrixAsset.OnReloaded, new Action<MatrixAsset>(OnMatrixAssetReloaded_ReGenerate));
		}

		public static void OnMatrixAssetReloaded_ReGenerate(MatrixAsset ma)
		{
			MapMagicObject[] array = UnityEngine.Object.FindObjectsOfType<MapMagicObject>();
			foreach (MapMagicObject mapMagicObject in array)
			{
				bool flag = false;
				if (mapMagicObject.graph != null)
				{
					foreach (Import200 item in mapMagicObject.graph.GeneratorsOfType<Import200>())
					{
						if (item.matrixAsset == ma)
						{
							item.version++;
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					mapMagicObject.Refresh();
				}
			}
		}

		public override void Generate(TileData data, StopToken stop)
		{
			if (stop != null && stop.stop)
			{
				return;
			}
			if (matrixAsset == null || matrixAsset.matrix == null || matrixAsset.matrix.rect.Count == 0 || !enabled)
			{
				data.RemoveProduct(this);
				return;
			}
			MatrixWorld matrixWorld = new MatrixWorld(data.area.full.rect, data.area.full.worldPos, data.area.full.worldSize, data.globals.height);
			MatrixWorld matrixWorld2 = new MatrixWorld(matrixAsset.matrix, (Vector2D)offset, data.area.active.worldSize * scale, data.globals.height);
			if (matrixWorld2.rect.size == data.area.active.rect.size && scale < 1.0001f && scale > 0.999f)
			{
				Matrix.ReadMatrix(matrixWorld2, matrixWorld, wrapMode);
			}
			else if (matrixWorld2.PixelSize.x >= matrixWorld.PixelSize.x)
			{
				ImportWithEnlarge(matrixWorld2, matrixWorld, wrapMode, stop, compatibility: true);
			}
			else
			{
				ImportWithDownscale(matrixWorld2, matrixWorld, wrapMode, stop);
			}
			data.StoreProduct(this, matrixWorld);
		}

		public void ImportWorldInerpolated(MatrixWorld src, MatrixWorld dst, StopToken stop)
		{
			Coord min = dst.rect.Min;
			Coord max = dst.rect.Max;
			for (int i = min.x; i < max.x; i++)
			{
				for (int j = min.z; j < max.z; j++)
				{
					Vector3 vector = dst.PixelToWorld(i, j);
					Coord coord = src.WorldToPixel(vector.x, vector.z);
					if (src.rect.Contains(coord))
					{
						dst[i, j] = src.GetWorldInterpolatedValue(vector.x, vector.z);
					}
				}
			}
		}

		public static void ImportWithEnlarge(MatrixWorld src, MatrixWorld dst, CoordRect.TileMode wrapMode, StopToken stop, bool compatibility = false)
		{
			Vector2D vector2D = (Vector2D)dst.worldSize / (Vector2D)src.worldSize;
			Vector2D vector2D2 = (Vector2D)dst.rect.size / (Vector2D)src.rect.size;
			Vector2D vector2D3 = vector2D / vector2D2;
			Vector2D vector2D4 = (Vector2D)dst.rect.offset * vector2D3;
			if (compatibility)
			{
				vector2D4 -= (Vector2D)src.worldPos / src.PixelSize;
			}
			Vector2D vector2D5 = (Vector2D)dst.rect.size * vector2D3;
			int num = (int)(0.5f / vector2D3.x);
			CoordRect rect = new CoordRect(Coord.Floor(vector2D4) - num, (Coord)vector2D5 + num * 2);
			if (stop == null || !stop.stop)
			{
				Matrix matrix = new Matrix(rect);
				Matrix.ReadMatrix(src, matrix, wrapMode);
				if (stop == null || !stop.stop)
				{
					MatrixOps.Upsize(matrix, vector2D4, vector2D5, dst);
				}
			}
		}

		public static void ImportWithDownscale(MatrixWorld src, MatrixWorld dst, CoordRect.TileMode wrapMode, StopToken stop)
		{
			Matrix matrix = new Matrix(dst.WorldRectToPixels((Vector2D)src.worldPos, (Vector2D)src.worldSize));
			if (stop == null || !stop.stop)
			{
				MatrixOps.Downsize(src, matrix);
				if (stop == null || !stop.stop)
				{
					Matrix.ReadMatrix(matrix, dst, wrapMode);
				}
			}
		}

		public void DrawGizmo()
		{
		}
	}
}
