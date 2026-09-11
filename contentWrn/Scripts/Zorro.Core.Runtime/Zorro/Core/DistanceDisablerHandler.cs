using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

namespace Zorro.Core
{
	public class DistanceDisablerHandler : Singleton<DistanceDisablerHandler>
	{
		private ObjectNativeBookkeeper<DistanceDisabler> m_distanceDisablerBookkeeper;

		private TransformAccessRecord m_transformAccessRecord;

		private NativeListRecord<DistanceDisablerData> m_distanceDisablerDataRecord;

		private Optionable<JobHandle> m_jobHandle;

		private NativeQueue<DistanceDisablerEvent> m_eventQueue;

		protected override void Awake()
		{
			base.Awake();
			m_distanceDisablerBookkeeper = new ObjectNativeBookkeeper<DistanceDisabler>(128);
			m_transformAccessRecord = new TransformAccessRecord(128);
			m_distanceDisablerDataRecord = new NativeListRecord<DistanceDisablerData>(128);
			m_distanceDisablerBookkeeper.RegisterRecord(m_transformAccessRecord);
			m_distanceDisablerBookkeeper.RegisterRecord(m_distanceDisablerDataRecord);
		}

		private void OnDestroy()
		{
			m_distanceDisablerBookkeeper.Dispose();
		}

		public void RegisterDistanceDisabler(DistanceDisabler distanceDisabler)
		{
			Proccess();
			m_distanceDisablerBookkeeper.Add(distanceDisabler);
			m_transformAccessRecord.Add(distanceDisabler.transform);
			m_distanceDisablerDataRecord.Add(new DistanceDisablerData
			{
				distance = distanceDisabler.Distance,
				culled = false
			});
		}

		public void UnregisterDistanceDisabler(DistanceDisabler distanceDisabler)
		{
			Proccess();
			m_distanceDisablerBookkeeper.Remove(distanceDisabler);
		}

		private void Update()
		{
			float3 cameraPosition = Camera.main.transform.position;
			m_eventQueue = new NativeQueue<DistanceDisablerEvent>(Allocator.TempJob);
			DistanceDisablerJob jobData = new DistanceDisablerJob
			{
				DistanceDisablerData = m_distanceDisablerDataRecord.NativeList,
				CameraPosition = cameraPosition,
				DistanceDisablerEventQueue = m_eventQueue.AsParallelWriter()
			};
			m_jobHandle = Optionable<JobHandle>.Some(jobData.Schedule(m_transformAccessRecord.TransformAccessArray));
		}

		private void LateUpdate()
		{
			Proccess();
		}

		private void Proccess()
		{
			if (m_jobHandle.IsSome)
			{
				m_jobHandle.Value.Complete();
				m_jobHandle = Optionable<JobHandle>.None;
				for (int i = 0; i < m_eventQueue.Count; i++)
				{
					DistanceDisablerEvent distanceDisablerEvent = m_eventQueue.Dequeue();
					int keyFromValue = m_distanceDisablerBookkeeper.InstanceIDtoIndexHash.GetKeyFromValue(distanceDisablerEvent.Index);
					m_distanceDisablerBookkeeper.GetFromInstanceID(keyFromValue).SetNewState(!distanceDisablerEvent.Culled);
				}
				m_eventQueue.Dispose();
			}
		}
	}
}
