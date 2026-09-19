using UnityEngine;

namespace Obi
{
	[RequireComponent(typeof(LineRenderer))]
	[RequireComponent(typeof(ObiParticlePicker))]
	public class ObiParticleDragger : MonoBehaviour
	{
		public float springStiffness = 500f;

		public float springDamping = 50f;

		public bool drawSpring = true;

		private LineRenderer lineRenderer;

		private ObiParticlePicker picker;

		private ObiParticlePicker.ParticlePickEventArgs pickArgs;

		private void OnEnable()
		{
			lineRenderer = GetComponent<LineRenderer>();
			picker = GetComponent<ObiParticlePicker>();
			picker.OnParticlePicked.AddListener(Picker_OnParticleDragged);
			picker.OnParticleDragged.AddListener(Picker_OnParticleDragged);
			picker.OnParticleReleased.AddListener(Picker_OnParticleReleased);
			picker.solver.OnSimulationStart += Solver_OnEndSimulation;
		}

		private void OnDisable()
		{
			picker.solver.OnSimulationStart -= Solver_OnEndSimulation;
			picker.OnParticlePicked.RemoveListener(Picker_OnParticleDragged);
			picker.OnParticleDragged.RemoveListener(Picker_OnParticleDragged);
			picker.OnParticleReleased.RemoveListener(Picker_OnParticleReleased);
			lineRenderer.positionCount = 0;
		}

		private void Solver_OnEndSimulation(ObiSolver solver, float timeToSimulate, float substepTime)
		{
			if (!(solver != null) || pickArgs == null)
			{
				return;
			}
			Vector4 vector = solver.transform.InverseTransformPoint(pickArgs.worldPosition);
			float num = solver.invMasses[pickArgs.particleIndex];
			if (num > 0f)
			{
				Vector4 vector2 = solver.positions[pickArgs.particleIndex];
				Vector4 vector3 = solver.velocities[pickArgs.particleIndex];
				solver.externalForces[pickArgs.particleIndex] = ((vector - vector2) * springStiffness - vector3 * springDamping) / num;
				if (drawSpring)
				{
					lineRenderer.positionCount = 2;
					lineRenderer.SetPosition(0, pickArgs.worldPosition);
					lineRenderer.SetPosition(1, solver.transform.TransformPoint(vector2));
				}
				else
				{
					lineRenderer.positionCount = 0;
				}
			}
		}

		private void Picker_OnParticleDragged(ObiParticlePicker.ParticlePickEventArgs e)
		{
			pickArgs = e;
		}

		private void Picker_OnParticleReleased(ObiParticlePicker.ParticlePickEventArgs e)
		{
			pickArgs = null;
			lineRenderer.positionCount = 0;
		}
	}
}
