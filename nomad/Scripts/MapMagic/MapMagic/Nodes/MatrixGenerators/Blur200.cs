using System;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Modifiers", name = "Blur", iconName = "GeneratorIcons/Blur", disengageable = true, helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/Blur")]
	public class Blur200 : Generator, IInlet<MatrixWorld>, IUnit, IOutlet<MatrixWorld>
	{
		[Val("Downsample")]
		public float downsample = 10f;

		[Val("Blur")]
		public float blur = 3f;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\MatrixModifiers.cs", 420);
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
			MatrixWorld matrixWorld2 = new MatrixWorld(matrixWorld);
			int num = (int)(downsample / Mathf.Sqrt(matrixWorld2.PixelSize.x));
			float num2 = blur / matrixWorld2.PixelSize.x;
			if (num > 1)
			{
				MatrixOps.DownsampleBlur(matrixWorld, matrixWorld2, num, num2);
			}
			else
			{
				MatrixOps.GaussianBlur(matrixWorld, matrixWorld2, num2);
			}
			data.StoreProduct(this, matrixWorld2);
		}
	}
}
