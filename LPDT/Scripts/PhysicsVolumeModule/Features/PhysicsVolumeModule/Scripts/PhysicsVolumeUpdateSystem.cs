using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Features.PhysicsVolumeModule.Scripts
{
	public class PhysicsVolumeUpdateSystem : IInitializable, IDisposable
	{
		private readonly List<PhysicsInfluenceVolume> _ordered = new List<PhysicsInfluenceVolume>();

		private CancellationTokenSource _cts;

		public void Initialize()
		{
			_cts = new CancellationTokenSource();
			RunEarlyPredictLoop(_cts.Token).Forget();
			RunLateApplyLoop(_cts.Token).Forget();
			RunPhysicsApplyLoop(_cts.Token).Forget();
		}

		public void Dispose()
		{
			_cts?.Cancel();
			_cts?.Dispose();
			_cts = null;
		}

		private async UniTaskVoid RunEarlyPredictLoop(CancellationToken token)
		{
			while (!(await UniTask.Yield(PlayerLoopTiming.PreUpdate, token).SuppressCancellationThrow()))
			{
				ResolveOrder();
				for (int i = 0; i < _ordered.Count; i++)
				{
					_ordered[i].TickEarlyPredict();
				}
			}
		}

		private async UniTaskVoid RunLateApplyLoop(CancellationToken token)
		{
			while (!(await UniTask.Yield(PlayerLoopTiming.PostLateUpdate, token).SuppressCancellationThrow()))
			{
				ResolveOrder();
				for (int i = 0; i < _ordered.Count; i++)
				{
					_ordered[i].TickLateApply();
				}
			}
		}

		private async UniTaskVoid RunPhysicsApplyLoop(CancellationToken token)
		{
			while (!(await UniTask.Yield(PlayerLoopTiming.LastFixedUpdate, token).SuppressCancellationThrow()))
			{
				ResolveOrder();
				for (int i = 0; i < _ordered.Count; i++)
				{
					_ordered[i].TickPhysicsApply();
				}
			}
		}

		private void ResolveOrder()
		{
			IReadOnlyList<PhysicsInfluenceVolume> activeVolumes = PhysicsInfluenceVolume.ActiveVolumes;
			_ordered.Clear();
			for (int i = 0; i < activeVolumes.Count; i++)
			{
				PhysicsInfluenceVolume physicsInfluenceVolume = activeVolumes[i];
				if (physicsInfluenceVolume != null && physicsInfluenceVolume.isActiveAndEnabled)
				{
					_ordered.Add(physicsInfluenceVolume);
				}
			}
			_ordered.Sort(CompareOuterFirst);
		}

		private static int CompareOuterFirst(PhysicsInfluenceVolume left, PhysicsInfluenceVolume right)
		{
			if (left == right)
			{
				return 0;
			}
			if (left.IsInnerTo(right))
			{
				return 1;
			}
			if (right.IsInnerTo(left))
			{
				return -1;
			}
			return 0;
		}
	}
}
