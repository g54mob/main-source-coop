using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Rendering;
using Zorro.Core.CLI;

namespace Zorro.Core.SmallShadows
{
	[DefaultExecutionOrder(-1000)]
	public class SmallShadowHandler : Singleton<SmallShadowHandler>
	{
		public ObjectNativeBookkeeper<SmallShadowMesh> m_smallShadowMeshes;

		public NativeListRecord<bool> m_shadowCastingStates;

		public NativeListRecord<float> m_meshMaxDistances;

		public TransformAccessRecord m_transformAccessRecord;

		private Optionable<(JobHandle jobHandle, NativeQueue<CheckResult> results)> m_jobHandle;

		protected bool m_debugDisable;

		protected override void OnCreated()
		{
			base.OnCreated();
			m_smallShadowMeshes = new ObjectNativeBookkeeper<SmallShadowMesh>(200);
			m_shadowCastingStates = new NativeListRecord<bool>(200);
			m_meshMaxDistances = new NativeListRecord<float>(200);
			m_transformAccessRecord = new TransformAccessRecord(200);
			m_smallShadowMeshes.RegisterRecord(m_shadowCastingStates);
			m_smallShadowMeshes.RegisterRecord(m_meshMaxDistances);
			m_smallShadowMeshes.RegisterRecord(m_transformAccessRecord);
		}

		public static void RegisterSmallShadowMesh(SmallShadowMesh mesh)
		{
			Singleton<SmallShadowHandler>.Instance.Process();
			Singleton<SmallShadowHandler>.Instance.m_smallShadowMeshes.Add(mesh);
			Singleton<SmallShadowHandler>.Instance.m_shadowCastingStates.Add(value: false);
			Singleton<SmallShadowHandler>.Instance.m_meshMaxDistances.Add(mesh.maxDistance);
			Singleton<SmallShadowHandler>.Instance.m_transformAccessRecord.Add(mesh.transform);
		}

		public static void UnregisterSmallShadowMesh(SmallShadowMesh mesh)
		{
			if (!(Singleton<SmallShadowHandler>.Instance == null))
			{
				Singleton<SmallShadowHandler>.Instance.Process();
				if (Singleton<SmallShadowHandler>.Instance.m_smallShadowMeshes.Contains(mesh))
				{
					Singleton<SmallShadowHandler>.Instance.m_smallShadowMeshes.Remove(mesh);
				}
				else
				{
					Debug.LogError(mesh.gameObject.name + " not found in SmallShadowHandler book keeper", mesh.gameObject);
				}
			}
		}

		private void Update()
		{
			if (!m_debugDisable)
			{
				ScheduleShadowDistanceChecks(Camera.main.transform.position);
			}
		}

		[ConsoleCommand]
		public static void DebugDisable()
		{
			Singleton<SmallShadowHandler>.Instance.m_debugDisable = true;
			PerformantList<SmallShadowMesh> keyList = Singleton<SmallShadowHandler>.Instance.m_smallShadowMeshes.GetKeyList();
			for (int i = 0; i < keyList.Count; i++)
			{
				keyList[i].SetShadowMode(ShadowCastingMode.On);
			}
		}

		[ConsoleCommand]
		public static void DebugEnable()
		{
			Singleton<SmallShadowHandler>.Instance.m_debugDisable = false;
			PerformantList<SmallShadowMesh> keyList = Singleton<SmallShadowHandler>.Instance.m_smallShadowMeshes.GetKeyList();
			for (int i = 0; i < keyList.Count; i++)
			{
				bool flag = Singleton<SmallShadowHandler>.Instance.m_shadowCastingStates.NativeList[i];
				keyList[i].SetShadowMode(flag ? ShadowCastingMode.On : ShadowCastingMode.Off);
			}
		}

		private void LateUpdate()
		{
			Process();
		}

		public void ScheduleShadowDistanceChecks(float3 cameraPos)
		{
			Process();
			NativeQueue<CheckResult> item = new NativeQueue<CheckResult>(Allocator.TempJob);
			JobHandle item2 = new SmallShadowCheckJob
			{
				MeshShadowCastingStates = m_shadowCastingStates.NativeList.AsArray(),
				MeshMaxDistances = m_meshMaxDistances.NativeList.AsArray(),
				CameraPos = cameraPos,
				CheckResults = item.AsParallelWriter(),
				DistanceFactor = GetDistanceFactor()
			}.Schedule(m_transformAccessRecord.TransformAccessArray);
			m_jobHandle = Optionable<(JobHandle, NativeQueue<CheckResult>)>.Some((item2, item));
		}

		protected virtual float GetDistanceFactor()
		{
			return 1f;
		}

		public void Process()
		{
			if (m_jobHandle.IsSome)
			{
				m_jobHandle.Value.jobHandle.Complete();
				NativeQueue<CheckResult> item = m_jobHandle.Value.results;
				int count = item.Count;
				for (int i = 0; i < count; i++)
				{
					CheckResult checkResult = item.Dequeue();
					m_smallShadowMeshes.GetKeyFromIndex(checkResult.Index).SetShadowMode(checkResult.CastShadow ? ShadowCastingMode.On : ShadowCastingMode.Off);
				}
				item.Dispose();
				m_jobHandle = Optionable<(JobHandle, NativeQueue<CheckResult>)>.None;
			}
		}

		private void OnDestroy()
		{
			m_smallShadowMeshes?.Dispose();
		}
	}
}
