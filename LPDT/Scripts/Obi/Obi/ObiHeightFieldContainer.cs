using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	public class ObiHeightFieldContainer
	{
		public Dictionary<TerrainData, ObiHeightFieldHandle> handles;

		public ObiNativeHeightFieldHeaderList headers;

		public ObiNativeFloatList samples;

		public ObiHeightFieldContainer()
		{
			handles = new Dictionary<TerrainData, ObiHeightFieldHandle>();
			headers = new ObiNativeHeightFieldHeaderList();
			samples = new ObiNativeFloatList();
		}

		public ObiHeightFieldHandle GetOrCreateHeightField(TerrainData source)
		{
			if (!handles.TryGetValue(source, out var value))
			{
				int heightmapResolution = source.heightmapResolution;
				int heightmapResolution2 = source.heightmapResolution;
				float[,] heights = source.GetHeights(0, 0, heightmapResolution, heightmapResolution2);
				bool[,] holes = source.GetHoles(0, 0, heightmapResolution - 1, heightmapResolution2 - 1);
				float[] array = new float[heightmapResolution * heightmapResolution2];
				for (int i = 0; i < heightmapResolution2; i++)
				{
					for (int j = 0; j < heightmapResolution; j++)
					{
						array[i * heightmapResolution + j] = heights[i, j] * (float)(holes[Mathf.Min(i, heightmapResolution2 - 2), Mathf.Min(j, heightmapResolution - 2)] ? 1 : (-1));
					}
				}
				value = new ObiHeightFieldHandle(source, headers.count);
				handles.Add(source, value);
				headers.Add(new HeightFieldHeader(samples.count, array.Length));
				samples.AddRange(array);
			}
			return value;
		}

		public void DestroyHeightField(ObiHeightFieldHandle handle)
		{
			if (handle == null || !handle.isValid || handle.index >= handles.Count)
			{
				return;
			}
			HeightFieldHeader heightFieldHeader = headers[handle.index];
			for (int i = 0; i < headers.count; i++)
			{
				HeightFieldHeader value = headers[i];
				if (value.firstSample > heightFieldHeader.firstSample)
				{
					value.firstSample -= heightFieldHeader.sampleCount;
					headers[i] = value;
				}
			}
			foreach (KeyValuePair<TerrainData, ObiHeightFieldHandle> handle2 in handles)
			{
				if (handle2.Value.index > handle.index)
				{
					handle2.Value.index--;
				}
			}
			samples.RemoveRange(heightFieldHeader.firstSample, heightFieldHeader.sampleCount);
			headers.RemoveAt(handle.index);
			handles.Remove(handle.owner);
			handle.Invalidate();
		}

		public void Dispose()
		{
			if (headers != null)
			{
				headers.Dispose();
			}
			if (samples != null)
			{
				samples.Dispose();
			}
		}
	}
}
