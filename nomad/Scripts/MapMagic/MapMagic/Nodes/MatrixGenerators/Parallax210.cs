using System;
using System.Collections.Generic;
using Den.Tools;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Products;

namespace MapMagic.Nodes.MatrixGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Map/Modifiers", name = "Parallax", section = 2, colorType = typeof(MatrixWorld), iconName = "GeneratorIcons/Parallax", helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/Parallax")]
	public class Parallax210 : Generator, IInlet<MatrixWorld>, IUnit, IMultiInlet, IOutlet<MatrixWorld>
	{
		public enum Interpolation
		{
			None = 0,
			Always = 1,
			OnTransitions = 2
		}

		[Val("Intensity X", "Inlet")]
		public readonly Inlet<MatrixWorld> intensityInX = new Inlet<MatrixWorld>();

		[Val("Intensity Z", "Inlet")]
		public readonly Inlet<MatrixWorld> intensityInZ = new Inlet<MatrixWorld>();

		[Val("Offset")]
		public Vector2D offset;

		[Val("Interpolation")]
		public Interpolation interpolation = Interpolation.Always;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Matrix\\Runtime\\MatrixModifiers.cs", 1257);
		}

		public virtual IEnumerable<IInlet<object>> Inlets()
		{
			yield return intensityInX;
			yield return intensityInZ;
		}

		public override void Generate(TileData data, StopToken stop)
		{
			if (stop != null && stop.stop)
			{
				return;
			}
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
			MatrixWorld maskX = data.ReadInletProduct(intensityInX);
			MatrixWorld maskZ = data.ReadInletProduct(intensityInZ);
			if (stop == null || !stop.stop)
			{
				Vector2D vector2D = offset / matrixWorld.PixelSize;
				MatrixWorld matrixWorld2 = new MatrixWorld(matrixWorld);
				matrixWorld2.Parallax(vector2D, matrixWorld, maskX, maskZ, (int)interpolation);
				if (stop == null || !stop.stop)
				{
					data.StoreProduct(this, matrixWorld2);
				}
			}
		}
	}
}
