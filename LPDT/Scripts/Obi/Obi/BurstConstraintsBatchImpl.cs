using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	public abstract class BurstConstraintsBatchImpl : IConstraintsBatchImpl
	{
		[BurstCompile]
		public struct ClearLambdasJob : IJobParallelFor
		{
			public NativeArray<float> lambdas;

			public void Execute(int i)
			{
				lambdas[i] = 0f;
			}
		}

		protected IBurstConstraintsImpl m_Constraints;

		protected Oni.ConstraintType m_ConstraintType;

		protected bool m_Enabled = true;

		protected int m_ConstraintCount;

		protected NativeArray<int> particleIndices;

		protected NativeArray<float> lambdas;

		public Oni.ConstraintType constraintType => m_ConstraintType;

		public bool enabled
		{
			get
			{
				return m_Enabled;
			}
			set
			{
				m_Enabled = value;
			}
		}

		public IConstraints constraints => m_Constraints;

		public ObiSolver solverAbstraction => ((BurstSolverImpl)m_Constraints.solver).abstraction;

		public BurstSolverImpl solverImplementation => (BurstSolverImpl)m_Constraints.solver;

		public virtual JobHandle Initialize(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			if (lambdas.IsCreated)
			{
				inputDeps = IJobParallelForExtensions.Schedule(new ClearLambdasJob
				{
					lambdas = lambdas
				}, lambdas.Length, 256, inputDeps);
			}
			return inputDeps;
		}

		public abstract JobHandle Evaluate(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft);

		public abstract JobHandle Apply(JobHandle inputDeps, float substepTime);

		public virtual void Destroy()
		{
		}

		public void SetConstraintCount(int constraintCount)
		{
			m_ConstraintCount = constraintCount;
		}

		public int GetConstraintCount()
		{
			return m_ConstraintCount;
		}

		public static void ApplyPositionDelta(int particleIndex, float sorFactor, ref NativeArray<float4> positions, ref NativeArray<float4> deltas, ref NativeArray<int> counts)
		{
			if (counts[particleIndex] > 0)
			{
				positions[particleIndex] += deltas[particleIndex] * sorFactor / counts[particleIndex];
				deltas[particleIndex] = float4.zero;
				counts[particleIndex] = 0;
			}
		}

		public static void ApplyOrientationDelta(int particleIndex, float sorFactor, ref NativeArray<quaternion> orientations, ref NativeArray<quaternion> deltas, ref NativeArray<int> counts)
		{
			if (counts[particleIndex] > 0)
			{
				quaternion q = orientations[particleIndex];
				q.value += deltas[particleIndex].value * sorFactor / counts[particleIndex];
				orientations[particleIndex] = math.normalize(q);
				deltas[particleIndex] = new quaternion(0f, 0f, 0f, 0f);
				counts[particleIndex] = 0;
			}
		}
	}
}
