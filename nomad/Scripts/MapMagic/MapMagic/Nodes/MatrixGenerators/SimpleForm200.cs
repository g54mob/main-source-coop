using System;
using Den.Tools;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Initial", name = "Simple Form", iconName = "GeneratorIcons/SimpleForm", disengageable = true, helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/SimpleForm")]
	public class SimpleForm200 : Generator, IOutlet<MatrixWorld>, IUnit, ISceneGizmo
	{
		public enum FormType
		{
			GradientX = 0,
			GradientZ = 1,
			Pyramid = 2,
			Cone = 3
		}

		[Val("Type")]
		public FormType type = FormType.Cone;

		[Val("Intensity")]
		public float intensity = 1f;

		[Val("Scale")]
		public float scale = 1f;

		[Val("Ratio")]
		public float ratio = 1f;

		[Val("Offset")]
		public Vector2 offset;

		[Val("Wrap")]
		public CoordRect.TileMode wrap = CoordRect.TileMode.Tile;

		public bool drawOffsetScaleGizmo;

		public bool hideDefaultToolGizmo { get; set; }

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\MatrixInitial.cs", 192);
		}

		public override void Generate(TileData data, StopToken stop)
		{
			MatrixWorld matrixWorld = new MatrixWorld(data.area.full.rect, data.area.full.worldPos, data.area.full.worldSize, data.globals.height);
			Vector2D vector2D = ((ratio < 1f) ? new Vector2D(scale * ratio, scale) : new Vector2D(scale, scale * (2f - ratio)));
			SimpleForm(matrixWorld, (Vector2D)offset, vector2D * data.area.active.worldSize, stop);
			matrixWorld.Clamp01();
			data.StoreProduct(this, matrixWorld);
		}

		public void SimpleForm(MatrixWorld matrix, Vector2D formOffset, Vector2D formSize, StopToken stop = null)
		{
			Vector2D vector2D = formSize / 2f + formOffset;
			Coord min = matrix.rect.Min;
			Coord max = matrix.rect.Max;
			for (int i = min.x; i < max.x; i++)
			{
				if (stop != null && stop.stop)
				{
					break;
				}
				for (int j = min.z; j < max.z; j++)
				{
					Vector2D vector2D2 = new Vector2D((float)(i - matrix.rect.offset.x) / (float)(matrix.rect.size.x - 1), (float)(j - matrix.rect.offset.z) / (float)(matrix.rect.size.z - 1));
					Vector2D pos = new Vector2D(vector2D2.x * matrix.worldSize.x + matrix.worldPos.x, vector2D2.z * matrix.worldSize.z + matrix.worldPos.z);
					Vector2D vector2D3 = Tile(pos, formOffset, formSize, wrap);
					float num = 0f;
					switch (type)
					{
					case FormType.GradientX:
						num = (vector2D3.x - formOffset.x) / formSize.x;
						break;
					case FormType.GradientZ:
						num = (vector2D3.z - formOffset.z) / formSize.z;
						break;
					case FormType.Pyramid:
					{
						float num2 = (vector2D3.x - formOffset.x) / formSize.x;
						if (num2 > 1f - num2)
						{
							num2 = 1f - num2;
						}
						float num3 = (vector2D3.z - formOffset.z) / formSize.z;
						if (num3 > 1f - num3)
						{
							num3 = 1f - num3;
						}
						num = ((num2 < num3) ? (num2 * 2f) : (num3 * 2f));
						break;
					}
					case FormType.Cone:
					{
						float magnitude = (2f * (vector2D - vector2D3) / formSize).Magnitude;
						num = 1f - magnitude;
						if (num < 0f)
						{
							num = 0f;
						}
						break;
					}
					}
					matrix[i, j] = num * intensity;
				}
			}
		}

		public Vector2D Tile(Vector2D pos, Vector2D rectOffset, Vector2D rectSize, CoordRect.TileMode tileMode)
		{
			pos.x -= rectOffset.x;
			pos.z -= rectOffset.z;
			switch (tileMode)
			{
			case CoordRect.TileMode.Clamp:
				if (pos.x < 0f)
				{
					pos.x = 0f;
				}
				if (pos.x >= rectSize.x)
				{
					pos.x = rectSize.x - 1f;
				}
				if (pos.z < 0f)
				{
					pos.z = 0f;
				}
				if (pos.z >= rectSize.z)
				{
					pos.z = rectSize.z - 1f;
				}
				break;
			case CoordRect.TileMode.Tile:
				pos.x %= rectSize.x;
				if (pos.x < 0f)
				{
					pos.x = rectSize.x + pos.x;
				}
				pos.z %= rectSize.z;
				if (pos.z < 0f)
				{
					pos.z = rectSize.z + pos.z;
				}
				break;
			case CoordRect.TileMode.PingPong:
				pos.x %= rectSize.x * 2f;
				if (pos.x < 0f)
				{
					pos.x = rectSize.x * 2f + pos.x;
				}
				if (pos.x >= rectSize.x)
				{
					pos.x = rectSize.x * 2f - pos.x - 1f;
				}
				pos.z %= rectSize.z * 2f;
				if (pos.z < 0f)
				{
					pos.z = rectSize.z * 2f + pos.z;
				}
				if (pos.z >= rectSize.z)
				{
					pos.z = rectSize.z * 2f - pos.z - 1f;
				}
				break;
			}
			pos.x += rectOffset.x;
			pos.z += rectOffset.z;
			return pos;
		}

		public void DrawGizmo()
		{
		}
	}
}
