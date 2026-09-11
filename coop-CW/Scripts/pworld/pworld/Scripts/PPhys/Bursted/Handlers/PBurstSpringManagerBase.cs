using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

namespace pworld.Scripts.PPhys.Bursted.Handlers
{
	public abstract class PBurstSpringManagerBase<TJob, TSpringData, TTarget> : MonoBehaviour where TJob : struct, IJobParallelForTransform where TSpringData : struct where TTarget : struct
	{
		[NonSerialized]
		public NativeArray<TSpringData> datas;

		[NonSerialized]
		public NativeArray<TTarget> targets;

		[NonSerialized]
		public TransformAccessArray transforms;

		[NonSerialized]
		public NativeArray<Vector3> velocities;

		public readonly List<IPBurstSpringWorker<TTarget, TSpringData>> workers = new List<IPBurstSpringWorker<TTarget, TSpringData>>();

		public static PBurstSpringManagerBase<TJob, TSpringData, TTarget> Me { get; private set; }

		protected abstract TJob GetNewJob();

		private void Awake()
		{
			Me = this;
		}

		private void LateUpdate()
		{
			RunJobs();
		}

		public virtual void OnDestroy()
		{
			transforms.Dispose();
			velocities.Dispose();
			datas.Dispose();
		}

		public void AddWorker(IPBurstSpringWorker<TTarget, TSpringData> _worker)
		{
			workers.Add(_worker);
			AllocateDatas(_added: true);
		}

		private void AllocateDatas(bool _added)
		{
			datas = new NativeArray<TSpringData>(workers.Count, Allocator.Persistent);
			Transform[] array = new Transform[workers.Count];
			targets = new NativeArray<TTarget>(workers.Count, Allocator.Persistent);
			NativeArray<Vector3> nativeArray = default(NativeArray<Vector3>);
			bool flag = workers.Count > 1 || !_added;
			if (flag)
			{
				nativeArray = new NativeArray<Vector3>(workers.Count, Allocator.Persistent);
			}
			for (int i = 0; i < workers.Count; i++)
			{
				if (flag)
				{
					nativeArray[i] = workers[i].Velocity;
				}
				datas[i] = workers[i].Data;
				array[i] = workers[i].Transform;
				targets[i] = workers[i].Target;
				workers[i].Hire(i);
			}
			transforms = new TransformAccessArray(array);
			velocities.Dispose();
			if (flag)
			{
				velocities = nativeArray;
			}
			else
			{
				velocities = new NativeArray<Vector3>(workers.Count, Allocator.Persistent);
			}
		}

		protected void RunJobs()
		{
			if (workers.Count != 0)
			{
				GetNewJob().Schedule(transforms).Complete();
			}
		}

		public void RemoveWorker(IPBurstSpringWorker<TTarget, TSpringData> _worker)
		{
			workers.Remove(_worker);
			_worker.Fire();
			AllocateDatas(_added: false);
		}
	}
}
