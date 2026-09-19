namespace Obi
{
	public class BurstBackend : IObiBackend
	{
		public ISolverImpl CreateSolver(ObiSolver solver, int capacity)
		{
			return new BurstSolverImpl(solver);
		}

		public void DestroySolver(ISolverImpl solver)
		{
			solver?.Destroy();
		}
	}
}
