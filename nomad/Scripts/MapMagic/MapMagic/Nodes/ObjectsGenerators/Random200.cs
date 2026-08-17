using System;
using Den.Tools;
using Den.Tools.Matrices;

namespace MapMagic.Nodes.ObjectsGenerators
{
	[Serializable]
	[GeneratorMenu(menu = null, name = "Random", iconName = "GeneratorIcons/Random", disengageable = true, colorType = typeof(TransitionsList), helpLink = "https://gitlab.com/denispahunov/mapmagic/-/wikis/ObjectsGenerators/Random")]
	public class Random200 : Random207, IInlet<MatrixWorld>, IUnit
	{
	}
}
