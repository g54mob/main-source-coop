using System;
using System.Collections.Generic;
using Den.Tools;
using Den.Tools.GUI;
using Den.Tools.Matrices;
using MapMagic.Products;

namespace MapMagic.Nodes.ObjectsGenerators
{
	[Serializable]
	[GeneratorMenu(menu = "Objects/Modifiers", name = "Move", section = 2, colorType = typeof(TransitionsList), iconName = "GeneratorIcons/Move", helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/MatrixGenerators/Move")]
	public class Move210 : Generator, IInlet<TransitionsList>, IUnit, IMultiInlet, IOutlet<TransitionsList>
	{
		[Val("Intensity", "Inlet")]
		public readonly Inlet<MatrixWorld> intensityIn = new Inlet<MatrixWorld>();

		[Val("Direction")]
		public Vector2D direction;

		public override (string, int) GetCodeFileLine()
		{
			return GetCodeFileLineBase("A:\\Nomad Drive Folder\\NomadDrive\\Assets\\Excluded\\ThirdParty\\MapMagic\\Generators\\Objects\\Runtime\\ObjectsModifiers.cs", 1326);
		}

		public IEnumerable<IInlet<object>> Inlets()
		{
			yield return intensityIn;
		}

		public override void Generate(TileData data, StopToken stop)
		{
			if (stop != null && stop.stop)
			{
				return;
			}
			TransitionsList transitionsList = data.ReadInletProduct(this);
			if (transitionsList == null)
			{
				return;
			}
			MatrixWorld matrixWorld = data.ReadInletProduct(intensityIn);
			if (!enabled || matrixWorld == null)
			{
				data.StoreProduct(this, transitionsList);
			}
			else
			{
				if (stop != null && stop.stop)
				{
					return;
				}
				TransitionsList transitionsList2 = new TransitionsList(transitionsList);
				for (int i = 0; i < transitionsList2.count; i++)
				{
					Transition transition = transitionsList.arr[i];
					if (!(transition.pos.x <= matrixWorld.worldPos.x) && !(transition.pos.x >= matrixWorld.worldPos.x + matrixWorld.worldSize.x) && !(transition.pos.z <= matrixWorld.worldPos.z) && !(transition.pos.z >= matrixWorld.worldPos.z + matrixWorld.worldSize.z))
					{
						float worldInterpolatedValue = matrixWorld.GetWorldInterpolatedValue(transition.pos.x, transition.pos.z);
						transitionsList2.arr[i].pos += direction * worldInterpolatedValue;
					}
				}
				if (stop == null || !stop.stop)
				{
					data.StoreProduct(this, transitionsList2);
				}
			}
		}
	}
}
