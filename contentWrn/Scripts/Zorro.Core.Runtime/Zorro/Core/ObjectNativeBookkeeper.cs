using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace Zorro.Core
{
	public class ObjectNativeBookkeeper<T> : NativeBookkeeper<T> where T : Object
	{
		private Dictionary<int, T> m_instanceIDLookup;

		public BidirectionalNativeDictionary<int, int> InstanceIDtoIndexHash;

		public ObjectNativeBookkeeper(int defaultCapacity)
			: base(defaultCapacity)
		{
			m_instanceIDLookup = new Dictionary<int, T>();
			InstanceIDtoIndexHash = new BidirectionalNativeDictionary<int, int>(defaultCapacity, Allocator.Persistent);
		}

		public T GetFromInstanceID(int instanceID)
		{
			return m_instanceIDLookup[instanceID];
		}

		public override int Add(T newEntry)
		{
			int instanceID = newEntry.GetInstanceID();
			m_instanceIDLookup.Add(instanceID, newEntry);
			int num = base.Add(newEntry);
			InstanceIDtoIndexHash.Add(instanceID, num);
			return num;
		}

		public override BookkeperRemovalInfo Remove(T entry)
		{
			BookkeperRemovalInfo result = base.Remove(entry);
			int instanceID = entry.GetInstanceID();
			m_instanceIDLookup.Remove(instanceID);
			InstanceIDtoIndexHash.RemoveFromKey(instanceID);
			if (result.IndexRemoved != result.SwapbackIndex)
			{
				instanceID = InstanceIDtoIndexHash.RemoveFromValue(result.SwapbackIndex);
				InstanceIDtoIndexHash.Add(instanceID, result.IndexRemoved);
			}
			return result;
		}

		public override void Dispose()
		{
			base.Dispose();
			InstanceIDtoIndexHash.Dispose();
		}
	}
}
