using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Jobs;

namespace pworld.Scripts.PPhys.Bursted.Jobs
{
	[BurstCompile]
	public struct SpringJobPosition : IJobParallelForTransform
	{
		[Serializable]
		public struct SpringData
		{
			public float damp;

			public float spring;
		}

		[NativeDisableUnsafePtrRestriction]
		private TransformAccess transform;

		[NonSerialized]
		public NativeArray<Vector3> targets;

		[NonSerialized]
		[ReadOnly]
		public NativeArray<SpringData> springDatas;

		[NonSerialized]
		public NativeArray<Vector3> velocities;

		[NonSerialized]
		public float dt;

		private int index;

		public Vector3 Velocity
		{
			get
			{
				return velocities[index];
			}
			set
			{
				velocities[index] = value;
			}
		}

		public Vector3 Target => targets[index];

		public float Damp => springDatas[index].damp;

		public float Spring => springDatas[index].spring;

		public void Execute(int _index, TransformAccess _transform)
		{
			index = _index;
			transform = _transform;
			LagControll();
		}

		public void PhysicsStep(float _dt)
		{
			Velocity = FRILerp.PLerp(Velocity, (Target - transform.position) * Spring, Damp, _dt);
			transform.position += Velocity * _dt;
		}

		public void LagControll(float maxStepSize = 0.004f)
		{
			for (float num = dt / Mathf.Max(maxStepSize, 0.0025f); num > 0f; num -= 1f)
			{
				if (num > 1f)
				{
					PhysicsStep(maxStepSize);
				}
				else
				{
					PhysicsStep(maxStepSize * num);
				}
			}
		}
	}
}
