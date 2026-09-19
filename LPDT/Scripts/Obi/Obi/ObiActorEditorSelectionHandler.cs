using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	[ExecuteAlways]
	public class ObiActorEditorSelectionHandler
	{
		private static HashSet<ObiSolver> solvers = new HashSet<ObiSolver>();

		private static ObiSolver clickedSolver;

		private static int particleIndex;
	}
}
