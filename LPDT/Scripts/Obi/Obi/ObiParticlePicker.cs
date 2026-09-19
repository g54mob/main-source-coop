using System;
using UnityEngine;
using UnityEngine.Events;

namespace Obi
{
	public class ObiParticlePicker : MonoBehaviour
	{
		public class ParticlePickEventArgs : EventArgs
		{
			public int particleIndex;

			public Vector3 worldPosition;

			public ParticlePickEventArgs(int particleIndex, Vector3 worldPosition)
			{
				this.particleIndex = particleIndex;
				this.worldPosition = worldPosition;
			}
		}

		[Serializable]
		public class ParticlePickUnityEvent : UnityEvent<ParticlePickEventArgs>
		{
		}

		public ObiSolver solver;

		public float radiusScale = 1f;

		public ParticlePickUnityEvent OnParticlePicked;

		public ParticlePickUnityEvent OnParticleHeld;

		public ParticlePickUnityEvent OnParticleDragged;

		public ParticlePickUnityEvent OnParticleReleased;

		private Vector3 lastMousePos = Vector3.zero;

		private int pickedParticleIndex = -1;

		private float pickedParticleDepth;

		private void Awake()
		{
			lastMousePos = Input.mousePosition;
		}

		private void LateUpdate()
		{
			if (solver != null)
			{
				if (Input.GetMouseButtonDown(0))
				{
					pickedParticleIndex = -1;
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					float num = float.MaxValue;
					float num2 = float.MaxValue;
					Matrix4x4 localToWorldMatrix = solver.transform.localToWorldMatrix;
					for (int i = 0; i < solver.positions.count; i++)
					{
						Vector3 vector = localToWorldMatrix.MultiplyPoint3x4(solver.positions[i]);
						float mu;
						Vector3 vector2 = ObiUtils.ProjectPointLine(ray.origin, ray.origin + ray.direction, vector, out mu, clampToSegment: false);
						float num3 = Vector3.SqrMagnitude(vector - vector2);
						mu = Mathf.Max(0f, mu);
						float num4 = solver.principalRadii[i][0] * radiusScale;
						if (num3 <= num4 * num4 && num3 < num2 && mu < num)
						{
							num = mu;
							num2 = num3;
							pickedParticleIndex = i;
						}
					}
					if (pickedParticleIndex >= 0)
					{
						pickedParticleDepth = Camera.main.transform.InverseTransformVector(localToWorldMatrix.MultiplyPoint3x4(solver.positions[pickedParticleIndex]) - Camera.main.transform.position).z;
						if (OnParticlePicked != null)
						{
							Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, pickedParticleDepth));
							OnParticlePicked.Invoke(new ParticlePickEventArgs(pickedParticleIndex, worldPosition));
						}
					}
				}
				else if (pickedParticleIndex >= 0)
				{
					if ((Input.mousePosition - lastMousePos).magnitude > 0.01f && OnParticleDragged != null)
					{
						Vector3 worldPosition2 = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, pickedParticleDepth));
						OnParticleDragged.Invoke(new ParticlePickEventArgs(pickedParticleIndex, worldPosition2));
					}
					else if (OnParticleHeld != null)
					{
						Vector3 worldPosition3 = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, pickedParticleDepth));
						OnParticleHeld.Invoke(new ParticlePickEventArgs(pickedParticleIndex, worldPosition3));
					}
					if (Input.GetMouseButtonUp(0))
					{
						if (OnParticleReleased != null)
						{
							Vector3 worldPosition4 = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, pickedParticleDepth));
							OnParticleReleased.Invoke(new ParticlePickEventArgs(pickedParticleIndex, worldPosition4));
						}
						pickedParticleIndex = -1;
					}
				}
			}
			lastMousePos = Input.mousePosition;
		}
	}
}
