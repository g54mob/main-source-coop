using Unity.Profiling;
using UnityEngine;

namespace Obi
{
	public abstract class ObiPathSmootherRenderSystem : RenderSystem<ObiPathSmoother>, IRenderSystem
	{
		protected static ProfilerMarker m_SetupRenderMarker = new ProfilerMarker("SetupSmoothPathRendering");

		protected static ProfilerMarker m_RenderMarker = new ProfilerMarker("SmoothPathRendering");

		protected ObiSolver m_Solver;

		public ObiNativeList<int> particleIndices;

		public ObiNativeList<int> chunkOffsets;

		public ObiNativeList<BurstPathSmootherData> pathData;

		public ObiNativeList<ObiPathFrame> rawFrames;

		public ObiNativeList<int> rawFrameOffsets;

		public ObiNativeList<int> decimatedFrameCounts;

		public ObiNativeList<ObiPathFrame> smoothFrames;

		public ObiNativeList<int> smoothFrameOffsets;

		public ObiNativeList<int> smoothFrameCounts;

		public Oni.RenderingSystemType typeEnum => Oni.RenderingSystemType.AllSmoothedRopes;

		public RendererSet<ObiPathSmoother> renderers { get; } = new RendererSet<ObiPathSmoother>();

		public uint tier => 0u;

		public ObiPathSmootherRenderSystem(ObiSolver solver)
		{
			m_Solver = solver;
			pathData = new ObiNativeList<BurstPathSmootherData>();
			particleIndices = new ObiNativeList<int>();
			chunkOffsets = new ObiNativeList<int>();
			rawFrames = new ObiNativeList<ObiPathFrame>();
			rawFrameOffsets = new ObiNativeList<int>();
			decimatedFrameCounts = new ObiNativeList<int>();
			smoothFrames = new ObiNativeList<ObiPathFrame>();
			smoothFrameOffsets = new ObiNativeList<int>();
			smoothFrameCounts = new ObiNativeList<int>();
		}

		public void Dispose()
		{
			if (particleIndices != null)
			{
				particleIndices.Dispose();
			}
			if (chunkOffsets != null)
			{
				chunkOffsets.Dispose();
			}
			if (pathData != null)
			{
				pathData.Dispose();
			}
			if (rawFrames != null)
			{
				rawFrames.Dispose();
			}
			if (rawFrameOffsets != null)
			{
				rawFrameOffsets.Dispose();
			}
			if (decimatedFrameCounts != null)
			{
				decimatedFrameCounts.Dispose();
			}
			if (smoothFrames != null)
			{
				smoothFrames.Dispose();
			}
			if (smoothFrameOffsets != null)
			{
				smoothFrameOffsets.Dispose();
			}
			if (smoothFrameCounts != null)
			{
				smoothFrameCounts.Dispose();
			}
		}

		private void Clear()
		{
			pathData.Clear();
			particleIndices.Clear();
			chunkOffsets.Clear();
			rawFrames.Clear();
			rawFrameOffsets.Clear();
			decimatedFrameCounts.Clear();
			smoothFrames.Clear();
			smoothFrameOffsets.Clear();
			smoothFrameCounts.Clear();
		}

		private int GetChaikinCount(int initialPoints, uint recursionLevel)
		{
			if (recursionLevel == 0 || initialPoints < 3)
			{
				return initialPoints;
			}
			int num = (int)Mathf.Pow(2f, recursionLevel);
			return (initialPoints - 2) * num + 2;
		}

		public virtual void Setup()
		{
			using (m_SetupRenderMarker.Auto())
			{
				Clear();
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				for (int i = 0; i < renderers.Count; i++)
				{
					ObiPathSmoother obiPathSmoother = renderers[i];
					ObiRopeBase obiRopeBase = obiPathSmoother.actor as ObiRopeBase;
					BurstPathSmootherData item = new BurstPathSmootherData(obiRopeBase, obiPathSmoother);
					chunkOffsets.Add(num2);
					for (int j = 0; j < obiRopeBase.elements.Count; j++)
					{
						num3++;
						particleIndices.Add(obiRopeBase.elements[j].particle1);
						if (j < obiRopeBase.elements.Count - 1 && obiRopeBase.elements[j].particle2 != obiRopeBase.elements[j + 1].particle1)
						{
							rawFrameOffsets.Add(++num3);
							particleIndices.Add(obiRopeBase.elements[j].particle2);
							pathData.Add(item);
							num2++;
						}
					}
					num2++;
					rawFrameOffsets.Add(++num3);
					particleIndices.Add(obiRopeBase.elements[obiRopeBase.elements.Count - 1].particle2);
					pathData.Add(item);
					obiPathSmoother.indexInSystem = num++;
				}
				chunkOffsets.Add(num2);
				rawFrames.ResizeUninitialized(num3);
				decimatedFrameCounts.ResizeUninitialized(rawFrameOffsets.count);
				smoothFrameOffsets.ResizeUninitialized(rawFrameOffsets.count);
				smoothFrameCounts.ResizeUninitialized(rawFrameOffsets.count);
				int num4 = 0;
				for (int k = 0; k < rawFrameOffsets.count; k++)
				{
					int initialPoints = rawFrameOffsets[k] - ((k > 0) ? rawFrameOffsets[k - 1] : 0);
					int chaikinCount = GetChaikinCount(initialPoints, pathData[k].smoothing);
					smoothFrameOffsets[k] = num4;
					smoothFrameCounts[k] = chaikinCount;
					num4 += chaikinCount;
				}
				smoothFrames.ResizeUninitialized(num4);
			}
		}

		public int GetChunkCount(int rendererIndex)
		{
			rendererIndex = Mathf.Clamp(rendererIndex, 0, renderers.Count);
			if (rendererIndex >= chunkOffsets.count)
			{
				return 0;
			}
			return chunkOffsets[rendererIndex + 1] - chunkOffsets[rendererIndex];
		}

		public int GetSmoothFrameCount(int rendererIndex)
		{
			rendererIndex = Mathf.Clamp(rendererIndex, 0, renderers.Count);
			int num = 0;
			if (rendererIndex >= chunkOffsets.count)
			{
				return num;
			}
			for (int i = chunkOffsets[rendererIndex]; i < chunkOffsets[rendererIndex + 1]; i++)
			{
				num += smoothFrameCounts[i];
			}
			return num;
		}

		public int GetSmoothFrameCount(int rendererIndex, int chunkIndex)
		{
			rendererIndex = Mathf.Clamp(rendererIndex, 0, renderers.Count);
			if (rendererIndex >= chunkOffsets.count)
			{
				return 0;
			}
			int max = chunkOffsets[rendererIndex + 1] - chunkOffsets[rendererIndex];
			int index = chunkOffsets[rendererIndex] + Mathf.Clamp(chunkIndex, 0, max);
			return smoothFrameCounts[index];
		}

		public float GetSmoothLength(int rendererIndex)
		{
			rendererIndex = Mathf.Clamp(rendererIndex, 0, renderers.Count);
			float num = 0f;
			if (rendererIndex >= chunkOffsets.count)
			{
				return num;
			}
			for (int i = chunkOffsets[rendererIndex]; i < chunkOffsets[rendererIndex + 1]; i++)
			{
				num += pathData[i].smoothLength;
			}
			return num;
		}

		public ObiPathFrame GetFrameAt(int rendererIndex, int chunkIndex, int frameIndex)
		{
			rendererIndex = Mathf.Clamp(rendererIndex, 0, renderers.Count);
			if (rendererIndex >= chunkOffsets.count)
			{
				return ObiPathFrame.Identity;
			}
			int max = chunkOffsets[rendererIndex + 1] - chunkOffsets[rendererIndex];
			int index = chunkOffsets[rendererIndex] + Mathf.Clamp(chunkIndex, 0, max);
			return smoothFrames[smoothFrameOffsets[index] + Mathf.Clamp(frameIndex, 0, smoothFrameCounts[index])];
		}

		public ObiPathFrame GetFrameAt(int rendererIndex, float mu)
		{
			rendererIndex = Mathf.Clamp(rendererIndex, 0, renderers.Count);
			if (rendererIndex >= chunkOffsets.count)
			{
				return ObiPathFrame.Identity;
			}
			float num = 0f;
			for (int i = chunkOffsets[rendererIndex]; i < chunkOffsets[rendererIndex + 1]; i++)
			{
				num += pathData[i].smoothLength;
			}
			num *= mu;
			float num2 = 0f;
			int num3 = 0;
			for (int j = chunkOffsets[rendererIndex]; j < chunkOffsets[rendererIndex + 1]; j++)
			{
				int num4 = smoothFrameOffsets[j];
				int num5 = smoothFrameCounts[j];
				for (int k = num4 + 1; k < num4 + num5; k++)
				{
					float num6 = Vector3.Distance(smoothFrames[k - 1].position, smoothFrames[k].position);
					num2 = num / num6;
					num -= num6;
					num3 = k;
					if (num <= 0f)
					{
						return (1f - num2) * smoothFrames[k - 1] + num2 * smoothFrames[k];
					}
				}
			}
			return (1f - num2) * smoothFrames[num3 - 1] + num2 * smoothFrames[num3];
		}

		public void Step()
		{
		}

		public virtual void Render()
		{
			for (int i = 0; i < renderers.Count; i++)
			{
				ObiRopeBase obiRopeBase = renderers[i].actor as ObiRopeBase;
				for (int j = chunkOffsets[i]; j < chunkOffsets[i + 1]; j++)
				{
					BurstPathSmootherData value = pathData[j];
					value.restLength = obiRopeBase.restLength;
					pathData[j] = value;
				}
			}
		}
	}
}
