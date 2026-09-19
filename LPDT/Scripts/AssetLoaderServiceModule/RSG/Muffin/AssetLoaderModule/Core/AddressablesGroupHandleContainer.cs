using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace RSG.Muffin.AssetLoaderModule.Core
{
	[SerializeField]
	public class AddressablesGroupHandleContainer
	{
		public readonly Dictionary<string, AsyncOperationHandle> CompletedHandles = new Dictionary<string, AsyncOperationHandle>();

		public readonly Dictionary<string, List<AsyncOperationHandle>> AllHandles = new Dictionary<string, List<AsyncOperationHandle>>();
	}
}
