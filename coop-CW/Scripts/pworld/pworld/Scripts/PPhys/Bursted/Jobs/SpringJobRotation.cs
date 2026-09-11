using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Jobs;
using pworld.Scripts.Extensions;

namespace pworld.Scripts.PPhys.Bursted.Jobs
{
	[BurstCompile]
	public struct SpringJobRotation : IJobParallelForTransform
	{
		[Serializable]
		public struct SpringData
		{
			public float damp;

			public float spring;
		}

		public NativeArray<Quaternion> targets;

		[ReadOnly]
		public NativeArray<SpringData> springDatas;

		public NativeArray<Vector3> velocities;

		public float dt;

		[NativeDisableUnsafePtrRestriction]
		private TransformAccess transform;

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

		public Quaternion Target => targets[index];

		public Vector3 Up => transform.rotation * Vector3.up;

		public Vector3 Forward => transform.rotation * Vector3.forward;

		public float Damp => springDatas[index].damp;

		public float Spring => springDatas[index].spring;

		public Vector3 TargetUp => Target * Vector3.up;

		public Vector3 TargetForward => Target * Vector3.forward;

		public void Execute(int _index, TransformAccess _transform)
		{
			index = _index;
			transform = _transform;
			LagControll();
		}

		private void PhysicsStep(float dt)
		{
			Vector3 vector = Vector3.Cross(Up, TargetUp).normalized * Vector3.Angle(Up, TargetUp);
			Vector3 vector2 = Vector3.Cross(Forward, TargetForward).normalized * Vector3.Angle(Forward, TargetForward);
			Velocity = FRILerp.PLerp(Velocity, (vector2 + vector) * Spring, Damp, dt);
			Rotate(Velocity * dt);
		}

		public void Rotate(Vector3 dRot)
		{
			transform.Rotate(dRot, Space.World);
		}

		private void LagControll(float maxStepSize = 0.004f)
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
