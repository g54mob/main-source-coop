using System;
using Unity.Mathematics;

namespace Obi
{
	public struct BurstContact : IConstraint, IComparable<BurstContact>
	{
		public float4 pointA;

		public float4 pointB;

		public float4 normal;

		public float4 tangent;

		public float distance;

		public float normalLambda;

		public float tangentLambda;

		public float bitangentLambda;

		public float stickLambda;

		public float rollingFrictionImpulse;

		public int bodyA;

		public int bodyB;

		public float4 bitangent => math.normalizesafe(new float4(math.cross(normal.xyz, tangent.xyz), 0f));

		public int GetParticleCount()
		{
			return 2;
		}

		public int GetParticle(int index)
		{
			if (index != 0)
			{
				return bodyB;
			}
			return bodyA;
		}

		public override string ToString()
		{
			return bodyA + "," + bodyB;
		}

		public int CompareTo(BurstContact other)
		{
			int num = bodyA.CompareTo(other.bodyA);
			if (num == 0)
			{
				return bodyB.CompareTo(other.bodyB);
			}
			return num;
		}

		public void CalculateTangent(float4 relativeVelocity)
		{
			tangent = math.normalizesafe(relativeVelocity - math.dot(relativeVelocity, normal) * normal);
		}

		public float SolveAdhesion(float normalMass, float4 posA, float4 posB, float stickDistance, float stickiness, float dt)
		{
			if (normalMass <= 0f || stickDistance <= 0f || stickiness <= 0f || dt <= 0f)
			{
				return 0f;
			}
			distance = math.dot(posA - posB, normal);
			float num = (0f - stickiness * (1f - math.max(distance / stickDistance, 0f)) * dt) / normalMass;
			float num2 = math.min(stickLambda + num, 0f);
			float result = num2 - stickLambda;
			stickLambda = num2;
			return result;
		}

		public float SolvePenetration(float normalMass, float4 posA, float4 posB, float maxDepenetrationDelta)
		{
			if (normalMass <= 0f)
			{
				return 0f;
			}
			distance = math.dot(posA - posB, normal);
			float num = math.max(0f - distance - maxDepenetrationDelta, 0f);
			float num2 = (0f - (distance + num)) / normalMass;
			float num3 = math.max(normalLambda + num2, 0f);
			float result = num3 - normalLambda;
			normalLambda = num3;
			return result;
		}

		public float2 SolveFriction(float tangentMass, float bitangentMass, float4 relativeVelocity, float staticFriction, float dynamicFriction, float dt)
		{
			float2 zero = float2.zero;
			if (tangentMass <= 0f || bitangentMass <= 0f || (dynamicFriction <= 0f && staticFriction <= 0f) || (normalLambda <= 0f && stickLambda <= 0f))
			{
				return zero;
			}
			float num = math.dot(relativeVelocity, tangent);
			float num2 = math.dot(relativeVelocity, bitangent);
			float num3 = normalLambda / dt * dynamicFriction;
			float num4 = normalLambda / dt * staticFriction;
			float num5 = (0f - num) / tangentMass;
			float num6 = tangentLambda + num5;
			if (math.abs(num6) > num4)
			{
				num6 = math.clamp(num6, 0f - num3, num3);
			}
			zero[0] = num6 - tangentLambda;
			tangentLambda = num6;
			float num7 = (0f - num2) / bitangentMass;
			float num8 = bitangentLambda + num7;
			if (math.abs(num8) > num4)
			{
				num8 = math.clamp(num8, 0f - num3, num3);
			}
			zero[1] = num8 - bitangentLambda;
			bitangentLambda = num8;
			return zero;
		}

		public float SolveRollingFriction(float4 angularVelocityA, float4 angularVelocityB, float rollingFriction, float invMassA, float invMassB, ref float4 rolling_axis)
		{
			float num = invMassA + invMassB;
			if (num <= 0f)
			{
				return 0f;
			}
			rolling_axis = math.normalizesafe(angularVelocityA - angularVelocityB);
			float num2 = math.dot(angularVelocityA, rolling_axis);
			float num3 = math.dot(angularVelocityB, rolling_axis);
			float num4 = num2 - num3;
			float num5 = normalLambda * rollingFriction;
			float num6 = math.clamp(rollingFrictionImpulse - num4 / num, 0f - num5, num5);
			float result = num6 - rollingFrictionImpulse;
			rollingFrictionImpulse = num6;
			return result;
		}
	}
}
