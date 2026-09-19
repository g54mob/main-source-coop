namespace Obi
{
	public class NullBackend : IObiBackend
	{
		public ISolverImpl CreateSolver(ObiSolver solver, int capacity)
		{
			return new NullSolverImpl();
		}

		public void DestroySolver(ISolverImpl solver)
		{
		}
	}
}
