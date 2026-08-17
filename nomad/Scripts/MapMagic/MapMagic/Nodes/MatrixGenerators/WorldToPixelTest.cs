using System;
using Den.Tools;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	public class WorldToPixelTest : Generator, IOutlet<MatrixWorld>, IUnit, IPrepare
	{
		[Val("Level")]
		public float level;

		[Val("Grid")]
		public float grid;

		[Val("Transform", allowSceneObject = true)]
		public Transform tfm;

		[Val("Interpolated")]
		public bool interpolated;

		private Vector3 pos;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\MatrixInitial.cs", 518);
		}

		public void Prepare(TileData data, Terrain terrain)
		{
			pos = tfm.position;
		}

		public override void Generate(TileData data, StopToken stop)
		{
			MatrixWorld matrixWorld = new MatrixWorld(data.area.full.rect, data.area.full.worldPos, data.area.full.worldSize, data.globals.height);
			for (int i = matrixWorld.rect.offset.x; i < matrixWorld.rect.offset.x + matrixWorld.rect.size.x; i++)
			{
				for (int j = matrixWorld.rect.offset.z; j < matrixWorld.rect.offset.z + matrixWorld.rect.size.z; j++)
				{
					matrixWorld[i, j] = grid * (float)(i % 2) * (float)(j % 2);
				}
			}
			if (interpolated)
			{
				Vector3 vector = matrixWorld.WorldToPixelInterpolated(pos.x, pos.z);
				Coord c = Coord.Floor((Vector2D)vector);
				matrixWorld[c] = level;
				matrixWorld[c.x + 1, c.z] = level * 0.75f;
				matrixWorld[c.x, c.z + 1] = level * 0.5f;
				matrixWorld[c.x + 1, c.z + 1] = level * 0.25f;
				Vector3 vector2 = matrixWorld.PixelToWorld(vector.x, vector.z);
				DebugGizmos.DrawDot("WorldToPixelTest", vector2, 6f, Color.green);
			}
			else
			{
				Coord c2 = matrixWorld.WorldToPixel(pos.x, pos.z);
				matrixWorld[c2] = level;
				Vector3 vector3 = matrixWorld.PixelToWorld(c2.x, c2.z);
				DebugGizmos.DrawDot("WorldToPixelTest", vector3, 6f, Color.green);
			}
			data.StoreProduct(this, matrixWorld);
		}
	}
}
