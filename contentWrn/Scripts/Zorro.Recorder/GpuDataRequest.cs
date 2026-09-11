using System.Collections.Generic;
using UnityEngine.Rendering;

public class GpuDataRequest
{
	public List<AsyncGPUReadbackRequest> gpuRequests;

	public bool isKeyFrame;

	public GpuDataRequest()
	{
		gpuRequests = new List<AsyncGPUReadbackRequest>();
	}
}
