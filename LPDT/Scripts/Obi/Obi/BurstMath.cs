using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Obi
{
	public static class BurstMath
	{
		public struct CachedTri
		{
			public float4 vertex;

			public float4 edge0;

			public float4 edge1;

			public float4 data;

			public void Cache(float4 v1, float4 v2, float4 v3)
			{
				vertex = v1;
				edge0 = v2 - v1;
				edge1 = v3 - v1;
				data = float4.zero;
				data[0] = math.dot(edge0, edge0);
				data[1] = math.dot(edge0, edge1);
				data[2] = math.dot(edge1, edge1);
				data[3] = data[0] * data[2] - data[1] * data[1];
			}
		}

		public const float epsilon = 1E-07f;

		public const float zero = 0f;

		public const float one = 1f;

		public static readonly float golden = (math.sqrt(5f) + 1f) / 2f;

		public unsafe static void AddRange<T, U>(this NativeList<T> dst, U[] array) where T : unmanaged where U : unmanaged
		{
			int num = sizeof(U);
			if (sizeof(T) == num)
			{
				int length = dst.Length;
				dst.ResizeUninitialized(dst.Length + array.Length);
				fixed (U* source = array)
				{
					UnsafeUtility.MemCpy(dst.GetUnsafePtr() + length, source, num * array.Length);
				}
			}
		}

		public unsafe static void AddReplicate<T, U>(this NativeList<T> dst, U value, int length) where T : unmanaged where U : unmanaged
		{
			int num = sizeof(T);
			int num2 = sizeof(U);
			if (num == num2)
			{
				int length2 = dst.Length;
				dst.ResizeUninitialized(dst.Length + length);
				T* destination = dst.GetUnsafePtr() + length2;
				void* source = UnsafeUtility.AddressOf(ref value);
				UnsafeUtility.MemCpyReplicate(destination, source, num, length);
			}
		}

		public static float AtomicAdd(ref float location, float value)
		{
			float num = location;
			float num2;
			float num3;
			do
			{
				num2 = num;
				num3 = num2 + value;
				num = Interlocked.CompareExchange(ref location, num3, num2);
			}
			while (!num.Equals(num2));
			return num3;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void AtomicAdd(NativeArray<float4> array, int p, float4 data)
		{
			float4* unsafePtr = (float4*)array.GetUnsafePtr();
			AtomicAdd(ref unsafePtr[p].x, data.x);
			AtomicAdd(ref unsafePtr[p].y, data.y);
			AtomicAdd(ref unsafePtr[p].z, data.z);
			AtomicAdd(ref unsafePtr[p].w, data.w);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void AtomicAdd(NativeArray<float> array, int p, float data)
		{
			float* unsafePtr = (float*)array.GetUnsafePtr();
			AtomicAdd(ref unsafePtr[p], data);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3x3 multrnsp(in float4 column, in float4 row)
		{
			return new float3x3(column.xyz * row[0], column.xyz * row[1], column.xyz * row[2]);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 multrnsp4(in float4 column, float4 row)
		{
			row[3] = 0f;
			return new float4x4(column * row[0], column * row[1], column * row[2], float4.zero);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 project(this float4 vector, float4 onto)
		{
			float num = math.lengthsq(onto);
			if (num < 1E-07f)
			{
				return float4.zero;
			}
			return math.dot(onto, vector) * onto / num;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 project(this float3 vector, float3 onto)
		{
			float num = math.lengthsq(onto);
			if (num < 1E-07f)
			{
				return float3.zero;
			}
			return math.dot(onto, vector) * onto / num;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 GetParticleInertiaTensor(float4 principalRadii, float invRotationalMass)
		{
			float4 float5 = principalRadii * principalRadii;
			return 0.2f / (invRotationalMass + 1E-07f) * new float4(float5[1] + float5[2], float5[0] + float5[2], float5[0] + float5[1], 0f);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 TransformInertiaTensor(float4 tensor, quaternion rotation)
		{
			float4x4 float4x5 = rotation.toMatrix();
			return math.mul(float4x5, math.mul(tensor.asDiagonal(), math.transpose(float4x5)));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float RotationalInvMass(float4x4 inverseInertiaTensor, float4 point, float4 direction)
		{
			return math.dot(math.cross(math.mul(inverseInertiaTensor, new float4(math.cross(point.xyz, direction.xyz), 0f)).xyz, point.xyz), direction.xyz);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 GetParticleVelocityAtPoint(float4 position, float4 prevPosition, float4 point, float dt)
		{
			return BurstIntegration.DifferentiateLinear(position, prevPosition, dt);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 GetParticleVelocityAtPoint(float4 position, float4 prevPosition, quaternion orientation, quaternion prevOrientation, float4 point, float dt)
		{
			return BurstIntegration.DifferentiateLinear(position, prevPosition, dt) + new float4(math.cross(BurstIntegration.DifferentiateAngular(orientation, prevOrientation, dt).xyz, (point - prevPosition).xyz), 0f);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 GetRigidbodyVelocityAtPoint(int rigidbodyIndex, float4 point, NativeArray<BurstRigidbody> rigidbodies, NativeArray<float4> linearDeltas, NativeArray<float4> angularDeltas, BurstInertialFrame solverToWorld)
		{
			float4 obj = rigidbodies[rigidbodyIndex].velocity + linearDeltas[rigidbodyIndex];
			float4 float5 = rigidbodies[rigidbodyIndex].angularVelocity + angularDeltas[rigidbodyIndex];
			float4 float6 = solverToWorld.frame.TransformPoint(point) - rigidbodies[rigidbodyIndex].com;
			float4 float7 = obj + new float4(math.cross(float5.xyz, float6.xyz), 0f);
			float4 float8 = solverToWorld.velocity + new float4(math.cross(solverToWorld.angularVelocity.xyz, point.xyz), 0f);
			return solverToWorld.frame.InverseTransformVector(float7 - float8);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 GetRigidbodyVelocityAtPoint(int rigidbodyIndex, float4 point, NativeArray<BurstRigidbody> rigidbodies, BurstInertialFrame solverToWorld)
		{
			float4 velocity = rigidbodies[rigidbodyIndex].velocity;
			float4 angularVelocity = rigidbodies[rigidbodyIndex].angularVelocity;
			float4 float5 = solverToWorld.frame.TransformPoint(point) - rigidbodies[rigidbodyIndex].com;
			float4 float6 = velocity + new float4(math.cross(angularVelocity.xyz, float5.xyz), 0f);
			float4 float7 = solverToWorld.velocity + new float4(math.cross(solverToWorld.angularVelocity.xyz, point.xyz), 0f);
			return solverToWorld.frame.InverseTransformVector(float6 - float7);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ApplyImpulse(int rigidbodyIndex, float4 impulse, float4 point, NativeArray<BurstRigidbody> rigidbodies, NativeArray<float4> linearDeltas, NativeArray<float4> angularDeltas, BurstAffineTransform solverToWorld)
		{
			float4 float5 = solverToWorld.TransformVector(impulse);
			float4 float6 = solverToWorld.TransformPoint(point) - rigidbodies[rigidbodyIndex].com;
			float4 data = rigidbodies[rigidbodyIndex].inverseMass * float5;
			float4 data2 = math.mul(rigidbodies[rigidbodyIndex].inverseInertiaTensor, new float4(math.cross(float6.xyz, float5.xyz), 0f));
			AtomicAdd(linearDeltas, rigidbodyIndex, data);
			AtomicAdd(angularDeltas, rigidbodyIndex, data2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ApplyDeltaQuaternion(int rigidbodyIndex, quaternion rotation, quaternion delta, NativeArray<float4> angularDeltas, BurstAffineTransform solverToWorld, float dt)
		{
			quaternion prevRotation = math.mul(solverToWorld.rotation, rotation);
			quaternion quaternion2 = math.mul(solverToWorld.rotation, delta);
			quaternion rotation2 = math.normalize(new quaternion(prevRotation.value + quaternion2.value));
			AtomicAdd(angularDeltas, rigidbodyIndex, BurstIntegration.DifferentiateAngular(rotation2, prevRotation, dt));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void OneSidedNormal(float4 forward, ref float4 normal)
		{
			float num = math.dot(normal.xyz, forward.xyz);
			if (num < 0f)
			{
				normal -= 2f * num * forward;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void OneSidedNormal2(float4 xij, ref float4 nij)
		{
			float num = math.dot(xij.xyz, nij.xyz);
			if (num < 0f)
			{
				nij = xij - 2f * num * nij;
			}
			else
			{
				nij = xij;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float EllipsoidRadius(float4 normSolverDirection, quaternion orientation, float3 radii)
		{
			float num = math.lengthsq(math.mul(math.conjugate(orientation), normSolverDirection.xyz) / radii);
			if (!(num > 1E-07f))
			{
				return radii.x;
			}
			return math.sqrt(1f / num);
		}

		public static quaternion ExtractRotation(float4x4 matrix, quaternion rotation, int iterations)
		{
			return ExtractRotation((float3x3)matrix, rotation, iterations);
		}

		public static quaternion ExtractRotation(float3x3 matrix, quaternion rotation, int iterations)
		{
			for (int i = 0; i < iterations; i++)
			{
				float3x3 float3x5 = rotation.toMatrix3();
				float3 float5 = (math.cross(float3x5.c0, matrix.c0) + math.cross(float3x5.c1, matrix.c1) + math.cross(float3x5.c2, matrix.c2)) / (math.abs(math.dot(float3x5.c0, matrix.c0) + math.dot(float3x5.c1, matrix.c1) + math.dot(float3x5.c2, matrix.c2)) + 1E-07f);
				float num = math.length(float5);
				if (num < 1E-07f)
				{
					break;
				}
				rotation = math.normalize(math.mul(quaternion.AxisAngle(1f / num * float5, num), rotation));
			}
			return rotation;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SwingTwist(quaternion q, float3 twistAxis, out quaternion swing, out quaternion twist)
		{
			float num = math.dot(q.value.xyz, twistAxis);
			float3 float5 = twistAxis * num;
			twist = math.normalizesafe(new quaternion(float5[0], float5[1], float5[2], q.value.w));
			swing = math.mul(q, math.conjugate(twist));
		}

		public static float4x4 toMatrix(this quaternion q)
		{
			float num = q.value.x * q.value.x;
			float num2 = q.value.x * q.value.y;
			float num3 = q.value.x * q.value.z;
			float num4 = q.value.x * q.value.w;
			float num5 = q.value.y * q.value.y;
			float num6 = q.value.y * q.value.z;
			float num7 = q.value.y * q.value.w;
			float num8 = q.value.z * q.value.z;
			float num9 = q.value.z * q.value.w;
			return new float4x4(1f - 2f * (num5 + num8), 2f * (num2 - num9), 2f * (num3 + num7), 0f, 2f * (num2 + num9), 1f - 2f * (num + num8), 2f * (num6 - num4), 0f, 2f * (num3 - num7), 2f * (num6 + num4), 1f - 2f * (num + num5), 0f, 0f, 0f, 0f, 1f);
		}

		public static float3x3 toMatrix3(this quaternion q)
		{
			float num = q.value.x * q.value.x;
			float num2 = q.value.x * q.value.y;
			float num3 = q.value.x * q.value.z;
			float num4 = q.value.x * q.value.w;
			float num5 = q.value.y * q.value.y;
			float num6 = q.value.y * q.value.z;
			float num7 = q.value.y * q.value.w;
			float num8 = q.value.z * q.value.z;
			float num9 = q.value.z * q.value.w;
			return new float3x3(1f - 2f * (num5 + num8), 2f * (num2 - num9), 2f * (num3 + num7), 2f * (num2 + num9), 1f - 2f * (num + num8), 2f * (num6 - num4), 2f * (num3 - num7), 2f * (num6 + num4), 1f - 2f * (num + num5));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4x4 asDiagonal(this float4 v)
		{
			return new float4x4(v.x, 0f, 0f, 0f, 0f, v.y, 0f, 0f, 0f, 0f, v.z, 0f, 0f, 0f, 0f, v.w);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 nfmod(float3 a, float3 b)
		{
			return a - b * math.floor(a / b);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float nfmod(float a, float b)
		{
			return a - b * math.floor(a / b);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 diagonal(this float4x4 value)
		{
			return new float4(value.c0[0], value.c1[1], value.c2[2], value.c3[3]);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float frobeniusNorm(this float4x4 m)
		{
			return math.sqrt(math.lengthsq(m.c0) + math.lengthsq(m.c1) + math.lengthsq(m.c2) + math.lengthsq(m.c3));
		}

		public static void EigenSolve(float3x3 D, out float3 S, out float3x3 V)
		{
			S = EigenValues(D);
			float3 float5;
			float3 float6;
			float3 float7;
			if (S[0] - S[1] > S[1] - S[2])
			{
				float5 = EigenVector(D, S[0]);
				if (S[1] - S[2] < 1.1754944E-38f)
				{
					float6 = float5.unitOrthogonal();
				}
				else
				{
					float6 = EigenVector(D, S[2]);
					float6 -= float5 * math.dot(float5, float6);
					float6 = math.normalize(float6);
				}
				float7 = math.cross(float6, float5);
			}
			else
			{
				float6 = EigenVector(D, S[2]);
				if (S[0] - S[1] < 1.1754944E-38f)
				{
					float7 = float6.unitOrthogonal();
				}
				else
				{
					float7 = EigenVector(D, S[1]);
					float7 -= float6 * math.dot(float6, float7);
					float7 = math.normalize(float7);
				}
				float5 = math.cross(float7, float6);
			}
			V.c0 = float5;
			V.c1 = float7;
			V.c2 = float6;
		}

		private static float3 unitOrthogonal(this float3 input)
		{
			if (!(input.x < input.z * 1E-07f) || !(input.y < input.z * 1E-07f))
			{
				float num = 1f / math.length(input.xy);
				return new float3((0f - input.y) * num, input.x * num, 0f);
			}
			float num2 = 1f / math.length(input.yz);
			return new float3(0f, (0f - input.z) * num2, input.y * num2);
		}

		private static float3 EigenVector(float3x3 D, float S)
		{
			float3 c = D.c0;
			c[0] -= S;
			float3 c2 = D.c1;
			c2[1] -= S;
			float3 c3 = D.c2;
			c3[2] -= S;
			float3 float5 = new float3(c2[1] * c3[2] - c3[1] * c3[1], 0f, 0f);
			float3 float6 = new float3(c3[1] * c3[0] - c2[0] * c3[2], c[0] * c3[2] - c3[0] * c3[0], 0f);
			float3 float7 = new float3(c2[0] * c3[1] - c2[1] * c3[0], c2[0] * c3[0] - c[0] * c3[1], c[0] * c2[1] - c2[0] * c2[0]);
			float num = float6[0] * float6[0];
			float num2 = float7[0] * float7[0];
			float num3 = float7[1] * float7[1];
			float3 float8 = new float3(float5[0] * float5[0] + num + num2, num + float6[1] * float6[1] + num3, num2 + num3 + float7[2] * float7[2]);
			int num4 = 0;
			num4 = ((!(float8[0] > float8[1]) || !(float8[0] > float8[2])) ? ((float8[1] > float8[0] && float8[1] > float8[2]) ? 1 : 2) : 0);
			float3 float9 = float3.zero;
			if (float8[num4] < 1.1754944E-38f)
			{
				float9[0] = 1f;
				return float9;
			}
			switch (num4)
			{
			case 0:
				float9[0] = float5[0];
				float9[1] = float6[0];
				float9[2] = float7[0];
				break;
			case 1:
				float9[0] = float6[0];
				float9[1] = float6[1];
				float9[2] = float7[1];
				break;
			default:
				float9 = float7;
				break;
			}
			return math.normalize(float9);
		}

		private static float3 EigenValues(float3x3 D)
		{
			float num = 1f / 3f;
			float num2 = 1f / 6f;
			float num3 = math.sqrt(3f);
			float3 c = D.c0;
			float3 c2 = D.c1;
			float3 c3 = D.c2;
			float num4 = num * (c[0] + c2[1] + c3[2]);
			float num5 = c[0] - num4;
			float num6 = c2[1] - num4;
			float num7 = c3[2] - num4;
			float num8 = c2[0] * c2[0];
			float num9 = c3[0] * c3[0];
			float num10 = c3[1] * c3[1];
			float num11 = 0.5f * (num5 * (num6 * num7 - num10) - num7 * num8 - num6 * num9) + c2[0] * c3[1] * c[2];
			float num12 = num2 * (num5 * num5 + num6 * num6 + num7 * num7 + 2f * (num8 + num9 + num10));
			float num13 = math.sqrt(num12);
			float y = num12 * num12 * num12 - num11 * num11;
			float x = num * math.atan2(math.sqrt(math.max(0f, y)), num11);
			float num14 = math.cos(x);
			float num15 = math.sin(x);
			float num16 = num13 * num14;
			float num17 = num13 * num3 * num15;
			float num18 = num4 + 2f * num16;
			float num19 = num4 - num16 - num17;
			float num20 = num4 - num16 + num17;
			if (num18 > num19)
			{
				float num21 = num18;
				num18 = num19;
				num19 = num21;
			}
			if (num18 > num20)
			{
				float num22 = num18;
				num18 = num20;
				num20 = num22;
			}
			if (num19 > num20)
			{
				float num23 = num19;
				num19 = num20;
				num20 = num23;
			}
			return new float3(num20, num19, num18);
		}

		public static float4 NearestPointOnTri(in CachedTri tri, float4 p, out float4 bary)
		{
			float4 y = tri.vertex - p;
			float num = math.dot(tri.edge0, y);
			float num2 = math.dot(tri.edge1, y);
			float num3 = tri.data[1] * num2 - tri.data[2] * num;
			float num4 = tri.data[1] * num - tri.data[0] * num2;
			if (num3 + num4 <= tri.data[3])
			{
				if (num3 < 0f)
				{
					if (num4 < 0f)
					{
						if (num < 0f)
						{
							num4 = 0f;
							num3 = ((!(0f - num >= tri.data[0])) ? ((0f - num) / tri.data[0]) : 1f);
						}
						else
						{
							num3 = 0f;
							num4 = ((num2 >= 0f) ? 0f : ((!(0f - num2 >= tri.data[2])) ? ((0f - num2) / tri.data[2]) : 1f));
						}
					}
					else
					{
						num3 = 0f;
						num4 = ((num2 >= 0f) ? 0f : ((!(0f - num2 >= tri.data[2])) ? ((0f - num2) / tri.data[2]) : 1f));
					}
				}
				else if (num4 < 0f)
				{
					num4 = 0f;
					num3 = ((num >= 0f) ? 0f : ((!(0f - num >= tri.data[0])) ? ((0f - num) / tri.data[0]) : 1f));
				}
				else
				{
					float num5 = 1f / tri.data[3];
					num3 *= num5;
					num4 *= num5;
				}
			}
			else if (num3 < 0f)
			{
				float num6 = tri.data[1] + num;
				float num7 = tri.data[2] + num2;
				if (num7 > num6)
				{
					float num8 = num7 - num6;
					float num9 = tri.data[0] - 2f * tri.data[1] + tri.data[2];
					if (num8 >= num9)
					{
						num3 = 1f;
						num4 = 0f;
					}
					else
					{
						num3 = num8 / num9;
						num4 = 1f - num3;
					}
				}
				else
				{
					num3 = 0f;
					num4 = ((num7 <= 0f) ? 1f : ((!(num2 >= 0f)) ? ((0f - num2) / tri.data[2]) : 0f));
				}
			}
			else if (num4 < 0f)
			{
				float num6 = tri.data[1] + num2;
				float num7 = tri.data[0] + num;
				if (num7 > num6)
				{
					float num8 = num7 - num6;
					float num9 = tri.data[0] - 2f * tri.data[1] + tri.data[2];
					if (num8 >= num9)
					{
						num4 = 1f;
						num3 = 0f;
					}
					else
					{
						num4 = num8 / num9;
						num3 = 1f - num4;
					}
				}
				else
				{
					num4 = 0f;
					num3 = ((num7 <= 0f) ? 1f : ((!(num >= 0f)) ? ((0f - num) / tri.data[0]) : 0f));
				}
			}
			else
			{
				float num8 = tri.data[2] + num2 - tri.data[1] - num;
				if (num8 <= 0f)
				{
					num3 = 0f;
					num4 = 1f;
				}
				else
				{
					float num9 = tri.data[0] - 2f * tri.data[1] + tri.data[2];
					if (num8 >= num9)
					{
						num3 = 1f;
						num4 = 0f;
					}
					else
					{
						num3 = num8 / num9;
						num4 = 1f - num3;
					}
				}
			}
			bary = new float4(1f - (num3 + num4), num3, num4, 0f);
			return tri.vertex + num3 * tri.edge0 + num4 * tri.edge1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 NearestPointOnEdge(float4 a, float4 b, float4 p, out float mu, bool clampToSegment = true)
		{
			float4 x = p - a;
			float4 float5 = b - a;
			mu = math.dot(x, float5) / (math.dot(float5, float5) + 1E-07f);
			if (clampToSegment)
			{
				mu = math.saturate(mu);
			}
			return a + float5 * mu;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float3 NearestPointOnEdge(float3 a, float3 b, float3 p, out float mu, bool clampToSegment = true)
		{
			float3 x = p - a;
			float3 float5 = b - a;
			mu = math.dot(x, float5) / (math.dot(float5, float5) + 1E-07f);
			if (clampToSegment)
			{
				mu = math.saturate(mu);
			}
			return a + float5 * mu;
		}

		public static float4 NearestPointsTwoEdges(float4 a, float4 b, float4 c, float4 d, out float mu1, out float mu2)
		{
			float4 float5 = d - c;
			float num = math.dot(float5, float5);
			float4 float6 = a - math.dot(a - c, float5) / num * float5;
			float4 float7 = b - math.dot(b - c, float5) / num * float5 - float6;
			float x = math.dot(c - float6, float7) / math.dot(float7, float7);
			float4 p = math.lerp(a, b, math.saturate(x));
			float4 float8 = NearestPointOnEdge(c, d, p, out mu1);
			NearestPointOnEdge(a, b, float8, out mu2);
			return float8;
		}

		public static float4 BaryCoords(in float4 A, in float4 B, in float4 C, in float4 P)
		{
			float4 obj = C - A;
			float4 float5 = B - A;
			float4 y = P - A;
			float num = math.dot(obj, obj);
			float num2 = math.dot(obj, float5);
			float num3 = math.dot(obj, y);
			float num4 = math.dot(float5, float5);
			float num5 = math.dot(float5, y);
			float num6 = num * num4 - num2 * num2;
			if (math.abs(num6) > 1E-07f)
			{
				float num7 = (num4 * num3 - num2 * num5) / num6;
				float num8 = (num * num5 - num2 * num3) / num6;
				return new float4(1f - num7 - num8, num8, num7, 0f);
			}
			return float4.zero;
		}

		public static float4 BaryCoords2(in float4 A, in float4 B, in float4 P)
		{
			float4 obj = P - A;
			float4 float5 = B - A;
			float num = math.sqrt(math.dot(obj, obj) / (math.dot(float5, float5) + 1E-07f));
			return new float4(1f - num, num, 0f, 0f);
		}

		public static float4 BaryIntrpl(in float4 p1, in float4 p2, in float4 p3, in float4 coords)
		{
			return coords[0] * p1 + coords[1] * p2 + coords[2] * p3;
		}

		public static float4 BaryIntrpl(in float4 p1, in float4 p2, in float4 coords)
		{
			return coords[0] * p1 + coords[1] * p2;
		}

		public static float BaryIntrpl(float p1, float p2, float p3, float4 coords)
		{
			return coords[0] * p1 + coords[1] * p2 + coords[2] * p3;
		}

		public static float BaryIntrpl(float p1, float p2, float4 coords)
		{
			return coords[0] * p1 + coords[1] * p2;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float BaryScale(float4 coords)
		{
			return 1f / math.dot(coords, coords);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 BarycenterForSimplexOfSize(int simplexSize)
		{
			float value = 1f / (float)simplexSize;
			float4 result = float4.zero;
			for (int i = 0; i < simplexSize; i++)
			{
				result[i] = value;
			}
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint Part1By1(uint x)
		{
			x &= 0xFFFF;
			x = (x ^ (x << 8)) & 0xFF00FF;
			x = (x ^ (x << 4)) & 0xF0F0F0F;
			x = (x ^ (x << 2)) & 0x33333333;
			x = (x ^ (x << 1)) & 0x55555555;
			return x;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint Part1By2(uint x)
		{
			x &= 0x3FF;
			x = (x ^ (x << 16)) & 0xFF0000FFu;
			x = (x ^ (x << 8)) & 0x300F00F;
			x = (x ^ (x << 4)) & 0x30C30C3;
			x = (x ^ (x << 2)) & 0x9249249;
			return x;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint Compact1By1(uint x)
		{
			x &= 0x55555555;
			x = (x ^ (x >> 1)) & 0x33333333;
			x = (x ^ (x >> 2)) & 0xF0F0F0F;
			x = (x ^ (x >> 4)) & 0xFF00FF;
			x = (x ^ (x >> 8)) & 0xFFFF;
			return x;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint Compact1By2(uint x)
		{
			x &= 0x9249249;
			x = (x ^ (x >> 2)) & 0x30C30C3;
			x = (x ^ (x >> 4)) & 0x300F00F;
			x = (x ^ (x >> 8)) & 0xFF0000FFu;
			x = (x ^ (x >> 16)) & 0x3FF;
			return x;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint EncodeMorton2(uint2 coords)
		{
			return (Part1By1(coords.y) << 1) + Part1By1(coords.x);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint EncodeMorton3(uint3 coords)
		{
			return (Part1By2(coords.z) << 2) + (Part1By2(coords.y) << 1) + Part1By2(coords.x);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 DecodeMorton2(uint code)
		{
			return new uint3(Compact1By1(code), Compact1By1(code >> 1), 0u);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3 DecodeMorton3(uint code)
		{
			return new uint3(Compact1By2(code), Compact1By2(code >> 1), Compact1By2(code >> 2));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float4 UnpackFloatRGBA(float v)
		{
			uint num = math.asuint(v);
			float x = (float)((num & 0xFF000000u) >> 24) / 255f;
			float y = (float)((num & 0xFF0000) >> 16) / 255f;
			float z = (float)((num & 0xFF00) >> 8) / 255f;
			float w = (float)(num & 0xFF) / 255f;
			return new float4(x, y, z, w);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float PackFloatRGBA(float4 enc)
		{
			return math.asfloat(((uint)(enc.x * 255f) << 24) + ((uint)(enc.y * 255f) << 16) + ((uint)(enc.z * 255f) << 8) + (uint)(enc.w * 255f));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float2 UnpackFloatRG(float v)
		{
			uint num = math.asuint(v);
			float x = (float)((num & 0xFFFF0000u) >> 16) / 65535f;
			float y = (float)(num & 0xFFFF) / 65535f;
			return new float2(x, y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float PackFloatRG(float2 enc)
		{
			return math.asfloat(((uint)(enc.x * 65535f) << 16) + (uint)(enc.y * 65535f));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static float2 OctWrap(float2 v)
		{
			return (1f - math.abs(v.yx)) * new float2((v.x >= 0f) ? 1f : (-1f), (v.y >= 0f) ? 1f : (-1f));
		}

		public static float OctEncode(float3 n)
		{
			n /= math.abs(n.x) + math.abs(n.y) + math.abs(n.z);
			n.xy = (((double)n.z >= 0.0) ? n.xy : OctWrap(n.xy));
			n.xy = n.xy * 0.5f + 0.5f;
			uint num = (uint)(n.x * 65535f);
			uint num2 = (uint)(n.y * 65535f);
			return math.asfloat((num << 16) | (num2 & 0xFFFF));
		}

		public static float3 OctDecode(float k)
		{
			uint num = math.asuint(k);
			float2 float5 = new float2((float)(num >> 16) / 65535f, (float)(num & 0xFFFF) / 65535f) * 2f - 1f;
			float3 x = new float3(float5.x, float5.y, 1f - math.abs(float5.x) - math.abs(float5.y));
			float num2 = math.saturate(0f - x.z);
			x.x += ((x.x >= 0f) ? (0f - num2) : num2);
			x.y += ((x.y >= 0f) ? (0f - num2) : num2);
			return math.normalize(x);
		}

		public static float Remap01(float value, float min_, float max_)
		{
			return (math.min(value, max_) - math.min(value, min_)) / (max_ - min_);
		}

		public static float3 Sort(this float3 f)
		{
			if (f.x > f.y)
			{
				float x = f.x;
				f.x = f.y;
				f.y = x;
			}
			if (f.x > f.z)
			{
				float x = f.x;
				f.x = f.z;
				f.z = x;
			}
			if (f.y > f.z)
			{
				float x = f.y;
				f.y = f.z;
				f.z = x;
			}
			return new float3(f.z, f.y, f.x);
		}

		public unsafe static void RemoveRangeBurst<T>(this NativeList<T> list, int index, int count) where T : unmanaged
		{
			int num = UnsafeUtility.SizeOf<T>();
			byte* unsafePtr = (byte*)list.GetUnsafePtr();
			UnsafeUtility.MemMove(unsafePtr + index * num, unsafePtr + (index + count) * num, num * (list.Length - count - index));
			for (int i = 0; i < count; i++)
			{
				list.RemoveAtSwapBack(list.Length - 1);
			}
		}

		public static float3 Hash33(float3 p3)
		{
			p3 = math.frac(p3 * new float3(0.1031f, 0.103f, 0.0973f));
			p3 += math.dot(p3, p3.yxz + 33.33f);
			return math.frac((p3.xxy + p3.yxx) * p3.zyx);
		}

		public static float Hash13(float3 p3)
		{
			p3 = math.frac(p3 * 0.1031f);
			p3 += math.dot(p3, p3.zyx + 31.32f);
			return math.frac((p3.x + p3.y) * p3.z);
		}

		public static float2 Hash21(float p)
		{
			float3 x = math.frac(new float3(p) * new float3(0.1031f, 0.103f, 0.0973f));
			x += math.dot(x, x.yzx + 33.33f);
			return math.frac((x.xx + x.yz) * x.zy);
		}

		public static float3 Hash31(float p)
		{
			float3 x = math.frac(new float3(p) * new float3(0.1031f, 0.103f, 0.0973f));
			x += math.dot(x, x.yzx + 33.33f);
			return math.frac((x.xxy + x.yzz) * x.zyx);
		}

		public static void RandomInCylinder(float seed, float4 pos, float4 dir, float length, float radius, out float4 position, out float3 velocity)
		{
			float3 float5 = Hash31(seed);
			float3 xyz = dir.xyz;
			float3 float6 = math.normalizesafe(math.cross(xyz, new float3(1f, 0f, 0f)));
			float3 float7 = math.cross(float6, xyz);
			float x = float5.y * 2f * MathF.PI;
			float2 float8 = radius * math.sqrt(float5.x) * new float2(math.cos(x), math.sin(x));
			velocity = float6 * float8.x + float7 * float8.y;
			position = new float4(pos.xyz + xyz * length * float5.z + velocity, 0f);
		}

		public static void RandomInBox(float seed, float4 center, float4 size, out float4 position, out float3 velocity)
		{
			float3 float5 = Hash31(seed);
			velocity = (float5 - new float3(0.5f, 0.5f, 0.5f)) * size.xyz;
			position = new float4(center.xyz + velocity, 0f);
		}
	}
}
