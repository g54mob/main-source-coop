using System.Collections.Generic;
using UnityEngine;

namespace RSG.Muffin.AssetLoaderModule.Core
{
	[SerializeField]
	public class ResourcesGroupHandleContainer
	{
		public readonly Dictionary<string, ResourceRequest> CompletedHandles = new Dictionary<string, ResourceRequest>();

		public readonly Dictionary<string, List<ResourceRequest>> AllHandles = new Dictionary<string, List<ResourceRequest>>();
	}
}
