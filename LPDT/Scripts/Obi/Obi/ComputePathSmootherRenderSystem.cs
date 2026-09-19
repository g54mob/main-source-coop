using UnityEngine;

namespace Obi
{
	public class ComputePathSmootherRenderSystem : ObiPathSmootherRenderSystem
	{
		private ComputeShader pathShader;

		private int parallelTransportKernel;

		private int decimateKernel;

		private int smoothKernel;

		public ComputePathSmootherRenderSystem(ObiSolver solver)
			: base(solver)
		{
			pathShader = Object.Instantiate(Resources.Load<ComputeShader>("Compute/PathSmoothing"));
			parallelTransportKernel = pathShader.FindKernel("ParallelTransport");
			decimateKernel = pathShader.FindKernel("Decimate");
			smoothKernel = pathShader.FindKernel("ChaikinSmooth");
		}

		public override void Setup()
		{
			base.Setup();
			particleIndices.AsComputeBuffer<int>();
			rawFrameOffsets.AsComputeBuffer<int>();
			pathData.AsComputeBuffer<BurstPathSmootherData>();
			rawFrames.AsComputeBuffer<ObiPathFrame>();
			decimatedFrameCounts.AsComputeBuffer<int>();
			smoothFrames.AsComputeBuffer<ObiPathFrame>();
			smoothFrameOffsets.AsComputeBuffer<int>();
			smoothFrameCounts.AsComputeBuffer<int>();
		}

		public override void Render()
		{
			using (ObiPathSmootherRenderSystem.m_RenderMarker.Auto())
			{
				ComputeSolverImpl computeSolverImpl = m_Solver.implementation as ComputeSolverImpl;
				if (computeSolverImpl.renderablePositionsBuffer != null && computeSolverImpl.renderablePositionsBuffer.count > 0)
				{
					pathData.WaitForReadback();
					smoothFrames.WaitForReadback();
					base.Render();
					pathData.Upload();
					int threadGroupsX = ComputeMath.ThreadGroupCount(rawFrameOffsets.count, 128);
					pathShader.SetInt("chunkCount", rawFrameOffsets.count);
					pathShader.SetBuffer(parallelTransportKernel, "frameOffsets", rawFrameOffsets.computeBuffer);
					pathShader.SetBuffer(parallelTransportKernel, "particleIndices", particleIndices.computeBuffer);
					pathShader.SetBuffer(parallelTransportKernel, "renderablePositions", computeSolverImpl.renderablePositionsBuffer);
					pathShader.SetBuffer(parallelTransportKernel, "renderableOrientations", computeSolverImpl.renderableOrientationsBuffer);
					pathShader.SetBuffer(parallelTransportKernel, "principalRadii", computeSolverImpl.principalRadiiBuffer);
					pathShader.SetBuffer(parallelTransportKernel, "colors", computeSolverImpl.colorsBuffer);
					pathShader.SetBuffer(parallelTransportKernel, "pathData", pathData.computeBuffer);
					pathShader.SetBuffer(parallelTransportKernel, "pathFrames", rawFrames.computeBuffer);
					pathShader.Dispatch(parallelTransportKernel, threadGroupsX, 1, 1);
					pathShader.SetBuffer(decimateKernel, "pathFrames", rawFrames.computeBuffer);
					pathShader.SetBuffer(decimateKernel, "frameOffsets", rawFrameOffsets.computeBuffer);
					pathShader.SetBuffer(decimateKernel, "decimatedFrameCounts", decimatedFrameCounts.computeBuffer);
					pathShader.SetBuffer(decimateKernel, "pathData", pathData.computeBuffer);
					pathShader.Dispatch(decimateKernel, threadGroupsX, 1, 1);
					pathShader.SetBuffer(smoothKernel, "pathFrames", rawFrames.computeBuffer);
					pathShader.SetBuffer(smoothKernel, "frameOffsets", rawFrameOffsets.computeBuffer);
					pathShader.SetBuffer(smoothKernel, "decimatedFrameCounts", decimatedFrameCounts.computeBuffer);
					pathShader.SetBuffer(smoothKernel, "smoothFrames", smoothFrames.computeBuffer);
					pathShader.SetBuffer(smoothKernel, "smoothFrameOffsets", smoothFrameOffsets.computeBuffer);
					pathShader.SetBuffer(smoothKernel, "smoothFrameCounts", smoothFrameCounts.computeBuffer);
					pathShader.SetBuffer(smoothKernel, "pathData", pathData.computeBuffer);
					pathShader.Dispatch(smoothKernel, threadGroupsX, 1, 1);
					pathData.Readback();
					smoothFrames.Readback();
				}
			}
		}
	}
}
