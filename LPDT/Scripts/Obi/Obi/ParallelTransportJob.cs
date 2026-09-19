using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	[BurstCompile]
	internal struct ParallelTransportJob : IJobParallelFor
	{
		[NativeDisableParallelForRestriction]
		public NativeArray<BurstPathFrame> pathFrames;

		[ReadOnly]
		public NativeArray<int> frameOffsets;

		[ReadOnly]
		public NativeArray<int> particleIndices;

		[ReadOnly]
		public NativeArray<float4> renderablePositions;

		[ReadOnly]
		public NativeArray<quaternion> renderableOrientations;

		[ReadOnly]
		public NativeArray<float4> principalRadii;

		[ReadOnly]
		public NativeArray<float4> colors;

		[ReadOnly]
		public NativeArray<BurstPathSmootherData> pathData;

		public void Execute(int i)
		{
			BurstPathFrame frame = default(BurstPathFrame);
			BurstPathFrame frame2 = default(BurstPathFrame);
			BurstPathFrame burstPathFrame = default(BurstPathFrame);
			frame.Reset();
			frame2.Reset();
			burstPathFrame.Reset();
			int num = ((i > 0) ? frameOffsets[i - 1] : 0);
			int num2 = frameOffsets[i] - num;
			PathFrameFromParticle(ref frame2, particleIndices[num], pathData[i].usesOrientedParticles == 1);
			burstPathFrame = frame2;
			for (int j = 1; j <= num2; j++)
			{
				int index = num + math.min(j, num2 - 1);
				int particleIndex = particleIndices[index];
				PathFrameFromParticle(ref frame, particleIndex, pathData[i].usesOrientedParticles == 1);
				if (pathData[i].usesOrientedParticles == 1)
				{
					burstPathFrame = frame2;
				}
				else
				{
					frame2.tangent = math.normalizesafe(frame2.position - burstPathFrame.position + (frame.position - frame2.position), burstPathFrame.tangent);
					burstPathFrame.Transport(in frame2, pathData[i].twist);
				}
				frame2 = frame;
				pathFrames[num + j - 1] = burstPathFrame;
			}
		}

		private void PathFrameFromParticle(ref BurstPathFrame frame, int particleIndex, bool useOrientedParticles, bool interpolateOrientation = false)
		{
			frame.position = renderablePositions[particleIndex].xyz;
			frame.thickness = principalRadii[particleIndex][0];
			frame.color = colors[particleIndex];
			if (useOrientedParticles)
			{
				quaternion quaternion2 = renderableOrientations[particleIndex];
				quaternion q = renderableOrientations[math.max(0, particleIndex - 1)];
				float4x4 float4x5 = (interpolateOrientation ? math.slerp(quaternion2, q, 0.5f) : quaternion2).toMatrix();
				frame.normal = float4x5.c1.xyz;
				frame.binormal = float4x5.c0.xyz;
				frame.tangent = float4x5.c2.xyz;
			}
		}
	}
}
