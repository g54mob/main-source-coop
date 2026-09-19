namespace Obi
{
	public class ComputeBackend : IObiBackend
	{
		public ISolverImpl CreateSolver(ObiSolver solver, int capacity)
		{
			return new ComputeSolverImpl(solver);
		}

		public void DestroySolver(ISolverImpl solver)
		{
			solver?.Destroy();
		}
	}
}
