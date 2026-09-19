using UnityEngine;

namespace Obi
{
	[AddComponentMenu("Physics/Obi/Obi Foam Emitter", 1000)]
	[ExecuteInEditMode]
	[RequireComponent(typeof(ObiActor))]
	[DisallowMultipleComponent]
	public class ObiFoamEmitter : ObiFoamGenerator
	{
		public enum ShapeType
		{
			Cylinder = 0,
			Box = 1
		}

		[Header("Emission shape")]
		public ShapeType shape;

		public Transform shapeTransform;

		public Vector3 shapeSize = Vector3.one;

		private float emissionAccumulator;

		public int GetParticleNumberToEmit(float deltaTime)
		{
			emissionAccumulator += foamGenerationRate * deltaTime;
			int num = (int)emissionAccumulator;
			emissionAccumulator -= num;
			return num;
		}

		public void Reset()
		{
			emissionAccumulator = 0f;
		}
	}
}
