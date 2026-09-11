using UnityEngine;
using UnityEngine.Jobs;

namespace pworld.Scripts.PPhys.Bursted.Handlers
{
	public abstract class PBurstSpringWorkerBase<TTarget, TSpringData, TJob, TManager> : MonoBehaviour, IPBurstSpringWorker<TTarget, TSpringData> where TTarget : struct where TSpringData : struct where TJob : struct, IJobParallelForTransform where TManager : PBurstSpringManagerBase<TJob, TSpringData, TTarget>
	{
		[SerializeField]
		protected TSpringData data;

		protected bool IsHired { get; private set; }

		protected int WorkerIndex { get; private set; }

		public abstract TManager Manager { get; }

		public abstract TTarget Target { get; }

		TSpringData IPBurstSpringWorker<TTarget, TSpringData>.Data
		{
			get
			{
				if (!IsHired)
				{
					return data;
				}
				return Manager.datas[WorkerIndex];
			}
			set
			{
				if (!IsHired)
				{
					data = value;
					Manager.datas[WorkerIndex] = data;
				}
			}
		}

		Transform IPBurstSpringWorker<TTarget, TSpringData>.Transform => base.transform;

		public virtual Vector3 Velocity
		{
			get
			{
				if (!IsHired)
				{
					return Vector3.zero;
				}
				return Manager.velocities[WorkerIndex];
			}
			set
			{
				if (IsHired)
				{
					Manager.velocities[WorkerIndex] = value;
				}
			}
		}

		private void JobDataChanged()
		{
			if (IsHired)
			{
				Manager.datas[WorkerIndex] = data;
			}
		}

		public virtual void UpdateTarget()
		{
			Manager.targets[WorkerIndex] = Target;
		}

		public virtual void Start()
		{
			Manager.AddWorker(this);
		}

		public virtual void OnDestroy()
		{
			Manager.RemoveWorker(this);
		}

		public virtual void Hire(int _index)
		{
			WorkerIndex = _index;
			IsHired = true;
		}

		public void Fire()
		{
			WorkerIndex = 0;
			IsHired = false;
		}

		public void Push(Vector3 dir, float f = 100f)
		{
			Velocity = ((dir == default(Vector3)) ? Vector3.up : dir) * f;
		}

		protected abstract TSpringData GetDefaultData();

		public void DefaultSpringData()
		{
			data = GetDefaultData();
		}
	}
}
