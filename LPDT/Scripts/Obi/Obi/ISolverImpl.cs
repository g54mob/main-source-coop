using UnityEngine;

namespace Obi
{
	public interface ISolverImpl
	{
		uint activeFoamParticleCount { get; }

		void Destroy();

		void InitializeFrame(Vector4 translation, Vector4 scale, Quaternion rotation);

		void UpdateFrame(Vector4 translation, Vector4 scale, Quaternion rotation, float deltaTime);

		IObiJobHandle ApplyFrame(float worldLinearInertiaScale, float worldAngularInertiaScale, float deltaTime);

		void ParticleCountChanged(ObiSolver solver);

		void MaxFoamParticleCountChanged(ObiSolver solver);

		void SetActiveParticles(ObiNativeIntList indices);

		void SetRigidbodyArrays(ObiSolver solver);

		IConstraintsBatchImpl CreateConstraintsBatch(Oni.ConstraintType type);

		void DestroyConstraintsBatch(IConstraintsBatchImpl batch);

		int GetConstraintCount(Oni.ConstraintType type);

		void SetConstraintGroupParameters(Oni.ConstraintType type, ref Oni.ConstraintParameters parameters);

		IObiJobHandle UpdateBounds(IObiJobHandle inputDeps, float stepTime);

		IObiJobHandle CollisionDetection(IObiJobHandle inputDeps, float stepTime);

		IObiJobHandle Substep(IObiJobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft);

		IObiJobHandle ApplyInterpolation(IObiJobHandle inputDeps, ObiNativeVector4List startPositions, ObiNativeQuaternionList startOrientations, float stepTime, float unsimulatedTime);

		void FinishSimulation();

		void PushData();

		void RequestReadback();

		void SetDeformableTriangles(ObiNativeIntList indices, ObiNativeVector2List uvs);

		void SetDeformableEdges(ObiNativeIntList indices);

		void SetSimplices(ObiNativeIntList simplices, SimplexCounts counts);

		void SetParameters(Oni.SolverParameters parameters);

		void GetBounds(ref Vector3 min, ref Vector3 max);

		int GetParticleGridSize();

		void GetParticleGrid(ObiNativeAabbList cells);

		void SpatialQuery(ObiNativeQueryShapeList shapes, ObiNativeAffineTransformList transforms, ObiNativeQueryResultList results);
	}
}
