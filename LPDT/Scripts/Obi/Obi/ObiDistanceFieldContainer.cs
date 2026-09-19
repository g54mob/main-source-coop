using System.Collections.Generic;

namespace Obi
{
	public class ObiDistanceFieldContainer
	{
		public Dictionary<ObiDistanceField, ObiDistanceFieldHandle> handles;

		public ObiNativeDistanceFieldHeaderList headers;

		public ObiNativeDFNodeList dfNodes;

		public ObiDistanceFieldContainer()
		{
			handles = new Dictionary<ObiDistanceField, ObiDistanceFieldHandle>();
			headers = new ObiNativeDistanceFieldHeaderList();
			dfNodes = new ObiNativeDFNodeList();
		}

		public ObiDistanceFieldHandle GetOrCreateDistanceField(ObiDistanceField source)
		{
			if (!handles.TryGetValue(source, out var value))
			{
				value = new ObiDistanceFieldHandle(source, headers.count);
				handles.Add(source, value);
				headers.Add(new DistanceFieldHeader(dfNodes.count, source.nodes.Count));
				dfNodes.AddRange(source.nodes);
			}
			return value;
		}

		public void DestroyDistanceField(ObiDistanceFieldHandle handle)
		{
			if (handle == null || !handle.isValid || handle.index >= handles.Count)
			{
				return;
			}
			DistanceFieldHeader distanceFieldHeader = headers[handle.index];
			for (int i = 0; i < headers.count; i++)
			{
				DistanceFieldHeader value = headers[i];
				if (value.firstNode > distanceFieldHeader.firstNode)
				{
					value.firstNode -= distanceFieldHeader.nodeCount;
					headers[i] = value;
				}
			}
			foreach (KeyValuePair<ObiDistanceField, ObiDistanceFieldHandle> handle2 in handles)
			{
				if (handle2.Value.index > handle.index)
				{
					handle2.Value.index--;
				}
			}
			dfNodes.RemoveRange(distanceFieldHeader.firstNode, distanceFieldHeader.nodeCount);
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
			if (dfNodes != null)
			{
				dfNodes.Dispose();
			}
		}
	}
}
