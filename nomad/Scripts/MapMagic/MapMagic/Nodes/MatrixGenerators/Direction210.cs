using System;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Modifiers", name = "Direction", iconName = "GeneratorIcons/Direction", disengageable = true, helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/Direction")]
	public class Direction210 : Generator, IInlet<MatrixWorld>, IUnit, IOutlet<MatrixWorld>
	{
		[Val("Hor Angle", min = -360f, max = 360f)]
		public float horAngle;

		[Val("Vert Angle", min = -89.99f, max = 89.99f)]
		public float vertAngle;

		[Val("Intensity", min = 0f)]
		public float intensity = 1f;

		[Val("Wrapping", min = -1f, max = 1f)]
		public float wrapping;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\MatrixModifiers.cs", 704);
		}

		public override void Generate(TileData data, StopToken stop)
		{
			MatrixWorld matrixWorld = data.ReadInletProduct(this);
			if (matrixWorld == null)
			{
				return;
			}
			if (!enabled)
			{
				data.StoreProduct(this, matrixWorld);
				return;
			}
			Vector3 normalized = new Vector3(Mathf.Sin(horAngle * ((float)Math.PI / 180f)), Mathf.Tan(vertAngle * ((float)Math.PI / 180f)), Mathf.Cos(horAngle * ((float)Math.PI / 180f))).normalized;
			MatrixWorld matrixWorld2 = new MatrixWorld(matrixWorld.rect, matrixWorld.worldPos, matrixWorld.worldSize);
			if (stop == null || !stop.stop)
			{
				MatrixOps.NormalsDir(matrixWorld, matrixWorld2, normalized, data.area.PixelSize.x, data.globals.height, intensity, wrapping);
				if (stop == null || !stop.stop)
				{
					data.StoreProduct(this, matrixWorld2);
				}
			}
		}
	}
}
